﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Math.Geometry;

namespace EdgeDetectionApp
{
    public class BlobDetector
    {
        Bitmap img;
        Bitmap img2;
        Color white = Color.White;
        
        public List<List<IntPoint>> blobs;
        public List<IntPoint> shapeBlob;
        public int boxWidth, boxHeight, minX, minY, maxX, maxY;

        public BlobDetector(Bitmap img)
        {
            this.img = img;
            img2 = img;
        }

        public void blobDetection()
        {
            blobs = new List<List<IntPoint>>();
            bool[,] burned = new bool[img.Width, img.Height];
            for (int x = 0; x < img.Width; x++)
            {
                for (int y = 0; y < img.Height; y++)
                {
                    Color color = img.GetPixel(x, y);
                    if (color.ToArgb() == white.ToArgb() && !burned[x, y])
                    {
                        Queue<IntPoint> q = new Queue<IntPoint>();
                        q.Enqueue(new IntPoint(x, y));

                        List<IntPoint> blob = new List<IntPoint>();
                        while (q.Count > 0)
                        {
                            IntPoint p = q.Dequeue();
                            if (p.X >= 0 && p.X < img.Width && p.Y >= 0 && p.Y < img.Height)
                            {
                                Color col = img.GetPixel(p.X, p.Y);
                                if (!burned[p.X, p.Y] && col.ToArgb() == white.ToArgb())
                                {
                                    burned[p.X, p.Y] = true;
                                    blob.Add(p);

                                    q.Enqueue(new IntPoint(p.X + 1, p.Y));
                                    q.Enqueue(new IntPoint(p.X, p.Y + 1));
                                    q.Enqueue(new IntPoint(p.X - 1, p.Y));
                                    q.Enqueue(new IntPoint(p.X, p.Y - 1));
                                    q.Enqueue(new IntPoint(p.X + 1, p.Y + 1));
                                    q.Enqueue(new IntPoint(p.X - 1, p.Y - 1));
                                    q.Enqueue(new IntPoint(p.X + 1, p.Y - 1));
                                    q.Enqueue(new IntPoint(p.X - 1, p.Y + 1));
                                }
                            }
                        }
                        blobs.Add(blob);
                        sort();
                    }
                }
            }
        }

        private void sort()
        {
            for (int i = 0; i < blobs.Count - 1; i++)
            {
                int j = i + 1;
                while (j > 0)
                {
                    if (blobs[j - 1].Count < blobs[j].Count)
                    {
                        List<IntPoint> temp = blobs[j - 1];
                        blobs[j - 1] = blobs[j];
                        blobs[j] = temp;
                    }
                    j--;
                }
            }
        }

        public Bitmap drawConvexHull(int blobNo)
        {
            shapeBlob = blobs[blobNo];
            IConvexHullAlgorithm hullFinder = new GrahamConvexHull();
            List<IntPoint> hull = hullFinder.FindHull(shapeBlob);

            Bitmap blobImage = new Bitmap(img2.Width, img2.Height, PixelFormat.Format24bppRgb);
            BitmapData shapeImg = blobImage.LockBits(new Rectangle(0, 0, img2.Width, img2.Height), ImageLockMode.ReadWrite, blobImage.PixelFormat);
            Drawing.Polygon(shapeImg, hull, Color.White);
            blobImage.UnlockBits(shapeImg);

            return blobImage;
        }

        private int MinX()
        {
            minX = img.Width;
            foreach (IntPoint p in shapeBlob)
            {
                if (p.X < minX)
                {
                    minX = p.X;
                }
            }
            return minX;
        }

        private int MinY()
        {
            minY = img.Height;
            foreach (IntPoint p in shapeBlob)
            {
                if (p.Y < minY)
                {
                    minY = p.Y;
                }
            }
            return minY;
        }

        private int MaxX()
        {
            maxX = 0;
            foreach (IntPoint p in shapeBlob)
            {
                if (p.X > maxX)
                {
                    maxX = p.X;
                }
            }
            return maxX;
        }

        private int MaxY()
        {
            maxY = 0;
            foreach (IntPoint p in shapeBlob)
            {
                if (p.Y > maxY)
                {
                    maxY = p.Y;
                }
            }
            return maxY;
        }

        public void boundingBoxSize()
        {
            boxWidth = MaxX() - MinX();
            boxHeight = MaxY() - MinY();
        }
    }
}
