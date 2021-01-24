using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.ML;
using Microsoft.ML.Data;
using ML_GUI.DataModels;

namespace ML_GUI
{
   class Prediction
    {   
        //Paths
        static string assetsRelativePath;// = @"../../../assets";
        static string assetsAbsolutePath;
        static string imageClassifierModelZipFilePath;
        //Model for machine learning
        ITransformer loadedModel;
        //prediction engine for machine learning
        PredictionEngine<InMemoryImageData, ImagePrediction> predictionEngine;
        public Prediction()
        {   //MLcontext to load model and predictionEngine
            MLContext mlContext = new MLContext(seed: 1);
            //Init paths
            assetsRelativePath = @"../../../assets";
            assetsAbsolutePath = GetAbsolutePath(assetsRelativePath);
            imageClassifierModelZipFilePath = Path.Combine(assetsAbsolutePath, "outputs", "imageClassifier.zip");
            //load model
            loadedModel = mlContext.Model.Load(imageClassifierModelZipFilePath, out var modelInputSchema);
            //create  prediction engine
            predictionEngine = mlContext.Model.CreatePredictionEngine<InMemoryImageData, ImagePrediction>(loadedModel);
        }
        
        public string predict(string imagePath)
        {   //check if imagePath is not empty and model is trained
            if (imagePath == String.Empty)
            {
                return "Please take or select a Picture first";
            }
           
           
            try
            {
                //get Image for prediction
                string[] imageName = imagePath.Split('\\');
                var testImagePredict = new InMemoryImageData(File.ReadAllBytes(imagePath), "", imageName[imageName.Length-1]);
               
                //single prediction
                var prediction = predictionEngine.Predict(testImagePredict);

                // Get the highest score and its index
                var maxScore = prediction.Score.Max();
                if (maxScore <= 0.89) {
                    return "No Object detected";
                }
                else
                {
                  return $"Image Filename : [{testImagePredict.ImageFileName}], " +
                  $"Predicted Label : [{prediction.PredictedLabel}], " +
                  $"Probability : [{maxScore}] ";
                }
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