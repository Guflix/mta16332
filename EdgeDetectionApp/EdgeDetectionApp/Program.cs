using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EdgeDetectionApp
{
    class ProgramMain
    {
        static void Main(string[] args)
        {
            //Image2 test = new Image2("8");
            //test.preprocess(11);
            //test.blobDetect();            
            //test.shapeDetect();
            //test.draw();
            //test.displayImage(0.3, "shape");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormEdge());
        }
    }
}
