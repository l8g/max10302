
import ch341
from address import *
from graphic import graphic

class MAX30102:
    def __init__(self) -> None:
        ch341.open()
        ch341.write(REG_MODE_CONFIG, 0x40)

        # Set interrupts
        ch341.write(REG_INTR_ENABLE_1, 0xC0)
        ch341.write(REG_INTR_ENABLE_2, 0x00)
        
        # Initialize FIFO write pointer
        ch341.write(REG_FIFO_WR_PTR, 0x00)
        
        # Initialize overflow counter
        ch341.write(REG_OVF_COUNTER, 0x00)
        
        # Initialize FIFO read pointer
        ch341.write(REG_FIFO_RD_PTR, 0x00)
        
        # Set FIFO configuration
        ch341.write(REG_FIFO_CONFIG, 0x0F)
        
        # Set mode configuration
        ch341.write(REG_MODE_CONFIG, 0x03)
        
        # Set SpO2 configuration
        ch341.write(REG_SPO2_CONFIG, 0x27)
        
        # Set LED1 pulse amplitude
        ch341.write(REG_LED1_PA, 0x32)
        
        # Set LED2 pulse amplitude
        ch341.write(REG_LED2_PA, 0x32)
        
        # Set Pilot LED pulse amplitude
        ch341.write(REG_PILOT_PA, 0x7F)

    def read_fifo(self):
        num_avail_samples = self.get_num_avail_samples()
        while num_avail_samples == 0:
            num_avail_samples = self.get_num_avail_samples()

        for _ in range(num_avail_samples):
            bytes = ch341.read(REG_FIFO_DATA, 6)
            red = extract_bits(bytes[0], bytes[1], bytes[2])
            ir = extract_bits(bytes[3], bytes[4], bytes[5])
        return red, ir

        
    def get_num_avail_samples(self):
        num_avail_samples = self.get_write_pointer() - self.get_read_pointer()
        
        if num_avail_samples < 0:
            num_avail_samples += 32
        
        if num_avail_samples == 0 and self.get_overflow_counter() > 0:
            num_avail_samples = 32

        return num_avail_samples
    
    def ppg_ready(self):
        status = ch341.read(REG_INTR_STATUS_1)
        return bool(status & PPG_RDY_BIT_MASK)
    
    def reset_read_pointer(self):
        ch341.write(REG_FIFO_RD_PTR, 0x00)

    def reset_write_pointer(self):
        ch341.write(REG_FIFO_WR_PTR, 0x00)

    def get_read_pointer(self):
        return ch341.read(REG_FIFO_RD_PTR)
    
    def get_write_pointer(self):
        return ch341.read(REG_FIFO_WR_PTR)
    
    def get_overflow_counter(self):
        return ch341.read(REG_OVF_COUNTER) & 0x1f

    def close(self):
        ch341.close()
    







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
    try:
        max30102 = MAX30102()
        g = graphic()

        while True:
            red, ir = max30102.read_fifo()
            g.draw_data(ir)
    finally:
        max30102.close()