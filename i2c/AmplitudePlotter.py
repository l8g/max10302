import numpy as np
import matplotlib.pyplot as plt

class AmplitudePlotter:
    def __init__(self, sampling_rate):
        self.sampling_rate = sampling_rate
        self.fig, self.ax = plt.subplots()
        self.ax.set_title('Amplitude Spectrum')
        self.ax.set_xlabel('Frequency [Hz]')
        self.ax.set_ylabel('Amplitude')
        plt.ion()  # 打开交互模式

    def plot(self, signal):
        # 清除旧图
        self.ax.clear()
        self.ax.set_title('Amplitude Spectrum')
        self.ax.set_xlabel('Frequency [per minute]')
        self.ax.set_ylabel('Amplitude')

        # 计算傅里叶变换
        N = len(signal)
        yf = np.fft.fft(signal)
        xf = np.fft.fftfreq(N, 1 / self.sampling_rate)[1:N // 2]  # 跳过0频率
        xf_per_minute = xf * 60  # 将频率转换为每分钟

        amplitude = 2.0 / N * np.abs(yf[1:N // 2])  # 跳过0频率

        # 过滤频率
        filter_mask = xf_per_minute <= 200
        xf_filtered = xf_per_minute[filter_mask]
        amplitude_filtered = amplitude[filter_mask]

        # 画新图
        self.ax.plot(xf_filtered, amplitude_filtered)  # 使用过滤后的数据
        plt.draw()
        plt.pause(0.01)

    def close(self):
        plt.ioff()  # 关闭交互模式
        plt.show()

# 示例使用
if __name__ == '__main__':
    Fs = 500  # 采样率
    plotter = AmplitudePlotter(Fs)

    # 第一次画图
    t = np.linspace(0, 1, Fs)
    y1 = np.sin(2 * np.pi * 5 * t) + np.sin(2 * np.pi * 50 * t)
    plotter.plot(y1)

    # 等待2秒，然后画新图
    plt.pause(2)
    y2 = np.sin(2 * np.pi * 10 * t) + np.sin(2 * np.pi * 40 * t)
    plotter.plot(y2)

    # 关闭
    plotter.close()
