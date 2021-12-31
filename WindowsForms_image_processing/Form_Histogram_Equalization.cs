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
{/*
    Goal: to produce an image with equally distributed brightness levels over the whole brightness scale
    Effect: enhancing contrast for brightness values close to histogram maxima, and decreasing contrast near minima.
    Result is Cr than just stretching, and method fully automatic

  */
    public partial class Form_Histogram_Equalization : Form
    {
        public enum mode { Histogram, Histogram_Equalization };
        public mode Histogram_mode = mode.Histogram_Equalization;
        public Bitmap bitmap { get; set; }
        public Bitmap Historgram_Equalization_bitmap { get; set; }
        public Bitmap grayImage { get; set; }

        Graphics g_grayImage;
        Graphics g_Image;

        int[] histogramValue_R;
        int[] histogramValue_G;
        int[] histogramValue_B;
        int[] histogramValue_Gray;

        int[] new_histogramValue_R;
        int[] new_histogramValue_G;
        int[] new_histogramValue_B;
        int[] new_histogramValue_Gray;

        int[] cdf_Value_R;
        int[] cdf_Value_G;
        int[] cdf_Value_B;
        int[] cdf_Value_Gray;

        int[] max;
        int[] min;

        int[] cdf_max;
        int[] cdf_min;
        int bit_depth;
        public myPixel[,] read_photo;

        public void chart_setup()
        {
            chart1.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;
            chart1.ChartAreas[0].AxisY.MajorGrid.LineWidth = 0;
            chart1.ChartAreas[0].AxisX.Minimum = -10;
            chart1.ChartAreas[0].AxisX.Maximum = 275;
            chart1.ChartAreas[0].AxisX.Interval = 20;
            chart1.ChartAreas[0].AxisY2.Enabled = AxisEnabled.False;

            chart2.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;
            chart2.ChartAreas[0].AxisY.MajorGrid.LineWidth = 0;
            chart2.ChartAreas[0].AxisX.Minimum = -10;
            chart2.ChartAreas[0].AxisX.Maximum = 275;
            chart2.ChartAreas[0].AxisX.Interval = 20;
            chart2.ChartAreas[0].AxisY2.Enabled = AxisEnabled.False;

            chart3.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;
            chart3.ChartAreas[0].AxisY.MajorGrid.LineWidth = 0;
            chart3.ChartAreas[0].AxisX.Minimum = -10;
            chart3.ChartAreas[0].AxisX.Maximum = 275;
            chart3.ChartAreas[0].AxisX.Interval = 20;
            chart3.ChartAreas[0].AxisY2.Enabled = AxisEnabled.False;

            chart4.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;
            chart4.ChartAreas[0].AxisY.MajorGrid.LineWidth = 0;
            chart4.ChartAreas[0].AxisX.Minimum = -10;
            chart4.ChartAreas[0].AxisX.Maximum = 275;
            chart4.ChartAreas[0].AxisX.Interval = 20;
            chart4.ChartAreas[0].AxisY2.Enabled = AxisEnabled.False;

            var dataPointSeries_R = new Series
            {
                Name = "Red",
                Color = Color.Red,
                ChartType = SeriesChartType.Column
            };
            var dataPointSeries_R_cdf = new Series
            {
                Name = "Red_cdf",
                Color = Color.Red,
                ChartType = SeriesChartType.Line
            };
            var dataPointSeries_G = new Series
            {
                Name = "Green",
                Color = Color.LimeGreen,
                ChartType = SeriesChartType.Column
            };
            var dataPointSeries_G_cdf = new Series
            {
                Name = "Green_cdf",
                Color = Color.LimeGreen,
                ChartType = SeriesChartType.Line
            };
            var dataPointSeries_B = new Series
            {
                Name = "Blue",
                Color = Color.Blue,
                ChartType = SeriesChartType.Column
            };
            var dataPointSeries_B_cdf = new Series
            {
                Name = "Blue_cdf",
                Color = Color.Blue,
                ChartType = SeriesChartType.Line
            };
            var dataPointSeries_Gray = new Series
            {
                Name = "Gray",
                Color = Color.Gray,
                ChartType = SeriesChartType.Column
            };
            var dataPointSeries_Gray_cdf = new Series
            {
                Name = "Gray_cdf",
                Color = Color.Black,
                ChartType = SeriesChartType.Line
            };
            chart1.Series.Add(dataPointSeries_R);
            chart1.Series.Add(dataPointSeries_G);
            chart1.Series.Add(dataPointSeries_B);
            chart2.Series.Add(dataPointSeries_Gray);
            chart3.Series.Add(dataPointSeries_R_cdf);
            chart3.Series.Add(dataPointSeries_G_cdf);
            chart3.Series.Add(dataPointSeries_B_cdf);
            chart4.Series.Add(dataPointSeries_Gray_cdf);
        }
        public void chart_value_setup()
        {
            for (int Ycount = 0; Ycount < bitmap.Height; Ycount++)
            {
                for (int Xcount = 0; Xcount < bitmap.Width; Xcount++)
                {
                    int R = read_photo[Xcount, Ycount].R;
                    int G = read_photo[Xcount, Ycount].G;
                    int B = read_photo[Xcount, Ycount].B;
                    int Gray = (int)(R * 0.299 + G * 0.587 + B * 0.114);

                    histogramValue_R[R]++;
                    histogramValue_G[G]++;
                    histogramValue_B[B]++;
                    histogramValue_Gray[Gray]++;

                    //find the max color in picture
                    max[0] = max[0] < R ? R : max[0];
                    max[1] = max[1] < G ? G : max[1];
                    max[2] = max[2] < B ? B : max[2];
                    max[3] = max[3] < Gray ? Gray : max[2];

                    //find the min color in picture
                    min[0] = min[0] > R ? R : min[0];
                    min[1] = min[1] > G ? G : min[1];
                    min[2] = min[2] > B ? B : min[2];
                    min[3] = min[3] > Gray ? Gray : min[2];
                }
            }

            int[] total = new int[4];
            Array.Clear(total, 0, total.Length);

            //cumulate cdf array
            for (int count = 0; count < histogramValue_R.Length; count++)
            {
                total[0] += histogramValue_R[count];
                total[1] += histogramValue_G[count];
                total[2] += histogramValue_B[count];
                total[3] += histogramValue_Gray[count];

                cdf_min[0] = (cdf_min[0] > histogramValue_R[count] && histogramValue_R[count]>0) ? histogramValue_R[count] : cdf_min[0];
                cdf_min[1] = (cdf_min[1] > histogramValue_G[count] && histogramValue_G[count]>0) ? histogramValue_G[count] : cdf_min[1];
                cdf_min[2] = (cdf_min[2] > histogramValue_B[count] && histogramValue_B[count]>0) ? histogramValue_B[count] : cdf_min[2];
                cdf_min[3] = (cdf_min[3] > histogramValue_Gray[count] && histogramValue_Gray[count]>0) ? histogramValue_Gray[count] : cdf_min[3];

                cdf_Value_R[count]    = total[0];
                cdf_Value_G[count]    = total[1];
                cdf_Value_B[count]    = total[2];
                cdf_Value_Gray[count] = total[3];
            }


            if(Histogram_mode == mode.Histogram)
                for (int i = 0; i <= 255; i++)
                {
                    chart3.Series["Red_cdf"  ].Points.AddXY(i, cdf_Value_R[i]);
                    chart3.Series["Blue_cdf"].Points.AddXY(i, cdf_Value_G[i]);
                    chart3.Series["Green_cdf"].Points.AddXY(i, cdf_Value_B[i]);
                    chart4.Series["Gray_cdf"].Points.AddXY(i, cdf_Value_Gray[i]);
                }


            //chart1.Series["Series 1"]["PixelPointWidth"] = "1";
        }
        public void drawHistogram()
        {
            chart_value_setup();
            for (int Ycount = 0; Ycount < bitmap.Height; Ycount++)
            {
                for (int Xcount = 0; Xcount < bitmap.Width; Xcount++)
                {
                    int R = read_photo[Xcount, Ycount].R;
                    int G = read_photo[Xcount, Ycount].G;
                    int B = read_photo[Xcount, Ycount].B;
                    int Gray = (int)(R * 0.299 + G * 0.587 + B * 0.114);

                    if (Histogram_mode == mode.Histogram_Equalization)
                    {
                        //histogram equalization color
                        R = (int)(((cdf_Value_R[R] - cdf_min[0]) / (double)(cdf_max[0] - cdf_min[0])) * (bit_depth - 1));
                        G = (int)(((cdf_Value_G[G] - cdf_min[1]) / (double)(cdf_max[1] - cdf_min[1])) * (bit_depth - 1));
                        B = (int)(((cdf_Value_B[B] - cdf_min[2]) / (double)(cdf_max[2] - cdf_min[2])) * (bit_depth - 1));
                        Gray = (int)(((cdf_Value_Gray[Gray] - cdf_min[3]) / (double)(cdf_max[3] - cdf_min[3])) * (bit_depth - 1));
                        new_histogramValue_R[R]++;
                        new_histogramValue_G[G]++;
                        new_histogramValue_B[B]++;
                        new_histogramValue_Gray[Gray]++;
                    }

                    g_Image.FillRectangle(new SolidBrush(Color.FromArgb(R, G, B)), Xcount, Ycount, 1, 1);
                    g_grayImage.FillRectangle(new SolidBrush(Color.FromArgb(Gray, Gray, Gray)), Xcount, Ycount, 1, 1);

                }
            }

            if (Histogram_mode == mode.Histogram_Equalization)
            {
                int[] total = new int[4];
                Array.Clear(total, 0, total.Length);

                //cumulate new cdf array
                for (int count = 0; count < histogramValue_R.Length; count++)
                {
                    total[0] += new_histogramValue_R[count];
                    total[1] += new_histogramValue_G[count];
                    total[2] += new_histogramValue_B[count];
                    total[3] += new_histogramValue_Gray[count];

                    cdf_Value_R[count] = total[0];
                    cdf_Value_G[count] = total[1];
                    cdf_Value_B[count] = total[2];
                    cdf_Value_Gray[count] = total[3];
                }

            
                for (int i = 0; i <= 255; i++)
                {
                    chart1.Series["Red"  ].Points.AddXY(i, new_histogramValue_R[i]);
                    chart1.Series["Blue" ].Points.AddXY(i,new_histogramValue_G[i]);
                    chart1.Series["Green"].Points.AddXY(i,new_histogramValue_B[i]);
                    chart2.Series["Gray" ].Points.AddXY(i, new_histogramValue_Gray[i]);

                    chart3.Series["Red_cdf"].Points.AddXY(i, cdf_Value_R[i]);
                    chart3.Series["Blue_cdf"].Points.AddXY(i, cdf_Value_G[i]);
                    chart3.Series["Green_cdf"].Points.AddXY(i, cdf_Value_B[i]);
                    chart4.Series["Gray_cdf"].Points.AddXY(i, cdf_Value_Gray[i]);
                }
            }
            else if(Histogram_mode == mode.Histogram)
            {
                for (int i = 0; i <= 255; i++)
                {
                    chart1.Series["Red"].Points.AddXY(i, histogramValue_R[i]);
                    chart1.Series["Blue"].Points.AddXY(i,histogramValue_G[i]);
                    chart1.Series["Green"].Points.AddXY(i,histogramValue_B[i]);
                    chart2.Series["Gray"].Points.AddXY(i,histogramValue_Gray[i]);

                }
            }
        }
        public Form_Histogram_Equalization()
        {

            InitializeComponent();
            chart_setup();
            max = new int[4];
            min = new int[4];
            Array.Clear(max, 0, max.Length);

            arr_init(min, int.MaxValue);
            Text = "Historgram Equalization";
            checkBox1.Checked = true;
            checkBox2.Checked = true;
            checkBox3.Checked = true;
            checkBox4.Checked = true;
            
        }
        public void arr_init(int []arr,int value)
        {
            for (int i = 0; i < arr.Length; i++)
                arr[i] = value;
        }

        private void Form_Histogram_Equalization_Shown(object sender, EventArgs e)
        {
            histogramValue_R = new int[256];
            histogramValue_G = new int[256];
            histogramValue_B = new int[256];
            histogramValue_Gray = new int[256];
            Array.Clear(histogramValue_R, 0, histogramValue_R.Length);
            Array.Clear(histogramValue_G, 0, histogramValue_G.Length);
            Array.Clear(histogramValue_B, 0, histogramValue_B.Length);
            Array.Clear(histogramValue_Gray, 0, histogramValue_Gray.Length);

            new_histogramValue_R = new int[256];
            new_histogramValue_G = new int[256];
            new_histogramValue_B = new int[256];
            new_histogramValue_Gray = new int[256];
            Array.Clear(new_histogramValue_R, 0, new_histogramValue_R.Length);
            Array.Clear(new_histogramValue_G, 0, new_histogramValue_G.Length);
            Array.Clear(new_histogramValue_B, 0, new_histogramValue_B.Length);
            Array.Clear(new_histogramValue_Gray, 0, new_histogramValue_Gray.Length);

            cdf_Value_R = new int[256];
            cdf_Value_G = new int[256];
            cdf_Value_B = new int[256];
            cdf_Value_Gray = new int[256];
            Array.Clear(cdf_Value_R, 0,    cdf_Value_R.Length);
            Array.Clear(cdf_Value_G, 0,    cdf_Value_G.Length);
            Array.Clear(cdf_Value_B, 0,    cdf_Value_B.Length);
            Array.Clear(cdf_Value_Gray, 0, cdf_Value_Gray.Length);

            

            cdf_max = new int[4];
            cdf_min = new int[4];
            arr_init(cdf_max, bitmap.Width * bitmap.Height);

            arr_init(cdf_min, int.MaxValue);

            bit_depth = 256;
            grayImage = new Bitmap(bitmap.Width, bitmap.Height);
            g_grayImage = Graphics.FromImage(grayImage);
            Historgram_Equalization_bitmap = new Bitmap(bitmap.Width, bitmap.Height);
            g_Image = Graphics.FromImage(Historgram_Equalization_bitmap);

            drawHistogram();

            pictureBox1.Image = Historgram_Equalization_bitmap;
            pictureBox2.Image = grayImage;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked == true)
            {
                chart1.Series["Red"].Enabled = true;
                chart3.Series["Red_cdf"].Enabled = true;
            }
            else
            {
                chart1.Series["Red"].Enabled = false;
                chart3.Series["Red_cdf"].Enabled = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                chart1.Series["Green"].Enabled = true;
                chart3.Series["Green_cdf"].Enabled = true;
            }
            else
            {
                chart1.Series["Green"].Enabled = false;
                chart3.Series["Green_cdf"].Enabled = false;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked == true)
            {
                chart1.Series["Blue"].Enabled = true;
                chart3.Series["Blue_cdf"].Enabled = true;
            }
            else
            {
                chart1.Series["Blue"].Enabled = false;
                chart3.Series["Blue_cdf"].Enabled = false;
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked == true)
            {
                chart2.Series["Gray"].Enabled = true;
                chart4.Series["Gray_cdf"].Enabled = true;
            }
            else
            {
                chart2.Series["Gray"].Enabled = false;
                chart4.Series["Gray_cdf"].Enabled = false;
            }
        }
    }
}
