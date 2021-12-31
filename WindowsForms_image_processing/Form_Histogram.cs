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
    public partial class Form_Histogram : Form
    {
        public Bitmap bitmap { get; set; }
        public Bitmap grayImage { get; set; }

        Graphics g_grayImage;

        int[] histogramValue_R;
        int[] histogramValue_G;
        int[] histogramValue_B;
        int[] histogramValue_Gray;

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

            for (int i = 0; i <= 255; i++)
            {
                dataPointSeries_R.Points.AddXY(i, histogramValue_R[i]);
                dataPointSeries_G.Points.AddXY(i, histogramValue_G[i]);
                dataPointSeries_B.Points.AddXY(i, histogramValue_B[i]);
                dataPointSeries_Gray.Points.AddXY(i, histogramValue_Gray[i]);
            }

            chart1.Series.Add(dataPointSeries_R);
            chart1.Series.Add(dataPointSeries_G);
            chart1.Series.Add(dataPointSeries_B);
            chart2.Series.Add(dataPointSeries_Gray);
            //chart1.Series["Series 1"]["PixelPointWidth"] = "1";
        }
        public void drawHistogram()
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

                    g_grayImage.FillRectangle(new SolidBrush(Color.FromArgb(Gray, Gray, Gray)),  Xcount,Ycount, 1, 1);

                }
            }
            chart_setup();
        }
        public Form_Histogram()
        {
            InitializeComponent();
        }

        private void Form_Histogram_Shown(object sender, EventArgs e)
        {
            histogramValue_R = new int[256];
            histogramValue_G = new int[256];
            histogramValue_B = new int[256];
            histogramValue_Gray = new int[256];
            Array.Clear(histogramValue_R, 0, histogramValue_R.Length);
            Array.Clear(histogramValue_G, 0, histogramValue_G.Length);
            Array.Clear(histogramValue_B, 0, histogramValue_B.Length);
            Array.Clear(histogramValue_Gray, 0, histogramValue_Gray.Length);
            pictureBox1.Image = bitmap;
            grayImage = new Bitmap(bitmap.Width, bitmap.Height);
            g_grayImage = Graphics.FromImage(grayImage);
            drawHistogram();
            pictureBox2.Image = grayImage;
        }
    }
}
