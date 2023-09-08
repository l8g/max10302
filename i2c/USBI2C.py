#! /usr/bin/env python
#coding=utf-8
import os
import time
from ctypes import *

PPG_RDY_BIT_MASK = 0x40  # 第6位对应的位掩码，值为64或者0x40
REG_INTR_STATUS_1 = 0x00  # MAX30102的Interrupt Status 1寄存器地址

class USBI2C():
    ch341 = windll.LoadLibrary("CH341DLL.dll")
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
        self.write(0x09, 0x40)

    def initialize_sensor(self):
        # MAX30102 初始化
        self.write(0x02, 0xC0)  # 设置中断
        self.write(0x03, 0x00)  
        self.write(0x04, 0x00)  # FIFO写指针
        self.write(0x05, 0x00)  # 溢出计数器
        self.write(0x06, 0x00)  # FIFO读指针
        self.write(0x08, 0x0F)  # FIFO配置
        self.write(0x09, 0x03)  # 工作模式
        self.write(0x0A, 0x27)  # SpO2配置
        self.write(0x0C, 0x32)  # LED1功率
        self.write(0x0D, 0x32)  # LED2功率
        self.write(0x10, 0x7F)  # Pilot LED功率


    def max30102_read_fifo(self):

        if USBI2C.ch341.CH341OpenDevice(self.usb_id) != -1:

            obuf = (c_byte * 2)()  # 2 bytes: one for device address and one for register address
            ibuf = (c_byte * 6)()  # 6 bytes buffer for storing read data
            obuf[0] = self.dev_addr
            obuf[1] = 0x07  # REG_FIFO_DATA

            # Writing 2 bytes, reading 6 bytes
            print(USBI2C.ch341.CH341StreamI2C(self.usb_id, 2, obuf, 6, ibuf))

            intr_status_1 = self.read(0x00)  # Read REG_INTR_STATUS_1
            intr_status_2 = self.read(0x01)  # Read REG_INTR_STATUS_2

            print("intr_status_1: ", intr_status_1, "intr_status_2: ", intr_status_2)

            USBI2C.ch341.CH341CloseDevice(self.usb_id)

            # 解析数据
            fifo_red = 0
            fifo_ir = 0
            
            un_temp = ibuf[0]
            un_temp <<= 14
            fifo_red += un_temp
            un_temp = ibuf[1]
            un_temp <<= 6
            fifo_red += un_temp
            un_temp = ibuf[2]
            un_temp >>= 2
            fifo_red += un_temp
            
            un_temp = ibuf[3]
            un_temp <<= 14
            fifo_ir += un_temp
            un_temp = ibuf[4]
            un_temp <<= 6
            fifo_ir += un_temp
            un_temp = ibuf[5]
            un_temp >>= 2
            fifo_ir += un_temp

            return fifo_red, fifo_ir
        else:
            print("USB CH341 Open Failed!")
            return 0, 0
        
    def check_PPG_RDY(self):
        status = self.read(REG_INTR_STATUS_1)
        return bool(status & PPG_RDY_BIT_MASK)
    


if __name__ == "__main__":
    usbi2c = USBI2C(0, 0xAE)
    usbi2c.reset_sensor()
    usbi2c.initialize_sensor()
    while True:
        while usbi2c.check_PPG_RDY() == False:
            print(usbi2c.max30102_read_fifo())