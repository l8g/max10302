namespace Recorder
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pictureBox1 = new PictureBox();
            plotView = new OxyPlot.WindowsForms.PlotView();
            begin = new Button();
            stop = new Button();
            subject = new TextBox();
            log = new TextBox();
            cameraIndex = new TextBox();
            label1 = new Label();
            label2 = new Label();
            start = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(38, 401);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(368, 309);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // plotView
            // 
            plotView.Location = new Point(38, 35);
            plotView.Name = "plotView";
            plotView.PanCursor = Cursors.Hand;
            plotView.Size = new Size(729, 327);
            plotView.TabIndex = 1;
            plotView.Text = "plotView1";
            plotView.ZoomHorizontalCursor = Cursors.SizeWE;
            plotView.ZoomRectangleCursor = Cursors.SizeNWSE;
            plotView.ZoomVerticalCursor = Cursors.SizeNS;
            // 
            // begin
            // 
            begin.Location = new Point(555, 626);
            begin.Name = "begin";
            begin.Size = new Size(75, 23);
            begin.TabIndex = 3;
            begin.Text = "开始采集";
            begin.UseVisualStyleBackColor = true;
            begin.Click += begin_Click;
            // 
            // stop
            // 
            stop.Location = new Point(655, 626);
            stop.Name = "stop";
            stop.Size = new Size(75, 23);
            stop.TabIndex = 4;
            stop.Text = "停止采集";
            stop.UseVisualStyleBackColor = true;
            stop.Click += stop_Click;
            // 
            // subject
            // 
            subject.Location = new Point(541, 440);
            subject.Name = "subject";
            subject.Size = new Size(100, 23);
            subject.TabIndex = 5;
            // 
            // log
            // 
            log.Location = new Point(412, 669);
            log.Multiline = true;
            log.Name = "log";
            log.ReadOnly = true;
            log.Size = new Size(355, 41);
            log.TabIndex = 6;
            // 
            // cameraIndex
            // 
            cameraIndex.Location = new Point(541, 507);
            cameraIndex.Name = "cameraIndex";
            cameraIndex.Size = new Size(100, 23);
            cameraIndex.TabIndex = 7;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(444, 443);
            label1.Name = "label1";
            label1.Size = new Size(44, 17);
            label1.TabIndex = 8;
            label1.Text = "编号：";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(444, 510);
            label2.Name = "label2";
            label2.Size = new Size(80, 17);
            label2.TabIndex = 9;
            label2.Text = "摄像机索引：";
            // 
            // start
            // 
            start.Location = new Point(444, 626);
            start.Name = "start";
            start.Size = new Size(75, 23);
            start.TabIndex = 10;
            start.Text = "启动";
            start.UseVisualStyleBackColor = true;
            start.Click += start_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 727);
            Controls.Add(start);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(cameraIndex);
            Controls.Add(log);
            Controls.Add(subject);
            Controls.Add(stop);
            Controls.Add(begin);
            Controls.Add(plotView);
            Controls.Add(pictureBox1);
            Name = "Form1";
            Text = "Form1";
            FormClosing += Form1_FormClosing;
            FormClosed += Form1_FormClosed;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private OxyPlot.WindowsForms.PlotView plotView;
        private Button begin;
        private Button stop;
        private TextBox subject;
        private TextBox log;
        private TextBox cameraIndex;
        private Label label1;
        private Label label2;
        private Button start;
    }
}