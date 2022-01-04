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
    public partial class Form_Constrast_Stretching : Form
    {
        public Bitmap bitmap { get; set; }
        public Bitmap stretchingImage { get; set; }

        Graphics g_stretching;

        int[] histogramValue_R;
        int[] histogramValue_G;
        int[] histogramValue_B;
        int[] histogramValue_Gray;
        int histogram_Max = 0;

        public myPixel[,] read_photo;

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

                    histogramValue_R[R]++;
                    histogramValue_G[G]++;
                    histogramValue_B[B]++;

                }
            }
            foreach (int max in histogramValue_R)
                if (max > histogram_Max)
                    histogram_Max = max;
            foreach (int max in histogramValue_G)
                if (max > histogram_Max)
                    histogram_Max = max;
            foreach (int max in histogramValue_B)
                if (max > histogram_Max)
                    histogram_Max = max;
            histogram_Max += 1000;


            chart1.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;
            chart1.ChartAreas[0].AxisY.MajorGrid.LineWidth = 0;
            chart1.ChartAreas[0].AxisX.Minimum = -10;
            chart1.ChartAreas[0].AxisX.Maximum = 275;
            chart1.ChartAreas[0].AxisX.Interval = 20;
            chart1.ChartAreas[0].AxisY2.Enabled = AxisEnabled.False;

            var dataPointSeries_R = new Series
            {
                Name = "Red",
                Color = Color.Red,
                ChartType = SeriesChartType.Column
            };
            var dataPointSeries_G = new Series
            {
                Name = "Green",
                Color = Color.LimeGreen,
                ChartType = SeriesChartType.Column
            };
            var dataPointSeries_B = new Series
            {
                Name = "Blue",
                Color = Color.Blue,
                ChartType = SeriesChartType.Column
            };
            var dataPointSeries_Gray = new Series
            {
                Name = "Gray",
                Color = Color.Black,
                ChartType = SeriesChartType.Column
            };
            var dataPointSeries_R1 = new Series
            {
                Name = "r1",
                Color = Color.Red,
                ChartType = SeriesChartType.Column
            };
            var dataPointSeries_R2 = new Series
            {
                Name = "r2",
                Color = Color.Red,
                ChartType = SeriesChartType.Column
            };
            var dataPointSeries_S1 = new Series
            {
                Name = "s1",
                Color = Color.Blue,
                ChartType = SeriesChartType.Column
            };
            var dataPointSeries_S2 = new Series
            {
                Name = "s2",
                Color = Color.Blue,
                ChartType = SeriesChartType.Column
            };
            

            dataPointSeries_R1.Points.AddXY(0, histogram_Max);
            dataPointSeries_R2.Points.AddXY(255, histogram_Max);
            dataPointSeries_S1.Points.AddXY(0, histogram_Max);
            dataPointSeries_S2.Points.AddXY(255, histogram_Max);

            for (int i = 0; i <= 255; i++)
            {
                dataPointSeries_R.Points.AddXY(i, histogramValue_R[i]);
                dataPointSeries_G.Points.AddXY(i, histogramValue_G[i]);
                dataPointSeries_B.Points.AddXY(i, histogramValue_B[i]);
            }

            chart1.Series.Add(dataPointSeries_R);
            chart1.Series.Add(dataPointSeries_G);
            chart1.Series.Add(dataPointSeries_B);
            chart1.Series.Add(dataPointSeries_R1);
            chart1.Series.Add(dataPointSeries_R2);
            chart1.Series.Add(dataPointSeries_S1);
            chart1.Series.Add(dataPointSeries_S2);
        }

        public void drawStretchingImage(double r1, double r2, double s1, double s2)
        {

            for (int Ycount = 0; Ycount < bitmap.Height; Ycount++)
            {
                for (int Xcount = 0; Xcount < bitmap.Width; Xcount++)
                {
                    int R = read_photo[Xcount, Ycount].R;
                    int G = read_photo[Xcount, Ycount].G;
                    int B = read_photo[Xcount, Ycount].B;

                    double y = 0;

                    if (R < r1)
                    {
                        y = (R / r1) * s1;
                    }
                    else if (R <= r2)
                    {
                        y = ((R - r1) / (r2 - r1)) * (s2 - s1) + s1;
                    }
                    else if(R > r2 )
                    {
                        y = ((R - r2) / (255 - r2)) * (255 - s2) + s2;
                    }
                    else
                    {
                        y = R;
                    }
                    int stretch_R = (int)Math.Round(y);

                    if (G < r1)
                    {
                        y = (G / r1) * s1;
                    }
                    else if (G <= r2)
                    {
                        y = ((G - r1) / (r2 - r1)) * (s2 - s1) + s1;
                    }
                    else if (G > r2)
                    {
                        y = ((G - r2) / (255 - r2)) * (255 - s2) + s2;
                    }
                    else
                    {
                        y = G;
                    }
                    int stretch_G = (int)Math.Round(y);

                    if (B < r1)
                    {
                        y = (B / r1) * s1;
                    }
                    else if (B <= r2)
                    {
                        y = ((B - r1) / (r2 - r1)) * (s2 - s1) + s1;
                    }
                    else if (B > r2)
                    {
                        y = ((B - r2) / (255 - r2)) * (255 - s2) + s2;
                    }
                    else
                    {
                        y = B;
                    }
                    int stretch_B = (int)Math.Round(y);

                    g_stretching.FillRectangle(new SolidBrush(Color.FromArgb(stretch_R, stretch_G, stretch_B)), Xcount, Ycount, 1, 1);

                }
            }

        }

        public Form_Constrast_Stretching()
        {
            InitializeComponent();
            histogramValue_R = new int[256];
            histogramValue_G = new int[256];
            histogramValue_B = new int[256];
            histogramValue_Gray = new int[256];
            Array.Clear(histogramValue_R, 0, histogramValue_R.Length);
            Array.Clear(histogramValue_G, 0, histogramValue_G.Length);
            Array.Clear(histogramValue_B, 0, histogramValue_B.Length);
            Array.Clear(histogramValue_Gray, 0, histogramValue_Gray.Length);
        }

        private void Form_Constrast_Stretching_Shown(object sender, EventArgs e)
        {
            Text = "Image Constrast Stretching";
            stretchingImage = new Bitmap(bitmap.Width, bitmap.Height);
            g_stretching = Graphics.FromImage(stretchingImage);

            int i;
            bool valid = int.TryParse(textBox1.Text, out i);//check value is valid
            int r1 = valid ? i : 0;

            valid = int.TryParse(textBox2.Text, out i);//check value is valid
            int r2 = valid ? i : 0;

            valid = int.TryParse(textBox3.Text, out i);//check value is valid
            int s1 = valid ? i : 0;

            valid = int.TryParse(textBox4.Text, out i);//check value is valid
            int s2 = valid ? i : 0;

            drawStretchingImage(r1,r2,s1,s2);
            chart_setup();
            pictureBox2.Image = bitmap;
            pictureBox1.Image = stretchingImage;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double d;
            bool valid = double.TryParse(textBox1.Text, out d);//check value is valid
            double r1 = valid ? d : 0;
            if (r1 < 0 || r1 > 255) r1 = 255;

            valid = double.TryParse(textBox2.Text, out d);//check value is valid
            double r2 = valid ? d : 0;
            if (r2 < 0 || r2 > 255) r2 = 255;

            valid = double.TryParse(textBox3.Text, out d);//check value is valid
            double s1 = valid ? d : 0;
            if (s1 < 0 || s1 > 255) s1 = 255;

            valid = double.TryParse(textBox4.Text, out d);//check value is valid
            double s2 = valid ? d : 0;
            if (s2 < 0 || s2 > 255) s2 = 255;

            if(r1 < 0 || r1 > 255 || r2 < 0 || r2 > 255 || s1 < 0 || s1 > 255 || s2 < 0 || s2 > 255)
                textBox1.Text = textBox2.Text = textBox3.Text = textBox4.Text ="255";

            drawStretchingImage(r1, r2, s1, s2);
            chart1.Series["r1"].Points.RemoveAt(0);
            chart1.Series["r1"].Points.AddXY((int)r1, histogram_Max);

            chart1.Series["r2"].Points.RemoveAt(0);
            chart1.Series["r2"].Points.AddXY((int)r2, histogram_Max);

            chart1.Series["s1"].Points.RemoveAt(0);
            chart1.Series["s1"].Points.AddXY((int)s1, histogram_Max);

            chart1.Series["s2"].Points.RemoveAt(0);
            chart1.Series["s2"].Points.AddXY((int)s2, histogram_Max);

            pictureBox1.Image = stretchingImage;
        }
    }
}
