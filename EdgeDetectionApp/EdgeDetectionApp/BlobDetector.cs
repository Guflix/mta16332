using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using Emgu.CV;
using Emgu.CV.Structure;
using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Math.Geometry;

namespace EdgeDetectionApp
{
    class BlobDetector
    {
        Bitmap img;
        public IntPoint blobCoor;

        public BlobDetector(Bitmap img)
        {
            this.img = img;
        }

        public Bitmap biggestShape()
        {
            ExtractBiggestBlob filter = new ExtractBiggestBlob();
            img = filter.Apply(img);
            blobCoor = filter.BlobPosition;
            return img;
        }
    }
}
