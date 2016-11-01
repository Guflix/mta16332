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
using AForge.Imaging.Filters;
using AForge.Fuzzy;
using AForge.Math.Geometry;
using AForge.Math;
using AForge.Math.Metrics;
namespace EdgeDetectionApp
{
     class Detection1
    {
         //Mat img;
         Image<Gray, Byte> imgMatrix;
         //Image<Gray, Byte> outputMatrix;
         Bitmap image;
        
         public Detection1(string img_path)
         {
             //img = CvInvoke.Imread("C:\\Github\\P3\\" + img_path, LoadImageType.Color);
             //imgMatrix = img.ToImage<Gray, Byte>();
             image = AForge.Imaging.Image.FromFile("C:\\Github\\P3\\" + img_path + ".jpg");
         }

         public void grayscale()
         {
             Grayscale filter = new Grayscale(0.2125, 0.7154, 0.0721);
             image = filter.Apply(image);
         }

         public void scaling(double scalar)
         {
             int height = (int)(scalar * image.Height);
             int width = (int)(scalar * image.Width);
             ResizeBilinear filter = new ResizeBilinear(width, height);
             image = filter.Apply(image);
         }

         public void noiseReduce(int ksize)
         {
             GaussianBlur filter = new GaussianBlur(1, ksize);
             image = filter.Apply(image);
         }

         public void histogramEq()
         {
             HistogramEqualization filter = new HistogramEqualization();
             filter.ApplyInPlace(image);
         }

         public void edgeDetection()
         {
             SobelEdgeDetector filter = new SobelEdgeDetector();
             filter.ApplyInPlace(image);
         }

         public void thresholding()
         {
             OtsuThreshold filter = new OtsuThreshold();
             filter.ApplyInPlace(image);
         }

         public void closing()
         {
             Dilatation filter = new Dilatation();
             Erosion eFilter = new Erosion();
             image = filter.Apply(image);
             image = eFilter.Apply(image);
         }

         public void displayImage(string windowCaption)
         {
             imgMatrix = new Image<Gray, Byte>(image);
             CvInvoke.Imshow(windowCaption, imgMatrix);
         }
     }
}

