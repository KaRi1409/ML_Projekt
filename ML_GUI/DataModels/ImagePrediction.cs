using Microsoft.ML.Data;

namespace ML_GUI.DataModels
{
    class ImagePrediction
    {
        [ColumnName("Score")]
        public float[] Score;

        [ColumnName("PredictedLabel")]
        public string PredictedLabel;
    }
}
