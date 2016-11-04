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
            Detection1 test = new Detection1("smoke");
            test.scaling(0.6);
            test.grayscale();
            test.thresholding();
            test.noiseReduce(11);
            test.thresholding();
            test.edgeDetection();
            test.closing();
            test.extractShape();
            test.thresholding();
            test.drawShape();
            test.perimeterDetection();
            test.areaDetection();
            test.circularity();
            test.displayImage("Helo");
            CvInvoke.WaitKey();
        }
    }
}
