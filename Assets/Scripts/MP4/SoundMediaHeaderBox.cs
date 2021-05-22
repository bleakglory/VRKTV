using System.IO;
using System.Text;
/// <summary>
/// 用在音频track中，包含当前track的音频描述信息（如编码格式等信息），type域为smhd
/// </summary>
public class SoundMediaHeaderBox : FullBox
{
    /// <summary>
    /// 立体声平衡，占2个字节，高8位和低8位分别为小数点整数部分和小数部分；
    /// 一般为0，-1.0表示全部左声道，1.0表示全部右声道；
    /// </summary>
    public float Balance;

    /// <summary>
    /// 保留位，占2个字节
    /// </summary>
    public ushort Reserved;

    public override void ReadContent(BinaryReader br)
    {
        Balance = br.ReadByte() + br.ReadByte() / 10.0f;
        Reserved = GetUint16(br);
    }

    public override string ToString()
    {
        StringBuilder str = new StringBuilder();
        str.Append(base.ToString());

        str.AppendLine("  Balance : " + Balance);
        str.AppendLine("  Reserved : " + Reserved);

        return str.ToString();
    }
}