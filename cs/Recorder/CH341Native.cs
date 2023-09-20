using System.Runtime.InteropServices;

public class CH341Native
{
    const string dllName = "CH341DLLA64.dll";

    [DllImport(dllName)]
    public static extern int CH341OpenDevice(int index);

    [DllImport(dllName)]
    public static extern void CH341SetStream(int index, int param);

    [DllImport(dllName)]
    public static extern void CH341StreamI2C(int index, int writeLength, byte[] writeBuffer, int readLength, byte[] readBuffer);

    [DllImport(dllName)]
    public static extern void CH341CloseDevice(int index);
}
