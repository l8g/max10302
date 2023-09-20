import tkinter as tk
from matplotlib.figure import Figure
from matplotlib.backends.backend_tkagg import FigureCanvasTkAgg

def plot():
    x = [1, 2, 3, 4]
    y = [1, 4, 9, 16]
    ax.plot(x, y)
    canvas.draw()

root = tk.Tk()
root.title("Grid布局示例")

fig = Figure(figsize=(5, 4), dpi=100)
ax = fig.add_subplot(1, 1, 1)

canvas = FigureCanvasTkAgg(fig, master=root)
canvas.get_tk_widget().grid(row=0, column=0, columnspan=2)

button1 = tk.Button(root, text="绘图", command=plot)
button1.grid(row=1, column=0)

button2 = tk.Button(root, text="退出", command=root.quit)
button2.grid(row=2, column=1)

root.mainloop()
