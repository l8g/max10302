import numpy as np
import matplotlib.pyplot as plt
from mpl_toolkits.mplot3d import Axes3D

# 参数设置
Fs = 1000  # 采样率
T = 1/Fs  # 采样间隔
t = np.linspace(0.0, 1.0, Fs)  # 时间轴
num_slices = 10  # 水平图中的切片数量

# 创建一个3D子图
fig = plt.figure()
ax = fig.add_subplot(111, projection='3d')

# 创建并绘制每个时间切片
for i in range(num_slices):
    f1, f2 = 5 + i, 50 - i  # 频率随时间变化
    y = np.sin(2 * np.pi * f1 * t) + np.sin(2 * np.pi * f2 * t)  # 生成信号

    # 执行FFT并获取幅度
    yf = np.fft.fft(y)
    xf = np.fft.fftfreq(len(y), T)[:Fs//2]
    amplitude = 2.0/Fs * np.abs(yf[:Fs//2])

    # 在3D图中添加切片
    ax.plot(xf, np.full_like(xf, i), amplitude)

# 设置图形属性
ax.set_xlabel('Frequency [Hz]')
ax.set_ylabel('Time Slice')
ax.set_zlabel('Amplitude')
plt.title('Waterfall Plot')
plt.show()
