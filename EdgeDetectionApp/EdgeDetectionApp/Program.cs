namespace EdgeDetectionApp
{
    class ProgramMain
    {
        static void Main(string[] args)
        {
            Image test = new Image("8");
            test.preprocess(11);
            test.blobDetect();            
            test.shapeDetect();
            test.draw();
            test.displayImage(0.3, "shape");
        }
    }
}
