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
    public partial class Form_Connected_Component : Form
    {
        public myPicture image;
        Graphics g_Connected;
        Bitmap bitmap;
        int threshold = 0;
        int[,] connect_Table;

        int[] histogramValue_Gray;
        public Form_Connected_Component()
        {
            InitializeComponent();
            histogramValue_Gray = new int[256];

            Array.Clear(histogramValue_Gray, 0, histogramValue_Gray.Length);
        }
        public void connect_component(Graphics g,myPicture picture,int[,] table)
        {
            g.Clear(Color.White);
            int marker = 0;
            int[] connect_Mask = new int[9];
            Array.Clear(connect_Mask, 0, connect_Mask.Length);
            Dictionary<int, int> pair = new Dictionary<int, int>();

            for (int Ycount = 0; Ycount < picture.Height; Ycount++)
            {
                for (int Xcount = 0; Xcount < picture.Width; Xcount++)
                {
                    //((Xcount - 1)>0 && (Ycount - 1)>0)
                    //((Xcount + 1)<picture.Width && (Ycount + 1)<picture.Height)
                    connect_Mask[0] = ((Xcount - 1) >= 0 && (Ycount - 1) >= 0)? picture.my_Pixel[Xcount-1, Ycount-1].Gray:0;
                    connect_Mask[1] = ( (Ycount - 1) >= 0) ? picture.my_Pixel[Xcount, Ycount-1].Gray:0;
                    connect_Mask[2] = ((Xcount + 1) < picture.Width && (Ycount - 1) >= 0) ? picture.my_Pixel[Xcount+1, Ycount-1].Gray:0;
                    connect_Mask[3] = ((Xcount - 1) >= 0 )?picture.my_Pixel[Xcount-1, Ycount].Gray:0;
                    connect_Mask[4] = picture.my_Pixel[Xcount, Ycount].Gray;
                    connect_Mask[5] = ((Xcount + 1) < picture.Width)?picture.my_Pixel[Xcount+1, Ycount].Gray:0;
                    connect_Mask[6] = ((Xcount - 1) >= 0 && (Ycount + 1) < picture.Height) ?picture.my_Pixel[Xcount-1, Ycount+1].Gray:0;
                    connect_Mask[7] = (Ycount + 1) < picture.Height ? picture.my_Pixel[Xcount, Ycount+1].Gray:0;
                    connect_Mask[8] = ((Xcount + 1) < picture.Width && (Ycount + 1) < picture.Height)?picture.my_Pixel[Xcount+1, Ycount+1].Gray:0;

                    for(int i = 0; i < connect_Mask.Length; i++)
                    {
                        if (connect_Mask[i] > threshold)
                            connect_Mask[i] = 255;
                        else
                            connect_Mask[i] = 0;
                    }


                    if(((Xcount - 1) >= 0 && (Ycount - 1) >= 0) && ((Xcount + 1) < picture.Width && (Ycount + 1) < picture.Height))
                    {
                        //check central not 0
                        if(connect_Mask[4] != 0)
                        {
                            int[] chk_Table = new int[4];
                            chk_Table[0] = table[Xcount - 1, Ycount - 1];//0
                            chk_Table[1] = table[Xcount , Ycount - 1];//1
                            chk_Table[2] = table[Xcount+1 , Ycount - 1];//2                           
                            chk_Table[3] = table[Xcount - 1, Ycount ];//3
                            foreach(int chk in chk_Table)
                                if(chk!=0)
                                    table[Xcount, Ycount] = chk;

                            //make label pair
                            for(int i = 0; i < chk_Table.Length; i++)
                            {
                                for(int j = i+1; j < chk_Table.Length; j++)
                                {
                                    if (chk_Table[i] != 0 && chk_Table[j] != 0)
                                        if (chk_Table[i] != chk_Table[j])
                                        {
                                            if(!pair.ContainsKey(chk_Table[i]))
                                                pair.Add(chk_Table[i], chk_Table[j]);
                                        }
                                }
                            }

                            //if connected before , check new and connect to it
                            if (table[Xcount, Ycount] != 0)
                            {
                                if (connect_Mask[5] != 0)
                                    table[Xcount + 1, Ycount ] = table[Xcount, Ycount];

                                if (connect_Mask[6] != 0)
                                    table[Xcount - 1, Ycount + 1] = table[Xcount, Ycount];

                                if (connect_Mask[7] != 0)
                                    table[Xcount, Ycount - 1] = table[Xcount, Ycount];

                                if (connect_Mask[8] != 0)
                                    table[Xcount + 1, Ycount+1] = table[Xcount, Ycount];
                                

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
            Color []color = new Color[marker+1];
            var rand = new Random();
            for(int i = 1;i <= marker; i++)
            {
                color[i] = Color.FromArgb(rand.Next(128, 255), rand.Next(128, 255), rand.Next(128, 255));
            }

            //combine color of label pair
            for(int i = 0; i < pair.Count; i++)
            {
                color[pair.ElementAt(i).Key] = color[pair.ElementAt(i).Value];
            }

            Color[] unique_Color = color.Distinct().ToArray();
            
            //draw color
            for (int Ycount = 0; Ycount < picture.Height; Ycount++)
            {
                for (int Xcount = 0; Xcount < picture.Width; Xcount++)
                {
                    for(int i = 1;i <= marker; i++)
                    {
                        if(i == table[Xcount, Ycount])
                        {
                            g.FillRectangle(new SolidBrush(color[i]), Xcount, Ycount, 1, 1);
                            break;
                        }

                    }                    
                }
            }

            richTextBox1.AppendText("Threshold : " + threshold + " .\n");
            richTextBox1.AppendText("There are " + (unique_Color.Length-1) + " object.");
        }
        public void histogram_calc(myPicture picture,int[] histogram)
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
        public int get_Otsu_Thresholding(myPicture picture,int[] histogram)
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
        private void Form_Connected_Component_Shown(object sender, EventArgs e)
        {
            connect_Table = new int[image.Width, image.Height];
            Array.Clear(connect_Table, 0, connect_Table.Length);
            bitmap = new Bitmap(image.Width, image.Height);
            g_Connected = Graphics.FromImage(bitmap);
            //image.draw(g_Connected);
            histogram_calc(image, histogramValue_Gray);
            threshold = get_Otsu_Thresholding(image, histogramValue_Gray);
            connect_component(g_Connected, image, connect_Table);
            pictureBox1.Size = new Size(image.Width, image.Height);
            pictureBox1.Image = bitmap;
            //for (int Ycount = 0; Ycount < image.Height; Ycount++)
            //{
            //    for (int Xcount = 0; Xcount < image.Width; Xcount++)
            //    {
            //        richTextBox1.AppendText( connect_Table[Xcount, Ycount]+" ");
            //    }
            //    richTextBox1.AppendText("\n");
            //}
            
        }
    }
}
