using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsForms_image_processing
{
    public partial class Form_Thresholding : Form
    {
        public Bitmap bitmap { get; set; }
        public Bitmap grayImage { get; set; }

        Graphics g_grayImage;

        int[] histogramValue_Gray;

        public myPixel[,] read_photo;

        int threshold = 0;
        int histogram_Max = 0;

        public void chart_setup()
        {
            for (int Ycount = 0; Ycount < bitmap.Height; Ycount++)
            {
                for (int Xcount = 0; Xcount < bitmap.Width; Xcount++)
                {
                    int R = read_photo[Xcount, Ycount].R;
                    int G = read_photo[Xcount, Ycount].G;
                    int B = read_photo[Xcount, Ycount].B;
                    int Gray = (int)(R * 0.299 + G * 0.587 + B * 0.114);

                    histogramValue_Gray[Gray]++;
                }
            }

            foreach (int max in histogramValue_Gray)
                if ( max > histogram_Max )
                    histogram_Max = max;
            histogram_Max += 100;

            chart1.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;
            chart1.ChartAreas[0].AxisY.MajorGrid.LineWidth = 0;
            chart1.ChartAreas[0].AxisX.Minimum = -10;
            chart1.ChartAreas[0].AxisX.Maximum = 275;
            chart1.ChartAreas[0].AxisX.Interval = 20;
            chart1.ChartAreas[0].AxisY2.Enabled = AxisEnabled.False;

            var dataPointSeries_Gray = new Series
            {
                Name = "Gray",
                Color = Color.Black,
                ChartType = SeriesChartType.Column
            };
            var dataPointSeries_Threshold = new Series
            {
                Name = "Threshold",
                Color = Color.Red,
                ChartType = SeriesChartType.Column
            };
            dataPointSeries_Threshold.Points.AddXY(threshold, histogram_Max);
            for (int i = 0; i <= 255; i++)
            {
                dataPointSeries_Gray.Points.AddXY(i, histogramValue_Gray[i]);
            }
            chart1.Series.Add(dataPointSeries_Gray);
            chart1.Series.Add(dataPointSeries_Threshold);
        }
        public void drawGrayImage()
        {

            for (int Ycount = 0; Ycount < bitmap.Height; Ycount++)
            {
                for (int Xcount = 0; Xcount < bitmap.Width; Xcount++)
                {
                    int R = read_photo[Xcount, Ycount].R;
                    int G = read_photo[Xcount, Ycount].G;
                    int B = read_photo[Xcount, Ycount].B;
                    int Gray = (int)(R * 0.299 + G * 0.587 + B * 0.114);

                    Gray = Gray > threshold ? 255 : 0;

                    g_grayImage.FillRectangle(new SolidBrush(Color.FromArgb(Gray, Gray, Gray)), Xcount, Ycount, 1, 1);

                }
            }
            
        }
        public Form_Thresholding()
        {
            InitializeComponent();
            label1.Text = "0";
            histogramValue_Gray = new int[256];

            Array.Clear(histogramValue_Gray, 0, histogramValue_Gray.Length);
        }

        private void Form_Thresholding_Shown(object sender, EventArgs e)
        {
            Text = "Image Thresholding";
            grayImage = new Bitmap(bitmap.Width, bitmap.Height);
            g_grayImage = Graphics.FromImage(grayImage);
            drawGrayImage();
            chart_setup();
            pictureBox2.Image = bitmap;
            pictureBox1.Image = grayImage;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            threshold = trackBar1.Value;
            label1.Text = threshold.ToString();
            chart1.Series["Threshold"].Points.RemoveAt(0);
            chart1.Series["Threshold"].Points.AddXY(threshold, histogram_Max) ;
            
            drawGrayImage();
            pictureBox1.Image = grayImage;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            threshold = get_Otsu_Thresholding(histogramValue_Gray);
            trackBar1.Value = threshold;
            
            label1.Text = threshold.ToString();
            chart1.Series["Threshold"].Points.RemoveAt(0);
            chart1.Series["Threshold"].Points.AddXY(threshold, histogram_Max);

            drawGrayImage();
            pictureBox1.Image = grayImage;
        }


        //find best threshold by Otsu
        public int get_Otsu_Thresholding(int[] histogram)
        {   
            //計算最小方差和
            
            int pixelNumb = grayImage.Height * grayImage.Width;
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


    }
}
