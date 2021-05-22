using System.Collections.Generic;
using System.IO;
using System.Text;
/// <summary>
/// type域为stts；
/// stts box存储了sample的duration，描述了sample时序的映射方法，我们通过它可以找到任何时间的sample。
/// stts box可以包含一个压缩的表来映射时间和sample序号，用其他的表来提供每个sample的长度和指针。
/// 表中每个条目提供了在同一时间偏移量里面连续的sample序号，以及sample的偏移量。
/// 递增这些偏移量，就可以建立一个完整的time to sample表（时间戳到sample序号的映射表）。
/// </summary>
public class TimeToSampleBox : FullBox
{
    /// <summary>
    /// 个数，占四个字节
    /// </summary>
    public uint EntryCount;

    /// <summary>
    /// 占EntryCount*4个字节
    /// </summary>
    public List<uint> SampleCounts = new List<uint>();

    /// <summary>
    /// 占EntryCount*4个字节
    /// </summary>
    public List<uint> SampleDeltas = new List<uint>();

    public override void ReadContent(BinaryReader br)
    {
        EntryCount = GetUint32(br);
        for (int i = 0; i < EntryCount; i++)
        {
            uint sampleCount = GetUint32(br);
            SampleCounts.Add(sampleCount);

            uint sampleDelta = GetUint32(br);
            SampleDeltas.Add(sampleDelta);
        }
    }

    public override string ToString()
    {
        StringBuilder str = new StringBuilder();
        str.Append(base.ToString());

        str.AppendLine("  EntryCount : " + EntryCount);
        str.AppendLine("  SampleCount : " + string.Join(",", SampleCounts));
        str.AppendLine("  SampleDeltas : " + string.Join(",", SampleDeltas));

        //计算Sample总数
        //uint sampleCount = 0;
        //for (int i = 0; i < SampleCounts.Count; i++)
        //{
        //    sampleCount += SampleCounts[i];
        //}
        //str.AppendLine("  Sample总数 : " + sampleCount);

        //计算每个sample的时间戳
        //List<uint> timestamps = new List<uint>();
        //for (int i = 0; i < EntryCount; i++)
        //{
        //    uint j = 0;
        //    while (j < SampleCounts[i])
        //    {
        //        if (timestamps.Count < 1)
        //        {
        //            timestamps.Add(SampleDeltas[i]);
        //        }
        //        else
        //        {
        //            timestamps.Add(timestamps[timestamps.Count - 1] + SampleDeltas[i]);
        //        }
        //        j++;
        //    }
        //}
        //str.AppendLine("  Sample时间戳 : " + string.Join(",", timestamps));

        return str.ToString();
    }
}