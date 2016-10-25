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
            //LoadImage
            string img_path = "C:\\Github\\P3\\box.jpg"; 			// OBS: use double slash
            Mat img = CvInvoke.Imread("C:\\Github\\P3\\box.jpg", LoadImageType.Color); 	// LoadImageType.Graysca
            CvInvoke.Imwrite(img_path, img);
     
            //Scaling

            //Blur
            //CvInvoke.MedianBlur(img, img_thresh, 10);

            //Edgedetection
            //CvInvoke.Canny(img, img_thresh, 10, 20, 3);

            // Thresholding
            Mat img_thresh = new Mat();
            CvInvoke.CvtColor(img, img_thresh, ColorConversion.Bgr2Gray);
            CvInvoke.Threshold(img_thresh, img_thresh, 100, 255, ThresholdType.Binary);
      
            //Blobs?
      
            //Showimages
            CvInvoke.Imshow("Normal", img);
            CvInvoke.Imshow("Threshhold", img_thresh);
            CvInvoke.WaitKey(0);  
        }
    }
}

