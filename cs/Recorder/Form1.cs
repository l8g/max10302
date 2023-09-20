using Emgu.CV;
using Emgu.CV.Structure;

using OxyPlot;
using OxyPlot.Series;
using OxyPlot.WindowsForms;

using System.Drawing.Imaging;

namespace Recorder
{
    public partial class Form1 : Form
    {
        private LineSeries dataSeries;
        private DataCollector dataCollector;

        public Form1()
        {
            InitializeComponent();

            plotView.Model = new PlotModel();
            plotView.Model.Title = "Heart Rate";
            dataSeries = new LineSeries();
            plotView.Model.Series.Add(dataSeries);
            dataSeries.Title = "Sensor Data";

            log.Text = "未开始采集数据";

            start.Enabled = true;
            begin.Enabled = false;
            stop.Enabled = false;



        }

        private void UpdateCameraFrame(Mat frame)
        {
            try
            {
                Invoke((MethodInvoker)delegate
                {
                    pictureBox1.Image = frame.ToBitmap();
                });
            }
            catch (Exception)
            {

            }
        }

        private void UpdateSensorData(int red, int ir)
        {
            try
            {
                Invoke((MethodInvoker)delegate
                {
                    dataSeries.Points.Add(new DataPoint(dataSeries.Points.Count, red));

                    if (dataSeries.Points.Count > 500)
                    {
                        dataSeries.Points.RemoveAt(0);

                        for (int i = 0; i < dataSeries.Points.Count; i++)
                        {
                            DataPoint point = dataSeries.Points[i];
                            dataSeries.Points[i] = new DataPoint(i, point.Y);
                        }
                    }

                    plotView.InvalidatePlot(true);
                });
            }
            catch (Exception)
            {

            }
        }


        private void BeginRecord()
        {
            if (string.IsNullOrEmpty(subject.Text))
            {
                MessageBox.Show("请输入被试编号");
                return;
            }

            string folder = Path.Combine(Environment.CurrentDirectory, subject.Text);
            if (Directory.Exists(folder))
            {
                MessageBox.Show("被试编号已存在");
                return;
            }

            Directory.CreateDirectory(folder);

            string videoPath = Path.Combine(folder, "video.avi");
            string sensorPath = Path.Combine(folder, "sensor.txt");


            dataCollector.StartRecording(videoPath, sensorPath);

            log.Text = "正在采集数据";

            begin.Enabled = false;
            stop.Enabled = true;
        }

        private void StopRecord()
        {

            dataCollector.StopRecording();

            begin.Enabled = true;
            stop.Enabled = false;

            log.Text = "未开始采集数据";

            MessageBox.Show("采集完成");
        }


        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            dataCollector.Dispose();
        }

        private void begin_Click(object sender, EventArgs e)
        {
            BeginRecord();
        }

        private void stop_Click(object sender, EventArgs e)
        {
            StopRecord();
        }

        private async void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (dataCollector.IsRecording)
            {
                MessageBox.Show("请先停止采集");
                e.Cancel = true;
            }
            else
            {
                await dataCollector.Stop();
            }
        }

        private void start_Click(object sender, EventArgs e)
        {
            // Initialize DataCollector
            dataCollector = new DataCollector(int.Parse(cameraIndex.Text));
            dataCollector.NewFrameReceived += UpdateCameraFrame;
            dataCollector.NewSensorDataReceived += UpdateSensorData;

            dataCollector.Start();
            begin.Enabled = true;
            start.Enabled = false;
        }
    }
}