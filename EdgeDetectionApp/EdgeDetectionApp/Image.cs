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
        Shapecheck sc;

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

        public Bitmap draw(Bitmap img) //drawing colours depending on shapes
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

            Rectangle rect = new Rectangle(0, 0, img.Width, img.Height);
            using (Graphics g = Graphics.FromImage(img))
            {
                g.DrawImage(shapeImg, rect);
            }
            return img;
        }

        //private void scaling(double scalar)
        //{
        //    int height = (int)(scalar * orgImg.Height);
        //    int width = (int)(scalar * orgImg.Width);
        //    ResizeBilinear filter = new ResizeBilinear(width, height); //AForge filter to scale the image
        //    orgImg = filter.Apply(orgImg);
        //}

        //public void displayImage(double scalar, string caption) //displaying the image using the OPENCV stuff
        //{
        //    scaling(scalar);
        //    imgMatrix = new Image<Bgr, Byte>(orgImg);
        //    CvInvoke.Imshow(caption, imgMatrix);
        //    CvInvoke.WaitKey();
        //}
    }
}