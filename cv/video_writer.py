import cv2
import numpy as np

# 创建一个VideoCapture对象，从摄像头捕获视频
cap = cv2.VideoCapture(0)

# 设置摄像头的分辨率（这里假设摄像头支持640x480分辨率）
cap.set(cv2.CAP_PROP_FRAME_WIDTH, 640)
cap.set(cv2.CAP_PROP_FRAME_HEIGHT, 480)

# 获取视频的宽度和高度
width = int(cap.get(cv2.CAP_PROP_FRAME_WIDTH))
height = int(cap.get(cv2.CAP_PROP_FRAME_HEIGHT))

# 创建VideoWriter对象
fourcc = cv2.VideoWriter_fourcc('Y', 'U', 'Y', '2')
out = cv2.VideoWriter('output_yuy2.avi', fourcc, 30.0, (width, height))

while True:
    ret, frame = cap.read()  # 读取一帧

    if not ret:
        print("Failed to capture image")
        break

    # 这里您可以进行一些图像处理
    # ...
    
    out.write(frame)  # 将帧写入输出视频文件

    cv2.imshow('frame', frame)  # 在窗口中显示当前帧

    # 按“q”退出循环
    if cv2.waitKey(1) & 0xFF == ord('q'):
        break

# 释放所有资源
cap.release()
out.release()
cv2.destroyAllWindows()
