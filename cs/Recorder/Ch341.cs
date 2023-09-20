public class Ch341
{
    private static int usb_id = 0;
    private static byte dev_addr = Max30102Constants.MAX30102_ADDRESS;

    public static void OpenDevice()
    {
        if (CH341Native.CH341OpenDevice(usb_id) != -1)
        {
            CH341Native.CH341SetStream(usb_id, 0x82);
        }
        else
        {
            throw new Exception("USB CH341 Open Failed!");
        }
    }

    public static void CloseDevice()
    {
        CH341Native.CH341CloseDevice(usb_id);
    }

    public static int[] Read(byte addr, int byte_num = 1)
    {
        byte[] ibuf = new byte[2];
        byte[] obuf = new byte[byte_num];
        ibuf[0] = dev_addr;
        ibuf[1] = addr;

        CH341Native.CH341StreamI2C(usb_id, 2, ibuf, byte_num, obuf);

        int[] result = new int[byte_num];
        for (int i = 0; i < byte_num; ++i)
        {
            result[i] = obuf[i] & 0xFF;
        }

        return result;
    }

    public static void Write(byte addr, byte dat)
    {
        byte[] ibuf = new byte[3];
        byte[] obuf = new byte[1];
        ibuf[0] = dev_addr;
        ibuf[1] = addr;
        ibuf[2] = (byte)(dat & 0xFF);

        CH341Native.CH341StreamI2C(usb_id, 3, ibuf, 0, obuf);
    }

    public static void Run()
    {
        try
        {
            OpenDevice();
            Console.WriteLine(string.Join(",", Read(Max30102Constants.REG_FIFO_DATA, 2)));
            Console.WriteLine(string.Join(",", Read(Max30102Constants.REG_FIFO_DATA, 2)));
            Console.WriteLine(string.Join(",", Read(Max30102Constants.REG_FIFO_DATA, 2)));
            Console.WriteLine(string.Join(",", Read(Max30102Constants.REG_FIFO_DATA, 2)));
            Console.WriteLine(string.Join(",", Read(Max30102Constants.REG_FIFO_DATA, 2)));
        }
        finally
        {
            CloseDevice();
        }
    }
}
