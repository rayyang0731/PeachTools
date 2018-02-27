/// <summary>
/// 计算 CRC32 校验值
/// </summary>
public static class CRC32 {
    private static ulong[] Crc32Table;

    /// <summary>
    /// 生成CRC32码表
    /// </summary>
    private static void GetCRC32Table () {
        ulong Crc;
        Crc32Table = new ulong[256];
        int i, j;
        for (i = 0; i < 256; i++) {
            Crc = (ulong) i;
            for (j = 8; j > 0; j--) {
                if ((Crc & 1) == 1)
                    Crc = (Crc >> 1) ^ 0xEDB88320;
                else
                    Crc >>= 1;
            }
            Crc32Table[i] = Crc;
        }
    }
    /// <summary>
    /// 获取CRC32校验值
    /// </summary>
    /// <param name="_input">要进行计算的字符串</param>
    /// <returns></returns>
    public static ulong GetCRC32 (string _input) {
        byte[] buffer = System.Text.ASCIIEncoding.ASCII.GetBytes (_input);
        return GetCRC32 (buffer);
    }

    /// <summary>
    /// 获取CRC32校验值
    /// </summary>
    /// <param name="buffer">要进行计算的 buffer</param>
    /// <returns></returns>
    public static ulong GetCRC32 (byte[] buffer) {
        //生成码表
        GetCRC32Table ();

        ulong value = 0xffffffff;
        int len = buffer.Length;
        for (int i = 0; i < len; i++) {
            value = (value >> 8) ^ Crc32Table[(value & 0xFF) ^ buffer[i]];
        }
        return value ^ 0xffffffff;
    }
}