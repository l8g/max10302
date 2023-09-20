import cv2
import numpy as np

# 初始化摄像头
cap = cv2.VideoCapture(0)

# 获取视频帧的尺寸
width = int(cap.get(cv2.CAP_PROP_FRAME_WIDTH))
height = int(cap.get(cv2.CAP_PROP_FRAME_HEIGHT))

# 创建VideoWriter对象
fourcc = cv2.VideoWriter_fourcc('Y', 'U', 'Y', '2')
out = cv2.VideoWriter('output_yuy2.avi', fourcc, 30.0, (width, height))

while cap.isOpened():
    ret, frame = cap.read()
    if not ret:
        print("Failed to grab frame")
        break

    # 转换到YUV色彩空间
    yuv_frame = cv2.cvtColor(frame, cv2.COLOR_BGR2YUV)
    
    # 分离Y、U、V通道
    y, u, v = cv2.split(yuv_frame)

    # 输出Y、U、V通道的统计数据
    print(f"Y channel: min = {np.min(y)}, max = {np.max(y)}, mean = {np.mean(y)}")
    print(f"U channel: min = {np.min(u)}, max = {np.max(u)}, mean = {np.mean(u)}")
    print(f"V channel: min = {np.min(v)}, max = {np.max(v)}, mean = {np.mean(v)}")

    # 写入帧到VideoWriter
    out.write(frame)

    # 显示帧
    cv2.imshow('Frame', frame)

    if cv2.waitKey(1) & 0xFF == ord('q'):
        break

# 释放资源
cap.release()
out.release()
cv2.destroyAllWindows()
