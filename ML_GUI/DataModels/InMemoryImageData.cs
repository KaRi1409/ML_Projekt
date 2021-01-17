using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML_GUI.DataModels
{
    class InMemoryImageData
    {
        public InMemoryImageData(byte[] image, string label, string imageFileName)
        {
            Image = image;
            Label = label;
            ImageFileName = imageFileName;
        }

        public readonly byte[] Image;

        public readonly string Label;

        public readonly string ImageFileName;
    }
}
