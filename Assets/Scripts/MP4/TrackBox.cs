using System.Collections.Generic;
using System.IO;
using System.Text;
/// <summary>
/// 按时间排序的相关的采样，对于媒体数据来说，track表示一个视频或音频序列，type域为trak；
/// tack box是一个container box，其子box包含了该track的媒体数据引用和描述（hint track除外）。
/// 一个MP4文件中的媒体可以包含多个track，且至少有一个track，这些track之间彼此独立，有自己的时间和空间信息。
/// trak box必须包含一个tkhd box和一个mdia box
/// </summary>
public class TrackBox : Box
{
    /// <summary>
    /// 该track的特性和总体信息，如时长、宽高等
    /// </summary>
    public TrackHeaderBox TrackHeader = new TrackHeaderBox();

    /// <summary>
    /// 注释或描述信息
    /// </summary>
    public MetaBox Meta = null;

    /// <summary>
    /// 
    /// </summary>
    public MediaBox Media = new MediaBox();

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
                case "tkhd":
                    TrackHeader.Copy(box);
                    TrackHeader.ReadFullHeader(br);
                    TrackHeader.ReadContent(br);
                    break;
                case "meta":
                    Meta = new MetaBox();
                    Meta.Copy(box);
                    Meta.ReadFullHeader(br);
                    Meta.ReadContent(br);
                    break;
                case "mdia":
                    Media.Copy(box);
                    Media.ReadContent(br);
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

        str.Append(TrackHeader.ToString());
        if (Meta != null)
        {
            str.Append(Meta.ToString());
        }
        str.Append(Media.ToString());

        for (int i = 0; i < Boxs.Count; i++)
        {
            str.Append(Boxs[i].ToString());
        }

        return str.ToString();
    }
}