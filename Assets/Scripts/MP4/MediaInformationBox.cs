using System.Collections.Generic;
using System.IO;
using System.Text;
/// <summary>
/// 包含了所有描述该track中的媒体信息的对象，信息存储在其子box中，type域为minf；
/// </summary>
public class MediaInformationBox : Box
{
    /// <summary>
    /// 用在视频track中，包含当前track的视频描述信息（如视频编码等信息）
    /// </summary>
    public VideoMediaHeaderBox VideoMediaHeader;

    /// <summary>
    /// 用在音频track中，包含当前track的音频描述信息（如编码格式等信息）
    /// </summary>
    public SoundMediaHeaderBox SoundMediaHeader;

    /// <summary>
    /// 
    /// </summary>
    public DataInformationBox DataInformation = new DataInformationBox();

    /// <summary>
    /// 
    /// </summary>
    public SampleTableBox SampleTable = new SampleTableBox();

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
                case "vmhd":
                    VideoMediaHeader = new VideoMediaHeaderBox();
                    VideoMediaHeader.Copy(box);
                    VideoMediaHeader.ReadFullHeader(br);
                    VideoMediaHeader.ReadContent(br);
                    break;
                case "smhd":
                    SoundMediaHeader = new SoundMediaHeaderBox();
                    SoundMediaHeader.Copy(box);
                    SoundMediaHeader.ReadFullHeader(br);
                    SoundMediaHeader.ReadContent(br);
                    break;
                case "dinf":
                    DataInformation.Copy(box);
                    DataInformation.ReadContent(br);
                    break;
                case "stbl":
                    SampleTable.Copy(box);
                    SampleTable.ReadContent(br);
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

        if (VideoMediaHeader != null)
        {
            str.Append(VideoMediaHeader.ToString());
        }
        if (SoundMediaHeader != null)
        {
            str.Append(SoundMediaHeader.ToString());
        }
        str.Append(DataInformation.ToString());
        str.Append(SampleTable.ToString());

        for (int i = 0; i < Boxs.Count; i++)
        {
            str.Append(Boxs[i].ToString());
        }

        return str.ToString();
    }
}