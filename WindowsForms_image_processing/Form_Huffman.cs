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
    public partial class Form_Huffman : Form
    {

        public myPicture image;
        Dictionary<int ,int> color_Probability;
        int[] histogramValue_Gray;

        
        List<encode> huffman_encode;
        int max_Length;
        public void chart_setup()
        {
            chart2.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;
            chart2.ChartAreas[0].AxisY.MajorGrid.LineWidth = 0;
            chart2.ChartAreas[0].AxisX.Minimum = -10;
            chart2.ChartAreas[0].AxisX.Maximum = 275;
            chart2.ChartAreas[0].AxisX.Interval = 20;
            chart2.ChartAreas[0].AxisY2.Enabled = AxisEnabled.False;

            var dataPointSeries_Color = new Series
            {
                Name = "Color",
                Color = Color.Gray,
                ChartType = SeriesChartType.Column
            };
            chart2.Series.Add(dataPointSeries_Color);

        }

        public void drawHistogram(Dictionary<int, int> D)
        {

            foreach(var d in D)
            {               
                chart2.Series["Color"].Points.AddXY("", d.Value);
                
            }
            

        }
        public Form_Huffman()
        {
            InitializeComponent();
            chart_setup();
        }
        public void setDataGridView(List<encode> encodes)
        {
            dataGridView1.ColumnCount = 4;                             // 定義所需要的行數
            //dataGridView1.Columns[0].Name = "Number";
            dataGridView1.Columns[0].Name = "Intensity";
            dataGridView1.Columns[1].Name = "Probability(%)";
            dataGridView1.Columns[2].Name = "Huffman code";

            //是否允許使用者編輯
            dataGridView1.ReadOnly = true;
            int i = 0;
            foreach (var item in encodes)
            {
                //string[] row = new string[] { item.color.ToString(), item.probability.ToString()+" %", item.huffman_Code };  // 定義一列的字串陣列
                //dataGridView1.Rows.Add(item.number,"#"+(Color.FromArgb(item.color).Name).ToUpper(), item.probability, item.huffman_Code); // 加入列  
                dataGridView1.Rows.Add("#"+(Color.FromArgb(item.color).Name).ToUpper(), item.probability, item.huffman_Code); // 加入列  
                dataGridView1.Rows[i++].Cells[0].Style.BackColor = Color.FromArgb(item.color);
            }
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
        }
        private void Form_Huffman_Shown(object sender, EventArgs e)
        {
            Text = "Huffman code";
            Bitmap bitmap = new Bitmap(image.Width, image.Height);
            Graphics graphics = Graphics.FromImage(bitmap);
            image.draw(graphics);
            pictureBox1.Size = new Size(image.picture_size.Width, image.picture_size.Height);
            pictureBox1.Image = bitmap;


            histogramValue_Gray = new int[256];
            Array.Clear(histogramValue_Gray, 0, histogramValue_Gray.Length);


            max_Length = image.picture_size.Width * image.picture_size.Height;
            for (int Ycount = 0; Ycount < image.Height; Ycount++)           
                for (int Xcount = 0; Xcount < image.Width; Xcount++)                   
                    histogramValue_Gray[image.my_Pixel[Xcount,Ycount].Gray]++;

            color_Probability = new Dictionary<int, int>();
            for (int Ycount = 0; Ycount < image.Height; Ycount++)
                for (int Xcount = 0; Xcount < image.Width; Xcount++)
                {
                    if (color_Probability.ContainsKey((image.my_Pixel[Xcount, Ycount].color).ToArgb()))
                        color_Probability[(image.my_Pixel[Xcount, Ycount].color).ToArgb()]++;
                    else
                        color_Probability.Add((image.my_Pixel[Xcount, Ycount].color).ToArgb(),1 );
                }
            

            //for (int i = 0; i < 256; i++)
            //    if (histogramValue_Gray[i] != 0)
            //        color_Probability.Add(i, histogramValue_Gray[i]);


            drawHistogram(color_Probability);

            huffman_encode = new List<encode>();

            // creating a priority queue q.
            // makes a min-priority queue(min-heap).
            Queue<HuffmanNode> q = new Queue<HuffmanNode>();
            int counter = 0;
            foreach (var item in color_Probability.OrderBy(value => value.Value))
            {
               // Console.WriteLine(item.Key+" "+item.Value);
                // creating a Huffman node object
                // and add it to the priority queue.
                HuffmanNode hn = new HuffmanNode();

                hn.color = item.Key;
                hn.value = item.Value;
                hn.probability = (item.Value / (double)max_Length)* 100.0;
                hn.number = counter;
                hn.left = null;
                hn.right = null;

                // add functions adds
                // the huffman node to the queue.
                q.Enqueue(hn);
                counter++;
            }

            // create a root node
            HuffmanNode root = null;

            // Here we will extract the two minimum value
            // from the heap each time until
            // its size reduces to 1, extract until
            // all the nodes are extracted.
            while (q.Count > 1)
            {

                // first min extract.
                HuffmanNode x = q.Peek();
                q.Dequeue();

                // second min extract.
                HuffmanNode y = q.Peek();
                q.Dequeue();

                // new node f which is equal
                HuffmanNode f = new HuffmanNode();

                // to the sum of the frequency of the two nodes
                // assigning values to the f node.
                f.value = x.value + y.value;
                f.color = 1;

                // first extracted node as left child.
                f.left = x;

                // second extracted node as the right child.
                f.right = y;

                // marking the f node as the root node.
                root = f;

                // add this node to the priority-queue.
                q.Enqueue(f);
                q = new Queue<HuffmanNode>(q.OrderBy(value => value.value));
            }

            // print the codes by traversing the tree
            printCode(root, "");

            setDataGridView(huffman_encode);
        }

        // recursive function to print the
        // huffman-code through the tree traversal.
        // Here s is the huffman - code generated.
        public void printCode(HuffmanNode root, String s)
        {
            
            // base case; if the left and right are null
            // then its a leaf node and we print
            // the code s generated by traversing the tree.
            if (root.left == null && root.right == null && root.color!=1)
            {

                // c is the character in the node
                //Console.WriteLine(root.color+"(" +root.probability+ "%):" + s);
                huffman_encode.Add(new encode {number = root.number, color = root.color,probability = root.probability, huffman_Code = s });
                //richTextBox1.AppendText(root.color + " (" + root.probability + " % ) : " + s + "\n");
                return;
            }

            // if we go to left then add "0" to the code.
            // if we go to the right add"1" to the code.

            // recursive calls for left and
            // right sub-tree of the generated tree.
            printCode(root.left, s + "0");
            printCode(root.right, s + "1");
        }
    }
    public class HuffmanNode
    {
        public int color;
        public double probability;
        public int value;
        public int number;
        public HuffmanNode left;
        public HuffmanNode right;
    }
 
    public class encode
    {
        public int number;
        public int color;
        public double probability;
        public string huffman_Code;
    }
}
