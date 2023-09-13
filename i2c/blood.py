import numpy as np
from FFT import FFT

class BloodData:
    def __init__(self):
        self.heart = 0
        self.SpO2 = 0

        
def blood_data_translate(s1, s2, t):
    FFT_N = len(s1)
    blood_data = BloodData()
    
    # 直流滤波
    dc_red = np.mean([x.real for x in s1])
    dc_ir = np.mean([x.real for x in s2])
    
    s1 = s1 - dc_red
    s2 = s2 - dc_ir
    
    # 移动平均滤波
    for i in range(1, FFT_N-1):
        s1[i] = (s1[i-1] + 2 * s1[i] + s1[i+1]) / 4
        s2[i] = (s2[i-1] + 2 * s2[i] + s2[i+1]) / 4
    
    # 八点平均滤波
    for i in range(0, FFT_N-8):
        s1[i] = np.mean(s1[i:i+8])
        s2[i] = np.mean(s2[i:i+8])
        
    # FFT 变换
    s1_fft = np.fft.fft(s1)
    s2_fft = np.fft.fft(s2)
    
    # 解平方
    s1_magnitude = np.abs(s1_fft)
    s2_magnitude = np.abs(s2_fft)
    
    # 计算交流分量
    ac_red = np.sum(s1_magnitude[1:])
    ac_ir = np.sum(s2_magnitude[1:])
    
    # 读取峰值点的横坐标
    s1_max_index = np.argmax(s1_magnitude[30:])
    s2_max_index = np.argmax(s2_magnitude[30:])
    
    # 心率计算
    heart_rate = 60 * ((100.0 * s1_max_index) / 512.0)
    blood_data.heart = heart_rate
    
    # 血氧含量计算
    R = (ac_ir * dc_red) / (ac_red * dc_ir)
    sp02_num = -45.060 * R * R + 30.354 * R + 94.845
    blood_data.SpO2 = sp02_num
    
    return blood_data


