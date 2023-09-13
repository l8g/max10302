
# Interrupt status register addresses
REG_INTR_STATUS_1 = 0x00  # Interrupt status 1
REG_INTR_STATUS_2 = 0x01  # Interrupt status 2

# Interrupt enable register addresses
REG_INTR_ENABLE_1 = 0x02  # Interrupt enable 1
REG_INTR_ENABLE_2 = 0x03  # Interrupt enable 2

# FIFO register addresses
REG_FIFO_WR_PTR = 0x04  # FIFO write pointer
REG_OVF_COUNTER = 0x05  # Overflow counter
REG_FIFO_RD_PTR = 0x06  # FIFO read pointer
REG_FIFO_DATA = 0x07    # FIFO data
REG_FIFO_CONFIG = 0x08  # FIFO configuration

# Configuration register addresses
REG_MODE_CONFIG = 0x09    # Mode configuration
REG_SPO2_CONFIG = 0x0A    # SpO2 configuration

# LED pulse amplitude register addresses
REG_LED1_PA = 0x0C  # LED1 pulse amplitude
REG_LED2_PA = 0x0D  # LED2 pulse amplitude

# Pilot pulse amplitude register address
REG_PILOT_PA = 0x10  # Pilot pulse amplitude

# Multi-LED control register addresses
REG_MULTI_LED_CTRL1 = 0x11  # Multi-LED control 1
REG_MULTI_LED_CTRL2 = 0x12  # Multi-LED control 2

# Temperature register addresses
REG_TEMP_INTR = 0x1F  # Temperature integer
REG_TEMP_FRAC = 0x20  # Temperature fraction
REG_TEMP_CONFIG = 0x21  # Temperature configuration

# Proximity interrupt threshold register address
REG_PROX_INT_THRESH = 0x30  # Proximity interrupt threshold

# Device ID register addresses
REG_REV_ID = 0xFE  # Revision ID
REG_PART_ID = 0xFF  # Part ID
