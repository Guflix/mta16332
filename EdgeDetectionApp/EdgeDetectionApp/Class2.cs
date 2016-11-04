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
         Bitmap fuck;
         int perimeter, area;
         double c;
        
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

        public void edgeDetection()
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
         }

         public void perimeterDetection()
         {
             for (int i = 1; i < croppedImg.Width-1; i++)
             {
                 for (int j = 1; j < croppedImg.Height-1; j++)
                 {
                     Color color = croppedImg.GetPixel(i, j);
                     Color colr = croppedImg.GetPixel(i + 1, j);
                     Color colr1 = croppedImg.GetPixel(i, j + 1);
                     Color col = croppedImg.GetPixel(i - 1, j);
                     Color col1 = croppedImg.GetPixel(i, j - 1);
                     if (color.ToArgb() == Color.White.ToArgb()) { 
                         if (colr.ToArgb() == Color.Black.ToArgb() || colr1.ToArgb() == Color.Black.ToArgb() || col.ToArgb() == Color.Black.ToArgb() || col1.ToArgb() == Color.Black.ToArgb())
                            perimeter++;
                     }
                 }
             }
             Console.WriteLine(perimeter);
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
             c = perimeter/(2*Math.Sqrt(Math.PI*area));
             if (c > 0.97)
                 Console.WriteLine("circle " + c);
             else
                 Console.WriteLine(c);
         }

         public void drawShape()
         {
             BlobCounter bc = new BlobCounter();
             List<IntPoint> edgePoints;
             bc.ProcessImage(croppedImg);
             Blob[] blobs = bc.GetObjectsInformation();

             BitmapData data = croppedImg.LockBits(new Rectangle(0, 0, croppedImg.Width, croppedImg.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, image.PixelFormat);
             GrahamConvexHull hullFinder = new GrahamConvexHull();
             for (int i = 0; i < blobs.Length; i++)
             {
                 edgePoints = bc.GetBlobsEdgePoints(blobs[i]);
                 
                 foreach (Blob blob in blobs)
                 {
                     List<IntPoint> leftP, rightP;
                     bc.GetBlobsLeftAndRightEdges(blob, out leftP, out rightP);
                     edgePoints.AddRange(leftP);
                     edgePoints.AddRange(rightP);
                     List<IntPoint> hull = hullFinder.FindHull(edgePoints);
                     Drawing.Polygon(data, edgePoints, Color.White);
                     Console.WriteLine("holla " + blob.Area);
                 }
                 croppedImg.UnlockBits(data);
             }
         }

         public void displayImage(string windowCaption)
         {
             imgMatrix = new Image<Gray, Byte>(croppedImg);
             CvInvoke.Imshow(windowCaption, imgMatrix);
         }
     }
}