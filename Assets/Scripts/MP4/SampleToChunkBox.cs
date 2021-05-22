using System.Collections.Generic;
using System.IO;
using System.Text;
/// <summary>
/// type域为stsc；
/// 用chunk组织sample可以方便优化数据获取，一个chunk包含一个或多个sample。
/// stsc中用一个表描述了sample与chunk的映射关系，查看这张表就可以找到包含指定sample的chunk，从而找到这个sample。
/// </summary>
public class SampleToChunkBox : FullBox
{
    /// <summary>
    /// 占4个字节
    /// </summary>
    public uint EntryCount;

    /// <summary>
    /// 占EntryCount*4个字节
    /// </summary>
    public List<uint> FirstChunks = new List<uint>();

    /// <summary>
    /// 占EntryCount*4个字节
    /// </summary>
    public List<uint> SamplesPerChunks = new List<uint>();

    /// <summary>
    /// 占EntryCount*4个字节
    /// </summary>
    public List<uint> SmapleDescriptionIndexs = new List<uint>();

    public override void ReadContent(BinaryReader br)
    {
        EntryCount = GetUint32(br);
        for (int i = 0; i < EntryCount; i++)
        {
            FirstChunks.Add(GetUint32(br));
            SamplesPerChunks.Add(GetUint32(br));
            SmapleDescriptionIndexs.Add(GetUint32(br));
        }
    }

    public override string ToString()
    {
        StringBuilder str = new StringBuilder();
        str.Append(base.ToString());

        str.AppendLine("  EntrySize : " + EntryCount);
        str.AppendLine("  FirstChunks : " + string.Join(",", FirstChunks));
        str.AppendLine("  SamplesPerChunks : " + string.Join(",", SamplesPerChunks));
        str.AppendLine("  SmapleDescriptionIndexs : " + string.Join(",", SmapleDescriptionIndexs));

        return str.ToString();
    }
}