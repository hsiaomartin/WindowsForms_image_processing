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
    public partial class Form_Enlarge : Form
    {
        Bitmap bitmap;
        public myPicture myPicture;
        Graphics g_Enlarge;
        public Form_Enlarge()
        {
            InitializeComponent();
            radioButton1.Text = "Duplication";
            radioButton2.Text = "Interpolation";
            radioButton1.Checked = true;
        }

        private void Form_Enlarge_VisibleChanged(object sender, EventArgs e)
        {
            bitmap = new Bitmap(myPicture.Width, myPicture.Height);
            g_Enlarge = Graphics.FromImage(bitmap);
            myPicture.draw(g_Enlarge);
            Text = "Scaling";
            pictureBox1.Image = bitmap;
        }
        private void set_pictureBox_size(PictureBox pictureBox, Bitmap bitmap)
        {
            pictureBox.Location =new Point (pictureBox.Location.X,radioButton2.Location.Y + radioButton2.Height);
            pictureBox.Width = bitmap.Width;
            pictureBox.Height = bitmap.Height;
        }



        private void button1_Click(object sender, EventArgs e)
        {
                Class_Zoom zoom = new Class_Zoom();
                Bitmap zoom_pic = bitmap;
                double d;
                bool valid = double.TryParse(textBox1.Text, out d);//check value is valid
                double value = valid ? d : 1.0;
                if (d == 0) d = 1;
                if (radioButton1.Checked)
                {
                    zoom_pic = zoom.zoomInImageNear(zoom_pic, value);
                }
                else if (radioButton2.Checked)
                {
                    zoom_pic = zoom.zoomInImageLine(zoom_pic, value);
                }
                set_pictureBox_size(pictureBox1, zoom_pic);
                pictureBox1.Image = zoom_pic;
            }
        }
    
}
