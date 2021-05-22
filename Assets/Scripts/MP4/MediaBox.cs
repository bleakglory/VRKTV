using System.Collections.Generic;
using System.IO;
using System.Text;
/// <summary>
/// 包含整个track的媒体信息，比如媒体类型和sample信息，type域为mdia
/// </summary>
public class MediaBox : Box
{
    /// <summary>
    /// 包含该track的总体信息，针对media来设置，一般与track header一致
    /// </summary>
    public MediaHeaderBox MediaHeader = new MediaHeaderBox();

    /// <summary>
    /// 解释媒体的播放过程信息
    /// </summary>
    public HandlerReferenceBox Handler = new HandlerReferenceBox();

    /// <summary>
    /// 包含所有描述该track中的媒体信息的对象
    /// </summary>
    public MediaInformationBox MediaInformation = new MediaInformationBox();

    /// <summary>
    /// 其他
    /// </summary>
    public List<Box> Boxs = new List<Box>();

    public override void ReadContent(BinaryReader br)
    {
        ulong i = (ulong)headerLength;
        while (i < Size)
        {
            Box box = new Box();
            box.SetParentPath(GetPath());
            box.ReadHeader(br);
            switch (box.Type)
            {
                case "mdhd":
                    MediaHeader.Copy(box);
                    MediaHeader.ReadFullHeader(br);
                    MediaHeader.ReadContent(br);
                    break;
                case "hdlr":
                    Handler.Copy(box);
                    Handler.ReadFullHeader(br);
                    Handler.ReadContent(br);
                    break;
                case "minf":
                    MediaInformation.Copy(box);
                    MediaInformation.ReadContent(br);
                    break;
                default:
                    box.ReadContent(br);
                    Boxs.Add(box);
                    break;
            }
            i += box.Size;
        }
    }

    public override string ToString()
    {
        StringBuilder str = new StringBuilder();
        str.Append(base.ToString());

        str.Append(MediaHeader.ToString());
        str.Append(Handler.ToString());
        str.Append(MediaInformation.ToString());

        for (int i = 0; i < Boxs.Count; i++)
        {
            str.Append(Boxs[i].ToString());
        }

        return str.ToString();
    }
}