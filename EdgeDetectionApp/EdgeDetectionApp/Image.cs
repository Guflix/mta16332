using System;
using System.Drawing;
using System.Drawing.Imaging;
using Emgu.CV;
using Emgu.CV.Structure;
using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;

namespace EdgeDetectionApp
{
    class Image
    {
        Image<Bgr,Byte> imgMatrix;
        Bitmap orgImg;
        Bitmap preprocessedImg;
        Bitmap shapeImg;
        BlobDetector bd;
        Shapecheck sc;

        public Image(string img_path)
        {
            orgImg = AForge.Imaging.Image.FromFile("C:\\Github\\P3\\Kids\\" + img_path + ".jpg"); //original image
        }

        private void grayscale() //convers the image to grayscale, just a filter again
        {
            Grayscale filter = new Grayscale(0.299, 0.587, 0.114);
            preprocessedImg = filter.Apply(orgImg);
        }

        private void noiseReduce(int ksize) //filter, ksize - kernel for the gaussian filter,  
        {
            GaussianBlur filter = new GaussianBlur(1, ksize);
            preprocessedImg = filter.Apply(preprocessedImg);
        }

        private void thresholding() //finds threshold on its own
        {
            OtsuThreshold filter = new OtsuThreshold();
            preprocessedImg = filter.Apply(preprocessedImg);
        }

        private void edgeDetection() //sobel edge detection filter
        {
            SobelEdgeDetector filter = new SobelEdgeDetector();
            filter.ApplyInPlace(preprocessedImg);
        }

        private void dilation() //finds threshold on its own
        {
            Dilatation filter = new Dilatation();
            preprocessedImg = filter.Apply(preprocessedImg);
        }

        public void preprocess(int ksize) //it just calls all the functions for preprocesing
        {
            grayscale();
            noiseReduce(ksize);
            thresholding();
            edgeDetection();
            dilation();
            dilation();
        }

        public void blobDetect()
        {
            bd = new BlobDetector(preprocessedImg);
            shapeImg = bd.blobDetection();
            thresholding();
        }

        public void shapeDetect()
        {
            bd.boxSize();
            sc = new Shapecheck(shapeImg);
            sc.whichShape(bd.boxHeight, bd.boxWidth, bd.minX, bd.minY, bd.maxX, bd.maxY);
        }

        private Bitmap convert(Bitmap img)
        {
            Bitmap convertedImg = new Bitmap(img.Width, img.Height, PixelFormat.Format32bppArgb); //converts the image to bitmap in specific pixelformat - draws pixels on a blank bitmap
            using (Graphics g = Graphics.FromImage(convertedImg))
            {
                g.DrawImage(img, 0, 0);
            }
            return convertedImg;
        }

        public void draw() //drawing colours depending on shapes
        {
            shapeImg = convert(shapeImg);
            for (int x = 0; x < shapeImg.Width; x++)
            {
                for (int y = 0; y < shapeImg.Height; y++)
                {
                    Color color = shapeImg.GetPixel(x, y);
                    if (color.ToArgb() == Color.Black.ToArgb())
                        shapeImg.SetPixel(x, y, Color.Transparent);
                    else if (color.ToArgb() != Color.Black.ToArgb()){
                        if(sc.circle)
                            shapeImg.SetPixel(x, y, Color.Red);
                        else if (sc.triangle)
                            shapeImg.SetPixel(x, y, Color.Yellow);
                        else if (sc.rect)
                            shapeImg.SetPixel(x, y, Color.Purple);
                        else
                            shapeImg.SetPixel(x, y, Color.Blue);
                    }                        
                }
            }

            Rectangle rect = new Rectangle(0, 0, orgImg.Width, orgImg.Height);
            using (Graphics g = Graphics.FromImage(orgImg))
            {
                g.DrawImage(shapeImg, rect);
            }
        }

        private void scaling(double scalar)
        {
            int height = (int)(scalar * orgImg.Height);
            int width = (int)(scalar * orgImg.Width);
            ResizeBilinear filter = new ResizeBilinear(width, height); //AForge filter to scale the image
            orgImg = filter.Apply(orgImg);
        }

        public void displayImage(double scalar, string caption) //displaying the image using the OPENCV stuff
        {
            scaling(scalar);
            imgMatrix = new Image<Bgr, Byte>(orgImg);
            CvInvoke.Imshow(caption, imgMatrix);
            CvInvoke.WaitKey();
        }
    }
}