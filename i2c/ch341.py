


from ctypes import *
from address import *

ch341 = windll.LoadLibrary("CH341DLLA64.dll")

usb_id = 0
dev_addr = MAX30102_ADDRESS

def open():
    if (ch341.CH341OpenDevice(usb_id) != -1):
        ch341.CH341SetStream(usb_id, 0x82)
    else:
        raise Exception("USB CH341 Open Failed!")
    
def close():
    ch341.CH341CloseDevice(usb_id)


def read(addr, byte_num = 1):
    ibuf = (c_byte * 2)()
    obuf = (c_byte * byte_num)()
    ibuf[0] = dev_addr
    ibuf[1] = addr
    ch341.CH341StreamI2C(usb_id, 2, ibuf, byte_num, obuf)
    if byte_num == 1:
        return obuf[0] & 0xff
    else:
        return [x & 0xff for x in obuf]

def write(addr, dat):
    ibuf = (c_byte * 3)()
    obuf = (c_byte * 1)()
    ibuf[0] = dev_addr
    ibuf[1] = addr
    ibuf[2] = dat & 0xff
    ch341.CH341StreamI2C(usb_id, 3, ibuf, 0, obuf)


if __name__ == "__main__":
    try:
        open()
        print(read(REG_FIFO_DATA, 2))
        print(read(REG_FIFO_DATA, 2))
        print(read(REG_FIFO_DATA, 2))
        print(read(REG_FIFO_DATA, 2))
        print(read(REG_FIFO_DATA, 2))
    finally:
        close()
    