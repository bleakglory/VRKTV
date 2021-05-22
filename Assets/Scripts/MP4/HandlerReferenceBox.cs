using System.IO;
using System.Text;
/// <summary>
/// 解释媒体的播放过程信息，该box也可被包含在meta box中，type域为hdlr
/// </summary>
public class HandlerReferenceBox : FullBox
{
    /// <summary>
    /// 预定义，占四个字节
    /// </summary>
    public uint PreDefined;

    /// <summary>
    /// 在media box中，该值为4个字符，占四个字节；
    /// vide：video track
    /// soun：audio track
    /// hint：hint track
    /// </summary>
    public string HandlerType;

    /// <summary>
    /// 预留位，uint[3]，占12个字节
    /// </summary>
    public uint[] Reserved;

    /// <summary>
    /// human‐readable name for the track type
    /// </summary>
    public string Name;

    public override void ReadContent(BinaryReader br)
    {
        PreDefined = GetUint32(br);
        HandlerType = GetString(br, 4);
        Reserved = GetUint32Array(br, 3);
        Name = GetString(br, (int)Size - headerLength - 20);
    }

    public override string ToString()
    {
        StringBuilder str = new StringBuilder();
        str.Append(base.ToString());

        str.AppendLine("  PreDefined : " + PreDefined);
        str.AppendLine("  HandlerType : " + HandlerType);
        str.AppendLine("  Reserved : " + string.Join(",", Reserved));
        str.AppendLine("  Name : " + Name);

        return str.ToString();
    }
}