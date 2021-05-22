using System;
using System.IO;
using System.Text;
/// <summary>
/// 用来存放文件的总体信息，如时长和创建时间等，它是独立于媒体的并且与整个播放相关，type域为mvhd；
/// </summary>
public class MovieHeaderBox : FullBox
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
    /// 视频时长（秒）=Duration/TimeScale，不占字节，是计算出来的
    /// </summary>
    public float TimeLength;

    /// <summary>
    /// 推荐播放速率，占四个字节；
    /// 高16位和低16位分别为小数点整数部分和小数部分；
    /// 该值为1.0（0x00010000）表示正常前向播放
    /// </summary>
    public float Rate;

    /// <summary>
    /// 推荐音量，占两个字节；
    /// 高8位和低8位分别为小数点整数部分和小数部分；
    /// 1.0（0x0100）表示最大音量
    /// </summary>
    public float Volume;

    /// <summary>
    /// 保留字节，占10个字节：bit(16)+int(32)[2]
    /// </summary>
    public byte[] Reserved;

    /// <summary>
    /// 视频变换矩阵，3*3，占36个字节
    /// </summary>
    public int[] Matrix;

    /// <summary>
    /// 预定义，占24个字节：bit(32)[6]
    /// </summary>
    public byte[] PreDefined;

    /// <summary>
    /// 下一个track使用的id号，占4个字节
    /// </summary>
    public uint NextTrackId;

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
        TimeLength = (float)Duration / TimeScale;

        Rate = GetUint16(br) + GetUint16(br) / 100.0f;
        Volume = br.ReadByte() + br.ReadByte() / 10.0f;
        Reserved = br.ReadBytes(10);
        Matrix = GetInt32Array(br, 9);
        PreDefined = br.ReadBytes(24);
        NextTrackId = GetUint32(br);
    }

    public override string ToString()
    {
        StringBuilder str = new StringBuilder();
        str.Append(base.ToString());

        str.AppendLine("  CreateTime : " + CreateTime.ToString());
        str.AppendLine("  ModificationTime : " + ModificationTime.ToString());
        str.AppendLine("  TimeScale : " + TimeScale);
        str.AppendLine("  Duration : " + Duration);
        str.AppendLine("  TimeLength : " + TimeLength + " s");
        str.AppendLine("  Rate : " + Rate);
        str.AppendLine("  Volume : " + Volume);
        str.AppendLine("  Reserved : " + BitConverter.ToString(Reserved));
        str.AppendLine("  Matrix : " + string.Join(",", Matrix));
        str.AppendLine("  PreDefined : " + BitConverter.ToString(PreDefined));
        str.AppendLine("  NextTrackId : " + NextTrackId);

        return str.ToString();
    }
}