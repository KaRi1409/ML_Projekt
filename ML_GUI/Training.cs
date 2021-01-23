using Microsoft.ML;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using ML_GUI.DataModels;
using static Microsoft.ML.Transforms.ValueToKeyMappingEstimator;

namespace ML_GUI
{
    class Training
    {   
        private static string zipPath = Path.Combine(GetAbsolutePath(@"../../../assets"), "outputs", "imageClassifier.zip");
        private static bool isTrained = false;
        static MLContext mlContext = new MLContext(seed: 1);
        const string assetsRelativePath = @"../../../assets";
        private static string assetsPath = GetAbsolutePath(assetsRelativePath);
        private static string outputMlNetModelFilePath = Path.Combine(assetsPath, "outputs", "imageClassifier.zip");
        private static string imagesFolderPathForPredictions = Path.Combine(assetsPath, "inputs", "test-images");
        private static string fullImagesetFolderPath = Path.Combine(assetsPath, "inputs", "images", "DatasetSmall");

        public static bool IsTrained
        {
            get
            {
                if (File.Exists(zipPath))
                {
                    return true;
                }
                else return false;
            }
            set { isTrained = value; }
        }


        public static void train()
        {

            IEnumerable<ImageData> images = LoadImagesFromDirectory(folder: fullImagesetFolderPath, useFolderNameAsLabel: true);
            IDataView fullImagesDataset = mlContext.Data.LoadFromEnumerable(images);
            IDataView shuffledFullImageFilePathsDataset = mlContext.Data.ShuffleRows(fullImagesDataset);

            // Load Images with in-memory type within the IDataView and Transform Labels to Keys
            IDataView shuffledFullImagesDataset = mlContext.Transforms.Conversion.
                    MapValueToKey(outputColumnName: "LabelAsKey", inputColumnName: "Label", keyOrdinality: KeyOrdinality.ByValue)
                .Append(mlContext.Transforms.LoadRawImageBytes(
                                                outputColumnName: "Image",
                                                imageFolder: fullImagesetFolderPath,
                                                inputColumnName: "ImagePath"))
                .Fit(shuffledFullImageFilePathsDataset)
                .Transform(shuffledFullImageFilePathsDataset);

            // Split the data 80:20 into train and test sets, train and evaluate.
            var trainTestData = mlContext.Data.TrainTestSplit(shuffledFullImagesDataset, testFraction: 0.2);
            IDataView trainDataView = trainTestData.TrainSet;
            IDataView testDataView = trainTestData.TestSet;

            // Define the model's training pipeline
            var pipeline = mlContext.MulticlassClassification.Trainers
                    .ImageClassification(featureColumnName: "Image",
                                         labelColumnName: "LabelAsKey",
                                         validationSet: testDataView)
                .Append(mlContext.Transforms.Conversion.MapKeyToValue(outputColumnName: "PredictedLabel",
                                                                      inputColumnName: "PredictedLabel"));

            //Train the model
            ITransformer trainedModel = pipeline.Fit(trainDataView);
       
            // Get the quality metrics (accuracy, etc.) (Log.txt)
            EvaluateModel(mlContext, testDataView, trainedModel);

            // Save the model to assets/outputs
            mlContext.Model.Save(trainedModel, trainDataView.Schema, outputMlNetModelFilePath);
        }

        private static void EvaluateModel(MLContext mlContext, IDataView testDataset, ITransformer trainedModel)
        {
            // Measuring time
            var watch = Stopwatch.StartNew();

            var predictionsDataView = trainedModel.Transform(testDataset);

            var metrics = mlContext.MulticlassClassification.Evaluate(predictionsDataView, labelColumnName: "LabelAsKey", predictedLabelColumnName: "PredictedLabel");
            watch.Stop();
            var elapsed2Ms = watch.ElapsedMilliseconds;

            FileUtils.writeLog(Path.Combine(GetAbsolutePath(@"../../../assets"), "outputs"), metrics, elapsed2Ms/1000);
        }

        public static IEnumerable<ImageData> LoadImagesFromDirectory(
            string folder,
            bool useFolderNameAsLabel = true)
            => FileUtils.LoadImagesFromDirectory(folder, useFolderNameAsLabel)
                .Select(x => new ImageData(x.imagePath, x.label));

        public static string GetAbsolutePath(string relativePath)
            => FileUtils.GetAbsolutePath(typeof(Program).Assembly, relativePath);

       
    }
}
