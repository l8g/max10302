import matplotlib.pyplot as plt
import numpy as np

# 产生一个简单的信号作为例子
Fs = 500  # 采样率
T = 1/Fs  # 采样周期
t = np.linspace(0.0, 1.0, Fs)  # 时间轴

# 创建一个包含两个频率成分的信号
f1, f2 = 5, 50  # 频率
y = np.sin(2 * np.pi * f1 * t) + np.sin(2 * np.pi * f2 * t)

# 执行FFT
yf = np.fft.fft(y)
xf = np.fft.fftfreq(len(y), T)[:len(y)//2]

# 绘制幅度谱
plt.figure()
plt.title('Amplitude Spectrum')
plt.xlabel('Frequency [Hz]')
plt.ylabel('Amplitude')
plt.plot(xf, 2.0/len(y) * np.abs(yf[:len(y)//2]))
plt.show()
