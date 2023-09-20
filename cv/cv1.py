import cv2
import tkinter as tk
from PIL import Image, ImageTk

# 创建Tkinter窗口
root = tk.Tk()
root.title("摄像头显示")

# 初始化摄像头
cap = cv2.VideoCapture(0)


# 创建Canvas用于显示图像
canvas = tk.Canvas(root, width=640, height=480)
canvas.pack()

def update_image():
    ret, frame = cap.read()
    if ret:
        # OpenCV图像格式为BGR，需转为RGB
        rgb_frame = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
        
        # 将OpenCV图像格式转换为PIL图像格式
        pil_img = Image.fromarray(rgb_frame)
        
        # 将PIL图像格式转换为Tkinter可用的图像格式
        tk_img = ImageTk.PhotoImage(image=pil_img)
        
        # 在Canvas上显示图像
        canvas.create_image(0, 0, anchor=tk.NW, image=tk_img)
        
        # 更新窗口
        root.update()

# 创建一个循环以不断更新图像
while True:
    update_image()

# 释放摄像头并销毁所有窗口
cap.release()
root.destroy()
