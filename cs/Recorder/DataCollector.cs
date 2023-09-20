using Emgu.CV;


namespace Recorder
{
    public class DataCollector : IDisposable
    {
        private MAX30102 max30102;
        private VideoCapture capture;
        private VideoWriter videoWriter;
        private StreamWriter sensorWriter;
        private CancellationTokenSource cancellationSource;

        public event Action<Mat> NewFrameReceived;
        public event Action<int, int> NewSensorDataReceived;

        private Task cameraTask;
        private Task sensorTask;

        int cameraWidth = 640;
        int cameraHeight = 480;

        public bool IsRecording { get; private set; }
        public bool IsCanceled => cancellationSource.IsCancellationRequested;

        public DataCollector()
        {
            max30102 = new MAX30102();
            capture = new VideoCapture(0);

            capture.Set(Emgu.CV.CvEnum.CapProp.FrameWidth, cameraWidth);
            capture.Set(Emgu.CV.CvEnum.CapProp.FrameHeight, cameraHeight);

            // 获取视频的宽度和高度
            cameraWidth = (int)capture.Get(Emgu.CV.CvEnum.CapProp.FrameWidth);
            cameraHeight = (int)capture.Get(Emgu.CV.CvEnum.CapProp.FrameHeight);
        }

        public void StartRecording(string videoFilePath, string sensorFilePath)
        {
            videoWriter = new VideoWriter(videoFilePath, Max30102Constants.YUY2, 30, new Size(cameraWidth, cameraHeight), true);
            sensorWriter = new StreamWriter(sensorFilePath);
            IsRecording = true;
        }

        public void StopRecording()
        {
            videoWriter?.Dispose();
            sensorWriter?.Dispose();
            IsRecording = false;
        }

        public void Start()
        {
            cancellationSource = new CancellationTokenSource();

            sensorTask = Task.Run(() => SensorLoop(), cancellationSource.Token);
            cameraTask = Task.Run(() => CameraLoop(), cancellationSource.Token);
        }

        private void SensorLoop()
        {
            max30102.ClearFIFO();

            while (!cancellationSource.Token.IsCancellationRequested)
            {
                var (red, ir) = max30102.ReadFIFO();

                if (IsRecording)
                {
                    sensorWriter?.WriteLine($"{red}\t{ir}");
                }

                NewSensorDataReceived?.Invoke(red, ir);
            }
        }

        private void CameraLoop()
        {
            Mat frame = new Mat();
            while (!cancellationSource.Token.IsCancellationRequested)
            {
                capture.Read(frame);

                if (IsRecording)
                {
                    videoWriter?.Write(frame);
                }

                NewFrameReceived?.Invoke(frame);
            }
        }

        public async Task Stop()
        {
            cancellationSource.Cancel();
            await Task.WhenAll(sensorTask, cameraTask);
        }

        public void Dispose()
        {
            capture.Dispose();
            max30102.Close();
        }
    }
}
