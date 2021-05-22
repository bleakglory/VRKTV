using System.Collections.Generic;
using System.IO;
using System.Text;
/// <summary>
/// 该box用来存放媒体的metadata信息，其内容信息由子box诠释，type域为moov；
/// 该box有且只有一个并且包含在文件层，一般情况下moov box会紧随ftyp box出现，但也有放在文件末尾的。
/// </summary>
public class MovieBox : Box
{
    /// <summary>
    /// 用来存放文件的总体信息，如时长和创建时间等，它是独立于媒体的并且与整个播放相关
    /// </summary>
    public MovieHeaderBox MovieHeader = new MovieHeaderBox();

    /// <summary>
    /// 注释或描述信息
    /// </summary>
    public MetaBox Meta = null;

    /// <summary>
    /// track box序列
    /// </summary>
    public List<TrackBox> Tracks = new List<TrackBox>();

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
                case "mvhd":
                    MovieHeader.Copy(box);
                    MovieHeader.ReadFullHeader(br);
                    MovieHeader.ReadContent(br);
                    break;
                case "meta":
                    Meta = new MetaBox();
                    Meta.Copy(box);
                    Meta.ReadFullHeader(br);
                    Meta.ReadContent(br);
                    break;
                case "trak":
                    TrackBox track = new TrackBox();
                    track.Copy(box);
                    track.ReadContent(br);
                    Tracks.Add(track);
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

        str.Append(MovieHeader.ToString());
        if (Meta != null)
        {
            str.Append(Meta.ToString());
        }

        for (int i = 0; i < Tracks.Count; i++)
        {
            str.Append(Tracks[i].ToString());
        }

        for (int i = 0; i < Boxs.Count; i++)
        {
            str.Append(Boxs[i].ToString());
        }

        return str.ToString();
    }
}