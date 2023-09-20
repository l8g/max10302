public static class Max30102Constants
{
    public const byte MAX30102_ADDRESS = 0xAE;  // 7-bit I2C address

    // Interrupt status register addresses
    public const byte REG_INTR_STATUS_1 = 0x00;  // Interrupt status 1
    public const byte REG_INTR_STATUS_2 = 0x01;  // Interrupt status 2

    // Interrupt enable register addresses
    public const byte REG_INTR_ENABLE_1 = 0x02;  // Interrupt enable 1
    public const byte REG_INTR_ENABLE_2 = 0x03;  // Interrupt enable 2

    // FIFO register addresses
    public const byte REG_FIFO_WR_PTR = 0x04;  // FIFO write pointer
    public const byte REG_OVF_COUNTER = 0x05;  // Overflow counter
    public const byte REG_FIFO_RD_PTR = 0x06;  // FIFO read pointer
    public const byte REG_FIFO_DATA = 0x07;  // FIFO data
    public const byte REG_FIFO_CONFIG = 0x08;  // FIFO configuration

    // Configuration register addresses
    public const byte REG_MODE_CONFIG = 0x09;  // Mode configuration
    public const byte REG_SPO2_CONFIG = 0x0A;  // SpO2 configuration

    // LED pulse amplitude register addresses
    public const byte REG_LED1_PA = 0x0C;  // LED1 pulse amplitude
    public const byte REG_LED2_PA = 0x0D;  // LED2 pulse amplitude

    // Pilot pulse amplitude register address
    public const byte REG_PILOT_PA = 0x10;  // Pilot pulse amplitude

    // Multi-LED control register addresses
    public const byte REG_MULTI_LED_CTRL1 = 0x11;  // Multi-LED control 1
    public const byte REG_MULTI_LED_CTRL2 = 0x12;  // Multi-LED control 2

    // Temperature register addresses
    public const byte REG_TEMP_INTR = 0x1F;  // Temperature integer
    public const byte REG_TEMP_FRAC = 0x20;  // Temperature fraction
    public const byte REG_TEMP_CONFIG = 0x21;  // Temperature configuration

    // Proximity interrupt threshold register address
    public const byte REG_PROX_INT_THRESH = 0x30;  // Proximity interrupt threshold

    // Device ID register addresses
    public const byte REG_REV_ID = 0xFE;  // Revision ID
    public const byte REG_PART_ID = 0xFF;  // Part ID

    public const byte PPG_RDY_BIT_MASK = 0x40;  // Bit mask for 6th bit, value is 64 or 0x40

    public const int SIGNAL_LENGTH = 3000;  // Signal length
}
