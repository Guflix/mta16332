using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EdgeDetectionApp
{
    class ProgramMain
    {
        [STAThread]
        static void Main(string[] args)
        {
        //    Image2 test = new Image2("C:\\Github\\P3\\Kids\\perf2.jpg");
        //    test.preprocess(11);
        //    test.blobDetect();
        //    test.shapeDetect(0);
        //    test.draw(test.orgImg);
        //    test.displayImage(0.3, "shape");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormIntro());
        }
    }
}
