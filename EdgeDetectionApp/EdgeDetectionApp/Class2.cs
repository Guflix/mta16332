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
     class Detection1
    {
         Image<Gray, Byte> imgMatrix;
         Bitmap image;
         Bitmap croppedImg;
         double perimeter, area;
         bool circle, rect, square, triangle;
         AForge.IntPoint blob;
        
         public Detection1(string img_path)
         {
             image = AForge.Imaging.Image.FromFile("C:\\Github\\P3\\" + img_path + ".jpg");
         }

         public void scaling(double scalar)
         {
             int height = (int)(scalar * image.Height);
             int width = (int)(scalar * image.Width);
             ResizeBilinear filter = new ResizeBilinear(width, height);
             image = filter.Apply(image);
         }

         public void crop()
         {
             if (image.Width < image.Height)
             {
                 Crop filtr = new Crop(new Rectangle(0, image.Height / 6, image.Width, image.Height / 2));
                 croppedImg = filtr.Apply(image);
             }
             else
             {
                 Crop filtr = new Crop(new Rectangle(image.Width / 6, 0, image.Width / 2, image.Height));
                 croppedImg = filtr.Apply(image);
             }
             //croppedImg = image;
         }

         public void grayscale()
         {
             Grayscale filter = new Grayscale(0.299, 0.587, 0.114);
             croppedImg = filter.Apply(croppedImg);
         }

         public void noiseReduce(int ksize)
         {
             GaussianBlur filter = new GaussianBlur(1, ksize);
             croppedImg = filter.Apply(croppedImg);
         }

         public void hedgeDetection()
         {
             CannyEdgeDetector filter = new CannyEdgeDetector();
             filter.ApplyInPlace(croppedImg);
         }

         public void thresholding()
         {
             OtsuThreshold filter = new OtsuThreshold();
             croppedImg = filter.Apply(croppedImg);
         }

         public void closing()
         {
             Dilatation filter = new Dilatation();
             Erosion eFilter = new Erosion();
             croppedImg = filter.Apply(croppedImg);
             croppedImg = eFilter.Apply(croppedImg);
         }

         public void extractShape()
         {
             ExtractBiggestBlob filter = new ExtractBiggestBlob();
             croppedImg = filter.Apply(croppedImg);
             blob = filter.BlobPosition;
         }

         public void spoopy()
         {
             SimpleSkeletonization filter = new SimpleSkeletonization();
             filter.ApplyInPlace(croppedImg);
         }

         public void perimeterDetection()
         {
             for (int i = 0; i < croppedImg.Width; i++)
             {
                 for (int j = 0; j < croppedImg.Height; j++)
                 {
                     Color color = croppedImg.GetPixel(i, j);
                     if (color.ToArgb() == Color.White.ToArgb()) { 
                            perimeter++;
                     }
                 }
             }
             Console.WriteLine(perimeter);
         }

         public void drawShape()
         {
             PointedColorFloodFill filter = new PointedColorFloodFill();
             filter.FillColor = Color.White;
             filter.StartingPoint = new IntPoint(croppedImg.Width / 2, croppedImg.Width / 2);
             filter.ApplyInPlace(croppedImg);
         }

         public void areaDetection()
         {
             for (int i = 0; i < croppedImg.Width; i++)
             {
                 for (int j = 0; j < croppedImg.Height; j++)
                 {
                     Color color = croppedImg.GetPixel(i, j);
                     if (color.ToArgb()==Color.White.ToArgb())
                         area++;
                 }
             }
             Console.WriteLine(area);
         }

         public void circularity()
         {
             double c = (4*Math.PI*area)/Math.Pow(perimeter,2);
             if (c > 0.95){
                 Console.WriteLine("circle " + c);
                 circle = true;
             }
             else
             {
                 Console.WriteLine("nope " + c);
                 shape();
             }
                 
         }

         public void shape()
         {
             double bbArea = croppedImg.Height * croppedImg.Width;
             if (area / bbArea < 0.55)
             {
                 triangle = true;
             }

             double bbRatio;
             if (croppedImg.Width > croppedImg.Height)
                 bbRatio = croppedImg.Height / croppedImg.Width;
             else
                 bbRatio = croppedImg.Width / croppedImg.Height;

             if (bbRatio > 0.95 && triangle == false)
                 square = true;
             else
                 rect = true;
         }

         public void displayImage()
         {
             string shape;
             if (circle)
                 shape = "circle";
             else if (triangle)
                 shape = "triangle";
             else if (square)
                 shape = "square";
             else if (rect)
                 shape = "rectangle";
             else
                 shape = "idek man";
             imgMatrix = new Image<Gray, Byte>(croppedImg);
             CvInvoke.Imshow(shape, imgMatrix);
         }
     }
}