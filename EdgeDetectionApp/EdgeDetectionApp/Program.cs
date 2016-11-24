namespace EdgeDetectionApp
{
    class ProgramMain
    {
        static void Main(string[] args)
        {
            Image test = new Image("2");
            test.preprocess(11);
            test.blobDetect();            
            test.shapeDetect();
            test.draw();
            test.displayImage(0.3, "shape");
        }
    }
}
