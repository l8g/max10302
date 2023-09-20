public class MAX30102
{
    public MAX30102()
    {
        Ch341.OpenDevice();

        Ch341.Write(Max30102Constants.REG_MODE_CONFIG, 0x40);
        Ch341.Write(Max30102Constants.REG_INTR_ENABLE_1, 0xC0);
        Ch341.Write(Max30102Constants.REG_INTR_ENABLE_2, 0x00);
        Ch341.Write(Max30102Constants.REG_FIFO_WR_PTR, 0x00);
        Ch341.Write(Max30102Constants.REG_OVF_COUNTER, 0x00);
        Ch341.Write(Max30102Constants.REG_FIFO_RD_PTR, 0x00);
        Ch341.Write(Max30102Constants.REG_FIFO_CONFIG, 0x0F);
        Ch341.Write(Max30102Constants.REG_MODE_CONFIG, 0x03);
        Ch341.Write(Max30102Constants.REG_SPO2_CONFIG, 0x27);
        Ch341.Write(Max30102Constants.REG_LED1_PA, 0x32);
        Ch341.Write(Max30102Constants.REG_LED2_PA, 0x32);
        Ch341.Write(Max30102Constants.REG_PILOT_PA, 0x7F);
    }

    public (int red, int ir) ReadFIFO()
    {
        int numAvailSamples = GetNumAvailSamples();
        while (numAvailSamples == 0)
        {
            numAvailSamples = GetNumAvailSamples();
        }

        int red = 0, ir = 0;
        for (int i = 0; i < numAvailSamples; i++)
        {
            int[] bytes = Ch341.Read(Max30102Constants.REG_FIFO_DATA, 6);
            red = ExtractBits(bytes[0], bytes[1], bytes[2]);
            ir = ExtractBits(bytes[3], bytes[4], bytes[5]);
        }

        return (red, ir);
    }

    private static int ExtractBits(int byte1, int byte2, int byte3)
    {
        int lastTwoBitsByte1 = byte1 & 0x03;
        int lastTwoBitsByte1Shifted = lastTwoBitsByte1 << 16;
        int byte2Shifted = byte2 << 8;

        int combined = lastTwoBitsByte1Shifted | byte2Shifted | byte3;

        return combined;
    }

    private int GetNumAvailSamples()
    {
        int readPointer = GetReadPointer();
        int writePointer = GetWritePointer();

        int numAvailSamples = writePointer - readPointer;
        if (numAvailSamples < 0)
        {
            numAvailSamples += 32;
        }
        if (numAvailSamples == 0 && GetOverflowCounter() > 0)
        {
            numAvailSamples = 32;
        }
        return numAvailSamples;
    }

    private void ResetReadPointer()
    {
        Ch341.Write(Max30102Constants.REG_FIFO_RD_PTR, 0x00);
    }
    private void ResetWritePointer()
    {
          Ch341.Write(Max30102Constants.REG_FIFO_WR_PTR, 0x00);
    }

    public void ClearFIFO()
    {         
        ResetReadPointer();
        ResetWritePointer();
    }

    public int GetWritePointer()
    {
        int[] bytes = Ch341.Read(Max30102Constants.REG_FIFO_WR_PTR, 1);
        return bytes[0];
    }

    public int GetReadPointer()
    {
        int[] bytes = Ch341.Read(Max30102Constants.REG_FIFO_RD_PTR, 1);
        return bytes[0];
    }

    public int GetOverflowCounter()
    {
        int[] bytes = Ch341.Read(Max30102Constants.REG_OVF_COUNTER, 1);
        return bytes[0] & 0x1F;
    }

    public void Close()
    {
        Ch341.CloseDevice();
    }
}
