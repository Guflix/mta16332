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
            Detection1 test = new Detection1("box");
            test.scaling(0.6);
            test.crop();
            test.grayscale();
            test.noiseReduce(11);
            test.thresholding();
            test.hedgeDetection();
            test.extractShape();
            test.thresholding();
            test.spoopy();
            test.perimeterDetection();
            test.drawShape();
            test.areaDetection();
            test.circularity();
            test.displayImage();
            CvInvoke.WaitKey();
        }
    }
}
