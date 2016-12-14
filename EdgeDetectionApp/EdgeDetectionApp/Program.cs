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
            //Used during testing to show the processed image without the interface
            /*Image2 test = new Image2("C:\\Github\\P3\\box.jpg");
            test.preprocess();
            test.blobDetect();
            test.shapeDetect(0);
            test.drawShape(test.orgImg);
            test.drawColor(test.orgImg);
            test.displayImage(0.3, "shape");*/

            //Runs the interface
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormIntro());
        }
    }
}
