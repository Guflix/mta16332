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
        Image<Bgr,Byte> imgMatrix;
        Bitmap orgImg;
        Bitmap convertedImg;
        Bitmap croppedImg;
        Bitmap shapeImg;
        int shape;
        BlobDetector bd;
        Shapecheck sc;

        public Image(string img_path)
        {
            orgImg = AForge.Imaging.Image.FromFile("C:\\Github\\P3\\" + img_path + ".jpg"); //original image
        }
       
        public Bitmap convert(Bitmap img)
        {
            convertedImg = new Bitmap(img.Width, img.Height, PixelFormat.Format32bppArgb); //converts the image to bitmap - draws pixels on a blank bitmap
            using (Graphics g = Graphics.FromImage(convertedImg))
            {
                g.DrawImage(img, 0, 0);
            }
            return convertedImg;
        }

        public void scaling(double scalar)
        {
            int height = (int)(scalar * orgImg.Height);
            int width = (int)(scalar * orgImg.Width);
            ResizeBilinear filter = new ResizeBilinear(width, height); //AForge filter to scale the image
            orgImg = filter.Apply(orgImg);
        }
        
        public void crop() //crops the edge parts either on the right or left side (wide picture) or upper and downer side (tall picture)
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

        public void grayscale() //convers the image to grayscale, just a filter again
        {
            Grayscale filter = new Grayscale(0.299, 0.587, 0.114);
            croppedImg = filter.Apply(orgImg);
        }

        public void noiseReduce(int ksize) //filter, ksize - kernel for the gaussian filter,  
        {
            GaussianBlur filter = new GaussianBlur(1, ksize);
            croppedImg = filter.Apply(croppedImg);
        }

        public void hedgeDetection() //canny edge detection filter
        {
            CannyEdgeDetector filter = new CannyEdgeDetector();
            filter.ApplyInPlace(croppedImg);
        }

        public void thresholding() //finds threshold on its own
        {
            OtsuThreshold filter = new OtsuThreshold();
            croppedImg = filter.Apply(croppedImg);
        }

        public void spoopy() // makes the edges of the picture only 1 pixel wide
        {
            SimpleSkeletonization filter = new SimpleSkeletonization();
            filter.ApplyInPlace(croppedImg);
        }

        public void preprocess(double scalar, int ksize) //it just calls all the functions for preprocesing
        {
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
            bd = new BlobDetector(croppedImg);
            shapeImg = bd.blobDetection();
        }

        public void shapeDetect()
        {
            sc = new Shapecheck(shapeImg);
            shape = sc.whichShape();
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
                        if(shape == 1)
                            shapeImg.SetPixel(x, y, Color.Red);
                        else if (shape == 2)
                            shapeImg.SetPixel(x, y, Color.Yellow);
                        else if (shape == 3)
                            shapeImg.SetPixel(x, y, Color.Blue);
                        else
                            shapeImg.SetPixel(x, y, Color.Purple);
                    }                        
                }
            }
            int length = bd.biggestBlob.Count;

            using (Graphics g = Graphics.FromImage(orgImg))
            {
                g.DrawImage(shapeImg, 0, 0);
            }
        }

        public void displayImage() //displaying the image using the OPENCV stuff
        {
            Console.WriteLine("orgImg: " + orgImg.Width + " " + orgImg.Height);
            Console.WriteLine("shapeImg: " + shapeImg.Width + " " + shapeImg.Height);
            imgMatrix = new Image<Bgr, Byte>(orgImg);
            CvInvoke.Imshow("case!", imgMatrix);
        }
    }
}