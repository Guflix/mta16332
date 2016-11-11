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
    class Image
    {
        Image<Gray, Byte> imgMatrix;
        public Bitmap orgImg;
        public Bitmap convertedImg;
        public Bitmap croppedImg;
        string shape; 

        public Image(string img_path)
        {
            orgImg = AForge.Imaging.Image.FromFile("C:\\Github\\P3\\" + img_path + ".jpg");
        }

        public void convert()
        {
            convertedImg = new Bitmap(orgImg.Width, orgImg.Height, PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(convertedImg))
            {
                g.DrawImage(orgImg, 0, 0);
            }
        }

        public void scaling(double scalar)
        {
            int height = (int)(scalar * orgImg.Height);
            int width = (int)(scalar * orgImg.Width);
            ResizeBilinear filter = new ResizeBilinear(width, height);
            orgImg = filter.Apply(orgImg);
        }

        public void crop()
        {
            if (orgImg.Width < orgImg.Height)
            {
                Crop filter = new Crop(new Rectangle(0, orgImg.Height / 6, orgImg.Width, orgImg.Height / 2));
                croppedImg = filter.Apply(orgImg);
            }
            else
            {
                Crop filter = new Crop(new Rectangle(orgImg.Width / 6, 0, orgImg.Width / 2, orgImg.Height));
                croppedImg = filter.Apply(orgImg);
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

        public void spoopy()
        {
            SimpleSkeletonization filter = new SimpleSkeletonization();
            filter.ApplyInPlace(croppedImg);
        }

        public void preprocess(double scalar, int ksize)
        {
            convert();
            scaling(scalar);
            crop();
            grayscale();
            noiseReduce(ksize);
            thresholding();
            hedgeDetection();
            thresholding();
            spoopy();
        }

        public void blobDetect()
        {
            BlobDetector bd = new BlobDetector(croppedImg);
            croppedImg = bd.biggestShape();
        }

        public void shapeDetect()
        {
            Shapecheck sc = new Shapecheck(croppedImg);
            shape = sc.whichShape();
        }

        public void displayImage()
        {
            imgMatrix = new Image<Gray, Byte>(croppedImg);
            CvInvoke.Imshow(shape, imgMatrix);
        }
    }
}