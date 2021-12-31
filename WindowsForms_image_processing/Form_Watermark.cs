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
    public partial class Form_Watermark : Form
    {
        public Bitmap bitmap { get; set; }
        public Bitmap watermark { get; set; }
        public Bitmap resultImage { get; set; }
        readMidResource read_wartermark;
        Graphics g_Slicing;

        public myPixel[,] read_photo;
        myPixel[,] modify_photo;

        mySize sz;
        public Form_Watermark()
        {
            InitializeComponent();
        }
        public void slicing(int layer)
        {
            int count = 0;
            for (int Ycount = 0; Ycount < bitmap.Height; Ycount++)
            {
                for (int Xcount = 0; Xcount < bitmap.Width; Xcount++)
                {
                    int R = read_photo[Xcount, Ycount].R;
                    int G = read_photo[Xcount, Ycount].G;
                    int B = read_photo[Xcount, Ycount].B;
                    //int Gray = (int)(R * 0.299 + G * 0.587 + B * 0.114);

                    int watermark_R = read_wartermark.myRowData[Xcount, Ycount].R & (1 << layer);
                    int watermark_G = read_wartermark.myRowData[Xcount, Ycount].G & (1 << layer);
                    int watermark_B = read_wartermark.myRowData[Xcount, Ycount].B & (1 << layer);

                    //value = 11110000 & 0000100 > 0 ? 1 : 0
                    int slicing_R = (watermark_R > 0 ? R|watermark_R : R&watermark_R) ;
                    int slicing_G = (watermark_G > 0 ? G|watermark_G : G&watermark_G) ;
                    int slicing_B = (watermark_B > 0 ? B|watermark_B : B&watermark_B) ;
                    modify_photo[Xcount, Ycount] = new myPixel(slicing_R, slicing_G, slicing_B);
                    g_Slicing.FillRectangle(new SolidBrush(Color.FromArgb(slicing_R, slicing_G, slicing_B)),  Xcount,Ycount, 1, 1);

                    count++;
                }
            }

        }
        private void button1_Click(object sender, EventArgs e)
        {
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
            if (filePath != "")
            {
                read_wartermark = new readMidResource(filePath);
                watermark = read_wartermark.getDecoImage;
                pictureBox2.Image = watermark;
                label2.Location = new Point(label2.Location.X, pictureBox2.Height + pictureBox2.Location.Y);
                radioButton1.Enabled = true;
                radioButton2.Enabled = true;
                radioButton3.Enabled = true;
                radioButton4.Enabled = true;
                radioButton5.Enabled = true;
                radioButton6.Enabled = true;
                radioButton7.Enabled = true;
                radioButton8.Enabled = true;
                radioButton9.Enabled = true;
            }

        }

        private void Form_Watermark_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = bitmap;
            label1.Location = new Point(label1.Location.X , pictureBox1.Height + pictureBox1.Location.Y);
            radioButton9.Checked = true;
            radioButton1.Text = "Bit 0";
            radioButton2.Text = "Bit 1";
            radioButton3.Text = "Bit 2";
            radioButton4.Text = "Bit 3";
            radioButton5.Text = "Bit 4";
            radioButton6.Text = "Bit 5";
            radioButton7.Text = "Bit 6";
            radioButton8.Text = "Bit 7";
            radioButton9.Text = "None";
            resultImage = new Bitmap(bitmap.Width, bitmap.Height);

            g_Slicing = Graphics.FromImage(resultImage);

            radioButton1.Enabled = false;
            radioButton2.Enabled = false;
            radioButton3.Enabled = false;
            radioButton4.Enabled = false;
            radioButton5.Enabled = false;
            radioButton6.Enabled = false;
            radioButton7.Enabled = false;
            radioButton8.Enabled = false;
            radioButton9.Enabled = false;
            modify_photo = new myPixel[bitmap.Width, bitmap.Height];
            sz = new mySize(bitmap.Width, bitmap.Height);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            slicing(0);
            label3.Text = "Result SNR : " + Class_img_basic.SNR(read_photo, modify_photo, sz);
            pictureBox3.Image = resultImage;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            slicing(1);
            label3.Text = "Result SNR : " + Class_img_basic.SNR(read_photo, modify_photo, sz);
            pictureBox3.Image = resultImage;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            slicing(2);
            label3.Text = "Result SNR : " + Class_img_basic.SNR(read_photo, modify_photo, sz);
            pictureBox3.Image = resultImage;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            slicing(3);
            label3.Text = "Result SNR : " + Class_img_basic.SNR(read_photo, modify_photo, sz);
            pictureBox3.Image = resultImage;
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            slicing(4);
            label3.Text = "Result SNR : " + Class_img_basic.SNR(read_photo, modify_photo, sz);
            pictureBox3.Image = resultImage;
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            slicing(5);
            label3.Text = "Result SNR : " + Class_img_basic.SNR(read_photo, modify_photo, sz);
            pictureBox3.Image = resultImage;
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            slicing(6);
            label3.Text = "Result SNR : " + Class_img_basic.SNR(read_photo, modify_photo, sz);
            pictureBox3.Image = resultImage;
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            slicing(7);
            label3.Text = "Result SNR : " + Class_img_basic.SNR(read_photo, modify_photo, sz);
            pictureBox3.Image = resultImage;
        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            label3.Text = "Result ";
            pictureBox3.Image = bitmap;
        }
    }
}
