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

            log.Text = "δ��ʼ�ɼ�����";
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

            //// ��ȡ��Ƶ�Ŀ�Ⱥ͸߶�
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
                MessageBox.Show("�����뱻�Ա��");
                return;
            }

            string folder = Path.Combine(Environment.CurrentDirectory, subject.Text);
            if (Directory.Exists(folder))
            {
                MessageBox.Show("���Ա���Ѵ���");
                return;
            }

            Directory.CreateDirectory(folder);

            string videoPath = Path.Combine(folder, "video.avi");
            string sensorPath = Path.Combine(folder, "sensor.txt");

            //isRecording = true;
            //videoWriter = new VideoWriter(videoPath, Max30102Constants.YUY2, 30, new Size(cameraWidth, cameraHeight), true);

            //sensorWriter = new StreamWriter(sensorPath);

            dataCollector.StartRecording(videoPath, sensorPath);

            log.Text = "���ڲɼ�����";

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

            log.Text = "δ��ʼ�ɼ�����";

            MessageBox.Show("�ɼ����");
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

        //    // ��Ϊ���ǲ��������߳���ֱ�ӷ���UI��������ҪInvoke
        //    this.Invoke((MethodInvoker)delegate
        //    {
        //        // ����µ����ݵ�
        //        dataSeries.Points.Add(new DataPoint(dataSeries.Points.Count, red));

        //        // ������ݵ���������100��ɾ����ɵ����ݵ�
        //        if (dataSeries.Points.Count > 500)
        //        {
        //            dataSeries.Points.RemoveAt(0);

        //            // ���������������ݵ��X���꣬�Ա������ܹ���ȷ����ͼ����ʾ
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
                MessageBox.Show("����ֹͣ�ɼ�");
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