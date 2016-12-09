using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EdgeDetectionApp
{
    public partial class PopYes : Form
    {
        Bitmap image;
        Image2 Myimage;

        public PopYes(Bitmap image, Image2 Myimage)
        {
            this.Myimage = Myimage;
            this.image = image;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormEnd f = new FormEnd(Myimage.drawColor());
            f.Show();
            Hide();
        }
    }
}
