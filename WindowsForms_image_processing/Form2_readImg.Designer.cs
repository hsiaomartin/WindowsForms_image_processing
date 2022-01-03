
namespace WindowsForms_image_processing
{
    partial class Form2_readImg
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBox_filepath = new System.Windows.Forms.TextBox();
            this.button_filefilepath = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.processingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enlargeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rotateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rGBHSIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.brightnessToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.transparencyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.histogramToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.negativeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.thresholdingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.slicingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bitPlaneSlicingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wartermarkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contrastStretchingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.magicWandToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.histogramEqualizationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.histogramSpecificationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.huffmanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.filterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.outlierToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.medianToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lowpassHighpassToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lowpassToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.highpassToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.edgeCrispeningToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.highboostFilterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gradientToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel5 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel6 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel7 = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pseudoMedianToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox_filepath
            // 
            this.textBox_filepath.Location = new System.Drawing.Point(12, 44);
            this.textBox_filepath.Name = "textBox_filepath";
            this.textBox_filepath.ReadOnly = true;
            this.textBox_filepath.Size = new System.Drawing.Size(573, 22);
            this.textBox_filepath.TabIndex = 0;
            // 
            // button_filefilepath
            // 
            this.button_filefilepath.Location = new System.Drawing.Point(591, 42);
            this.button_filefilepath.Name = "button_filefilepath";
            this.button_filefilepath.Size = new System.Drawing.Size(75, 23);
            this.button_filefilepath.TabIndex = 1;
            this.button_filefilepath.Text = "...";
            this.button_filefilepath.UseVisualStyleBackColor = true;
            this.button_filefilepath.Visible = false;
            this.button_filefilepath.Click += new System.EventHandler(this.button_filefilepath_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(591, 42);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(197, 236);
            this.dataGridView1.TabIndex = 2;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.processingToolStripMenuItem,
            this.filterToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(802, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // processingToolStripMenuItem
            // 
            this.processingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cutToolStripMenuItem,
            this.enlargeToolStripMenuItem,
            this.rotateToolStripMenuItem,
            this.rGBHSIToolStripMenuItem,
            this.brightnessToolStripMenuItem,
            this.transparencyToolStripMenuItem,
            this.histogramToolStripMenuItem,
            this.negativeToolStripMenuItem,
            this.thresholdingToolStripMenuItem,
            this.slicingToolStripMenuItem,
            this.contrastStretchingToolStripMenuItem,
            this.magicWandToolStripMenuItem,
            this.histogramEqualizationToolStripMenuItem,
            this.histogramSpecificationToolStripMenuItem,
            this.huffmanToolStripMenuItem});
            this.processingToolStripMenuItem.Name = "processingToolStripMenuItem";
            this.processingToolStripMenuItem.Size = new System.Drawing.Size(79, 20);
            this.processingToolStripMenuItem.Text = "Processing";
            // 
            // cutToolStripMenuItem
            // 
            this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            this.cutToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.cutToolStripMenuItem.Text = "cut";
            this.cutToolStripMenuItem.Click += new System.EventHandler(this.cutToolStripMenuItem_Click);
            // 
            // enlargeToolStripMenuItem
            // 
            this.enlargeToolStripMenuItem.Name = "enlargeToolStripMenuItem";
            this.enlargeToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.enlargeToolStripMenuItem.Text = "Scaling";
            this.enlargeToolStripMenuItem.Click += new System.EventHandler(this.enlargeToolStripMenuItem_Click);
            // 
            // rotateToolStripMenuItem
            // 
            this.rotateToolStripMenuItem.Name = "rotateToolStripMenuItem";
            this.rotateToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.rotateToolStripMenuItem.Text = "rotate";
            this.rotateToolStripMenuItem.Click += new System.EventHandler(this.rotateToolStripMenuItem_Click);
            // 
            // rGBHSIToolStripMenuItem
            // 
            this.rGBHSIToolStripMenuItem.Name = "rGBHSIToolStripMenuItem";
            this.rGBHSIToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.rGBHSIToolStripMenuItem.Text = "RGB_HSI_Gray";
            this.rGBHSIToolStripMenuItem.Click += new System.EventHandler(this.rGBHSIToolStripMenuItem_Click);
            // 
            // brightnessToolStripMenuItem
            // 
            this.brightnessToolStripMenuItem.Name = "brightnessToolStripMenuItem";
            this.brightnessToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.brightnessToolStripMenuItem.Text = "Brightness";
            this.brightnessToolStripMenuItem.Click += new System.EventHandler(this.brightnessToolStripMenuItem_Click);
            // 
            // transparencyToolStripMenuItem
            // 
            this.transparencyToolStripMenuItem.Name = "transparencyToolStripMenuItem";
            this.transparencyToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.transparencyToolStripMenuItem.Text = "Transparency";
            this.transparencyToolStripMenuItem.Click += new System.EventHandler(this.transparencyToolStripMenuItem_Click);
            // 
            // histogramToolStripMenuItem
            // 
            this.histogramToolStripMenuItem.Name = "histogramToolStripMenuItem";
            this.histogramToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.histogramToolStripMenuItem.Text = "Histogram";
            this.histogramToolStripMenuItem.Click += new System.EventHandler(this.histogramToolStripMenuItem_Click);
            // 
            // negativeToolStripMenuItem
            // 
            this.negativeToolStripMenuItem.Name = "negativeToolStripMenuItem";
            this.negativeToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.negativeToolStripMenuItem.Text = "Negative";
            this.negativeToolStripMenuItem.Click += new System.EventHandler(this.negativeToolStripMenuItem_Click);
            // 
            // thresholdingToolStripMenuItem
            // 
            this.thresholdingToolStripMenuItem.Name = "thresholdingToolStripMenuItem";
            this.thresholdingToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.thresholdingToolStripMenuItem.Text = "Thresholding";
            this.thresholdingToolStripMenuItem.Click += new System.EventHandler(this.thresholdingToolStripMenuItem_Click);
            // 
            // slicingToolStripMenuItem
            // 
            this.slicingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bitPlaneSlicingToolStripMenuItem,
            this.wartermarkToolStripMenuItem});
            this.slicingToolStripMenuItem.Name = "slicingToolStripMenuItem";
            this.slicingToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.slicingToolStripMenuItem.Text = "Slicing";
            // 
            // bitPlaneSlicingToolStripMenuItem
            // 
            this.bitPlaneSlicingToolStripMenuItem.Name = "bitPlaneSlicingToolStripMenuItem";
            this.bitPlaneSlicingToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.bitPlaneSlicingToolStripMenuItem.Text = "Bit plane slicing";
            this.bitPlaneSlicingToolStripMenuItem.Click += new System.EventHandler(this.bitPlaneSlicingToolStripMenuItem_Click);
            // 
            // wartermarkToolStripMenuItem
            // 
            this.wartermarkToolStripMenuItem.Name = "wartermarkToolStripMenuItem";
            this.wartermarkToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.wartermarkToolStripMenuItem.Text = "Wartermark";
            this.wartermarkToolStripMenuItem.Click += new System.EventHandler(this.wartermarkToolStripMenuItem_Click);
            // 
            // contrastStretchingToolStripMenuItem
            // 
            this.contrastStretchingToolStripMenuItem.Name = "contrastStretchingToolStripMenuItem";
            this.contrastStretchingToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.contrastStretchingToolStripMenuItem.Text = "Contrast Stretching";
            this.contrastStretchingToolStripMenuItem.Click += new System.EventHandler(this.contrastStretchingToolStripMenuItem_Click);
            // 
            // magicWandToolStripMenuItem
            // 
            this.magicWandToolStripMenuItem.Name = "magicWandToolStripMenuItem";
            this.magicWandToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.magicWandToolStripMenuItem.Text = "Magic Wand";
            this.magicWandToolStripMenuItem.Click += new System.EventHandler(this.magicWandToolStripMenuItem_Click);
            // 
            // histogramEqualizationToolStripMenuItem
            // 
            this.histogramEqualizationToolStripMenuItem.Name = "histogramEqualizationToolStripMenuItem";
            this.histogramEqualizationToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.histogramEqualizationToolStripMenuItem.Text = "Histogram Equalization";
            this.histogramEqualizationToolStripMenuItem.Click += new System.EventHandler(this.histogramEqualizationToolStripMenuItem_Click);
            // 
            // histogramSpecificationToolStripMenuItem
            // 
            this.histogramSpecificationToolStripMenuItem.Name = "histogramSpecificationToolStripMenuItem";
            this.histogramSpecificationToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.histogramSpecificationToolStripMenuItem.Text = "Histogram Specification";
            this.histogramSpecificationToolStripMenuItem.Click += new System.EventHandler(this.histogramSpecificationToolStripMenuItem_Click);
            // 
            // huffmanToolStripMenuItem
            // 
            this.huffmanToolStripMenuItem.Name = "huffmanToolStripMenuItem";
            this.huffmanToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.huffmanToolStripMenuItem.Text = "Huffman";
            this.huffmanToolStripMenuItem.Click += new System.EventHandler(this.huffmanToolStripMenuItem_Click);
            // 
            // filterToolStripMenuItem
            // 
            this.filterToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.outlierToolStripMenuItem,
            this.medianToolStripMenuItem,
            this.pseudoMedianToolStripMenuItem,
            this.lowpassHighpassToolStripMenuItem,
            this.edgeCrispeningToolStripMenuItem,
            this.highboostFilterToolStripMenuItem,
            this.gradientToolStripMenuItem});
            this.filterToolStripMenuItem.Name = "filterToolStripMenuItem";
            this.filterToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.filterToolStripMenuItem.Text = "Filter";
            // 
            // outlierToolStripMenuItem
            // 
            this.outlierToolStripMenuItem.Name = "outlierToolStripMenuItem";
            this.outlierToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.outlierToolStripMenuItem.Text = "Outlier";
            this.outlierToolStripMenuItem.Click += new System.EventHandler(this.outlierToolStripMenuItem_Click);
            // 
            // medianToolStripMenuItem
            // 
            this.medianToolStripMenuItem.Name = "medianToolStripMenuItem";
            this.medianToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.medianToolStripMenuItem.Text = "Median";
            this.medianToolStripMenuItem.Click += new System.EventHandler(this.medianToolStripMenuItem_Click);
            // 
            // lowpassHighpassToolStripMenuItem
            // 
            this.lowpassHighpassToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lowpassToolStripMenuItem,
            this.highpassToolStripMenuItem});
            this.lowpassHighpassToolStripMenuItem.Name = "lowpassHighpassToolStripMenuItem";
            this.lowpassHighpassToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.lowpassHighpassToolStripMenuItem.Text = "Lowpass/Highpass";
            // 
            // lowpassToolStripMenuItem
            // 
            this.lowpassToolStripMenuItem.Name = "lowpassToolStripMenuItem";
            this.lowpassToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.lowpassToolStripMenuItem.Text = "Lowpass";
            this.lowpassToolStripMenuItem.Click += new System.EventHandler(this.lowpassToolStripMenuItem_Click);
            // 
            // highpassToolStripMenuItem
            // 
            this.highpassToolStripMenuItem.Name = "highpassToolStripMenuItem";
            this.highpassToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.highpassToolStripMenuItem.Text = "Highpass";
            this.highpassToolStripMenuItem.Click += new System.EventHandler(this.highpassToolStripMenuItem_Click);
            // 
            // edgeCrispeningToolStripMenuItem
            // 
            this.edgeCrispeningToolStripMenuItem.Name = "edgeCrispeningToolStripMenuItem";
            this.edgeCrispeningToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.edgeCrispeningToolStripMenuItem.Text = "Edge Crispening";
            this.edgeCrispeningToolStripMenuItem.Click += new System.EventHandler(this.edgeCrispeningToolStripMenuItem_Click);
            // 
            // highboostFilterToolStripMenuItem
            // 
            this.highboostFilterToolStripMenuItem.Name = "highboostFilterToolStripMenuItem";
            this.highboostFilterToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.highboostFilterToolStripMenuItem.Text = "High-boost Filter";
            this.highboostFilterToolStripMenuItem.Click += new System.EventHandler(this.highboostFilterToolStripMenuItem_Click);
            // 
            // gradientToolStripMenuItem
            // 
            this.gradientToolStripMenuItem.Name = "gradientToolStripMenuItem";
            this.gradientToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.gradientToolStripMenuItem.Text = "Gradient";
            this.gradientToolStripMenuItem.Click += new System.EventHandler(this.gradientToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel3,
            this.toolStripStatusLabel4,
            this.toolStripStatusLabel5,
            this.toolStripStatusLabel6,
            this.toolStripStatusLabel7});
            this.statusStrip1.Location = new System.Drawing.Point(0, 581);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(802, 22);
            this.statusStrip1.TabIndex = 17;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(32, 17);
            this.toolStripStatusLabel1.Text = "(0,0)";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(15, 17);
            this.toolStripStatusLabel2.Text = "R";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(16, 17);
            this.toolStripStatusLabel3.Text = "G";
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(14, 17);
            this.toolStripStatusLabel4.Text = "B";
            // 
            // toolStripStatusLabel5
            // 
            this.toolStripStatusLabel5.Name = "toolStripStatusLabel5";
            this.toolStripStatusLabel5.Size = new System.Drawing.Size(16, 17);
            this.toolStripStatusLabel5.Text = "H";
            // 
            // toolStripStatusLabel6
            // 
            this.toolStripStatusLabel6.Name = "toolStripStatusLabel6";
            this.toolStripStatusLabel6.Size = new System.Drawing.Size(14, 17);
            this.toolStripStatusLabel6.Text = "S";
            // 
            // toolStripStatusLabel7
            // 
            this.toolStripStatusLabel7.Name = "toolStripStatusLabel7";
            this.toolStripStatusLabel7.Size = new System.Drawing.Size(10, 17);
            this.toolStripStatusLabel7.Text = "I";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.pictureBox2);
            this.groupBox1.Location = new System.Drawing.Point(570, 351);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(220, 193);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Palette";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(18, 21);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(128, 128);
            this.pictureBox2.TabIndex = 18;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 72);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(97, 98);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseEnter += new System.EventHandler(this.pictureBox1_MouseEnter);
            this.pictureBox1.MouseLeave += new System.EventHandler(this.pictureBox1_MouseLeave);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            // 
            // pseudoMedianToolStripMenuItem
            // 
            this.pseudoMedianToolStripMenuItem.Name = "pseudoMedianToolStripMenuItem";
            this.pseudoMedianToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.pseudoMedianToolStripMenuItem.Text = "Pseudo_Median";
            this.pseudoMedianToolStripMenuItem.Click += new System.EventHandler(this.pseudoMedianToolStripMenuItem_Click);
            // 
            // Form2_readImg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(802, 603);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.button_filefilepath);
            this.Controls.Add(this.textBox_filepath);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form2_readImg";
            this.Text = "Form2_PCX";
            this.VisibleChanged += new System.EventHandler(this.Form2_PCX_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_filepath;
        private System.Windows.Forms.Button button_filefilepath;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem processingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem enlargeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rotateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rGBHSIToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel5;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel6;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel7;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.ToolStripMenuItem brightnessToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem transparencyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem histogramToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem negativeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem thresholdingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem slicingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem filterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem outlierToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bitPlaneSlicingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wartermarkToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem medianToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lowpassHighpassToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem edgeCrispeningToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem highboostFilterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gradientToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lowpassToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem highpassToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem contrastStretchingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem magicWandToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem histogramEqualizationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem histogramSpecificationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem huffmanToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pseudoMedianToolStripMenuItem;
    }
}