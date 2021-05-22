using System;
using System.Collections;
using System.IO;
using System.Text;
/// <summary>
/// 包含该track的总体信息，内容和track header的内容大致一样，type域为mdhd；
/// track header通常是对指定的track设定相关属性和内容。
/// media header是针对独立的media来设置，一般情况下二者相同。
/// </summary>
public class MediaHeaderBox : FullBox
{
    /// <summary>
    /// 创建时间，表示相对于UTC时间1904-01-01零点的秒数；
    /// 若fullbox中的version为0占四个字节，若version为1占8个字节；
    /// </summary>
    public DateTime CreateTime;

    /// <summary>
    /// 最后修改时间，表示相对于UTC时间1904-01-01零点的秒数；
    /// 若fullbox中的version为0占四个字节，若version为1占8个字节；
    /// </summary>
    public DateTime ModificationTime;

    /// <summary>
    /// 文件媒体在1秒时间内的刻度值，即1秒长度的时间单元数，占四个字节
    /// </summary>
    public uint TimeScale;

    /// <summary>
    /// 该track的时间长度；
    /// 若fullbox中的version为0占四个字节，若version为1占8个字节；
    /// 用duration和time scale可以计算track时长；
    /// 比如audio track的time scale = 8000, duration = 560128，时长为70.016秒；
    ///     video track的time scale = 600, duration = 42000，时长为70秒；
    /// </summary>
    public ulong Duration;

    /// <summary>
    /// 媒体语言码，占2个字节；
    /// 最高位为0，后面15位为3个字符（见ISO 639-2/T标准中定义）
    /// </summary>
    public string Language;

    /// <summary>
    /// 预留位，占2个字节
    /// </summary>
    public ushort PreDefined;

    public override void ReadContent(BinaryReader br)
    {
        if (Version == 1)
        {
            ulong seconds = GetUint64(br);
            CreateTime = new DateTime(1904, 1, 1).AddSeconds(seconds);
            seconds = GetUint64(br);
            ModificationTime = new DateTime(1904, 1, 1).AddSeconds(seconds);
            TimeScale = GetUint32(br);
            Duration = GetUint64(br);
        }
        else
        {
            uint seconds = GetUint32(br);
            CreateTime = new DateTime(1904, 1, 1).AddSeconds(seconds).ToLocalTime();
            seconds = GetUint32(br);
            ModificationTime = new DateTime(1904, 1, 1).AddSeconds(seconds).ToLocalTime();
            TimeScale = GetUint32(br);
            Duration = GetUint32(br);
        }

        byte[] arr = br.ReadBytes(2);
        BitArray bitArray = new BitArray(arr);
        // 后面15位为3个字符
        //Language = ("" + (char)GetByte(bitArray, 1, 5) + (char)GetByte(bitArray, 6, 5) + (char)GetByte(bitArray, 11, 5)).Trim('\0');
        // 以二进制输出
        Language = Convert.ToString(arr[0], 2).PadLeft(8, '0') + " " + Convert.ToString(arr[1], 2).PadLeft(8, '0');
        PreDefined = GetUint16(br);
    }

    public override string ToString()
    {
        StringBuilder str = new StringBuilder();
        str.Append(base.ToString());

        str.AppendLine("  CreateTime : " + CreateTime.ToString());
        str.AppendLine("  ModificationTime : " + ModificationTime.ToString());
        str.AppendLine("  TimeScale : " + TimeScale);
        str.AppendLine("  Duration : " + Duration);
        str.AppendLine("  Language : " + Language);
        str.AppendLine("  PreDefined : " + PreDefined);

        return str.ToString();
    }
}