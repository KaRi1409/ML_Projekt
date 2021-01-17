using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML_GUI.DataModels
{
    class ImageData
    {
        public ImageData(string imagePath, string label)
        {
            ImagePath = imagePath;
            Label = label;
        }

        public readonly string ImagePath;

        public readonly string Label;
    }
}
