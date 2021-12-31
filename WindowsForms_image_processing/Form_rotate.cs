using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsForms_image_processing
{
    public partial class Form_rotate : Form
    {
        public Bitmap bitmap { set; get; }
        public Bitmap rotate_pic;
        public Bitmap rotate_pic2;
        public Form_rotate()
        {
            InitializeComponent();
        }

        private void Form_rotate_VisibleChanged(object sender, EventArgs e)
        {
            pictureBox1.Image = bitmap;
            pictureBox2.Image = bitmap;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            double angle = trackBar1.Value*10;
            label1.Text = "Distortion rotate " + angle + "°";
            label2.Text = "No distortion rotate " + angle + "°";
            rotate_pic = Class_rotate.rotateImageDis(bitmap, angle);
            rotate_pic2 = Class_rotate.rotareImageOri(bitmap, angle);
            pictureBox1.Image = rotate_pic;
            pictureBox2.Image = rotate_pic2;
        }
    }
}
