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
    public partial class Form_Huffman : Form
    {

        public myPicture image;
        Dictionary<int ,double> color_Probability;
        int[] histogramValue_Gray;
        int max_length;
        public Form_Huffman()
        {
            InitializeComponent();
        }

        private void Form_Huffman_Shown(object sender, EventArgs e)
        {
            histogramValue_Gray = new int[256];
            Array.Clear(histogramValue_Gray, 0, histogramValue_Gray.Length);
            max_length = image.Height * image.Width;

            for (int Ycount = 0; Ycount < image.Height; Ycount++)           
                for (int Xcount = 0; Xcount < image.Width; Xcount++)                
                    histogramValue_Gray[image.my_Pixel[Xcount,Ycount].Gray]++;

            color_Probability = new Dictionary<int, double>();

            for (int i = 0; i < 256; i++)
                if (histogramValue_Gray[i] != 0)
                    color_Probability.Add(i, histogramValue_Gray[i]/(double)max_length);

            var sortedDict = from entry in color_Probability orderby entry.Value ascending select entry;

            richTextBox1.Text = "";
            foreach (var item in color_Probability.OrderBy(value => value.Value))
            {
                richTextBox1.AppendText(item.Key + " : " + item.Value*100+" %\n");

            }
            richTextBox1.AppendText("\n\n" + color_Probability.Count());


        }
    }
}
