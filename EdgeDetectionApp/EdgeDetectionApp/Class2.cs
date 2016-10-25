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
         Mat img;
         Image<Gray, Byte> imgMatrix;
         Image<Gray, Byte> outputMatrix;
        
         public Detection1(string img_path)
         {
             img = CvInvoke.Imread("C:\\Github\\P3\\" + img_path, LoadImageType.Color);
             imgMatrix = img.ToImage<Gray, Byte>();
         }

         public void scaling(double scalar)
         {
             int newWidth = (int)(imgMatrix.Cols * scalar);
             int newHeight = (int)(imgMatrix.Rows * scalar);
             outputMatrix = new Image<Gray, byte>(newWidth, newHeight);
             
             for (int y = 0; y < outputMatrix.Rows; y++)
             {
                 for (int x = 0; x < outputMatrix.Cols; x++)
                 {
                     int h = (int)(y / scalar);
                     int w = (int)(x / scalar);
                     outputMatrix.Data[y, x, 0] = imgMatrix.Data[h, w, 0];
                 }
             }
         }

         public void noiseReduce(int ksize)
         {
             outputMatrix = outputMatrix.SmoothGaussian(ksize, ksize, 0, 0);
         }

         public void cannyEdgeDetection()
         {
             CvInvoke.Canny(outputMatrix, outputMatrix, 0, 170, 3);
         }

         public void displayImage(string windowCaption)
         {
             CvInvoke.Imshow(windowCaption, outputMatrix);
         }
    }
}

