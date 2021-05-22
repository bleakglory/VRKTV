using System.Collections.Generic;
using System.IO;
using System.Text;
/// <summary>
/// type域为stsz或stz2；
/// stsz定义了每个sample的大小，包含了媒体中全部sample的数目和一张给出每个sample大小的表。
/// 这个box相对来说体积是比较大的。
/// </summary>
public class SampleSizeBox : FullBox
{
    /// <summary>
    /// 占四个字节
    /// </summary>
    public uint SampleSize;

    /// <summary>
    /// 占四个字节
    /// </summary>
    public uint SampleCount;

    /// <summary>
    /// 若SampleSize=0，则占SampleCount*4个字节
    /// </summary>
    public List<uint> EntrySizes = new List<uint>();

    public override void ReadContent(BinaryReader br)
    {
        SampleSize = GetUint32(br);
        SampleCount = GetUint32(br);
        if (SampleSize == 0)
        {
            for (int i = 0; i < SampleCount; i++)
            {
                EntrySizes.Add(GetUint32(br));
            }
        }
    }

    public override string ToString()
    {
        StringBuilder str = new StringBuilder();
        str.Append(base.ToString());

        str.AppendLine("  SampleSize : " + SampleSize);
        str.AppendLine("  SampleCount : " + SampleCount);
        str.AppendLine("  EntrySize : " + string.Join(",", EntrySizes));

        //计算sample总大小
        //uint sampleSizeSum = 0;
        //for (int i = 0; i < SampleCount; i++)
        //{
        //    sampleSizeSum += EntrySizes[i];
        //}
        //str.AppendLine("  Sample总大小 : " + sampleSizeSum);

        return str.ToString();
    }
}