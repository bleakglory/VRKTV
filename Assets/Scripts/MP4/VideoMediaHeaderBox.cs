using System.IO;
using System.Text;
/// <summary>
/// 用在视频track中，包含当前track的视频描述信息（如视频编码等信息），type域为vmhd
/// </summary>
public class VideoMediaHeaderBox : FullBox
{
    /// <summary>
    /// 视频合成模式，为0时为拷贝原始图像，否则与opcolor进行合成，占两个字节
    /// </summary>
    public ushort GraphicsMode;

    /// <summary>
    /// {red, green, blue}，占6个字节
    /// </summary>
    public ushort[] OpColor;

    public override void ReadContent(BinaryReader br)
    {
        GraphicsMode = GetUint16(br);
        OpColor = GetUint16Array(br, 3);
    }

    public override string ToString()
    {
        StringBuilder str = new StringBuilder();
        str.Append(base.ToString());

        str.AppendLine("  GraphicsMode : " + GraphicsMode);
        str.AppendLine("  OpColor : " + string.Join(",", OpColor));

        return str.ToString();
    }
}