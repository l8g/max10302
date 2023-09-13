
import matplotlib.pyplot as plt
import time

class graphic:

    def __init__(self) -> None:
        plt.ion()  # 开启交互模式
        self.fig, self.ax = plt.subplots()
        self.s1_list = []
        self.timestamps = []
        self.start_time = time.time()

    def draw_data(self, s1):
        self.s1_list.append(s1)

        current_time = time.time() - self.start_time
        self.timestamps.append(current_time)

        # 如果数据过多，可以考虑只保存和显示最近的N个数据点
        if len(self.s1_list) > 100:  # 这里以100为例
            del self.s1_list[0]
            del self.timestamps[0]

        self.ax.clear()  # 清除之前的图形
        self.ax.plot(self.timestamps, self.s1_list)  # 绘制实时数据
        plt.xlabel('Time')
        plt.ylabel('s1 Value')
        plt.title('Real-time s1 plot with Timestamps')
        plt.pause(0.01)  # 稍微暂停以便更新图形