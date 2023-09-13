import cmath
import numpy as np

FFT_N = 512  # 假设FFT的大小为512
SAMPLES_PER_SECOND = 100  # 假设每秒采样率
CORRECTED_VALUE = 47  # 标定血液氧气含量

# 血液数据存储
g_blooddata = {'heart': 0, 'SpO2': 0}

# 快速傅里叶变换（FFT）
def FFT(x):
    N = len(x)
    if N <= 1:
        return x
    even = FFT(x[0::2])
    odd = FFT(x[1::2])
    result = [0] * N
    for k in range(N // 2):
        t = cmath.exp(-2j * cmath.pi * k / N) * odd[k]
        result[k] = even[k] + t
        result[k + N // 2] = even[k] - t
    return result

# 找到最大数值的索引
def find_max_num_index(arr, start_index=0):
    max_val = 0
    max_index = start_index
    for i in range(start_index, len(arr)):
        if abs(arr[i]) > max_val:
            max_val = abs(arr[i])
            max_index = i
    return max_index

# 血液信息转换
def blood_data_translate(s1, s2):

    g_blooddata = dict()
    
    dc_red = np.mean([x.real for x in s1])
    dc_ir = np.mean([x.real for x in s2])
    
    s1 = [complex(x.real - dc_red, 0) for x in s1]
    s2 = [complex(x.real - dc_ir, 0) for x in s2]
    
    FFT_s1 = FFT(s1)
    FFT_s2 = FFT(s2)
    
    FFT_s1 = [cmath.sqrt(x.real**2 + x.imag**2) for x in FFT_s1]
    FFT_s2 = [cmath.sqrt(x.real**2 + x.imag**2) for x in FFT_s2]
    
    ac_red = np.mean([x.real for x in FFT_s1[1:]])
    ac_ir = np.mean([x.real for x in FFT_s2[1:]])
    
    s1_max_index = find_max_num_index(FFT_s1, 30)
    s2_max_index = find_max_num_index(FFT_s2, 30)
    
    Heart_Rate = 60.0 * ((100.0 * s1_max_index) / 512.0)
    
    R = (ac_ir * dc_red) / (ac_red * dc_ir)
    SpO2 = -45.060 * R * R + 30.354 * R + 94.845
    
    g_blooddata['heart'] = Heart_Rate
    g_blooddata['SpO2'] = SpO2

    return g_blooddata

