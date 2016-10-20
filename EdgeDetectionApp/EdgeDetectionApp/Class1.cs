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
    class Class1
    {
          string img_path = "C:\\Users\\thoma\\Desktop\\farmer.jpg"; 	
        Image<Rgb, Byte> img;
        Image<Rgb, Byte> img_thresh;

        public void Threshhold (){
          	

            Mat img = CvInvoke.Imread(img_path, LoadImageType.Color); 	// LoadImageType.Graysca
            //CvInvoke.CvtColor(img, img_gray, ColorConversion.Bgr2Gray);
            CvInvoke.Imwrite(img_path + "farmer_out.jpg", img);

            // Thresholding
            Mat img_thresh = new Mat();
            CvInvoke.CvtColor(img, img_thresh, ColorConversion.Bgr2Gray);
            CvInvoke.Threshold(img_thresh, img_thresh, 100, 255, ThresholdType.Binary);
            }

         public void ShowImage()
        {
            CvInvoke.Imshow("Look at all these shapes", img);
            CvInvoke.WaitKey(0);
            CvInvoke.Imshow("With threshhold", img_thresh);
            CvInvoke.WaitKey(0);  
             }
        }
    }

