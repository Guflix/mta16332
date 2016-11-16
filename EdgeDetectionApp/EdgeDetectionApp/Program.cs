using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.UI;

namespace EdgeDetectionApp
{
    class ProgramMain
    {
        static void Main(string[] args)
        {
            Image test = new Image("box");
            test.preprocess(0.6, 11);
            test.blobDetect();
            test.shapeDetect();
            test.draw();
            test.displayImage();
            CvInvoke.WaitKey();
        }
    }
}
