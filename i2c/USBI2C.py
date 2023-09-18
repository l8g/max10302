#! /usr/bin/env python
#coding=utf-8
import os
import time
from ctypes import *
import matplotlib.pyplot as plt
import numpy as np

# MAX30102寄存器地址
from address import *



class USBI2C():
    ch341 = windll.LoadLibrary("CH341DLLA64.dll")
    def __init__(self, usb_dev = 0, i2c_dev = 0x5c):
        self.usb_id   = usb_dev
        self.dev_addr = i2c_dev
        if USBI2C.ch341.CH341OpenDevice(self.usb_id) != -1:
            USBI2C.ch341.CH341SetStream(self.usb_id, 0x82)
            USBI2C.ch341.CH341CloseDevice(self.usb_id)
        else:
            print("USB CH341 Open Failed!")


    def read(self, addr):
        if USBI2C.ch341.CH341OpenDevice(self.usb_id) != -1:
            obuf = (c_byte * 2)()
            ibuf = (c_byte * 1)()
            obuf[0] = self.dev_addr
            obuf[1] = addr
            USBI2C.ch341.CH341StreamI2C(self.usb_id, 2, obuf, 1, ibuf)
            USBI2C.ch341.CH341CloseDevice(self.usb_id)
            return ibuf[0] & 0xff
        else:
            print("USB CH341 Open Failed!")
            return 0

    def write(self, addr, dat):
        if USBI2C.ch341.CH341OpenDevice(self.usb_id) != -1:
            obuf = (c_byte * 3)()
            ibuf = (c_byte * 1)()
            obuf[0] = self.dev_addr
            obuf[1] = addr
            obuf[2] = dat & 0xff
            USBI2C.ch341.CH341StreamI2C(self.usb_id, 3, obuf, 0, ibuf)
            USBI2C.ch341.CH341CloseDevice(self.usb_id)
        else:
            print("USB CH341 Open Failed!")

    def reset_sensor(self):
        self.write(REG_MODE_CONFIG, 0x40)

    def initialize_sensor(self):
        # Initialize the MAX30102 sensor using predefined constants
        
        # Set interrupts
        self.write(REG_INTR_ENABLE_1, 0xC0)
        self.write(REG_INTR_ENABLE_2, 0x00)
        
        # Initialize FIFO write pointer
        self.write(REG_FIFO_WR_PTR, 0x00)
        
        # Initialize overflow counter
        self.write(REG_OVF_COUNTER, 0x00)
        
        # Initialize FIFO read pointer
        self.write(REG_FIFO_RD_PTR, 0x00)
        
        # Set FIFO configuration
        self.write(REG_FIFO_CONFIG, 0x0F)
        
        # Set mode configuration
        self.write(REG_MODE_CONFIG, 0x03)
        
        # Set SpO2 configuration
        self.write(REG_SPO2_CONFIG, 0x27)
        
        # Set LED1 pulse amplitude
        self.write(REG_LED1_PA, 0x32)
        
        # Set LED2 pulse amplitude
        self.write(REG_LED2_PA, 0x32)
        
        # Set Pilot LED pulse amplitude
        self.write(REG_PILOT_PA, 0x7F)


    def max30102_read_fifo(self):

        if USBI2C.ch341.CH341OpenDevice(self.usb_id) != -1:

            obuf = (c_byte * 2)()  # 2 bytes: one for device address and one for register address
            ibuf = (c_byte * 6)()  # 6 bytes buffer for storing read data
            obuf[0] = self.dev_addr
            obuf[1] = 0x07  # REG_FIFO_DATA

            # Writing 2 bytes, reading 6 bytes
            USBI2C.ch341.CH341StreamI2C(self.usb_id, 2, obuf, 6, ibuf)

            intr_status_1 = self.read(0x00)  # Read REG_INTR_STATUS_1
            intr_status_2 = self.read(0x01)  # Read REG_INTR_STATUS_2

            # print("intr_status_1: ", intr_status_1, "intr_status_2: ", intr_status_2)

            USBI2C.ch341.CH341CloseDevice(self.usb_id)

            # print(ibuf[0], ibuf[1], ibuf[2], ibuf[3], ibuf[4], ibuf[5], type(ibuf[0]))

            # 解析数据
            fifo_red = extract_bits(ibuf[0], ibuf[1], ibuf[2])
            fifo_ir = extract_bits(ibuf[3], ibuf[4], ibuf[5])

            return fifo_red, fifo_ir
        else:
            print("USB CH341 Open Failed!")
            return 0, 0
        
    def check_PPG_RDY(self):
        status = self.read(REG_INTR_STATUS_1)
        return bool(status & PPG_RDY_BIT_MASK)
    
def extract_bits(byte1, byte2, byte3):
    # 取出byte1的后两位
    last_two_bits_byte1 = byte1 & 0b11  # 与二进制'11'进行按位与操作，相当于取出后两位

    # 将last_two_bits_byte1左移16位
    last_two_bits_byte1_shifted = (last_two_bits_byte1 & 0xff) << 16

    # 将byte2左移8位
    byte2_shifted = (byte2 & 0xff) << 8

    # 组合所有的位以形成一个18位的数字
    combined = last_two_bits_byte1_shifted | byte2_shifted | (byte3 & 0xff)
    
    return combined


if __name__ == "__main__":
    usbi2c = USBI2C(0, 0xAE)
    usbi2c.reset_sensor()
    usbi2c.initialize_sensor()

    plt.ion()  # 开启交互模式
    fig, ax = plt.subplots()
    s1_list = []
    timestamps = []

    start_time = time.time()

    while True:
        if usbi2c.check_PPG_RDY():
            s1, s2 = usbi2c.max30102_read_fifo()
            print(s1, s2)
           
            current_time = time.time() - start_time  # 获取当前时间戳
        
            # 将数据和时间戳添加到列表
            s1_list.append(s1)
            timestamps.append(current_time)
            
            # 如果数据过多，可以考虑只保存和显示最近的N个数据点
            if len(s1_list) > 100:  # 这里以100为例
                del s1_list[0]
                del timestamps[0]

            ax.clear()  # 清除之前的图形
            ax.plot(timestamps, s1_list)  # 绘制实时数据
            plt.xlabel('Time')
            plt.ylabel('s1 Value')
            plt.title('Real-time s1 plot with Timestamps')
            plt.pause(0.01)  # 稍微暂停以便更新图形