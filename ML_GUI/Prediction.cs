using System;
using System.IO;
using System.Linq;
using Microsoft.ML;
using Microsoft.ML.Data;
using ML_GUI.DataModels;

namespace ML_GUI
{
    class Prediction
    {
        private string imagePath = String.Empty;

        public Prediction(string _imagePath)
        {
            imagePath = _imagePath;
        }

        public string predict()
        {
            if (imagePath == String.Empty)
            {
                return "Please take or select a Picture first";
            }
            if (!Training.IsTrained)
            {
                return "Cannot predict if the Model is not trained";
            }
            const string assetsRelativePath = @"../../../assets";
            var assetsPath = GetAbsolutePath(assetsRelativePath);

            var imageClassifierModelZipFilePath = Path.Combine(assetsPath, "outputs", "imageClassifier.zip");

            try
            {
                var mlContext = new MLContext(seed: 1);

                // Load the model
                var loadedModel = mlContext.Model.Load(imageClassifierModelZipFilePath, out var modelInputSchema);

                // Create prediction engine to try a single prediction (input = ImageData, output = ImagePrediction)
                var predictionEngine = mlContext.Model.CreatePredictionEngine<InMemoryImageData, ImagePrediction>(loadedModel);

                string[] imageName = imagePath.Split('\\');
                var testImagePredict = new InMemoryImageData(File.ReadAllBytes(imagePath), "", imageName[imageName.Length-1]);

                // Measure #1 prediction execution time.
                var watch = System.Diagnostics.Stopwatch.StartNew();

                var prediction = predictionEngine.Predict(testImagePredict);

                // Stop measuring time.
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;

                // Get the highest score and its index
                var maxScore = prediction.Score.Max();

                // Double-check using the index
                var maxIndex = prediction.Score.ToList().IndexOf(maxScore);
                VBuffer<ReadOnlyMemory<char>> keys = default;
                predictionEngine.OutputSchema[3].GetKeyValues(ref keys);
                var keysArray = keys.DenseValues().ToArray();
                var predictedLabelString = keysArray[maxIndex];
                ////////

                return $"Image Filename : [{testImagePredict.ImageFileName}], " +
                                  $"Predicted Label : [{prediction.PredictedLabel}], " +
                                  $"Probability : [{maxScore}] ";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return ex.ToString();
            }
        }

        public string GetAbsolutePath(string relativePath)
            => FileUtils.GetAbsolutePath(typeof(Program).Assembly, relativePath);
    }
}