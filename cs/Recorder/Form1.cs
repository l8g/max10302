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

        private MAX30102 max30102;

        private VideoCapture capture;
        private Mat frame;

        private CancellationTokenSource sensorCancellion = new CancellationTokenSource();
        private CancellationTokenSource cameraCancellion = new CancellationTokenSource();

        int cameraWidth = 640;
        int cameraHeight = 480;

        public Form1()
        {
            InitializeComponent();

            plotView.Model = new PlotModel();
            plotView.Model.Title = "Heart Rate";
            dataSeries = new LineSeries();
            plotView.Model.Series.Add(dataSeries);
            dataSeries.Title = "Sensor Data";

            max30102 = new MAX30102();

            capture = new VideoCapture(0);
            frame = new Mat();

            capture.Set(Emgu.CV.CvEnum.CapProp.FrameWidth, 640);
            capture.Set(Emgu.CV.CvEnum.CapProp.FrameHeight, 480);

            // 获取视频的宽度和高度
            cameraWidth = (int)capture.Get(Emgu.CV.CvEnum.CapProp.FrameWidth);
            cameraHeight = (int)capture.Get(Emgu.CV.CvEnum.CapProp.FrameHeight);

            Task.Run(() =>
            {
                while (!sensorCancellion.Token.IsCancellationRequested)
                {
                    ReadSensorData();
                }
            });

            Task.Run(() =>
            {
                while (!cameraCancellion.Token.IsCancellationRequested)
                {
                    ReadCamera();
                }
            });
        }

        private bool isRecording = false;
        private VideoWriter writer;
        private void BeginRecord()
        {
            isRecording = true;
            int YUY2 = 'Y' | ('U' << 8) | ('Y' << 16) | ('2' << 24);
            writer = new VideoWriter("output.avi", YUY2, 30, new Size(cameraWidth, cameraHeight), true);
        }

        private void StopRecord()
        {
            isRecording = false;
            writer.Dispose();
        }

        private void ReadCamera()
        {
            capture.Read(frame);
            if (isRecording)
            {
                writer.Write(frame);
            }
            Invoke((MethodInvoker)delegate
            {
                pictureBox1.Image = frame.ToBitmap();
            });
        }


        private void ReadSensorData()
        {
            (int red, int ir) = max30102.ReadFIFO();

            // 因为我们不能在子线程中直接访问UI，所以需要Invoke
            this.Invoke((MethodInvoker)delegate
            {
                // 添加新的数据点
                dataSeries.Points.Add(new DataPoint(dataSeries.Points.Count, red));

                // 如果数据点数量超过100，删除最旧的数据点
                if (dataSeries.Points.Count > 500)
                {
                    dataSeries.Points.RemoveAt(0);

                    // 重新设置所有数据点的X坐标，以便它们能够正确地在图上显示
                    for (int i = 0; i < dataSeries.Points.Count; i++)
                    {
                        DataPoint point = dataSeries.Points[i];
                        dataSeries.Points[i] = new DataPoint(i, point.Y);
                    }
                }
                plotView.InvalidatePlot(true);
            });
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            sensorCancellion.Cancel();
            cameraCancellion.Cancel();
            capture.Dispose();
            frame.Dispose();
            max30102.Close();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            sensorCancellion.Cancel();
            cameraCancellion.Cancel();
        }

        private void begin_Click(object sender, EventArgs e)
        {
            BeginRecord();
        }

        private void stop_Click(object sender, EventArgs e)
        {
            StopRecord();
        }
    }
}