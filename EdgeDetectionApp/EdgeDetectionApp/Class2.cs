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
     class Detection1
    {
        public void Threshhold()
        {
            string img_path = "C:\\Github\\P3\\box.jpg"; 			// OBS: use double slash

            Mat img = CvInvoke.Imread("C:\\Github\\P3\\box.jpg", LoadImageType.Color); 	// LoadImageType.Graysca
            CvInvoke.Imwrite(img_path, img);
            CvInvoke.Imshow("Normal", img);
            CvInvoke.WaitKey(0);
  
            // Thresholding
            Mat img_thresh = new Mat();
            //CvInvoke.CvtColor(img, img_thresh, ColorConversion.Bgr2Gray);

            CvInvoke.Threshold(img_thresh, img_thresh, 100, 255, ThresholdType.Binary);
            //CvInvoke.MedianBlur(img, img_thresh, 10);
            CvInvoke.Canny(img, img_thresh, 10, 20, 3);
            //CvInvoke.Threshold(img_thresh, img_thresh, 100, 255, ThresholdType.Binary);

            CvInvoke.Imshow("Threshhold", img_thresh);
            CvInvoke.WaitKey(0);  
        }
    }
}

