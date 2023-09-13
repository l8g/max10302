import numpy as np
from scipy.fft import fft

g_fft_index = 0
FFT_N = 512  # 假设FFT_N是512，根据您的具体情况定义
s1 = np.zeros((FFT_N + 16), dtype=np.complex128)
s2 = np.zeros((FFT_N + 16), dtype=np.complex128)
g_BloodWave = {'Hp': 0, 'HpO2': 0}
g_blooddata = {'heart': 0, 'SpO2': 0}

# 假设max30102_read_fifo是读取FIFO的函数，并返回两个值，fifo_red和fifo_ir
def max30102_read_fifo():
    # 实现细节
    return 0, 0

def blood_data_update():
    global g_fft_index, s1, s2
    g_fft_index = 0
    while g_fft_index < FFT_N:
        fifo_red, fifo_ir = max30102_read_fifo()
        s1[g_fft_index] = fifo_red + 0j
        s2[g_fft_index] = fifo_ir + 0j
        g_fft_index += 1

def find_max_num_index(arr, start_idx=0):
    max_idx = np.argmax(np.abs(arr[start_idx:])) + start_idx
    return max_idx

def blood_data_translate():
    global g_fft_index, s1, s2, g_blooddata
    dc_red = np.mean(s1.real)
    dc_ir = np.mean(s2.real)

    s1.real -= dc_red
    s2.real -= dc_ir

    s1.real = np.convolve(s1.real, np.ones(4)/4, mode='same')
    s2.real = np.convolve(s2.real, np.ones(4)/4, mode='same')

    s1 = fft(s1)
    s2 = fft(s2)

    s1.real = np.sqrt(s1.real**2 + s1.imag**2)
    s2.real = np.sqrt(s2.real**2 + s2.imag**2)

    ac_red = np.sum(s1.real[1:])
    ac_ir = np.sum(s2.real[1:])

    s1_max_index = find_max_num_index(s1, 30)
    s2_max_index = find_max_num_index(s2, 30)

    Heart_Rate = 60.0 * (100.0 * s1_max_index) / 512.0

    g_blooddata['heart'] = Heart_Rate

    R = (ac_ir * dc_red) / (ac_red * dc_ir)
    sp02_num = -45.060 * R ** 2 + 30.354 * R + 94.845
    g_blooddata['SpO2'] = sp02_num

def blood_Loop():
    global g_blooddata
    print("fft start ... ")

    blood_data_update()
    blood_data_translate()

    print("fft end ... ")

    if g_blooddata['heart'] < 50:
        print(f"Heart: error")
        print(f"SpO2: error")
    else:
        print(f"Heart: {g_blooddata['heart']}/min")
        print(f"SpO2: {g_blooddata['SpO2']}%")
