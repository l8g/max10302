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

        //private MAX30102 max30102;

        //private VideoCapture capture;
        //private Mat frame;


        //private bool isRecording = false;

        //private VideoWriter? videoWriter;
        //private TextWriter? sensorWriter;


        //private CancellationTokenSource cancellion = new CancellationTokenSource();

        //int cameraWidth = 640;
        //int cameraHeight = 480;

        public Form1()
        {
            InitializeComponent();

            plotView.Model = new PlotModel();
            plotView.Model.Title = "Heart Rate";
            dataSeries = new LineSeries();
            plotView.Model.Series.Add(dataSeries);
            dataSeries.Title = "Sensor Data";

            log.Text = "未开始采集数据";
            begin.Enabled = true;
            stop.Enabled = false;

            // Initialize DataCollector
            dataCollector = new DataCollector();
            dataCollector.NewFrameReceived += UpdateCameraFrame;
            dataCollector.NewSensorDataReceived += UpdateSensorData;
            dataCollector.Start();

            //max30102 = new MAX30102();

            //capture = new VideoCapture(0);
            //frame = new Mat();

            //capture.Set(Emgu.CV.CvEnum.CapProp.FrameWidth, 640);
            //capture.Set(Emgu.CV.CvEnum.CapProp.FrameHeight, 480);

            //// 获取视频的宽度和高度
            //cameraWidth = (int)capture.Get(Emgu.CV.CvEnum.CapProp.FrameWidth);
            //cameraHeight = (int)capture.Get(Emgu.CV.CvEnum.CapProp.FrameHeight);

            //Task.Run(() =>
            //{
            //    while (!cancellion.Token.IsCancellationRequested)
            //    {
            //        ReadSensorData();
            //    }
            //}, cancellion.Token);

            //Task.Run(() =>
            //{
            //    while (!cancellion.Token.IsCancellationRequested)
            //    {
            //        ReadCamera();
            //    }
            //}, cancellion.Token);
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

            //isRecording = true;
            //videoWriter = new VideoWriter(videoPath, Max30102Constants.YUY2, 30, new Size(cameraWidth, cameraHeight), true);

            //sensorWriter = new StreamWriter(sensorPath);

            dataCollector.StartRecording(videoPath, sensorPath);

            log.Text = "正在采集数据";

            begin.Enabled = false;
            stop.Enabled = true;
        }

        private void StopRecord()
        {
            //isRecording = false;
            //videoWriter?.Dispose();
            //sensorWriter?.Dispose();

            dataCollector.StopRecording();

            begin.Enabled = true;
            stop.Enabled = false;

            log.Text = "未开始采集数据";

            MessageBox.Show("采集完成");
        }

        //private void ReadCamera()
        //{
        //    capture.Read(frame);
        //    if (isRecording)
        //    {
        //        videoWriter?.Write(frame);
        //    }
        //    Invoke((MethodInvoker)delegate
        //    {
        //        pictureBox1.Image = frame.ToBitmap();
        //    });
        //}


        //private void ReadSensorData()
        //{
        //    (int red, int ir) = max30102.ReadFIFO();
        //    if (isRecording)
        //    {
        //        sensorWriter?.WriteLine($"{red}\t{ir}");
        //    }

        //    // 因为我们不能在子线程中直接访问UI，所以需要Invoke
        //    this.Invoke((MethodInvoker)delegate
        //    {
        //        // 添加新的数据点
        //        dataSeries.Points.Add(new DataPoint(dataSeries.Points.Count, red));

        //        // 如果数据点数量超过100，删除最旧的数据点
        //        if (dataSeries.Points.Count > 500)
        //        {
        //            dataSeries.Points.RemoveAt(0);

        //            // 重新设置所有数据点的X坐标，以便它们能够正确地在图上显示
        //            for (int i = 0; i < dataSeries.Points.Count; i++)
        //            {
        //                DataPoint point = dataSeries.Points[i];
        //                dataSeries.Points[i] = new DataPoint(i, point.Y);
        //            }
        //        }
        //        plotView.InvalidatePlot(true);
        //    });
        //}

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            dataCollector.Dispose();
        }

        //private void cancel_Click(object sender, EventArgs e)
        //{
        //    //cancellion.Cancel();
        //}

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

        }
    }
}