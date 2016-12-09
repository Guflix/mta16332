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
            Image2 test = new Image2("C:\\Github\\P3\\Kids\\7.jpg");
            test.preprocess();
            test.blobDetect();
            test.shapeDetect(0);
            test.drawShape();
            test.drawColor();
            test.displayImage(0.3, "shape");

            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new FormIntro());
        }
    }
}
