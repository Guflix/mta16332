namespace EdgeDetectionApp
{
    class ProgramMain
    {
        static void Main(string[] args)
        {
            Image test = new Image("triangle1");
            test.preprocess(0.3, 11);
            test.blobDetect();
            test.shapeDetect();
            test.draw();
            test.displayImage();
        }
    }
}
