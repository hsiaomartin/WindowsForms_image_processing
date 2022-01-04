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
    public partial class Form_Slicing : Form
    {
        int straight = 0, gray = 1;
        public Bitmap bitmap { get; set; }
        public myPixel[,] read_photo;

        Bitmap[] bitmap_Slicing;
        Graphics[] g_Slicing;
        public Form_Slicing()
        {
            InitializeComponent();
        }
        public void slicing(int method)
        {                       
              
            for (int Ycount = 0; Ycount < bitmap.Height; Ycount++)
            {
                for (int Xcount = 0; Xcount < bitmap.Width; Xcount++)
                {
                    int R = read_photo[Xcount, Ycount].R;
                    int G = read_photo[Xcount, Ycount].G;
                    int B = read_photo[Xcount, Ycount].B;
                    int Gray = (int)(R * 0.299 + G * 0.587 + B * 0.114);
                if(method == straight)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        //value = 11110000 & 0000100 > 0 ? 1 : 0
                        int slicing = ((Gray&(1<<i))>0?1:0)*255;
                        g_Slicing[i].FillRectangle(new SolidBrush(Color.FromArgb(slicing, slicing, slicing)),  Xcount,Ycount, 1, 1);
                    }
                }
                else if(method == gray)
                {
                    int[] slicing = new int[8];
                    int[] b = new int[8];
                    //b'7 = "1"1110000 & "1"000000 >0 ? 1 : 0
                    b[7] = ((Gray & (1 << 7)) > 0 ? 1 : 0);
                    slicing[7] = b[7];

                    for (int i = 6; i >= 0; i--)
                    {
                        b[i] = ((Gray & (1 << i)) > 0 ? 1 : 0);
                        slicing[i] =(b[i + 1] ^ b[i])*255;
                    }
                    slicing[7] *= 255;
                    for (int i = 0; i < 8; i++)
                    {
                        g_Slicing[i].FillRectangle(new SolidBrush(Color.FromArgb(slicing[i], slicing[i], slicing[i])),  Xcount,Ycount, 1, 1);
                    }
                }

                
                }
            }

        }

        private void Form_Slicing_Shown(object sender, EventArgs e)
        {
            Text = "Image Slicing";
            slicing(straight);
            pictureBox1.Image = bitmap_Slicing[0];
            pictureBox2.Image = bitmap_Slicing[1];
            pictureBox3.Image = bitmap_Slicing[2];
            pictureBox4.Image = bitmap_Slicing[3];
            pictureBox5.Image = bitmap_Slicing[4];
            pictureBox6.Image = bitmap_Slicing[5];
            pictureBox7.Image = bitmap_Slicing[6];
            pictureBox8.Image = bitmap_Slicing[7];
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            slicing(straight);
            pictureBox1.Image = bitmap_Slicing[0];
            pictureBox2.Image = bitmap_Slicing[1];
            pictureBox3.Image = bitmap_Slicing[2];
            pictureBox4.Image = bitmap_Slicing[3];
            pictureBox5.Image = bitmap_Slicing[4];
            pictureBox6.Image = bitmap_Slicing[5];
            pictureBox7.Image = bitmap_Slicing[6];
            pictureBox8.Image = bitmap_Slicing[7];
        }

        private void Form_Slicing_Load(object sender, EventArgs e)
        {
            bitmap_Slicing = new Bitmap[8];
            g_Slicing = new Graphics[8];
            for (int i = 0; i < 8; i++)
            {
                bitmap_Slicing[i] = new Bitmap(bitmap.Width, bitmap.Height);
                g_Slicing[i] = Graphics.FromImage(bitmap_Slicing[i]);
                g_Slicing[i].Clear(Color.Black);
            }


            radioButton1.Text = "straight";
            radioButton2.Text = "gray";
            radioButton1.Checked = true;
            radioButton2.Checked = false;
            label1.Text = "bit 0";
            label2.Text = "bit 1";
            label3.Text = "bit 2";
            label4.Text = "bit 3";
            label5.Text = "bit 4";
            label6.Text = "bit 5";
            label7.Text = "bit 6";
            label8.Text = "bit 7";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            slicing(gray);
            pictureBox1.Image = bitmap_Slicing[0];
            pictureBox2.Image = bitmap_Slicing[1];
            pictureBox3.Image = bitmap_Slicing[2];
            pictureBox4.Image = bitmap_Slicing[3];
            pictureBox5.Image = bitmap_Slicing[4];
            pictureBox6.Image = bitmap_Slicing[5];
            pictureBox7.Image = bitmap_Slicing[6];
            pictureBox8.Image = bitmap_Slicing[7];
        }
    }
}
