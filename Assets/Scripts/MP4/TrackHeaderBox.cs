using System;
using System.IO;
using System.Text;
/// <summary>
/// 该track的特性和总体信息，如时长、宽高等，type域为tkhd；
/// </summary>
public class TrackHeaderBox : FullBox
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
    /// track的id号，不能重复且不能为0，占四个字节
    /// </summary>
    public uint TrackID;

    /// <summary>
    /// 保留位，占四个字节
    /// </summary>
    public uint Reserved1;

    /// <summary>
    /// 该track的时长；
    /// 若fullbox中的version为0占四个字节，若version为1占8个字节；
    /// </summary>
    public ulong Duration;

    /// <summary>
    /// 保留位，占8个字节
    /// </summary>
    public uint[] Reserved2;

    /// <summary>
    /// 指定视频层，默认为0，值小的在上层，占两个字节；
    /// </summary>
    public short Layer;

    /// <summary>
    /// 指定track分组信息，默认为0表示该track未与其他track有群组关系，占两个字节；
    /// </summary>
    public short AlternateGroup;

    /// <summary>
    /// 音量，占两个字节；
    /// 高8位和低8位分别为小数点整数部分和小数部分；
    /// 如果为音频track，1.0（0x0100）表示最大音量；否则为0；
    /// </summary>
    public float Volume;

    /// <summary>
    /// 保留位，占两个字节
    /// </summary>
    public ushort Reserved3;

    /// <summary>
    /// 视频变换矩阵，3*3，占36个字节
    /// </summary>
    public int[] Matrix;

    /// <summary>
    /// 宽，与sample描述中的实际画面大小比值，用于播放时的展示宽高，占四个字节；
    /// 高16位和低16位分别为小数点整数部分和小数部分；
    /// </summary>
    public float Width;

    /// <summary>
    /// 高，与sample描述中的实际画面大小比值，用于播放时的展示宽高，占四个字节；
    /// 高16位和低16位分别为小数点整数部分和小数部分；
    /// </summary>
    public float Height;

    public override void ReadContent(BinaryReader br)
    {
        if (Version == 1)
        {
            ulong seconds = GetUint64(br);
            CreateTime = new DateTime(1904, 1, 1).AddSeconds(seconds);
            seconds = GetUint64(br);
            ModificationTime = new DateTime(1904, 1, 1).AddSeconds(seconds);
            TrackID = GetUint32(br);
            Reserved1 = GetUint32(br);
            Duration = GetUint64(br);
        }
        else
        {
            uint seconds = GetUint32(br);
            CreateTime = new DateTime(1904, 1, 1).AddSeconds(seconds).ToLocalTime();
            seconds = GetUint32(br);
            ModificationTime = new DateTime(1904, 1, 1).AddSeconds(seconds).ToLocalTime();
            TrackID = GetUint32(br);
            Reserved1 = GetUint32(br);
            Duration = GetUint32(br);
        }
        Reserved2 = GetUint32Array(br, 2);
        Layer = GetInt16(br);
        AlternateGroup = GetInt16(br);
        Volume = br.ReadByte() + br.ReadByte() / 10.0f;
        Reserved3 = GetUint16(br);
        Matrix = GetInt32Array(br, 9);
        Width = GetUint16(br) + GetUint16(br) / 100.0f;
        Height = GetUint16(br) + GetUint16(br) / 100.0f;
    }

    public override string ToString()
    {
        StringBuilder str = new StringBuilder();
        str.Append(base.ToString());

        str.AppendLine("  CreateTime : " + CreateTime.ToString());
        str.AppendLine("  ModificationTime : " + ModificationTime.ToString());
        str.AppendLine("  TrackID : " + TrackID);
        str.AppendLine("  Reserved : " + Reserved1);
        str.AppendLine("  Duration : " + Duration);
        str.AppendLine("  Reserved : " + string.Join(",", Reserved2));
        str.AppendLine("  Layer : " + Layer);
        str.AppendLine("  AlternateGroup : " + AlternateGroup);
        str.AppendLine("  Volume : " + Volume);
        str.AppendLine("  Reserved : " + Reserved3);
        str.AppendLine("  Matrix : " + string.Join(",", Matrix));
        str.AppendLine("  Width : " + Width);
        str.AppendLine("  Height : " + Height);

        return str.ToString();
    }
}