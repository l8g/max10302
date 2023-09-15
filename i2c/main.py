

from USBI2C import USBI2C
from blood import blood_data_translate
import numpy as np
from time import time
from max30102 import MAX30102
from address import *



s1 = np.zeros(SIGNAL_LENGTH, dtype=complex)
s2 = np.zeros(SIGNAL_LENGTH, dtype=complex)
t = np.zeros(SIGNAL_LENGTH, dtype=float)

usbi2c = USBI2C(0, 0xAE)
usbi2c.reset_sensor()
usbi2c.initialize_sensor()

max30102 = MAX30102()

while True:
    g_fft_index = 0
    startTime = time()
    while g_fft_index < SIGNAL_LENGTH:
        fifo_red, fifo_ir = max30102.read_fifo()
        s1[g_fft_index] = fifo_red + 0j
        s2[g_fft_index] = fifo_ir + 0j
        t[g_fft_index] = time() - startTime
        g_fft_index += 1

    g_blooddata = blood_data_translate(s1, s2, t)
    print(g_blooddata.heart, g_blooddata.SpO2)







