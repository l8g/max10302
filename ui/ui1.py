import tkinter as tk
from matplotlib.figure import Figure
from matplotlib.backends.backend_tkagg import FigureCanvasTkAgg

# 定义Tkinter窗口
root = tk.Tk()
root.title("Tkinter与Matplotlib配合使用示例")

# 创建一个Matplotlib图表
fig = Figure(figsize=(5, 4), dpi=100)
plot = fig.add_subplot(1, 1, 1)
plot.plot([1, 2, 3, 4], [1, 4, 9, 16])

# 将Matplotlib图表嵌入到Tkinter窗口中
canvas = FigureCanvasTkAgg(fig, master=root)
canvas.draw()
canvas.get_tk_widget().pack(side=tk.TOP, fill=tk.BOTH, expand=1)

# 添加一个Tkinter按钮，用于退出程序
button = tk.Button(master=root, text="退出", command=root.quit)
button.pack(side=tk.BOTTOM)

# Tkinter事件循环
root.mainloop()
