using System.Collections.Generic;
using System.IO;
using System.Text;
/// <summary>
/// type域为ctts
/// </summary>
public class CompositionOffsetBox : FullBox
{
    /// <summary>
    /// 占四个字节
    /// </summary>
    public uint EntryCount;

    /// <summary>
    /// 占EntryCount*4个字节
    /// </summary>
    public List<uint> SampleCounts = new List<uint>();

    /// <summary>
    /// 占EntryCount*4个字节
    /// version = 0时，sample offset类型为uint32；
    /// version = 1时，sample offset类型为int32；
    /// </summary>
    public List<uint> SampleOffsetUint32 = new List<uint>();

    /// <summary>
    /// 占EntryCount*4个字节
    /// version = 0时，sample offset类型为uint32；
    /// version = 1时，sample offset类型为int32；
    /// </summary>
    public List<int> SampleOffsetInt32 = new List<int>();

    public override void ReadContent(BinaryReader br)
    {
        EntryCount = GetUint32(br);
        for (int i = 0; i < EntryCount; i++)
        {
            uint sampleCount = GetUint32(br);
            SampleCounts.Add(sampleCount);

            if (Version == 0)
            {
                uint offsetUint = GetUint32(br);
                SampleOffsetUint32.Add(offsetUint);
            }
            else
            {
                int offsetInt = GetInt32(br);
                SampleOffsetInt32.Add(offsetInt);
            }
        }
    }

    public override string ToString()
    {
        StringBuilder str = new StringBuilder();
        str.Append(base.ToString());

        str.AppendLine("  EntryCount : " + EntryCount);
        str.AppendLine("  SampleCount : " + string.Join(",", SampleCounts));
        str.AppendLine("  SampleOffset : " + (Version == 0 ? string.Join(",", SampleOffsetUint32) : string.Join(",", SampleOffsetInt32)));

        return str.ToString();
    }
}