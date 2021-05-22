using System.Collections.Generic;
using System.IO;
using System.Text;
/// <summary>
/// type域为stss;
/// stss确定media中的关键帧。对于压缩媒体数据，关键帧是一系列压缩序列的开始帧，其解压缩时不依赖以前的帧，
/// 而后续帧解压缩将依赖于这个关键帧。stss可以非常紧凑的标记媒体内的随机存取点，它包含一个sample序号表，
/// 表内的每一项严格按照sample的序号排列，说明了媒体中的那个sample是关键帧。如果此表不存在，
/// 说明每一个sample都是关键帧，是一个随机存取点。
/// </summary>
public class SyncSampleBox : FullBox
{
    /// <summary>
    /// 占4个字节
    /// </summary>
    public uint EntryCount;

    /// <summary>
    /// 占EntryCount*4个字节
    /// </summary>
    public List<uint> SampleNumbers = new List<uint>();

    public override void ReadContent(BinaryReader br)
    {
        EntryCount = GetUint32(br);
        for (int i = 0; i < EntryCount; i++)
        {
            SampleNumbers.Add(GetUint32(br));
        }
    }

    public override string ToString()
    {
        StringBuilder str = new StringBuilder();
        str.Append(base.ToString());

        str.AppendLine("  EntrySize : " + EntryCount);
        str.AppendLine("  SampleNumbers : " + string.Join(",", SampleNumbers));

        return str.ToString();
    }
}