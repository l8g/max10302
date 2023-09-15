
import matplotlib.pyplot as plt

plt.ion()  # 开启交互模式


def figure(index):
    plt.figure(index)

def update(index, data):
    plt.figure(index)
    plt.clf()
    plt.plot(data)
    plt.pause(0.001)
