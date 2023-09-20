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

            // ��ȡ��Ƶ�Ŀ�Ⱥ͸߶�
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

            // ��Ϊ���ǲ��������߳���ֱ�ӷ���UI��������ҪInvoke
            this.Invoke((MethodInvoker)delegate
            {
                // ����µ����ݵ�
                dataSeries.Points.Add(new DataPoint(dataSeries.Points.Count, red));

                // ������ݵ���������100��ɾ����ɵ����ݵ�
                if (dataSeries.Points.Count > 500)
                {
                    dataSeries.Points.RemoveAt(0);

                    // ���������������ݵ��X���꣬�Ա������ܹ���ȷ����ͼ����ʾ
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