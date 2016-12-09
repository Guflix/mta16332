using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;

namespace EdgeDetectionApp
{
    public class Image2
    {
        Image<Bgr, Byte> imgMatrix;
        public string filepath;
        
        public Bitmap orgImg;
        public Bitmap preprocessedImg;
        Bitmap shapeImg;

        public BlobDetector bd;
        public Shapecheck sc;
        Color shapeColor = Color.Beige;

        public Image2(string filepath)
        {
            this.filepath = filepath;
            orgImg = AForge.Imaging.Image.FromFile(filepath);
        }
        
        public void grayscale()
        {
            Grayscale filter = new Grayscale(0.299, 0.587, 0.114);
            preprocessedImg = filter.Apply(orgImg);
        }

        private void noiseReduce() //filter, ksize - kernel for the gaussian filter,  
        {
            Median filter = new Median();
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

        //private void dilation() //finds threshold on its own
        //{
        //    Dilatation filter = new Dilatation();
        //    preprocessedImg = filter.Apply(preprocessedImg);
        //}

        public void preprocess() //it just calls all the functions for preprocesing
        {
            grayscale();
            noiseReduce();
            thresholding();
            edgeDetection();
            //dilation();
            //dilation();
            //dilation();
        }

        public void blobDetect()
        {
            bd = new BlobDetector(preprocessedImg);
            bd.blobDetection();
        }

        public void shapeDetect(int blobNo)
        {
            shapeImg = bd.drawConvexHull(blobNo);
            bd.boundingBoxSize();
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

        public Bitmap drawShape()
        {
            shapeImg = convert(shapeImg);
            for (int x = 0; x < shapeImg.Width; x++)
            {
                for (int y = 0; y < shapeImg.Height; y++)
                {
                    Color color = shapeImg.GetPixel(x, y);
                    if (color.ToArgb() == Color.Black.ToArgb())
                        shapeImg.SetPixel(x, y, Color.Transparent);
                    else
                        shapeImg.SetPixel(x, y, shapeColor);
                }
            }

            Rectangle rect = new Rectangle(0, 0, orgImg.Width, orgImg.Height);
            using (Graphics g = Graphics.FromImage(orgImg))
            {
                g.DrawImage(shapeImg, rect);
            }
            return orgImg;
        }

        public Bitmap drawColor() //drawing colours depending on shapes
        {
            for (int x = bd.minX; x < bd.maxX; x++)
            {
                for (int y = bd.minY; y < bd.maxY; y++)
                {
                    Color color = shapeImg.GetPixel(x, y);
                    if (color.ToArgb() == shapeColor.ToArgb())
                    {
                        if(sc.circle)
                            shapeImg.SetPixel(x, y, Color.IndianRed);
                        else if (sc.triangle)
                            shapeImg.SetPixel(x, y, Color.Yellow);
                        else if (sc.rect)
                            shapeImg.SetPixel(x, y, Color.MediumOrchid);
                        else
                            shapeImg.SetPixel(x, y, Color.RoyalBlue);
                    }                           
                }
            }

            Rectangle rect = new Rectangle(0, 0, orgImg.Width, orgImg.Height);
            using (Graphics g = Graphics.FromImage(orgImg))
            {
                g.DrawImage(shapeImg, rect);
            }
            return orgImg;
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