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
            cancel = new Button();
            begin = new Button();
            stop = new Button();
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
            // cancel
            // 
            cancel.Location = new Point(574, 687);
            cancel.Name = "cancel";
            cancel.Size = new Size(75, 23);
            cancel.TabIndex = 2;
            cancel.Text = "停止";
            cancel.UseVisualStyleBackColor = true;
            cancel.Click += cancel_Click;
            // 
            // begin
            // 
            begin.Location = new Point(574, 401);
            begin.Name = "begin";
            begin.Size = new Size(75, 23);
            begin.TabIndex = 3;
            begin.Text = "button1";
            begin.UseVisualStyleBackColor = true;
            begin.Click += begin_Click;
            // 
            // stop
            // 
            stop.Location = new Point(574, 465);
            stop.Name = "stop";
            stop.Size = new Size(75, 23);
            stop.TabIndex = 4;
            stop.Text = "button1";
            stop.UseVisualStyleBackColor = true;
            stop.Click += stop_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 727);
            Controls.Add(stop);
            Controls.Add(begin);
            Controls.Add(cancel);
            Controls.Add(plotView);
            Controls.Add(pictureBox1);
            Name = "Form1";
            Text = "Form1";
            FormClosed += Form1_FormClosed;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pictureBox1;
        private OxyPlot.WindowsForms.PlotView plotView;
        private Button cancel;
        private Button begin;
        private Button stop;
    }
}