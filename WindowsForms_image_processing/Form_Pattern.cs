using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsForms_image_processing
{
    public partial class Form_Pattern : Form
    {
        public string filePath;
        myPicture my_Picture;
        Graphics gMyImg;
        Graphics g_Connected;
        Bitmap bitmap;
        int threshold = 0;
        int[,] connect_Table;
        bool isDetect = false;
        int[] histogramValue_Gray;
        double zoomOut = 0.2;
        public Form_Pattern()
        {
            InitializeComponent();
            //button1.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            //button5.Enabled = false;
            histogramValue_Gray = new int[256];

            Array.Clear(histogramValue_Gray, 0, histogramValue_Gray.Length);
        }
        public void connect_component(Graphics g, myPicture picture, int[,] table)
        {
            g.Clear(Color.White);
            int marker = 0;
            int[] connect_Mask = new int[9];
            Array.Clear(connect_Mask, 0, connect_Mask.Length);
            Dictionary<int, List<int>> pair = new Dictionary<int, List<int>>();
            //List<Pair> pairs = new List<Pair>();
            for (int Ycount = 0; Ycount < picture.Height; Ycount++)
            {
                for (int Xcount = 0; Xcount < picture.Width; Xcount++)
                {
                    //((Xcount - 1)>0 && (Ycount - 1)>0)
                    //((Xcount + 1)<picture.Width && (Ycount + 1)<picture.Height)
                    connect_Mask[0] = ((Xcount - 1) >= 0 && (Ycount - 1) >= 0) ? picture.my_Pixel[Xcount - 1, Ycount - 1].Gray : 0;
                    connect_Mask[1] = ((Ycount - 1) >= 0) ? picture.my_Pixel[Xcount, Ycount - 1].Gray : 0;
                    connect_Mask[2] = ((Xcount + 1) < picture.Width && (Ycount - 1) >= 0) ? picture.my_Pixel[Xcount + 1, Ycount - 1].Gray : 0;
                    connect_Mask[3] = ((Xcount - 1) >= 0) ? picture.my_Pixel[Xcount - 1, Ycount].Gray : 0;
                    connect_Mask[4] = picture.my_Pixel[Xcount, Ycount].Gray;
                    connect_Mask[5] = ((Xcount + 1) < picture.Width) ? picture.my_Pixel[Xcount + 1, Ycount].Gray : 0;
                    connect_Mask[6] = ((Xcount - 1) >= 0 && (Ycount + 1) < picture.Height) ? picture.my_Pixel[Xcount - 1, Ycount + 1].Gray : 0;
                    connect_Mask[7] = (Ycount + 1) < picture.Height ? picture.my_Pixel[Xcount, Ycount + 1].Gray : 0;
                    connect_Mask[8] = ((Xcount + 1) < picture.Width && (Ycount + 1) < picture.Height) ? picture.my_Pixel[Xcount + 1, Ycount + 1].Gray : 0;

                    for (int i = 0; i < connect_Mask.Length; i++)
                    {
                        if (connect_Mask[i] > threshold)
                            connect_Mask[i] = 255;
                        else
                            connect_Mask[i] = 0;
                    }


                    if (((Xcount - 1) >= 0 && (Ycount - 1) >= 0) && ((Xcount + 1) < picture.Width && (Ycount + 1) < picture.Height))
                    {
                        //check central not 0
                        if (connect_Mask[4] != 0)
                        {
                            int[] chk_Table = new int[4];
                            chk_Table[0] = table[Xcount - 1, Ycount - 1];//0
                            chk_Table[1] = table[Xcount, Ycount - 1];//1
                            chk_Table[2] = table[Xcount + 1, Ycount - 1];//2                           
                            chk_Table[3] = table[Xcount - 1, Ycount];//3
                            foreach (int chk in chk_Table)
                                if (chk != 0)
                                    table[Xcount, Ycount] = chk;

                            //make label pair
                            for (int i = 0; i < chk_Table.Length; i++)
                            {
                                for (int j = i + 1; j < chk_Table.Length; j++)
                                {
                                    if (chk_Table[i] != 0 && chk_Table[j] != 0)
                                        if (chk_Table[i] != chk_Table[j])
                                        {
                                            //pairs.Add(new Pair(chk_Table[i], chk_Table[j]));
                                            if (!pair.ContainsKey(chk_Table[i]))
                                            {
                                                pair.Add(chk_Table[i], new List<int>());
                                                pair[chk_Table[i]].Add(chk_Table[j]);
                                            }
                                            else
                                            {
                                                if (!pair[chk_Table[i]].Contains(chk_Table[j]))
                                                    pair[chk_Table[i]].Add(chk_Table[j]);
                                            }
                                        }
                                }
                            }

                            //if connected before , check new and connect to it
                            if (table[Xcount, Ycount] != 0)
                            {
                                if (connect_Mask[5] != 0)
                                    table[Xcount + 1, Ycount] = table[Xcount, Ycount];

                                if (connect_Mask[6] != 0)
                                    table[Xcount - 1, Ycount + 1] = table[Xcount, Ycount];

                                if (connect_Mask[7] != 0)
                                    table[Xcount, Ycount - 1] = table[Xcount, Ycount];

                                if (connect_Mask[8] != 0)
                                    table[Xcount + 1, Ycount + 1] = table[Xcount, Ycount];


                            }

                            //if new point ,assign a new marker
                            if (table[Xcount, Ycount] == 0)
                            {
                                marker++;
                                table[Xcount, Ycount] = marker;
                            }

                        }
                    }
                }
            }


            //draw color in marked area 
            Color[] color = new Color[marker + 1];
            var rand = new Random();
            for (int i = 1; i <= marker; i++)
            {
                color[i] = Color.FromArgb(rand.Next(128, 255), rand.Next(128, 255), rand.Next(128, 255));
            }

            //combine color of label pair
            for (int i = 0; i < pair.Count; i++)
            {
                for (int j = 0; j < pair.ElementAt(i).Value.Count; j++)
                    color[pair.ElementAt(i).Value[j]] = color[pair.ElementAt(i).Key];
                //richTextBox1.AppendText("pair " + i + " : " + pair.ElementAt(i).Key+" : " + string.Join(",",pair.ElementAt(i).Value) + " .\n");
            }

            for (int i = 0; i < pair.Count; i++)
            {
                for (int j = 0; j < pair.ElementAt(i).Value.Count; j++)
                    color[pair.ElementAt(i).Value[j]] = color[pair.ElementAt(i).Key];
                //richTextBox1.AppendText("pair " + i + " : " + pair.ElementAt(i).Key+" : " + string.Join(",",pair.ElementAt(i).Value) + " .\n");
            }

            //for(int i = 0;i <pairs.Count;i++)
            //    richTextBox1.AppendText("pair "+i +" : "+ pairs.ElementAt(i).ToString()  + " .\n");
            Color[] unique_Color = color.Distinct().ToArray();

            //draw color
            for (int Ycount = 0; Ycount < picture.Height; Ycount++)
            {
                for (int Xcount = 0; Xcount < picture.Width; Xcount++)
                {
                    for (int i = 1; i <= marker; i++)
                    {
                        if (i == table[Xcount, Ycount])
                        {
                            g.FillRectangle(new SolidBrush(color[i]), Xcount, Ycount, 1, 1);
                            my_Picture.my_Pixel[Xcount, Ycount].color = color[i];
                            break;
                        }

                    }
                }
            }


            richTextBox1.AppendText("There are " + (unique_Color.Length - 1) + " object.\n");
        }

        public void histogram_calc(myPicture picture, int[] histogram)
        {
            for (int Ycount = 0; Ycount < bitmap.Height; Ycount++)
            {
                for (int Xcount = 0; Xcount < bitmap.Width; Xcount++)
                {

                    histogram[picture.my_Pixel[Xcount, Ycount].Gray]++;
                }
            }
        }
        //find best threshold by Otsu
        public int get_Otsu_Thresholding(myPicture picture, int[] histogram)
        {
            //計算最小方差和

            int pixelNumb = picture.Height * picture.Width;
            int Otsu_Threshold;

            //store variances
            double[] variances = new double[256];


            //T = 0~255 ,determine variances
            for (int T = 0; T < histogram.Length; T++)
            {
                //两区域 pixel 个数，区域 pixel 值总和，区域平均数,区域权重;
                double area_pixel_total_1 = 0, area_pixel_total_2 = 0;
                double area_pixel_total_value_1 = 0, area_pixel_total_value_2 = 0;
                double area_aver1 = 0, area_aver2 = 0;
                double w1 = 0, w2 = 0;
                //area 1,2 variance
                double ft1 = 0, ft2 = 0;

                //0~T
                for (int i = 0; i < T; i++)
                {
                    area_pixel_total_1 += histogram[i];
                    area_pixel_total_value_1 += histogram[i] * i;
                }

                //T~max
                for (int j = T; j < variances.Length; j++)
                {
                    area_pixel_total_2 += histogram[j];
                    area_pixel_total_value_2 += histogram[j] * j;
                }

                //2 area of pixel 所佔比例
                w1 = area_pixel_total_1 / pixelNumb;
                w2 = area_pixel_total_2 / pixelNumb;

                //2 area of pixel value average
                area_aver1 = (area_pixel_total_1 == 0) ? 0 : (area_pixel_total_value_1 / area_pixel_total_1);
                area_aver2 = (area_pixel_total_2 == 0) ? 0 : (area_pixel_total_value_2 / area_pixel_total_2);

                // (Math.Pow((i - area_aver1), 2) 得出i值(當前gray value)與變異數的大小，
                // * histogram[i] 來得出跟變異數的量，若histogram[i]很小或是=0，影響ft值就小
                for (int i = 0; i < T; i++)
                {
                    ft1 += (Math.Pow((i - area_aver1), 2) * histogram[i]);
                }

                for (int j = T; j < 256; j++)
                {
                    ft2 += (Math.Pow((j - area_aver2), 2) * histogram[j]);
                }

                ft1 = (area_pixel_total_1 == 0) ? 0 : (ft1 / area_pixel_total_1);
                ft2 = (area_pixel_total_2 == 0) ? 0 : (ft2 / area_pixel_total_2);

                variances[T] = w1 * ft1 + w2 * ft2;
            }

            //find the minimum variance
            double min = variances[0];
            Otsu_Threshold = 0;
            for (int i = 1; i < variances.Length; i++)
            {
                if (variances[i] < min)
                {
                    min = variances[i];
                    Otsu_Threshold = i;
                }
            }
            return Otsu_Threshold;
        }

        public void drawMyImgData(myPicture picture, Graphics g)
        {
            //myPicture my_Picture;
            int w = picture.Width;
            int h = picture.Height;
            g.Clear(Color.Black);
            for (int Ycount = h - 1; Ycount >= 0; Ycount--)
            {
                for (int Xcount = 0; Xcount < w; Xcount++)
                {
                    int R = picture.my_Pixel[Xcount, Ycount].R;
                    int G = picture.my_Pixel[Xcount, Ycount].G;
                    int B = picture.my_Pixel[Xcount, Ycount].B;
                    g.FillRectangle(new SolidBrush(Color.FromArgb(R, G, B)), Xcount, Ycount, 1, 1);

                }
            }
        }

        private void Form_Pattern_Shown(object sender, EventArgs e)
        {
            Text = "Project";

            if (Path.GetExtension(filePath) == ".bmp")
            {
                read_Bitmap read_bitmap = new read_Bitmap(filePath);
                mySize sz = new mySize(read_bitmap.info_header.biWidth, read_bitmap.info_header.biHeight);
                myPixel[,] myImgData = read_bitmap.myRowData;

                my_Picture = Class_Zoom.zoomOut_Decimation(myImgData, new Size(sz.Width, sz.Height), zoomOut);
                myImgData = my_Picture.my_Pixel;
                sz = new mySize(my_Picture.Width, my_Picture.Height);               

                bitmap = new Bitmap(sz.Width, sz.Height);
                gMyImg = Graphics.FromImage(bitmap);
                drawMyImgData(my_Picture, gMyImg);
                pictureBox1.Image = bitmap;

                richTextBox1.AppendText("File : \n"+filePath+"\n");



            }
        }
        //gray
        private void button2_Click(object sender, EventArgs e)
        {
            my_Picture.draw(gMyImg, "gray");
            richTextBox1.AppendText("Gray level\n");
            pictureBox1.Image = bitmap;
            button2.Enabled = false;
            button1.Enabled = true;
        }

        public void drawImage(Graphics g,myPicture my_Picture, string gray, int threshold)
        {
            for (int Ycount = 0; Ycount < my_Picture.Height; Ycount++)
            {
                for (int Xcount = 0; Xcount < my_Picture.Width; Xcount++)
                {
                    int Gray = my_Picture.my_Pixel[Xcount, Ycount].Gray > threshold ? 255 : 0;
                    my_Picture.my_Pixel[Xcount, Ycount].Gray = Gray;
                    g.FillRectangle(new SolidBrush(Color.FromArgb(Gray, Gray, Gray)), Xcount, Ycount, 1, 1);
                }
            }
        }
        //threshold
        private void button1_Click(object sender, EventArgs e)
        {

            //image.draw(g_Connected);
            histogram_calc(my_Picture, histogramValue_Gray);
            threshold = get_Otsu_Thresholding(my_Picture, histogramValue_Gray);
            button1.Enabled = false;
            button3.Enabled = true;
            drawImage(gMyImg,my_Picture, "gray", threshold);
            richTextBox1.AppendText("Threshold : " + threshold + " .\n");
            pictureBox1.Image = bitmap;
        }


        //connected component
        private void button3_Click(object sender, EventArgs e)
        {
            connect_Table = new int[my_Picture.Width, my_Picture.Height];
            Array.Clear(connect_Table, 0, connect_Table.Length);

            for (int Ycount = 0; Ycount < my_Picture.Height; Ycount++)
            {
                for (int Xcount = 0; Xcount < my_Picture.Width; Xcount++)
                {
                    my_Picture.my_Pixel[Xcount, Ycount].color = Color.White;
                }
            }

            g_Connected = Graphics.FromImage(bitmap);
            connect_component(g_Connected, my_Picture, connect_Table);
            pictureBox1.Size = new Size(my_Picture.Width, my_Picture.Height);
            pictureBox1.Image = bitmap;
            button3.Enabled = false;
            button4.Enabled = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            isDetect = true;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (isDetect)
            {
                
                if(my_Picture.my_Pixel[e.X,e.Y].color != Color.White)
                {
                    Color color = my_Picture.my_Pixel[e.X, e.Y].color;
                    int area = 0;
                    for (int Ycount = 0; Ycount < my_Picture.Height; Ycount++)
                    {
                        for (int Xcount = 0; Xcount < my_Picture.Width; Xcount++)
                        {
                            if(my_Picture.my_Pixel[Xcount,Ycount].color == color)
                            {
                                area++;
                            }
                        }
                    }
                    MessageBox.Show("This object area : " + (int)(area / zoomOut) + " unit area");
                    richTextBox1.AppendText("This object area : " + (int)(area / zoomOut) + " unit area\n");
                }

                
            }
        }

        public void Gradient_Filter(Graphics g, myPicture picture)
        {
            int[] kernel = new int[] { -1, 0, 1, -2, 0, 2, -1, 0, 1 };
            int[] kernel_2 = new int[] { 1, 2, 1, 0, 0, 0, -1, -2, -1 };

            int[] maskGray = new int[9];

            for (int Ycount = 0; Ycount < bitmap.Height - 2; Ycount++)
            {
                for (int Xcount = 0; Xcount < bitmap.Width - 2; Xcount++)
                {
                    maskGray[0] = kernel[0] * picture.my_Pixel[Xcount, Ycount].Gray;
                    maskGray[1] = kernel[1] * picture.my_Pixel[Xcount, Ycount + 1].Gray;
                    maskGray[2] = kernel[2] * picture.my_Pixel[Xcount, Ycount + 2].Gray;
                    maskGray[3] = kernel[3] * picture.my_Pixel[(Xcount + 1), Ycount].Gray;
                    maskGray[4] = kernel[4] * picture.my_Pixel[(Xcount + 1), Ycount + 1].Gray;
                    maskGray[5] = kernel[5] * picture.my_Pixel[(Xcount + 1), Ycount + 2].Gray;
                    maskGray[6] = kernel[6] * picture.my_Pixel[(Xcount + 2), Ycount].Gray;
                    maskGray[7] = kernel[7] * picture.my_Pixel[(Xcount + 2), Ycount + 1].Gray;
                    maskGray[8] = kernel[8] * picture.my_Pixel[(Xcount + 2), Ycount + 2].Gray;
                    int meanGray_X = (maskGray[0] + maskGray[1] + maskGray[2] + maskGray[3] + maskGray[4] + maskGray[5] + maskGray[6] + maskGray[7] + maskGray[8]);

                    maskGray[0] = kernel_2[0] * picture.my_Pixel[Xcount, Ycount].Gray;
                    maskGray[1] = kernel_2[1] * picture.my_Pixel[Xcount, Ycount + 1].Gray;
                    maskGray[2] = kernel_2[2] * picture.my_Pixel[Xcount, Ycount + 2].Gray;
                    maskGray[3] = kernel_2[3] * picture.my_Pixel[(Xcount + 1), Ycount].Gray;
                    maskGray[4] = kernel_2[4] * picture.my_Pixel[(Xcount + 1), Ycount + 1].Gray;
                    maskGray[5] = kernel_2[5] * picture.my_Pixel[(Xcount + 1), Ycount + 2].Gray;
                    maskGray[6] = kernel_2[6] * picture.my_Pixel[(Xcount + 2), Ycount].Gray;
                    maskGray[7] = kernel_2[7] * picture.my_Pixel[(Xcount + 2), Ycount + 1].Gray;
                    maskGray[8] = kernel_2[8] * picture.my_Pixel[(Xcount + 2), Ycount + 2].Gray;
                    int meanGray_Y = (maskGray[0] + maskGray[1] + maskGray[2] + maskGray[3] + maskGray[4] + maskGray[5] + maskGray[6] + maskGray[7] + maskGray[8]);
                    int meanGray = Math.Abs(meanGray_X) + Math.Abs(meanGray_Y);
                    meanGray = meanGray > 0 ? (meanGray > 255 ? 255 : meanGray) : 0;

                    picture.my_Pixel[Xcount, Ycount].Gray = meanGray;
                    g.FillRectangle(new SolidBrush(Color.FromArgb(meanGray, meanGray, meanGray)), Xcount, Ycount, 1, 1);


                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Gradient_Filter(gMyImg, my_Picture);
            richTextBox1.AppendText("Sobel Filter \n");
        
            pictureBox1.Image = bitmap;
        }
    }
}
