using Microsoft.ML;
using System;
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
            var mlContext = new MLContext(seed: 1);
            const string assetsRelativePath = @"../../../assets";
            string assetsPath = GetAbsolutePath(assetsRelativePath);
            string outputMlNetModelFilePath = Path.Combine(assetsPath, "outputs", "imageClassifier.zip");
            string imagesFolderPathForPredictions = Path.Combine(assetsPath, "inputs", "test-images");
            string imagesDownloadFolderPath = Path.Combine(assetsPath, "inputs", "images");

            string fullImagesetFolderPath = Path.Combine(imagesDownloadFolderPath, "DatasetSmall");

            // Specify MLContext Filter to only show feedback log/traces about ImageClassification
            // This is not needed for feedback output if using the explicit MetricsCallback parameter
            mlContext.Log += FilterMLContextLog;

            // 2. Load the initial full image-set into an IDataView and shuffle so it'll be better balanced
            IEnumerable<ImageData> images = LoadImagesFromDirectory(folder: fullImagesetFolderPath, useFolderNameAsLabel: true);
            IDataView fullImagesDataset = mlContext.Data.LoadFromEnumerable(images);
            IDataView shuffledFullImageFilePathsDataset = mlContext.Data.ShuffleRows(fullImagesDataset);

            // 3. Load Images with in-memory type within the IDataView and Transform Labels to Keys (Categorical)
            IDataView shuffledFullImagesDataset = mlContext.Transforms.Conversion.
                    MapValueToKey(outputColumnName: "LabelAsKey", inputColumnName: "Label", keyOrdinality: KeyOrdinality.ByValue)
                .Append(mlContext.Transforms.LoadRawImageBytes(
                                                outputColumnName: "Image",
                                                imageFolder: fullImagesetFolderPath,
                                                inputColumnName: "ImagePath"))
                .Fit(shuffledFullImageFilePathsDataset)
                .Transform(shuffledFullImageFilePathsDataset);

            // 4. Split the data 80:20 into train and test sets, train and evaluate.
            var trainTestData = mlContext.Data.TrainTestSplit(shuffledFullImagesDataset, testFraction: 0.2);
            IDataView trainDataView = trainTestData.TrainSet;
            IDataView testDataView = trainTestData.TestSet;

            // 5. Define the model's training pipeline using DNN default values
            //
            var pipeline = mlContext.MulticlassClassification.Trainers
                    .ImageClassification(featureColumnName: "Image",
                                         labelColumnName: "LabelAsKey",
                                         validationSet: testDataView)
                .Append(mlContext.Transforms.Conversion.MapKeyToValue(outputColumnName: "PredictedLabel",
                                                                      inputColumnName: "PredictedLabel"));

            // 5.1 (OPTIONAL) Define the model's training pipeline by using explicit hyper-parameters
            //
            //var options = new ImageClassificationTrainer.Options()
            //{
            //    FeatureColumnName = "Image",
            //    LabelColumnName = "LabelAsKey",
            //    // Just by changing/selecting InceptionV3/MobilenetV2/ResnetV250  
            //    // you can try a different DNN architecture (TensorFlow pre-trained model). 
            //    Arch = ImageClassificationTrainer.Architecture.MobilenetV2,
            //    Epoch = 50,       //100
            //    BatchSize = 10,
            //    LearningRate = 0.01f,
            //    MetricsCallback = (metrics) => Console.WriteLine(metrics),
            //    ValidationSet = testDataView
            //};

            //var pipeline = mlContext.MulticlassClassification.Trainers.ImageClassification(options)
            //        .Append(mlContext.Transforms.Conversion.MapKeyToValue(
            //            outputColumnName: "PredictedLabel",
            //            inputColumnName: "PredictedLabel"));

            // 6. Train/create the ML model
            Console.WriteLine("*** Training the image classification model with DNN Transfer Learning on top of the selected pre-trained model/architecture ***");

            // Measuring training time
            var watch = Stopwatch.StartNew();

            //Train
            ITransformer trainedModel = pipeline.Fit(trainDataView);

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            Console.WriteLine($"Training with transfer learning took: {elapsedMs / 1000} seconds");

            // 7. Get the quality metrics (accuracy, etc.)
            EvaluateModel(mlContext, testDataView, trainedModel);

            // 8. Save the model to assets/outputs (You get ML.NET .zip model file and TensorFlow .pb model file)
            mlContext.Model.Save(trainedModel, trainDataView.Schema, outputMlNetModelFilePath);
            Console.WriteLine($"Model saved to: {outputMlNetModelFilePath}");


            // 9. Try a single prediction simulating an end-user app
            TrySinglePrediction(imagesFolderPathForPredictions, mlContext, trainedModel);

            Console.WriteLine("Press any key to finish");
            //Console.ReadKey();


        }

        private static void EvaluateModel(MLContext mlContext, IDataView testDataset, ITransformer trainedModel)
        {
            Console.WriteLine("Making predictions in bulk for evaluating model's quality...");

            // Measuring time
            var watch = Stopwatch.StartNew();

            var predictionsDataView = trainedModel.Transform(testDataset);

            var metrics = mlContext.MulticlassClassification.Evaluate(predictionsDataView, labelColumnName: "LabelAsKey", predictedLabelColumnName: "PredictedLabel");
            //ConsoleHelper.PrintMultiClassClassificationMetrics("TensorFlow DNN Transfer Learning", metrics);

            watch.Stop();
            var elapsed2Ms = watch.ElapsedMilliseconds;

            FileUtils.writeLog(Path.Combine(GetAbsolutePath(@"../../../assets"), "outputs"), metrics, elapsed2Ms/1000);
            //Console.WriteLine($"Predicting and Evaluation took: {elapsed2Ms / 1000} seconds");
        }

        private static void TrySinglePrediction(string imagesFolderPathForPredictions, MLContext mlContext, ITransformer trainedModel)
        {
            // Create prediction function to try one prediction
            var predictionEngine = mlContext.Model
                .CreatePredictionEngine<InMemoryImageData, ImagePrediction>(trainedModel);

            var testImages = FileUtils.LoadInMemoryImagesFromDirectory(
                imagesFolderPathForPredictions, false);

            var imageToPredict = testImages.First();

            var prediction = predictionEngine.Predict(imageToPredict);

            Console.WriteLine(
                $"Image Filename : [{imageToPredict.ImageFileName}], " +
                $"Scores : [{string.Join(",", prediction.Score)}], " +
                $"Predicted Label : {prediction.PredictedLabel}");
        }


        public static IEnumerable<ImageData> LoadImagesFromDirectory(
            string folder,
            bool useFolderNameAsLabel = true)
            => FileUtils.LoadImagesFromDirectory(folder, useFolderNameAsLabel)
                .Select(x => new ImageData(x.imagePath, x.label));


        public static string GetAbsolutePath(string relativePath)
            => FileUtils.GetAbsolutePath(typeof(Program).Assembly, relativePath);

        public static void ConsoleWriteImagePrediction(string ImagePath, string Label, string PredictedLabel, float Probability)
        {
            var defaultForeground = Console.ForegroundColor;
            var labelColor = ConsoleColor.Magenta;
            var probColor = ConsoleColor.Blue;

            Console.Write("Image File: ");
            Console.ForegroundColor = labelColor;
            Console.Write($"{Path.GetFileName(ImagePath)}");
            Console.ForegroundColor = defaultForeground;
            Console.Write(" original labeled as ");
            Console.ForegroundColor = labelColor;
            Console.Write(Label);
            Console.ForegroundColor = defaultForeground;
            Console.Write(" predicted as ");
            Console.ForegroundColor = labelColor;
            Console.Write(PredictedLabel);
            Console.ForegroundColor = defaultForeground;
            Console.Write(" with score ");
            Console.ForegroundColor = probColor;
            Console.Write(Probability);
            Console.ForegroundColor = defaultForeground;
            Console.WriteLine("");
        }

        private static void FilterMLContextLog(object sender, LoggingEventArgs e)
        {
            if (e.Message.StartsWith("[Source=ImageClassificationTrainer;"))
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
