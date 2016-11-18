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
        Bitmap convertedImg;
        Bitmap croppedImg;
        Bitmap shapeImg;
        BlobDetector bd;
        Shapecheck sc;

        public Image(string img_path)
        {
            orgImg = AForge.Imaging.Image.FromFile("C:\\Github\\P3\\" + img_path + ".jpg"); //original image
        }

        private Bitmap convert(Bitmap img)
        {
            convertedImg = new Bitmap(img.Width, img.Height, PixelFormat.Format32bppArgb); //converts the image to bitmap in specific pixelformat - draws pixels on a blank bitmap
            using (Graphics g = Graphics.FromImage(convertedImg))
            {
                g.DrawImage(img, 0, 0);
            }
            return convertedImg;
        }

        private void scaling(double scalar)
        {
            int height = (int)(scalar * orgImg.Height);
            int width = (int)(scalar * orgImg.Width);
            ResizeBilinear filter = new ResizeBilinear(width, height); //AForge filter to scale the image
            orgImg = filter.Apply(orgImg);
        }

        private void crop() //crops the edge parts either on the right or left side (wide picture) or upper and downer side (tall picture)
        {
            if (orgImg.Width < orgImg.Height)
            {
                Crop filter = new Crop(new Rectangle(0, orgImg.Height / 6, orgImg.Width, orgImg.Height / 2));
                orgImg = filter.Apply(orgImg);
            }
            else
            {
                Crop filter = new Crop(new Rectangle(orgImg.Width / 6, 0, orgImg.Width / 2, orgImg.Height));
                orgImg = filter.Apply(orgImg);
            }
        }

        private void grayscale() //convers the image to grayscale, just a filter again
        {
            Grayscale filter = new Grayscale(0.299, 0.587, 0.114);
            croppedImg = filter.Apply(orgImg);
        }

        private void noiseReduce(int ksize) //filter, ksize - kernel for the gaussian filter,  
        {
            GaussianBlur filter = new GaussianBlur(1, ksize);
            croppedImg = filter.Apply(croppedImg);
        }

        private void edgeDetection() //sobel edge detection filter
        {
            SobelEdgeDetector filter = new SobelEdgeDetector();
            filter.ApplyInPlace(croppedImg);
        }

        private void thresholding() //finds threshold on its own
        {
            OtsuThreshold filter = new OtsuThreshold();
            croppedImg = filter.Apply(croppedImg);
        }

        private void skeletonize() // makes the edges of the picture only 1 pixel wide
        {
            SimpleSkeletonization filter = new SimpleSkeletonization();
            filter.ApplyInPlace(croppedImg);
        }

        public void preprocess(double scalar, int ksize) //it just calls all the functions for preprocesing
        {
            scaling(scalar);
            //crop();
            grayscale();
            noiseReduce(ksize);
            thresholding();
            edgeDetection();
            thresholding();
            skeletonize();
        }

        public void blobDetect()
        {
            bd = new BlobDetector(croppedImg);
            shapeImg = bd.blobDetection();
        }

        public void shapeDetect()
        {
            bd.BBsize();
            sc = new Shapecheck(shapeImg);
            sc.whichShape(bd.BBheight, bd.BBwidth);
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

        public void displayImage() //displaying the image using the OPENCV stuff
        {
            imgMatrix = new Image<Bgr, Byte>(orgImg);
            CvInvoke.Imshow("shape", imgMatrix);
            CvInvoke.WaitKey();
        }
    }
}