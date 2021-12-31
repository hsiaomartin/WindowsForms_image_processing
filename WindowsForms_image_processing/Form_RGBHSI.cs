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
    public partial class Form_RGBHSI : Form
    {
        public Bitmap bitmap { get; set; }
        public myPixel[,] read_photo;
        Bitmap bitmap_R;
        Bitmap bitmap_G;
        Bitmap bitmap_B;
        Bitmap bitmap_Gray;
        Bitmap bitmap_Hue;
        Bitmap bitmap_Intensity;
        Bitmap bitmap_Saturation;

        private void Form_RGBHSI_Shown(object sender, EventArgs e)
        {
            bitmap_R    = new Bitmap(bitmap.Width, bitmap.Height);
            bitmap_G    = new Bitmap(bitmap.Width, bitmap.Height);
            bitmap_B    = new Bitmap(bitmap.Width, bitmap.Height);
            bitmap_Gray = new Bitmap(bitmap.Width, bitmap.Height);
            bitmap_Hue        = new Bitmap(bitmap.Width, bitmap.Height);
            bitmap_Intensity  = new Bitmap(bitmap.Width, bitmap.Height);
            bitmap_Saturation = new Bitmap(bitmap.Width, bitmap.Height);

            Graphics gR = Graphics.FromImage(bitmap_R);
            Graphics gG = Graphics.FromImage(bitmap_G);
            Graphics gB = Graphics.FromImage(bitmap_B);
            Graphics gGray = Graphics.FromImage(bitmap_Gray);
            Graphics g_Hue        = Graphics.FromImage(bitmap_Hue       );
            Graphics g_Intensity  = Graphics.FromImage(bitmap_Intensity );
            Graphics g_Saturation = Graphics.FromImage(bitmap_Saturation);

            for (int Ycount = 0; Ycount < bitmap.Height; Ycount++)
            {
                for (int Xcount = 0; Xcount < bitmap.Width; Xcount++)
                {
                    int R = read_photo[ Xcount, Ycount].R;
                    int G = read_photo[ Xcount, Ycount].G;
                    int B = read_photo[ Xcount, Ycount].B;

                    gR.FillRectangle(new SolidBrush(Color.FromArgb(R, 0, 0)), Xcount,Ycount,  1, 1);
                    gG.FillRectangle(new SolidBrush(Color.FromArgb(0, G, 0)), Xcount,Ycount,  1, 1);
                    gB.FillRectangle(new SolidBrush(Color.FromArgb(0, 0, B)), Xcount,Ycount,  1, 1);

                    //int Y = (int)(R * 0.299 + G * 0.587 + B * 0.114);
                    //int I = (int)(R * 0.596 - G * 0.274 - B * 0.322);
                    //int Q = (int)(R * 0.211 - G * 0.523 + B * 0.312);
                    //I = I > 0 ? I : -I;
                    //Q =Q>0?Q:-Q;
                    //g_Hue       .FillRectangle(new SolidBrush(Color.FromArgb(Y, Y, Y)),  Xcount,Ycount, 1, 1);
                    //g_Intensity .FillRectangle(new SolidBrush(Color.FromArgb(I, I, I)),  Xcount,Ycount, 1, 1);
                    //g_Saturation.FillRectangle(new SolidBrush(Color.FromArgb(Q, Q, Q)),  Xcount,Ycount, 1, 1);

                    int h = (byte)((read_photo[ Xcount, Ycount].hue*180/3.14));
                    int s = (byte)(Math.Abs(read_photo[ Xcount, Ycount].saturation)*255);
                    int i = (int)(read_photo[Xcount, Ycount].intensity);

                    g_Hue.FillRectangle(new SolidBrush(Color.FromArgb(h, h, h)), Xcount, Ycount, 1, 1);
                    g_Intensity.FillRectangle(new SolidBrush(Color.FromArgb(i, i, i)), Xcount, Ycount, 1, 1);
                    g_Saturation.FillRectangle(new SolidBrush(Color.FromArgb(s, s, s)), Xcount, Ycount, 1, 1);


                    int gray = (int)(R*0.299+G*0.587+B*0.114);
                    gGray.FillRectangle(new SolidBrush(Color.FromArgb(gray, gray, gray)),  Xcount,Ycount, 1, 1);
                    
                }
            }
            //pictureBox1.Size = new Size(bitmap.Width, bitmap.Height);
            pictureBox1.Image = bitmap_R;
            pictureBox2.Image = bitmap_G;
            pictureBox3.Image = bitmap_B;
            pictureBox4.Image = bitmap_Hue       ;
            pictureBox5.Image = bitmap_Intensity ;
            pictureBox6.Image = bitmap_Saturation;
            pictureBox7.Image = bitmap_Gray;

        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {

        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
        }

        public Form_RGBHSI()
        {
            InitializeComponent();

        }


    }
}
