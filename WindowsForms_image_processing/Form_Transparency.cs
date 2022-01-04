using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WindowsForms_image_processing
{
    public partial class Form_Transparency : Form
    {
        public Bitmap bitmap { get; set; }
        public Bitmap bitmap_B { get; set; }
        public Bitmap resultImage { get; set; }
        readMidResource read_Image_B;
        Graphics g_Transparency;

        public myPixel[,] read_photo;
        public myPixel[,] result_Pixel_Data;
        public Form_Transparency()
        {
            InitializeComponent();
        }

        public string getFilePath() {
            var fileContent = string.Empty;
            var filePath = string.Empty;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                //openFileDialog.InitialDirectory = filePath;
                openFileDialog.Filter = "image files (*.pcx)|*.pcx";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;

                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        fileContent = reader.ReadToEnd();
                    }
                }
            }
            return filePath;
        }

        public void alphaImage(int alpha)
        {

            for (int Ycount = 0; Ycount < bitmap.Height; Ycount++)
            {
                for (int Xcount = 0; Xcount < bitmap.Width; Xcount++)
                {
                    int R = read_photo[Xcount,Ycount].R;
                    int G = read_photo[Xcount,Ycount].G;
                    int B = read_photo[Xcount, Ycount].B;
                    //int Gray = (int)(R * 0.299 + G * 0.587 + B * 0.114);

                    int alpha_R = (int)(read_Image_B.myRowData[Xcount,Ycount].R *((alpha / 100.0))  + R * (1 - (alpha / 100.0)));
                    int alpha_G = (int)(read_Image_B.myRowData[Xcount,Ycount].G *((alpha / 100.0))  + G * (1 - (alpha / 100.0)));
                    int alpha_B = (int)(read_Image_B.myRowData[Xcount, Ycount].B *((alpha / 100.0))  + B * (1 - (alpha / 100.0)));

                    result_Pixel_Data[Xcount, Ycount] = new myPixel(alpha_R,alpha_G,alpha_B);
                    //Console.WriteLine(result_Pixel_Data[Xcount, Ycount].pixel_Value+" ");

                    g_Transparency.FillRectangle(new SolidBrush(Color.FromArgb(alpha_R, alpha_G, alpha_B)),  Xcount,Ycount, 1, 1);


                }
            }

        }
        private void button1_Click(object sender, EventArgs e)
        {
            string filePath = getFilePath();
            if (filePath != "")
            {
                read_Image_B = new readMidResource(filePath);
                bitmap_B = read_Image_B.getDecoImage;
                pictureBox2.Image = bitmap_B;
                label2.Location = new Point(label2.Location.X, pictureBox2.Height + pictureBox2.Location.Y);
                trackBar1.Enabled = true;
                alphaImage(trackBar1.Value);
                pictureBox3.Image = resultImage;
                label8.Text = "SNR :" + Class_img_basic.SNR(read_photo, result_Pixel_Data, new mySize(bitmap.Width, bitmap.Height));
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            alphaImage(trackBar1.Value);
            pictureBox3.Image = resultImage;
            label4.Text = (100 - trackBar1.Value) + " %";
            label5.Text = trackBar1.Value + " %";
            label8.Text = "SNR : " + Class_img_basic.SNR(read_photo, result_Pixel_Data, new mySize(bitmap.Width, bitmap.Height));
        }

        private void Form_Transparency_Load(object sender, EventArgs e)
        {
            result_Pixel_Data = new myPixel[bitmap.Width, bitmap.Height];
            pictureBox1.Image = bitmap;
            label1.Location = new Point(label1.Location.X, pictureBox1.Height + pictureBox1.Location.Y);
            resultImage = new Bitmap(bitmap.Width, bitmap.Height);
            g_Transparency = Graphics.FromImage(resultImage);
            pictureBox3.Image = bitmap;
            trackBar1.Enabled = false;
            label4.Text = (100 - trackBar1.Value) + " %";
            label5.Text = trackBar1.Value + " %";
            
        }

        private void Form_Transparency_Shown(object sender, EventArgs e)
        {
            Text = "Image Transparency";
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }
    }
}
