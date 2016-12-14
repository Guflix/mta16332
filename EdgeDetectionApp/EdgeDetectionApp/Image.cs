using System;
using System.Drawing;
using System.Drawing.Imaging;
//using Emgu.CV;
//using Emgu.CV.Structure;
using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;

namespace EdgeDetectionApp
{
    public class Image2
    {
        //Image<Bgr, Byte> imgMatrix;
        public string filepath;
        
        public Bitmap orgImg; // the original image of the picture, which is later drawn onto
        public Bitmap preprocessedImg; // the image after it has been preprocessed
        Bitmap shapeImg; // the image after BLOB detection and later floodfilling it

        public BlobDetector bd;
        public Shapecheck sc;
        Color shapeColor = Color.Beige; // the color drawn onto the shape before it has been guessed

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

        private void noiseReduce()
        {
            Median filter = new Median();
            preprocessedImg = filter.Apply(preprocessedImg);
        }

        private void thresholding()
        {
            OtsuThreshold filter = new OtsuThreshold();
            preprocessedImg = filter.Apply(preprocessedImg);
        }

        private void edgeDetection()
        {
            SobelEdgeDetector filter = new SobelEdgeDetector();
            filter.ApplyInPlace(preprocessedImg);
        }

        public void preprocess()
        {
            grayscale();
            noiseReduce();
            thresholding();
            edgeDetection();
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

        public Bitmap drawShape(Bitmap img)
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

            Rectangle rect = new Rectangle(0, 0, img.Width, img.Height);
            using (Graphics g = Graphics.FromImage(img))
            {
                g.DrawImage(shapeImg, rect);
            }
            return img;
        }

        public Bitmap drawColor(Bitmap img)
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

            Rectangle rect = new Rectangle(0, 0, img.Width, img.Height);
            using (Graphics g = Graphics.FromImage(img))
            {
                g.DrawImage(shapeImg, rect);
            }
            return img;
        }

        //Scaling and displaying the image, when not running the interface
        /*private void scaling(double scalar)
        {
            int height = (int)(scalar * orgImg.Height);
            int width = (int)(scalar * orgImg.Width);
            ResizeBilinear filter = new ResizeBilinear(width, height); //AForge filter to scale the image
            orgImg = filter.Apply(preprocessedImg);
        }

        public void displayImage(double scalar, string caption) //displaying the image using the OPENCV stuff
        {
            scaling(scalar);
            imgMatrix = new Image<Bgr, Byte>(orgImg);
            CvInvoke.Imshow(caption, imgMatrix);
            CvInvoke.WaitKey();
        }*/
    }
}