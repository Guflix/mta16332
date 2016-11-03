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
            Detection1 test = new Detection1("box"); //EditForKate
            test.scaling(0.3);
            test.grayscale();
            test.noiseReduce(9);
            test.edgeDetection();
            test.thresholding();
            test.closing();
            test.extractShape();
            test.blobDetection();
            test.drawShape();
            test.displayImage("Helo");
            CvInvoke.WaitKey();
        }
    }
}