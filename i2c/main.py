

from USBI2C import USBI2C
from blood import blood_data_translate
import numpy as np
from time import time

FFT_N = 512  # 假设一个FFT点数

s1 = np.zeros(FFT_N, dtype=complex)
s2 = np.zeros(FFT_N, dtype=complex)
t = np.zeros(FFT_N, dtype=float)

usbi2c = USBI2C(0, 0xAE)
usbi2c.reset_sensor()
usbi2c.initialize_sensor()



while True:
    g_fft_index = 0
    startTime = time()
    while g_fft_index < FFT_N:
        while usbi2c.check_PPG_RDY():
            fifo_red, fifo_ir = usbi2c.max30102_read_fifo()
            s1[g_fft_index] = fifo_red + 0j
            s2[g_fft_index] = fifo_ir + 0j
            t[g_fft_index] = time() - startTime
            g_fft_index += 1

    g_blooddata = blood_data_translate(s1, s2, t)
    print(g_blooddata.heart, g_blooddata.SpO2)







