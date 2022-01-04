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
            Text = "Image Rotate";
            pictureBox1.Image = bitmap;
            pictureBox2.Image = bitmap;
            pictureBox1.Size = new Size(bitmap.Size.Width, bitmap.Size.Height);
            pictureBox2.Size = new Size(bitmap.Size.Width, bitmap.Size.Height);
            pictureBox2.Location = new Point(pictureBox1.Location.X + pictureBox1.Width + 10, pictureBox1.Location.Y);
            label1.Text = "Distortion rotate " + 0 + "°";
            label2.Text = "No distortion rotate " + 0 + "°";
            label1.Location = new Point(pictureBox1.Location.X, pictureBox1.Location.Y + pictureBox1.Height + 10);
            label2.Location = new Point(pictureBox2.Location.X, pictureBox2.Location.Y + pictureBox2.Height + 10);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            double angle = trackBar1.Value*10;
            label1.Text = "Distortion rotate " + angle + "°";
            label2.Text = "No distortion rotate " + angle + "°";
            rotate_pic = Class_rotate.rotateImageDis(bitmap, angle);
            rotate_pic2 = Class_rotate.rotareImageOri(bitmap, angle);

            pictureBox1.Image = rotate_pic;

            pictureBox1.Size = new Size(rotate_pic.Size.Width, rotate_pic.Size.Height);
            label1.Location = new Point(pictureBox1.Location.X, pictureBox1.Location.Y + pictureBox1.Height+10);

            pictureBox2.Image = rotate_pic2;
            pictureBox2.Location = new Point(pictureBox1.Location.X + pictureBox1.Width+10, pictureBox1.Location.Y);
            pictureBox2.Size = new Size(rotate_pic2.Size.Width, rotate_pic2.Size.Height);
            label2.Location = new Point(pictureBox2.Location.X, pictureBox2.Location.Y + pictureBox2.Height+10);
        }
    }
}
