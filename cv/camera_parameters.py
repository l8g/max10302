import cv2

# 初始化摄像头
cap = cv2.VideoCapture(0)

# 检查摄像头是否成功打开
if not cap.isOpened():
    print("无法打开摄像头。")
    exit()

# 打印一些常用的摄像头参数
parameters = [
    ("CAP_PROP_FRAME_WIDTH", "帧宽度"),
    ("CAP_PROP_FRAME_HEIGHT", "帧高度"),
    ("CAP_PROP_FPS", "帧率"),
    ("CAP_PROP_BRIGHTNESS", "亮度"),
    ("CAP_PROP_CONTRAST", "对比度"),
    ("CAP_PROP_SATURATION", "饱和度"),
    ("CAP_PROP_HUE", "色调"),
    ("CAP_PROP_EXPOSURE", "曝光"),
    ("CAP_PROP_GAIN", "增益"),
]

for param, description in parameters:
    param_value = cap.get(getattr(cv2, param))
    print(f"{description} ({param}): {param_value}")

# 释放摄像头资源
cap.release()
