using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using AForge;
using AForge.Controls;
using AForge.Imaging;
using AForge.Imaging.Formats;
using AForge.Imaging.IPPrototyper;
using AForge.Imaging.Textures;
using AForge.Imaging.ColorReduction;
using AForge.Fuzzy;
using AForge.Math.Geometry;
using AForge.Math;
using AForge.Math.Metrics;
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

         public void blobDetect()
         {
             //outputMatrix.ToBitmap(outputMatrix.Rows,outputMatrix.Cols);
             BlobCounter bc = new BlobCounter();
             Bitmap image = AForge.Imaging.Image.FromFile("C:\\Github\\P3\\box.jpg");
             bc.ProcessImage(image);
             Blob[] blobs = bc.GetObjectsInformation();
             SimpleShapeChecker shapeChecker = new SimpleShapeChecker();

             Graphics g = Graphics.FromImage(image);
             Pen redPen = new Pen(Color.Red, 2);
             // check each object and draw circle around objects, which
             // are recognized as circles

             for (int i = 0, n = blobs.Length; i < n; i++)
             {
                 List<IntPoint> edgePoints = bc.GetBlobsEdgePoints(blobs[i]);

                 AForge.Point center;
                 float radius;

                 if (shapeChecker.IsCircle(edgePoints, out center, out radius))
                 {
                     g.DrawEllipse(redPen,
                         (int)(center.X - radius),
                         (int)(center.Y - radius),
                         (int)(radius * 2),
                         (int)(radius * 2));
                 }
             }

             redPen.Dispose();
             g.Dispose();

         }

         public void circleDetect()
         {

         }
    }
}

