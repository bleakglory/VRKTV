using System.Collections.Generic;
using System.IO;
using System.Text;
/// <summary>
/// 指明sample时序和物理布局的表，type域为stbl；
/// sample是媒体数据存储的单位，存储在media的chunk中，chunk和sample的长度均可互不相同；
/// stbl包含了关于track中sample所有时间和位置的信息，以及sample的编解码等信息。
/// 利用这个表，可以解释sample的时序、类型、大小以及在各自存储容器中的位置。
/// stbl是一个container box，其子box包括：sample description box（stsd）、time to sample box（stts）、
/// sample size box（stsz或stz2）、sample to chunk box（stsc）、chunk offset box（stco或co64）、
/// composition time to sample box（ctts）、sync sample box（stss）等；
/// </summary>
public class SampleTableBox : Box
{
    /// <summary>
    /// stsd必不可少，该box包含了data reference box进行sample数据检索的信息。
    /// </summary>
    public SampleDescriptionBox SampleDescription = new SampleDescriptionBox();

    /// <summary>
    /// 时间戳到sample序号的映射表
    /// </summary>
    public TimeToSampleBox TimeToSample = new TimeToSampleBox();

    /// <summary>
    /// 可选
    /// </summary>
    public CompositionOffsetBox CompositionOffset;

    /// <summary>
    /// 定义了每个sample的大小
    /// </summary>
    public SampleSizeBox SampleSize = new SampleSizeBox();

    /// <summary>
    /// 定义sample与chunk的映射关系
    /// </summary>
    public SampleToChunkBox SampleToChunk = new SampleToChunkBox();

    /// <summary>
    /// 描述哪一个sample是关键帧
    /// </summary>
    public SyncSampleBox SyncSample;

    /// <summary>
    /// 描述chunk在媒体流中的位置
    /// </summary>
    public ChunkOffsetBox ChunkOffset = new ChunkOffsetBox();

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
                case "stsd":
                    SampleDescription.Copy(box);
                    SampleDescription.ReadFullHeader(br);
                    SampleDescription.ReadContent(br);
                    break;
                case "stts":
                    TimeToSample.Copy(box);
                    TimeToSample.ReadFullHeader(br);
                    TimeToSample.ReadContent(br);
                    break;
                case "stss":
                    SyncSample = new SyncSampleBox();
                    SyncSample.Copy(box);
                    SyncSample.ReadFullHeader(br);
                    SyncSample.ReadContent(br);
                    break;
                case "stsz":
                    SampleSize.Copy(box);
                    SampleSize.ReadFullHeader(br);
                    SampleSize.ReadContent(br);
                    break;
                case "stsc":
                    SampleToChunk.Copy(box);
                    SampleToChunk.ReadFullHeader(br);
                    SampleToChunk.ReadContent(br);
                    break;
                case "stco":
                case "co64":
                    ChunkOffset.Copy(box);
                    ChunkOffset.ReadFullHeader(br);
                    ChunkOffset.ReadContent(br);
                    break;
                case "ctts":
                    CompositionOffset = new CompositionOffsetBox();
                    CompositionOffset.Copy(box);
                    CompositionOffset.ReadFullHeader(br);
                    CompositionOffset.ReadContent(br);
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

        str.Append(SampleDescription.ToString());
        str.Append(TimeToSample.ToString());
        if (CompositionOffset != null)
        {
            str.Append(CompositionOffset.ToString());
        }
        str.Append(SampleSize.ToString());
        str.Append(SampleToChunk.ToString());
        if (SyncSample != null)
        {
            str.Append(SyncSample.ToString());
        }
        str.Append(ChunkOffset.ToString());

        for (int i = 0; i < Boxs.Count; i++)
        {
            str.Append(Boxs[i].ToString());
        }

        return str.ToString();
    }
}