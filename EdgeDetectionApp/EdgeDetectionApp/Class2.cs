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
         Image<Bgr, Byte> imgMatrix;
         Bitmap image;
         Bitmap croppedImg;
        
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
             Grayscale filter = new Grayscale(0.2125, 0.7154, 0.0721);
             croppedImg = filter.Apply(croppedImg);
         }

         public void noiseReduce(int ksize)
         {
             GaussianBlur filter = new GaussianBlur(1, ksize);
             croppedImg = filter.Apply(croppedImg);
         }

        public void edgeDetection()
         {
             SobelEdgeDetector filter = new SobelEdgeDetector();
             filter.ApplyInPlace(croppedImg);
         }

         public void thresholding()
         {
             OtsuThreshold filter = new OtsuThreshold();
             filter.ApplyInPlace(croppedImg);
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

         public void blobDetection()
         {
             BlobCounter bc = new BlobCounter();
             bc.ProcessImage(croppedImg);
             Blob[] blobs = bc.GetObjectsInformation();
             SimpleShapeChecker sc = new SimpleShapeChecker();
             List<IntPoint> edgePoints;
             for(int i = 0; i < blobs.Length; i++){
                 edgePoints = bc.GetBlobsEdgePoints(blobs[i]);
                 AForge.Point center;
                 float radius;
                 List<IntPoint> corners;

                 if (sc.IsCircle(edgePoints, out center, out radius))
                 {                     
                     Console.Write("circle");                    
                 }
                 else if (sc.IsQuadrilateral(edgePoints, out corners))
                 {
                     Console.Write("quadrilateral");
                 }

                 else if (sc.IsTriangle(edgePoints))
                 {
                     Console.Write("triangle");
                 }
             }
             
         }

         public void drawShape()
         {
             BlobCounter bc = new BlobCounter();
             List<IntPoint> edgePoints;
             bc.ProcessImage(croppedImg);
             Blob[] blobs = bc.GetObjectsInformation();
             
             BitmapData data = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, image.PixelFormat);
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
                     Drawing.Polygon(data, edgePoints, Color.Red);
                 }
                 image.UnlockBits(data);
             }
         }

         public void displayImage(string windowCaption)
         {
             imgMatrix = new Image<Bgr, Byte>(image);
             CvInvoke.Imshow(windowCaption, imgMatrix);
         }
     }
}