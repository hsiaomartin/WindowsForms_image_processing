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
using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsForms_image_processing
{
    public partial class Form_Histogram_Specification : Form
    {

        public Bitmap bitmap { get; set; }
        public Bitmap target_bitmap { get; set; }
        public myPicture target_data;
        public Graphics g_Target;
        myHistogram target_Histogram;

        Graphics g_Image;

        int[] histogramValue_R;
        int[] histogramValue_G;
        int[] histogramValue_B;


        int[] cdf_Value_R;
        int[] cdf_Value_G;
        int[] cdf_Value_B;




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

            var dataPointSeries_R_Target = new Series
            {
                Name = "Red_Target",
                Color = Color.Red,
                ChartType = SeriesChartType.Column
            };
            var dataPointSeries_R_Target_cdf = new Series
            {
                Name = "Red_Target_cdf",
                Color = Color.Red,
                ChartType = SeriesChartType.Line
            };
            var dataPointSeries_G_Target = new Series
            {
                Name = "Green_Target",
                Color = Color.LimeGreen,
                ChartType = SeriesChartType.Column
            };
            var dataPointSeries_G_Target_cdf = new Series
            {
                Name = "Green_Target_cdf",
                Color = Color.LimeGreen,
                ChartType = SeriesChartType.Line
            };
            var dataPointSeries_B_Target = new Series
            {
                Name = "Blue_Target",
                Color = Color.Blue,
                ChartType = SeriesChartType.Column
            };
            var dataPointSeries_B_Target_cdf = new Series
            {
                Name = "Blue_Target_cdf",
                Color = Color.Blue,
                ChartType = SeriesChartType.Line
            };

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

            chart1.Series.Add(dataPointSeries_R);
            chart1.Series.Add(dataPointSeries_G);
            chart1.Series.Add(dataPointSeries_B);
            chart2.Series.Add(dataPointSeries_R_Target);
            chart2.Series.Add(dataPointSeries_G_Target);
            chart2.Series.Add(dataPointSeries_B_Target);
            chart3.Series.Add(dataPointSeries_R_cdf);
            chart3.Series.Add(dataPointSeries_G_cdf);
            chart3.Series.Add(dataPointSeries_B_cdf);
            chart4.Series.Add(dataPointSeries_R_Target_cdf);
            chart4.Series.Add(dataPointSeries_G_Target_cdf);
            chart4.Series.Add(dataPointSeries_B_Target_cdf);
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


                    histogramValue_R[R]++;
                    histogramValue_G[G]++;
                    histogramValue_B[B]++;


                    //find the max color in picture
                    max[0] = max[0] < R ? R : max[0];
                    max[1] = max[1] < G ? G : max[1];
                    max[2] = max[2] < B ? B : max[2];


                    //find the min color in picture
                    min[0] = min[0] > R ? R : min[0];
                    min[1] = min[1] > G ? G : min[1];
                    min[2] = min[2] > B ? B : min[2];

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


                cdf_min[0] = (cdf_min[0] > histogramValue_R[count] && histogramValue_R[count] > 0) ? histogramValue_R[count] : cdf_min[0];
                cdf_min[1] = (cdf_min[1] > histogramValue_G[count] && histogramValue_G[count] > 0) ? histogramValue_G[count] : cdf_min[1];
                cdf_min[2] = (cdf_min[2] > histogramValue_B[count] && histogramValue_B[count] > 0) ? histogramValue_B[count] : cdf_min[2];


                cdf_Value_R[count] = total[0];
                cdf_Value_G[count] = total[1];
                cdf_Value_B[count] = total[2];



            }



            for (int i = 0; i <= 255; i++)
            {
                chart3.Series["Red_cdf"].Points.AddXY(i, cdf_Value_R[i]);
                chart3.Series["Blue_cdf"].Points.AddXY(i, cdf_Value_G[i]);
                chart3.Series["Green_cdf"].Points.AddXY(i, cdf_Value_B[i]);

            }

        }

        public void chart_value_setup(myPixel[,] pixels,Size size,myHistogram histogram)
        {
            
            for (int Ycount = 0; Ycount < size.Height; Ycount++)
            {
                for (int Xcount = 0; Xcount < size.Width; Xcount++)
                {
                    int R = pixels[Xcount, Ycount].R;
                    int G = pixels[Xcount, Ycount].G;
                    int B = pixels[Xcount, Ycount].B;

                    histogram.R[R]++;
                    histogram.G[G]++;
                    histogram.B[B]++;

                }
            }

            int[] total = new int[4];
            Array.Clear(total, 0, total.Length);

            //cumulate cdf array
            for (int count = 0; count < bit_depth; count++)
            {
                total[0] += histogram.R[count];
                total[1] += histogram.G[count];
                total[2] += histogram.B[count];

                histogram.Cdf_R[count] = total[0];
                histogram.Cdf_G[count] = total[1];
                histogram.Cdf_B[count] = total[2];

                for(int i = histogram.Cdf_R[count]- histogram.R[count]; i<   histogram.Cdf_R[count]; i++)
                    histogram.r_R[i] = count;

                for (int i = histogram.Cdf_G[count] - histogram.G[count]; i < histogram.Cdf_G[count]; i++)
                    histogram.r_G[i] = count;

                for (int i = histogram.Cdf_B[count] - histogram.B[count]; i < histogram.Cdf_B[count]; i++)
                    histogram.r_B[i] = count;

              
                


                Console.WriteLine(histogram.r_R[histogram.Cdf_R[count]]);
            }
        }
        public void drawHistogram()
        {

            for (int i = 0; i <= 255; i++)
            {
                chart1.Series["Red"].Points.AddXY(i, histogramValue_R[i]);
                chart1.Series["Blue"].Points.AddXY(i, histogramValue_G[i]);
                chart1.Series["Green"].Points.AddXY(i, histogramValue_B[i]);


            }
            
        }
        public void drawGraphic(Graphics g,myPixel[,] pixels,Size size)
        {
            for (int Ycount = 0; Ycount < size.Height; Ycount++)
            {
                for (int Xcount = 0; Xcount < size.Width; Xcount++)
                {
                    int R = pixels[Xcount, Ycount].R;
                    int G = pixels[Xcount, Ycount].G;
                    int B = pixels[Xcount, Ycount].B;

                    g.FillRectangle(new SolidBrush(Color.FromArgb(R, G, B)), Xcount, Ycount, 1, 1);
                }
            }

        }
        public Form_Histogram_Specification()
        {
            InitializeComponent();
            chart_setup();
            bit_depth = 256;
            max = new int[4];
            min = new int[4];
            Array.Clear(max, 0, max.Length);

            arr_init(min, int.MaxValue);

            checkBox1.Checked = true;
            checkBox2.Checked = true;
            checkBox3.Checked = true;
        }

        public void arr_init(int[] arr, int value)
        {
            for (int i = 0; i < arr.Length; i++)
                arr[i] = value;
        }

        private void Form_Histogram_Specification_Shown(object sender, EventArgs e)
        {
            Text = "Historgram Specification";

            histogramValue_R = new int[256];
            histogramValue_G = new int[256];
            histogramValue_B = new int[256];

            Array.Clear(histogramValue_R, 0, histogramValue_R.Length);
            Array.Clear(histogramValue_G, 0, histogramValue_G.Length);
            Array.Clear(histogramValue_B, 0, histogramValue_B.Length);


            cdf_Value_R = new int[256];
            cdf_Value_G = new int[256];
            cdf_Value_B = new int[256];

            Array.Clear(cdf_Value_R, 0, cdf_Value_R.Length);
            Array.Clear(cdf_Value_G, 0, cdf_Value_G.Length);
            Array.Clear(cdf_Value_B, 0, cdf_Value_B.Length);


            cdf_max = new int[4];
            cdf_min = new int[4];
            arr_init(cdf_max, bitmap.Width * bitmap.Height);

            arr_init(cdf_min, int.MaxValue);

            bit_depth = 256;

            bitmap = new Bitmap(bitmap.Width, bitmap.Height);
            g_Image = Graphics.FromImage(bitmap);
            drawGraphic(g_Image, read_photo, new Size(bitmap.Width, bitmap.Height));
            pictureBox1.Image = bitmap;

            chart_value_setup();
            drawHistogram();



        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                chart1.Series["Red"].Enabled = true;
                chart3.Series["Red_cdf"].Enabled = true;
                
                chart2.Series["Red_Target"].Enabled = true;
                chart4.Series["Red_Target_cdf"].Enabled = true;
            }
            else
            {
                chart1.Series["Red"].Enabled = false;
                chart3.Series["Red_cdf"].Enabled = false;

                chart2.Series["Red_Target"].Enabled = false;
                chart4.Series["Red_Target_cdf"].Enabled = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                chart1.Series["Green"].Enabled = true;
                chart3.Series["Green_cdf"].Enabled = true;
                
                chart2.Series["Green_Target"].Enabled = true;
                chart4.Series["Green_Target_cdf"].Enabled = true;
            }
            else
            {
                chart1.Series["Green"].Enabled = false;
                chart3.Series["Green_cdf"].Enabled = false;

                chart2.Series["Green_Target"].Enabled = false;
                chart4.Series["Green_Target_cdf"].Enabled = false;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked == true)
            {
                chart1.Series["Blue"].Enabled = true;
                chart3.Series["Blue_cdf"].Enabled = true;
                
                chart2.Series["Blue_Target"].Enabled = true;
                chart4.Series["Blue_Target_cdf"].Enabled = true;
            }
            else
            {
                chart1.Series["Blue"].Enabled = false;
                chart3.Series["Blue_cdf"].Enabled = false;

                chart2.Series["Blue_Target"].Enabled = false;
                chart4.Series["Blue_Target_cdf"].Enabled = false;
            }
        }

        public void drawMyImgData(myPicture picture_data,Graphics g)
        {
            //myPicture my_Picture;
            int w = picture_data.Width;
            int h = picture_data.Height;
            //e.Graphics.Clear(Color.Black);
            g.Clear(Color.Black);
            //my_Picture = new myPicture(w,h );

            for (int Ycount = h - 1; Ycount >= 0; Ycount--)
            {
                for (int Xcount = 0; Xcount < w; Xcount++)
                {

                    int R = picture_data.my_Pixel[Xcount, Ycount].R;
                    int G = picture_data.my_Pixel[Xcount, Ycount].G;
                    int B = picture_data.my_Pixel[Xcount, Ycount].B;
                    g.FillRectangle(new SolidBrush(Color.FromArgb(R, G, B)), Xcount, Ycount, 1, 1);

                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var filePath = string.Empty;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                //openFileDialog.InitialDirectory = filePath;
                openFileDialog.Filter = "image files (*.bmp)|*.bmp|image files (*.pcx)|*.pcx|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;
                }
            }
            if (filePath != "")
            {
                chart2.Series["Red_Target"].Points.Clear();
                chart2.Series["Blue_Target"].Points.Clear();
                chart2.Series["Green_Target"].Points.Clear();

                chart4.Series["Red_Target_cdf"].Points.Clear();
                chart4.Series["Blue_Target_cdf"].Points.Clear();
                chart4.Series["Green_Target_cdf"].Points.Clear();

                if (Path.GetExtension(filePath) == ".bmp")
                {
                    read_Bitmap read_bitmap = new read_Bitmap(filePath);
                    target_data = new myPicture(read_bitmap.info_header.biWidth, read_bitmap.info_header.biHeight);                    
                    target_data.my_Pixel = read_bitmap.myRowData;
                    target_bitmap = new Bitmap(target_data.Width, target_data.Height);
                    g_Target = Graphics.FromImage(target_bitmap);
                    drawMyImgData(target_data,g_Target);
                }
                else if (Path.GetExtension(filePath) == ".pcx")
                {
                    readMidResource read_pcx = new readMidResource(filePath);
                    target_data = new myPicture(read_pcx.FileHead.Width, read_pcx.FileHead.Height);
                    target_data.my_Pixel = read_pcx.myRowData;
                    target_bitmap = read_pcx.getDecoImage;
                }
                else
                {
                    MessageBox.Show(Path.GetExtension(filePath) + " file didn't implement.");
                }
                target_Histogram = new myHistogram(bit_depth, target_data.picture_size.Width* target_data.picture_size.Height);

                chart_value_setup(target_data.my_Pixel, target_data.picture_size, target_Histogram);

                for (int i = 0; i < bit_depth; i++)
                {
                    cdf_min[0] = (cdf_min[0] > target_Histogram.R[i] && target_Histogram.R[i] > 0) ? target_Histogram.R[i] : cdf_min[0];
                    cdf_min[1] = (cdf_min[1] > target_Histogram.G[i] && target_Histogram.G[i] > 0) ? target_Histogram.G[i] : cdf_min[1];
                    cdf_min[2] = (cdf_min[2] > target_Histogram.B[i] && target_Histogram.B[i] > 0) ? target_Histogram.B[i] : cdf_min[2];
                    

                    chart2.Series["Red_Target"].Points.AddXY(i,   target_Histogram.R[i]);
                    chart2.Series["Blue_Target"].Points.AddXY(i,  target_Histogram.G[i]);
                    chart2.Series["Green_Target"].Points.AddXY(i, target_Histogram.B[i]);

                    chart4.Series["Red_Target_cdf"].Points.AddXY(i,   target_Histogram.Cdf_R[i]);
                    chart4.Series["Blue_Target_cdf"].Points.AddXY(i,  target_Histogram.Cdf_G[i]);
                    chart4.Series["Green_Target_cdf"].Points.AddXY(i, target_Histogram.Cdf_B[i]);

                }
                pictureBox2.Image = target_bitmap;

            }
            button2.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            myHistogram new_Histogram = new myHistogram(bit_depth, target_data.Width*target_data.Height);
            myPixel[,] pixels = new myPixel[target_data.Width,target_data.Height];
            Size size = new Size(target_data.Width, target_data.Height);
            for (int Ycount = 0; Ycount < target_data.Height; Ycount++)
            {
                for (int Xcount = 0; Xcount < target_data.Width; Xcount++)
                {
                    int R = read_photo[Xcount, Ycount].R;
                    int G = read_photo[Xcount, Ycount].G;
                    int B = read_photo[Xcount, Ycount].B;

                    R = target_Histogram.r_R[cdf_Value_R[R]];
                    G = target_Histogram.r_G[cdf_Value_G[G]];
                    B = target_Histogram.r_B[cdf_Value_B[B]];

                    pixels[Xcount, Ycount] = new myPixel(R, G, B);

                    g_Image.FillRectangle(new SolidBrush(Color.FromArgb(R, G, B)), Xcount, Ycount, 1, 1);
                }
            }
            chart_value_setup(pixels,size, new_Histogram);

            chart1.Series["Red"].Points.Clear();
            chart1.Series["Blue"].Points.Clear();
            chart1.Series["Green"].Points.Clear();

            chart3.Series["Red_cdf"].Points.Clear();
            chart3.Series["Blue_cdf"].Points.Clear();
            chart3.Series["Green_cdf"].Points.Clear();

            for (int i = 0; i < bit_depth; i++)
            {

                chart1.Series["Red"].Points.AddXY(i, new_Histogram.R[i]);
                chart1.Series["Blue"].Points.AddXY(i, new_Histogram.G[i]);
                chart1.Series["Green"].Points.AddXY(i, new_Histogram.B[i]);

                chart3.Series["Red_cdf"].Points.AddXY(i, new_Histogram.Cdf_R[i]);
                chart3.Series["Blue_cdf"].Points.AddXY(i, new_Histogram.Cdf_G[i]);
                chart3.Series["Green_cdf"].Points.AddXY(i, new_Histogram.Cdf_B[i]);

            }

            pictureBox1.Image = bitmap;
        }
    }
}
