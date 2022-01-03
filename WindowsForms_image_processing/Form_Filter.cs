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
    public partial class Form_Filter : Form
    {
        public enum filter_mode { Outlier,Median, pseudo_Median, Lowpass,Highpass,Edge_Crispening,High_boost, Gradient_Roberts, Gradient_Sobel, Gradient_Prewitt, Gradient }
        public filter_mode current_mode { get; set; }
        public Bitmap bitmap { get; set; }


        public myPixel[,] read_photo;
        Bitmap filter_Bitmap;
        
        int threshold = 0;
        int[] kernel = new int[] { -1, -1, -1,  -1,8, -1, -1, -1, -1 };
        int[] kernel_2 = new int[] { -1, -1, -1, -1, 8, -1, -1, -1, -1 };
        public Form_Filter()
        {
            InitializeComponent();
        }

        /*
         * each pixel is compared to the average of its eight neighbors. 
         * If the magnitude of the difference is greater than some threshold level, 
         * the pixel is judged to be noise, 
         * and it is replaced by its neighborhood average 
         */
        public void outlierFilter(int th)
        {
            filter_Bitmap = new Bitmap(bitmap.Width, bitmap.Height);

            Graphics g_outlier = Graphics.FromImage(filter_Bitmap);

            int[] maskR = new int[9];
            int[] maskG = new int[9];
            int[] maskB = new int[9];


            for (int Ycount = 0; Ycount < bitmap.Height-2; Ycount++)
            {
                for (int Xcount = 0; Xcount < bitmap.Width-2; Xcount++)
                {

                    maskR[0] = read_photo[Xcount , Ycount   ].R;
                    maskR[1] = read_photo[Xcount , Ycount +1].R;
                    maskR[2] = read_photo[Xcount , Ycount +2].R;
                    maskR[3] = read_photo[(Xcount + 1) , Ycount ].R;
                    maskR[4] = read_photo[(Xcount + 1) , Ycount  + 1].R;
                    maskR[5] = read_photo[(Xcount + 1) , Ycount  + 2].R;
                    maskR[6] = read_photo[(Xcount + 2) , Ycount ].R;
                    maskR[7] = read_photo[(Xcount + 2) , Ycount  + 1].R;
                    maskR[8] = read_photo[(Xcount + 2) , Ycount  + 2].R;
                    int meanR = (maskR[0] + maskR[1] + maskR[2] + maskR[3] +  maskR[5] + maskR[6] + maskR[7] + maskR[8]) / 8;
                    meanR = Math.Abs(maskR[4] - meanR) > th ? meanR : maskR[4];

                    maskG[0] = read_photo[Xcount ,  Ycount ].G;
                    maskG[1] = read_photo[Xcount ,  Ycount  + 1].G;
                    maskG[2] = read_photo[Xcount ,  Ycount  + 2].G;
                    maskG[3] = read_photo[(Xcount + 1) , Ycount ].G;
                    maskG[4] = read_photo[(Xcount + 1) , Ycount  + 1].G;
                    maskG[5] = read_photo[(Xcount + 1) , Ycount  + 2].G;
                    maskG[6] = read_photo[(Xcount + 2) , Ycount ].G;
                    maskG[7] = read_photo[(Xcount + 2) , Ycount  + 1].G;
                    maskG[8] = read_photo[(Xcount + 2) , Ycount  + 2].G;
                    int meanG = (maskG[0] + maskG[1] + maskG[2] + maskG[3] +  maskG[5] + maskG[6] + maskG[7] + maskG[8]) / 8;
                    meanG = Math.Abs(maskG[4] - meanG) > th ? meanG : maskG[4];

                    maskB[0] = read_photo[Xcount , Ycount ].B;
                    maskB[1] = read_photo[Xcount , Ycount  + 1].B;
                    maskB[2] = read_photo[Xcount , Ycount  + 2].B;
                    maskB[3] = read_photo[(Xcount + 1) , Ycount ].B;
                    maskB[4] = read_photo[(Xcount + 1) , Ycount  + 1].B;
                    maskB[5] = read_photo[(Xcount + 1) , Ycount  + 2].B;
                    maskB[6] = read_photo[(Xcount + 2) , Ycount ].B;
                    maskB[7] = read_photo[(Xcount + 2) , Ycount  + 1].B;
                    maskB[8] = read_photo[(Xcount + 2) , Ycount  + 2].B;
                    int meanB = (maskB[0] + maskB[1] + maskB[2] + maskB[3] +  maskB[5] + maskB[6] + maskB[7] + maskB[8]) / 8;
                    meanB = Math.Abs(maskB[4] - meanB) > th ? meanB : maskB[4];

                    g_outlier.FillRectangle(new SolidBrush(Color.FromArgb(meanR, meanG, meanB)),Xcount, Ycount,  1, 1);
                    
                    
                }
            }
        }

        public void MedianFilter()
        {
            filter_Bitmap = new Bitmap(bitmap.Width, bitmap.Height);

            Graphics g_MedianFilter = Graphics.FromImage(filter_Bitmap);

            int[] maskR = new int[9];
            int[] maskG = new int[9];
            int[] maskB = new int[9];


            for (int Ycount = 0; Ycount < bitmap.Height - 2; Ycount++)
            {
                for (int Xcount = 0; Xcount < bitmap.Width - 2; Xcount++)
                {

                    maskR[0] = read_photo[Xcount , Ycount].R;
                    maskR[1] = read_photo[Xcount , Ycount + 1].R;
                    maskR[2] = read_photo[Xcount , Ycount + 2].R;
                    maskR[3] = read_photo[(Xcount + 1) , Ycount].R;
                    maskR[4] = read_photo[(Xcount + 1) , Ycount + 1].R;
                    maskR[5] = read_photo[(Xcount + 1) , Ycount + 2].R;
                    maskR[6] = read_photo[(Xcount + 2) , Ycount].R;
                    maskR[7] = read_photo[(Xcount + 2) , Ycount + 1].R;
                    maskR[8] = read_photo[(Xcount + 2) , Ycount + 2].R;

                    Array.Sort(maskR);
                    int meanR = maskR[4];
                    
                    maskG[0] = read_photo[Xcount , Ycount].G;
                    maskG[1] = read_photo[Xcount , Ycount + 1].G;
                    maskG[2] = read_photo[Xcount , Ycount + 2].G;
                    maskG[3] = read_photo[(Xcount + 1) , Ycount].G;
                    maskG[4] = read_photo[(Xcount + 1) , Ycount + 1].G;
                    maskG[5] = read_photo[(Xcount + 1) , Ycount + 2].G;
                    maskG[6] = read_photo[(Xcount + 2) , Ycount].G;
                    maskG[7] = read_photo[(Xcount + 2) , Ycount + 1].G;
                    maskG[8] = read_photo[(Xcount + 2) , Ycount + 2].G;

                    Array.Sort(maskG);
                    int meanG = maskG[4];


                    maskB[0] = read_photo[Xcount , Ycount].B;
                    maskB[1] = read_photo[Xcount , Ycount + 1].B;
                    maskB[2] = read_photo[Xcount , Ycount + 2].B;
                    maskB[3] = read_photo[(Xcount + 1) , Ycount].B;
                    maskB[4] = read_photo[(Xcount + 1) , Ycount + 1].B;
                    maskB[5] = read_photo[(Xcount + 1) , Ycount + 2].B;
                    maskB[6] = read_photo[(Xcount + 2) , Ycount].B;
                    maskB[7] = read_photo[(Xcount + 2) , Ycount + 1].B;
                    maskB[8] = read_photo[(Xcount + 2) , Ycount + 2].B;

                    Array.Sort(maskB);
                    int meanB = maskB[4];


                    g_MedianFilter.FillRectangle(new SolidBrush(Color.FromArgb(meanR, meanG, meanB)), Xcount, Ycount, 1, 1);


                }
            }
        }
        public void MedianFilter(string cross)
        {
            filter_Bitmap = new Bitmap(bitmap.Width, bitmap.Height);

            Graphics g_MedianFilter = Graphics.FromImage(filter_Bitmap);

            int[] maskR = new int[9];
            int[] maskG = new int[9];
            int[] maskB = new int[9];
            /*  0
                1 
            5 6 2 7 8
                3
                4
             */

            for (int Ycount = 0; Ycount < bitmap.Height - 2; Ycount++)
            {
                for (int Xcount = 0; Xcount < bitmap.Width - 2; Xcount++)
                {

                    maskR[0] = (((Ycount-2) >= 0 ) ?  read_photo[Xcount, Ycount - 2].R:0);
                    maskR[1] = (((Ycount - 1) >= 0 ) ? read_photo[Xcount, Ycount - 1].R : 0);
                    maskR[2] = read_photo[Xcount, Ycount].R;
                    maskR[3] = (((Ycount + 1) < bitmap.Height - 2) ? read_photo[Xcount, Ycount + 1].R : 0);
                    maskR[4] = (((Ycount + 2) < bitmap.Height - 2) ? read_photo[Xcount, Ycount + 2].R : 0);
                    maskR[5] = (((Xcount - 2) >= 0) ? read_photo[Xcount - 2, Ycount].R : 0);
                    maskR[6] = (((Xcount - 1) >= 0) ? read_photo[Xcount - 1, Ycount].R : 0);
                    maskR[7] = (((Xcount + 1) < bitmap.Width - 2) ? read_photo[Xcount + 1, Ycount].R : 0);
                    maskR[8] = (((Xcount + 2) < bitmap.Width - 2) ? read_photo[Xcount + 2, Ycount].R : 0);

                    Array.Sort(maskR);
                    int meanR = maskR[2];

                    maskG[0] = (((Ycount-2) >= 0 ) ?   read_photo[Xcount, Ycount - 2].G:0);
                    maskG[1] = (((Ycount - 1) >= 0) ?  read_photo[Xcount, Ycount - 1].G : 0);
                    maskG[2] = read_photo[Xcount, Ycount].G;
                    maskG[3] = (((Ycount + 1) < bitmap.Height - 2) ? read_photo[Xcount, Ycount + 1].G : 0);
                    maskG[4] = (((Ycount + 2) < bitmap.Height - 2) ? read_photo[Xcount, Ycount + 2].G : 0);
                    maskG[5] = (((Xcount - 2) >= 0) ?  read_photo[Xcount - 2, Ycount].G : 0);
                    maskG[6] = (((Xcount - 1) >= 0) ? read_photo[Xcount - 1, Ycount].G : 0);
                    maskG[7] = (((Xcount + 1) < bitmap.Width - 2) ? read_photo[Xcount + 1, Ycount].G : 0);
                    maskG[8] = (((Xcount + 2) < bitmap.Width - 2) ? read_photo[Xcount + 2, Ycount].G : 0);

                    Array.Sort(maskG);
                    int meanG = maskG[2];


                    maskB[0] =  (((Ycount-2) >= 0 ) ? read_photo[Xcount, Ycount - 2].B:0);
                    maskB[1] = (((Ycount - 1) >= 0) ?  read_photo[Xcount, Ycount - 1].B : 0);
                    maskB[2] = read_photo[Xcount, Ycount].B;
                    maskB[3] = (((Ycount + 1) < bitmap.Height - 2) ? read_photo[Xcount, Ycount + 1].B : 0);
                    maskB[4] = (((Ycount + 2) < bitmap.Height - 2) ? read_photo[Xcount, Ycount + 2].B : 0);
                    maskB[5] = (((Xcount - 2) >= 0) ? read_photo[Xcount - 2, Ycount].B : 0);
                    maskB[6] = (((Xcount - 1) >= 0) ? read_photo[Xcount - 1, Ycount].B : 0);
                    maskB[7] = (((Xcount + 1) < bitmap.Width - 2) ? read_photo[Xcount + 1, Ycount].B : 0);
                    maskB[8] = (((Xcount + 2) < bitmap.Width - 2) ? read_photo[Xcount + 2, Ycount].B : 0);

                    Array.Sort(maskB);
                    int meanB = maskB[2];


                    g_MedianFilter.FillRectangle(new SolidBrush(Color.FromArgb(meanR, meanG, meanB)), Xcount, Ycount, 1, 1);


                }
            }
        }
        /*
        M=(L+1)/2 

         */
        public void pseudo_MedianFilter()
        {
            filter_Bitmap = new Bitmap(bitmap.Width, bitmap.Height);

            Graphics g_pseudo_MedianFilter = Graphics.FromImage(filter_Bitmap);
            int length = 9;
            int M = (length + 1) / 2;
            int[] maskR = new int[9];
            int[] maskG = new int[9];
            int[] maskB = new int[9];
            int []min = new int[2];
            int []max = new int[2];

            for (int Ycount = 0; Ycount < bitmap.Height - 2; Ycount++)
            {
                for (int Xcount = 0; Xcount < bitmap.Width - 2; Xcount++)
                {

                    maskR[0] = read_photo[Xcount, Ycount].R;
                    maskR[1] = read_photo[Xcount, Ycount + 1].R;
                    maskR[2] = read_photo[Xcount, Ycount + 2].R;
                    maskR[3] = read_photo[(Xcount + 1), Ycount].R;
                    maskR[4] = read_photo[(Xcount + 1), Ycount + 1].R;
                    maskR[5] = read_photo[(Xcount + 1), Ycount + 2].R;
                    maskR[6] = read_photo[(Xcount + 2), Ycount].R;
                    maskR[7] = read_photo[(Xcount + 2), Ycount + 1].R;
                    maskR[8] = read_photo[(Xcount + 2), Ycount + 2].R;

                    min[0] = int.MaxValue;
                    max[0] = int.MinValue;

                    min[1] = int.MaxValue;
                    max[1] = int.MinValue;

                    foreach (int i in maskR)
                        Console.WriteLine(i);
                    //do M times ,
                    for (int i = 0; i < M; i++)
                    {
                        min[0] = maskR[i];
                        max[1] = maskR[i];
                        for (int j = i+1; j < i+M; j++)
                        {
                            min[0] = (maskR[j] < min[0] ? maskR[j] : min[0]);//find the minimum
                            max[1] = (max[1] > maskR[j] ? max[1] : maskR[j]);//find the maximum 

                        }

                        max[0] = (min[0] > max[0] ? min[0] : max[0]);//find the maximum of minimum
                        min[1] = (max[1] < min[1] ? max[1] : min[1]);//find the minimum of maximum 
                    }
                    Console.WriteLine("max of min : "+ max[0]+ " min of max : " + min[1]);
                    int meanR = (max[0]+min[1])/2;

                    maskG[0] = read_photo[Xcount, Ycount].G;
                    maskG[1] = read_photo[Xcount, Ycount + 1].G;
                    maskG[2] = read_photo[Xcount, Ycount + 2].G;
                    maskG[3] = read_photo[(Xcount + 1), Ycount].G;
                    maskG[4] = read_photo[(Xcount + 1), Ycount + 1].G;
                    maskG[5] = read_photo[(Xcount + 1), Ycount + 2].G;
                    maskG[6] = read_photo[(Xcount + 2), Ycount].G;
                    maskG[7] = read_photo[(Xcount + 2), Ycount + 1].G;
                    maskG[8] = read_photo[(Xcount + 2), Ycount + 2].G;

                    min[0] = int.MaxValue;
                    max[0] = int.MinValue;

                    min[1] = int.MaxValue;
                    max[1] = int.MinValue;

                    foreach (int i in maskG)
                        Console.WriteLine(i);
                    //do M times ,
                    for (int i = 0; i < M; i++)
                    {
                        min[0] = maskG[i];
                        max[1] = maskG[i];
                        for (int j = i + 1; j < i + M; j++)
                        {
                            min[0] = (maskG[j] < min[0] ? maskG[j] : min[0]);//find the minimum
                            max[1] = (max[1] > maskG[j] ? max[1] : maskG[j]);//find the maximum 

                        }

                        max[0] = (min[0] > max[0] ? min[0] : max[0]);//find the maximum of minimum
                        min[1] = (max[1] < min[1] ? max[1] : min[1]);//find the minimum of maximum 
                    }
                    Console.WriteLine("max of min" + max[0] + " min of max" + min[1]);
                    int meanG = (max[0] + min[1]) / 2;


                    maskB[0] = read_photo[Xcount, Ycount].B;
                    maskB[1] = read_photo[Xcount, Ycount + 1].B;
                    maskB[2] = read_photo[Xcount, Ycount + 2].B;
                    maskB[3] = read_photo[(Xcount + 1), Ycount].B;
                    maskB[4] = read_photo[(Xcount + 1), Ycount + 1].B;
                    maskB[5] = read_photo[(Xcount + 1), Ycount + 2].B;
                    maskB[6] = read_photo[(Xcount + 2), Ycount].B;
                    maskB[7] = read_photo[(Xcount + 2), Ycount + 1].B;
                    maskB[8] = read_photo[(Xcount + 2), Ycount + 2].B;

                    min[0] = int.MaxValue;
                    max[0] = int.MinValue;

                    min[1] = int.MaxValue;
                    max[1] = int.MinValue;

                    foreach (int i in maskB)
                        Console.WriteLine(i);
                    //do M times ,
                    for (int i = 0; i < M; i++)
                    {
                        min[0] = maskB[i];
                        max[1] = maskB[i];
                        for (int j = i + 1; j < i + M; j++)
                        {
                            min[0] = (maskB[j] < min[0] ? maskB[j] : min[0]);//find the minimum
                            max[1] = (max[1] > maskB[j] ? max[1] : maskB[j]);//find the maximum 

                        }

                        max[0] = (min[0] > max[0] ? min[0] : max[0]);//find the maximum of minimum
                        min[1] = (max[1] < min[1] ? max[1] : min[1]);//find the minimum of maximum 
                    }
                    Console.WriteLine("max of min" + max[0] + " min of max" + min[1]);
                    int meanB = (max[0] + min[1]) / 2;


                    g_pseudo_MedianFilter.FillRectangle(new SolidBrush(Color.FromArgb(meanR, meanG, meanB)), Xcount, Ycount, 1, 1);


                }
            }
        }

        public void LowpassFilter()
        {
            filter_Bitmap = new Bitmap(bitmap.Width, bitmap.Height);

            Graphics g_LowpassFilter = Graphics.FromImage(filter_Bitmap);

            int[] maskR = new int[9];
            int[] maskG = new int[9];
            int[] maskB = new int[9];


            for (int Ycount = 0; Ycount < bitmap.Height - 2; Ycount++)
            {
                for (int Xcount = 0; Xcount < bitmap.Width - 2; Xcount++)
                {

                    maskR[0] = read_photo[Xcount , Ycount].R;
                    maskR[1] = read_photo[Xcount , Ycount + 1].R;
                    maskR[2] = read_photo[Xcount , Ycount + 2].R;
                    maskR[3] = read_photo[(Xcount + 1) , Ycount].R;
                    maskR[4] = read_photo[(Xcount + 1) , Ycount + 1].R;
                    maskR[5] = read_photo[(Xcount + 1) , Ycount + 2].R;
                    maskR[6] = read_photo[(Xcount + 2) , Ycount].R;
                    maskR[7] = read_photo[(Xcount + 2) , Ycount + 1].R;
                    maskR[8] = read_photo[(Xcount + 2) , Ycount + 2].R;
                    int meanR = (maskR[0] + maskR[1] + maskR[2] + maskR[3] + maskR[4] + maskR[5] + maskR[6] + maskR[7] + maskR[8]) / 9;


                    maskG[0] = read_photo[Xcount , Ycount].G;
                    maskG[1] = read_photo[Xcount , Ycount + 1].G;
                    maskG[2] = read_photo[Xcount , Ycount + 2].G;
                    maskG[3] = read_photo[(Xcount + 1) , Ycount].G;
                    maskG[4] = read_photo[(Xcount + 1) , Ycount + 1].G;
                    maskG[5] = read_photo[(Xcount + 1) , Ycount + 2].G;
                    maskG[6] = read_photo[(Xcount + 2) , Ycount].G;
                    maskG[7] = read_photo[(Xcount + 2) , Ycount + 1].G;
                    maskG[8] = read_photo[(Xcount + 2) , Ycount + 2].G;
                    int meanG = (maskG[0] + maskG[1] + maskG[2] + maskG[3] + maskG[4] + maskG[5] + maskG[6] + maskG[7] + maskG[8]) / 9;


                    maskB[0] = read_photo[Xcount , Ycount].B;
                    maskB[1] = read_photo[Xcount , Ycount + 1].B;
                    maskB[2] = read_photo[Xcount , Ycount + 2].B;
                    maskB[3] = read_photo[(Xcount + 1) , Ycount].B;
                    maskB[4] = read_photo[(Xcount + 1) , Ycount + 1].B;
                    maskB[5] = read_photo[(Xcount + 1) , Ycount + 2].B;
                    maskB[6] = read_photo[(Xcount + 2) , Ycount].B;
                    maskB[7] = read_photo[(Xcount + 2) , Ycount + 1].B;
                    maskB[8] = read_photo[(Xcount + 2) , Ycount + 2].B;
                    int meanB = (maskB[0] + maskB[1] + maskB[2] + maskB[3] + maskB[4] + maskB[5] + maskB[6] + maskB[7] + maskB[8]) / 9;


                    g_LowpassFilter.FillRectangle(new SolidBrush(Color.FromArgb(meanR, meanG, meanB)), Xcount, Ycount, 1, 1);


                }
            }
        }

        public void HighpassFilter()
        {
            filter_Bitmap = new Bitmap(bitmap.Width, bitmap.Height);

            Graphics g_HighpassFilter = Graphics.FromImage(filter_Bitmap);

            int[] maskR = new int[9];
            int[] maskG = new int[9];
            int[] maskB = new int[9];
            //int[] kernel = new int[] {-1,-1,-1,8,-1,-1,-1,-1,-1};
            //int sumOfKernel = kernel.Sum();
            int sumOfKernel =9;
            for (int Ycount = 0; Ycount < bitmap.Height - 2; Ycount++)
            {
                for (int Xcount = 0; Xcount < bitmap.Width - 2; Xcount++)
                {

                    maskR[0] = kernel[0] * read_photo[Xcount , Ycount].R;
                    maskR[1] = kernel[1] * read_photo[Xcount , Ycount + 1].R;
                    maskR[2] = kernel[2] * read_photo[Xcount , Ycount + 2].R;
                    maskR[3] = kernel[3] * read_photo[(Xcount + 1) , Ycount].R;
                    maskR[4] = kernel[4] * read_photo[(Xcount + 1) , Ycount + 1].R;
                    maskR[5] = kernel[5] * read_photo[(Xcount + 1) , Ycount + 2].R;
                    maskR[6] = kernel[6] * read_photo[(Xcount + 2) , Ycount].R;
                    maskR[7] = kernel[7] * read_photo[(Xcount + 2) , Ycount + 1].R;
                    maskR[8] = kernel[8] * read_photo[(Xcount + 2) , Ycount + 2].R;
                    int meanR = (maskR[0] + maskR[1] + maskR[2] + maskR[3] + maskR[4] + maskR[5] + maskR[6] + maskR[7] + maskR[8]) / sumOfKernel;
                    meanR = meanR > 0 ? meanR : 0;

                    maskG[0] = kernel[0] * read_photo[Xcount , Ycount].G;
                    maskG[1] = kernel[1] * read_photo[Xcount , Ycount + 1].G;
                    maskG[2] = kernel[2] * read_photo[Xcount , Ycount + 2].G;
                    maskG[3] = kernel[3] * read_photo[(Xcount + 1) , Ycount].G;
                    maskG[4] = kernel[4] * read_photo[(Xcount + 1) , Ycount + 1].G;
                    maskG[5] = kernel[5] * read_photo[(Xcount + 1) , Ycount + 2].G;
                    maskG[6] = kernel[6] * read_photo[(Xcount + 2) , Ycount].G;
                    maskG[7] = kernel[7] * read_photo[(Xcount + 2) , Ycount + 1].G;
                    maskG[8] = kernel[8] * read_photo[(Xcount + 2) , Ycount + 2].G;
                    int meanG = (maskG[0] + maskG[1] + maskG[2] + maskG[3] + maskG[4] + maskG[5] + maskG[6] + maskG[7] + maskG[8]) / sumOfKernel;
                    meanG = meanG > 0 ? meanG : 0;

                    maskB[0] = kernel[0] * read_photo[Xcount , Ycount].B;
                    maskB[1] = kernel[1] * read_photo[Xcount , Ycount + 1].B;
                    maskB[2] = kernel[2] * read_photo[Xcount , Ycount + 2].B;
                    maskB[3] = kernel[3] * read_photo[(Xcount + 1) , Ycount].B;
                    maskB[4] = kernel[4] * read_photo[(Xcount + 1) , Ycount + 1].B;
                    maskB[5] = kernel[5] * read_photo[(Xcount + 1) , Ycount + 2].B;
                    maskB[6] = kernel[6] * read_photo[(Xcount + 2) , Ycount].B;
                    maskB[7] = kernel[7] * read_photo[(Xcount + 2) , Ycount + 1].B;
                    maskB[8] = kernel[8] * read_photo[(Xcount + 2) , Ycount + 2].B;
                    int meanB = (maskB[0] + maskB[1] + maskB[2] + maskB[3] + maskB[4] + maskB[5] + maskB[6] + maskB[7] + maskB[8]) / sumOfKernel;
                    meanB = meanB > 0 ? meanB : 0;

                    g_HighpassFilter.FillRectangle(new SolidBrush(Color.FromArgb(meanR, meanG, meanB)), Xcount, Ycount, 1, 1);


                }
            }
        }

        public void Gradient_Filter()
        {
            filter_Bitmap = new Bitmap(bitmap.Width, bitmap.Height);

            Graphics g_Prewitt_Filter = Graphics.FromImage(filter_Bitmap);

            int[] maskR = new int[9];
            int[] maskG = new int[9];
            int[] maskB = new int[9];

            //int[] kernel = new int[] {-1,-1,-1,8,-1,-1,-1,-1,-1};
            //int sumOfKernel = kernel.Sum();
            int sumOfKernel = 1;
            for (int Ycount = 0; Ycount < bitmap.Height - 2; Ycount++)
            {
                for (int Xcount = 0; Xcount < bitmap.Width - 2; Xcount++)
                {

                    maskR[0] = kernel[0] * read_photo[Xcount, Ycount].R;
                    maskR[1] = kernel[1] * read_photo[Xcount, Ycount + 1].R;
                    maskR[2] = kernel[2] * read_photo[Xcount, Ycount + 2].R;
                    maskR[3] = kernel[3] * read_photo[(Xcount + 1), Ycount].R;
                    maskR[4] = kernel[4] * read_photo[(Xcount + 1), Ycount + 1].R;
                    maskR[5] = kernel[5] * read_photo[(Xcount + 1), Ycount + 2].R;
                    maskR[6] = kernel[6] * read_photo[(Xcount + 2), Ycount].R;
                    maskR[7] = kernel[7] * read_photo[(Xcount + 2), Ycount + 1].R;
                    maskR[8] = kernel[8] * read_photo[(Xcount + 2), Ycount + 2].R;
                    int meanR_X = (maskR[0] + maskR[1] + maskR[2] + maskR[3] + maskR[4] + maskR[5] + maskR[6] + maskR[7] + maskR[8]) / sumOfKernel;

                    maskR[0] = kernel_2[0] * read_photo[Xcount, Ycount].R;
                    maskR[1] = kernel_2[1] * read_photo[Xcount, Ycount + 1].R;
                    maskR[2] = kernel_2[2] * read_photo[Xcount, Ycount + 2].R;
                    maskR[3] = kernel_2[3] * read_photo[(Xcount + 1), Ycount].R;
                    maskR[4] = kernel_2[4] * read_photo[(Xcount + 1), Ycount + 1].R;
                    maskR[5] = kernel_2[5] * read_photo[(Xcount + 1), Ycount + 2].R;
                    maskR[6] = kernel_2[6] * read_photo[(Xcount + 2), Ycount].R;
                    maskR[7] = kernel_2[7] * read_photo[(Xcount + 2), Ycount + 1].R;
                    maskR[8] = kernel_2[8] * read_photo[(Xcount + 2), Ycount + 2].R;
                    int meanR_Y = (maskR[0] + maskR[1] + maskR[2] + maskR[3] + maskR[4] + maskR[5] + maskR[6] + maskR[7] + maskR[8]) / sumOfKernel;
                    int meanR = Math.Abs(meanR_X) + Math.Abs(meanR_Y);
                    //meanR+= read_photo[Xcount, Ycount].R;
                    meanR = meanR > 0 ? (meanR > 255 ? 255 : meanR) : 0;

                    maskG[0] = kernel[0] * read_photo[Xcount, Ycount].G;
                    maskG[1] = kernel[1] * read_photo[Xcount, Ycount + 1].G;
                    maskG[2] = kernel[2] * read_photo[Xcount, Ycount + 2].G;
                    maskG[3] = kernel[3] * read_photo[(Xcount + 1), Ycount].G;
                    maskG[4] = kernel[4] * read_photo[(Xcount + 1), Ycount + 1].G;
                    maskG[5] = kernel[5] * read_photo[(Xcount + 1), Ycount + 2].G;
                    maskG[6] = kernel[6] * read_photo[(Xcount + 2), Ycount].G;
                    maskG[7] = kernel[7] * read_photo[(Xcount + 2), Ycount + 1].G;
                    maskG[8] = kernel[8] * read_photo[(Xcount + 2), Ycount + 2].G;
                    int meanG_X = (maskG[0] + maskG[1] + maskG[2] + maskG[3] + maskG[4] + maskG[5] + maskG[6] + maskG[7] + maskG[8]) / sumOfKernel;

                    maskG[0] = kernel_2[0] * read_photo[Xcount, Ycount].G;
                    maskG[1] = kernel_2[1] * read_photo[Xcount, Ycount + 1].G;
                    maskG[2] = kernel_2[2] * read_photo[Xcount, Ycount + 2].G;
                    maskG[3] = kernel_2[3] * read_photo[(Xcount + 1), Ycount].G;
                    maskG[4] = kernel_2[4] * read_photo[(Xcount + 1), Ycount + 1].G;
                    maskG[5] = kernel_2[5] * read_photo[(Xcount + 1), Ycount + 2].G;
                    maskG[6] = kernel_2[6] * read_photo[(Xcount + 2), Ycount].G;
                    maskG[7] = kernel_2[7] * read_photo[(Xcount + 2), Ycount + 1].G;
                    maskG[8] = kernel_2[8] * read_photo[(Xcount + 2), Ycount + 2].G;
                    int meanG_Y = (maskG[0] + maskG[1] + maskG[2] + maskG[3] + maskG[4] + maskG[5] + maskG[6] + maskG[7] + maskG[8]) / sumOfKernel;
                    int meanG = Math.Abs(meanG_X) + Math.Abs(meanG_Y);
                    //meanG += read_photo[Xcount, Ycount].G;
                    meanG = meanG > 0 ? (meanG > 255 ? 255 : meanG) : 0;


                    maskB[0] = kernel[0] * read_photo[Xcount, Ycount].B;
                    maskB[1] = kernel[1] * read_photo[Xcount, Ycount + 1].B;
                    maskB[2] = kernel[2] * read_photo[Xcount, Ycount + 2].B;
                    maskB[3] = kernel[3] * read_photo[(Xcount + 1), Ycount].B;
                    maskB[4] = kernel[4] * read_photo[(Xcount + 1), Ycount + 1].B;
                    maskB[5] = kernel[5] * read_photo[(Xcount + 1), Ycount + 2].B;
                    maskB[6] = kernel[6] * read_photo[(Xcount + 2), Ycount].B;
                    maskB[7] = kernel[7] * read_photo[(Xcount + 2), Ycount + 1].B;
                    maskB[8] = kernel[8] * read_photo[(Xcount + 2), Ycount + 2].B;
                    int meanB_X = (maskB[0] + maskB[1] + maskB[2] + maskB[3] + maskB[4] + maskB[5] + maskB[6] + maskB[7] + maskB[8]) / sumOfKernel;

                    maskB[0] = kernel_2[0] * read_photo[Xcount, Ycount].B;
                    maskB[1] = kernel_2[1] * read_photo[Xcount, Ycount + 1].B;
                    maskB[2] = kernel_2[2] * read_photo[Xcount, Ycount + 2].B;
                    maskB[3] = kernel_2[3] * read_photo[(Xcount + 1), Ycount].B;
                    maskB[4] = kernel_2[4] * read_photo[(Xcount + 1), Ycount + 1].B;
                    maskB[5] = kernel_2[5] * read_photo[(Xcount + 1), Ycount + 2].B;
                    maskB[6] = kernel_2[6] * read_photo[(Xcount + 2), Ycount].B;
                    maskB[7] = kernel_2[7] * read_photo[(Xcount + 2), Ycount + 1].B;
                    maskB[8] = kernel_2[8] * read_photo[(Xcount + 2), Ycount + 2].B;
                    int meanB_Y = (maskB[0] + maskB[1] + maskB[2] + maskB[3] + maskB[4] + maskB[5] + maskB[6] + maskB[7] + maskB[8]) / sumOfKernel;
                    int meanB = Math.Abs(meanB_X) + Math.Abs(meanB_Y);
                    //meanB+= read_photo[Xcount, Ycount].B;
                    meanB = meanB > 0 ? (meanB > 255 ? 255 : meanB) : 0;


                    g_Prewitt_Filter.FillRectangle(new SolidBrush(Color.FromArgb(meanR, meanG, meanB)), Xcount, Ycount, 1, 1);


                }
            }
        }

        public void Edge_Crispening_Filter()
        {
            filter_Bitmap = new Bitmap(bitmap.Width, bitmap.Height);

            Graphics g_Edge_Crispening_Filter = Graphics.FromImage(filter_Bitmap);

            int[] maskR = new int[9];
            int[] maskG = new int[9];
            int[] maskB = new int[9];
            kernel = new int[] {0,-1,0,-1,5,-1,0,-1,0};
            //kernel = new int[] {1,-2,1,-2,5,-2,1,-2,1};
            //kernel = new int[] {-1,-1,-1,-1,9,-1,-1,-1,-1};
            //kernel = new int[] {1,1,1,1,1,1,1,1,1};
            //int sumOfKernel = kernel.Sum();
            int sumOfKernel = 1;
            for (int Ycount = 0; Ycount < bitmap.Height - 2; Ycount++)
            {
                for (int Xcount = 0; Xcount < bitmap.Width - 2; Xcount++)
                {
                    maskR[0] = kernel[0] * read_photo[Xcount, Ycount].R;
                    maskR[1] = kernel[1] * read_photo[Xcount, Ycount + 1].R;
                    maskR[2] = kernel[2] * read_photo[Xcount, Ycount + 2].R;
                    maskR[3] = kernel[3] * read_photo[(Xcount + 1), Ycount].R;
                    maskR[4] = kernel[4] * read_photo[(Xcount + 1), Ycount + 1].R;
                    maskR[5] = kernel[5] * read_photo[(Xcount + 1), Ycount + 2].R;
                    maskR[6] = kernel[6] * read_photo[(Xcount + 2), Ycount].R;
                    maskR[7] = kernel[7] * read_photo[(Xcount + 2), Ycount + 1].R;
                    maskR[8] = kernel[8] * read_photo[(Xcount + 2), Ycount + 2].R;
                    int meanR = (maskR[0] + maskR[1] + maskR[2] + maskR[3] + maskR[4] + maskR[5] + maskR[6] + maskR[7] + maskR[8]) / sumOfKernel;
                    //meanR+= read_photo[Xcount, Ycount].R;
                    meanR = meanR > 0 ? (meanR>255?255:meanR) : 0;

                    maskG[0] = kernel[0] * read_photo[Xcount, Ycount].G;
                    maskG[1] = kernel[1] * read_photo[Xcount, Ycount + 1].G;
                    maskG[2] = kernel[2] * read_photo[Xcount, Ycount + 2].G;
                    maskG[3] = kernel[3] * read_photo[(Xcount + 1), Ycount].G;
                    maskG[4] = kernel[4] * read_photo[(Xcount + 1), Ycount + 1].G;
                    maskG[5] = kernel[5] * read_photo[(Xcount + 1), Ycount + 2].G;
                    maskG[6] = kernel[6] * read_photo[(Xcount + 2), Ycount].G;
                    maskG[7] = kernel[7] * read_photo[(Xcount + 2), Ycount + 1].G;
                    maskG[8] = kernel[8] * read_photo[(Xcount + 2), Ycount + 2].G;
                    int meanG = (maskG[0] + maskG[1] + maskG[2] + maskG[3] + maskG[4] + maskG[5] + maskG[6] + maskG[7] + maskG[8]) / sumOfKernel;
                    //meanG += read_photo[Xcount, Ycount].G;
                    meanG = meanG > 0 ? (meanG > 255 ? 255 : meanG) : 0;


                    maskB[0] = kernel[0] * read_photo[Xcount, Ycount].B;
                    maskB[1] = kernel[1] * read_photo[Xcount, Ycount + 1].B;
                    maskB[2] = kernel[2] * read_photo[Xcount, Ycount + 2].B;
                    maskB[3] = kernel[3] * read_photo[(Xcount + 1), Ycount].B;
                    maskB[4] = kernel[4] * read_photo[(Xcount + 1), Ycount + 1].B;
                    maskB[5] = kernel[5] * read_photo[(Xcount + 1), Ycount + 2].B;
                    maskB[6] = kernel[6] * read_photo[(Xcount + 2), Ycount].B;
                    maskB[7] = kernel[7] * read_photo[(Xcount + 2), Ycount + 1].B;
                    maskB[8] = kernel[8] * read_photo[(Xcount + 2), Ycount + 2].B;
                    int meanB = (maskB[0] + maskB[1] + maskB[2] + maskB[3] + maskB[4] + maskB[5] + maskB[6] + maskB[7] + maskB[8]) / sumOfKernel;
                    //meanB+= read_photo[Xcount, Ycount].B;
                    meanB = meanB > 0 ? (meanB > 255 ? 255 : meanB) : 0;

                    g_Edge_Crispening_Filter.FillRectangle(new SolidBrush(Color.FromArgb(meanR, meanG, meanB)), Xcount, Ycount, 1, 1);


                }
            }
        }
        public void High_Boost_Filter(double A)
        {
            filter_Bitmap = new Bitmap(bitmap.Width, bitmap.Height);

            Graphics g_High_Boost_Filter = Graphics.FromImage(filter_Bitmap);

            int[] maskR = new int[9];
            int[] maskG = new int[9];
            int[] maskB = new int[9];
            //kernel = new int[] { 0, -1, 0, -1, 5, -1, 0, -1, 0 };
            //kernel = new int[] {1,-2,1,-2,5,-2,1,-2,1};
            kernel = new int[] {-1,-1,-1,-1, (int)((9*A)-1), -1,-1,-1,-1};
            //kernel = new int[] {1,1,1,1,1,1,1,1,1};
            //int sumOfKernel = kernel.Sum();
            int sumOfKernel = 9;
            for (int Ycount = 0; Ycount < bitmap.Height - 2; Ycount++)
            {
                for (int Xcount = 0; Xcount < bitmap.Width - 2; Xcount++)
                {
                    maskR[0] = kernel[0] * read_photo[Xcount, Ycount].R;
                    maskR[1] = kernel[1] * read_photo[Xcount, Ycount + 1].R;
                    maskR[2] = kernel[2] * read_photo[Xcount, Ycount + 2].R;
                    maskR[3] = kernel[3] * read_photo[(Xcount + 1), Ycount].R;
                    maskR[4] = kernel[4] * read_photo[(Xcount + 1), Ycount + 1].R;
                    maskR[5] = kernel[5] * read_photo[(Xcount + 1), Ycount + 2].R;
                    maskR[6] = kernel[6] * read_photo[(Xcount + 2), Ycount].R;
                    maskR[7] = kernel[7] * read_photo[(Xcount + 2), Ycount + 1].R;
                    maskR[8] = kernel[8] * read_photo[(Xcount + 2), Ycount + 2].R;
                    int meanR = (maskR[0] + maskR[1] + maskR[2] + maskR[3] + maskR[4] + maskR[5] + maskR[6] + maskR[7] + maskR[8]) / sumOfKernel;
                    //meanR+= read_photo[Xcount, Ycount].R;
                    meanR = meanR > 0 ? (meanR > 255 ? 255 : meanR) : 0;

                    maskG[0] = kernel[0] * read_photo[Xcount, Ycount].G;
                    maskG[1] = kernel[1] * read_photo[Xcount, Ycount + 1].G;
                    maskG[2] = kernel[2] * read_photo[Xcount, Ycount + 2].G;
                    maskG[3] = kernel[3] * read_photo[(Xcount + 1), Ycount].G;
                    maskG[4] = kernel[4] * read_photo[(Xcount + 1), Ycount + 1].G;
                    maskG[5] = kernel[5] * read_photo[(Xcount + 1), Ycount + 2].G;
                    maskG[6] = kernel[6] * read_photo[(Xcount + 2), Ycount].G;
                    maskG[7] = kernel[7] * read_photo[(Xcount + 2), Ycount + 1].G;
                    maskG[8] = kernel[8] * read_photo[(Xcount + 2), Ycount + 2].G;
                    int meanG = (maskG[0] + maskG[1] + maskG[2] + maskG[3] + maskG[4] + maskG[5] + maskG[6] + maskG[7] + maskG[8]) / sumOfKernel;
                    //meanG += read_photo[Xcount, Ycount].G;
                    meanG = meanG > 0 ? (meanG > 255 ? 255 : meanG) : 0;


                    maskB[0] = kernel[0] * read_photo[Xcount, Ycount].B;
                    maskB[1] = kernel[1] * read_photo[Xcount, Ycount + 1].B;
                    maskB[2] = kernel[2] * read_photo[Xcount, Ycount + 2].B;
                    maskB[3] = kernel[3] * read_photo[(Xcount + 1), Ycount].B;
                    maskB[4] = kernel[4] * read_photo[(Xcount + 1), Ycount + 1].B;
                    maskB[5] = kernel[5] * read_photo[(Xcount + 1), Ycount + 2].B;
                    maskB[6] = kernel[6] * read_photo[(Xcount + 2), Ycount].B;
                    maskB[7] = kernel[7] * read_photo[(Xcount + 2), Ycount + 1].B;
                    maskB[8] = kernel[8] * read_photo[(Xcount + 2), Ycount + 2].B;
                    int meanB = (maskB[0] + maskB[1] + maskB[2] + maskB[3] + maskB[4] + maskB[5] + maskB[6] + maskB[7] + maskB[8]) / sumOfKernel;
                    //meanB+= read_photo[Xcount, Ycount].B;
                    meanB = meanB > 0 ? (meanB > 255 ? 255 : meanB) : 0;
                    int Gray = (meanR+meanG+meanB)/3;
                    g_High_Boost_Filter.FillRectangle(new SolidBrush(Color.FromArgb(Gray, Gray, Gray)), Xcount, Ycount, 1, 1);


                }
            }
        }
        private void Form_Filter_Load(object sender, EventArgs e)
        {

            groupBox1.Visible = false;
            groupBox2.Visible = false;
            groupBox3.Visible = false;
            button3.Visible = false;
            button2.Visible = false;
            label4.Visible = false;
            comboBox1.Visible = false;
            if (current_mode == filter_mode.Outlier)
            {
                Text = "Outlier Filter";
                groupBox1.Text = "Outlier Filter";
                textBox1.Text = "30";
                outlierFilter(30);
                pictureBox1.Image = bitmap;
                pictureBox2.Image = filter_Bitmap;
            }
            else if (current_mode == filter_mode.Median)
            {
                radioButton1.Visible = true;
                radioButton2.Visible = true;
                radioButton1.Checked = true;
                Text = "Median Filter";
                MedianFilter();
                pictureBox1.Image = bitmap;
                pictureBox2.Image = filter_Bitmap;
            }
            else if (current_mode == filter_mode.pseudo_Median)
            {
                Text = "Pseudo Median Filter";
                pseudo_MedianFilter();
                pictureBox1.Image = bitmap;
                pictureBox2.Image = filter_Bitmap;
            }
            else if (current_mode == filter_mode.Lowpass)
            {
                Text = "Lowpass Filter";
                LowpassFilter();
                pictureBox1.Image = bitmap;
                pictureBox2.Image = filter_Bitmap;
            }
            else if (current_mode == filter_mode.Highpass)
            {
                Text = "Highpass Filter";
                groupBox2.Visible = true;
                HighpassFilter();
                pictureBox1.Image = bitmap;
                pictureBox2.Image = filter_Bitmap;
            }
            else if (current_mode == filter_mode.Edge_Crispening)
            {
                Text = "Edge Crispening Filter";
                groupBox2.Visible = true;
                Edge_Crispening_Filter();
                pictureBox1.Image = bitmap;
                pictureBox2.Image = filter_Bitmap;
            }
            else if (current_mode == filter_mode.High_boost)
            {
                Text = "High-Boost Filter";
                groupBox2.Visible = true;
                High_Boost_Filter(1.5);
                pictureBox1.Image = bitmap;
                pictureBox2.Image = filter_Bitmap;
            }
            else if (current_mode == filter_mode.Gradient)
            {
                groupBox2.Visible = true;
                groupBox3.Visible = true;
                label4.Visible = true;
                comboBox1.Visible = true;
                comboBox1.Items.Add("Roberts Filter");
                comboBox1.Items.Add("Sobel Filter");
                comboBox1.Items.Add("Prewitt Filter");
                comboBox1.SelectedIndex = 0;

            }
            

            textBox2.Text = kernel[0].ToString();
            textBox3.Text = kernel[1].ToString();
            textBox4.Text = kernel[2].ToString();

            textBox5.Text = kernel[3].ToString();
            textBox6.Text = kernel[4].ToString();
            textBox7.Text = kernel[5].ToString();

            textBox8.Text = kernel[6].ToString();
            textBox9.Text = kernel[7].ToString();
            textBox10.Text = kernel[8].ToString();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int i;
            bool valid = int.TryParse(textBox1.Text, out i);//check value is valid
            threshold = valid ? i : 0;
            outlierFilter(threshold);

            pictureBox2.Image = filter_Bitmap;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int i;
            bool valid = int.TryParse(textBox1.Text, out i);//check value is valid
            threshold = valid ? i : 0;
            outlierFilter(threshold);

            pictureBox2.Image = filter_Bitmap;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != -1)
            {
                Console.WriteLine(comboBox1.SelectedIndex);

                if (comboBox1.SelectedIndex == 0)
                {
                    Text = "Gradient Roberts Filter";
                    kernel = new int[] { 1,0,0,0,-1,0,0,0,0};
                    kernel_2 = new int[] { 0,-1,0,1,0,0,0,0,0 };
                    Gradient_Filter();


                    pictureBox1.Image = bitmap;
                    pictureBox2.Image = filter_Bitmap;
                }
                else if (comboBox1.SelectedIndex == 1)
                {
                    Text = "Gradient Sobel Filter";
                    kernel = new int[] {-1,0,1,-2,0,2,-1,0,1};
                    kernel_2 = new int[] { 1,2,1,0,0,0,-1,-2,-1 };
                    Gradient_Filter();


                    pictureBox1.Image = bitmap;
                    pictureBox2.Image = filter_Bitmap;
                }
                else if (comboBox1.SelectedIndex == 2)
                {
                    Text = "Gradient Prewitt Filter";
                    kernel = new int[] { -1, 0, 1, -1, 0, 1, -1, 0, 1 };
                    kernel_2 = new int[] { 1, 1, 1, 0, 0, 0, -1, -1, -1 };
                    Gradient_Filter();

                    pictureBox1.Image = bitmap;
                    pictureBox2.Image = filter_Bitmap;
                }

            }
            else
            {
                MessageBox.Show("please select a file");
            }
            textBox2.Text = kernel[0].ToString();
            textBox3.Text = kernel[1].ToString();
            textBox4.Text = kernel[2].ToString();
            textBox5.Text = kernel[3].ToString();
            textBox6.Text = kernel[4].ToString();
            textBox7.Text = kernel[5].ToString();
            textBox8.Text = kernel[6].ToString();
            textBox9.Text = kernel[7].ToString();
            textBox10.Text = kernel[8].ToString();

            textBox_k2_0.Text = kernel_2[0].ToString();
            textBox_k2_1.Text = kernel_2[1].ToString();
            textBox_k2_2.Text = kernel_2[2].ToString();
            textBox_k2_3.Text = kernel_2[3].ToString();
            textBox_k2_4.Text = kernel_2[4].ToString();
            textBox_k2_5.Text = kernel_2[5].ToString();
            textBox_k2_6.Text = kernel_2[6].ToString();
            textBox_k2_7.Text = kernel_2[7].ToString();
            textBox_k2_8.Text = kernel_2[8].ToString();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton1.Checked == true)
            {
                Text = "Median Filter";
                MedianFilter();
                pictureBox1.Image = bitmap;
                pictureBox2.Image = filter_Bitmap;
            }

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton2.Checked == true)
            {
                Text = "Median Filter";
                MedianFilter("cross");
                pictureBox1.Image = bitmap;
                pictureBox2.Image = filter_Bitmap;
            }

        }
    }
}
