using System.Collections.Generic;
using System.IO;
using System.Text;
/// <summary>
/// 定义和描述轨中的采样的格式的结构，type域为stsd；
/// stsd必不可少，且至少包含一个条目，该box包含了data reference box进行sample数据检索的信息。
/// 没有stsd就无法计算media sample的存储位置。
/// sdsd包含了编码的信息，其存储的信息随媒体类型不同而不同。
/// </summary>
public class SampleDescriptionBox : FullBox
{
    /// <summary>
    /// entry数目，占四个字节
    /// </summary>
    public uint EntryCount;

    /// <summary>
    /// stsd在box header和version字段后会有一个entry count字段，
    /// 根据entry的个数，每个entry会有type信息，如“vide”、“sund”等，
    /// 根据type不同sample description会提供不同的信息，例如对于video track，
    /// 会有video sample entry类型信息，对于audio track会有audio sample entry类型信息。
    /// 视频的编码类型、宽高、长度、音频的声道、采样等信息都会出现在这个box中。
    /// </summary>
    public List<SampleEntry> SampleEntrys = new List<SampleEntry>();

    public override void ReadContent(BinaryReader br)
    {
        EntryCount = GetUint32(br);
        ulong i = (ulong)headerLength + 4;
        while (i < Size)
        {
            SampleEntry box = new SampleEntry();
            box.SetParentPath(GetPath());
            box.ReadHeader(br);
            box.ReadContent(br);
            SampleEntrys.Add(box);
            i += box.Size;
        }
    }

    public override string ToString()
    {
        StringBuilder str = new StringBuilder();
        str.Append(base.ToString());

        str.AppendLine("  EntryCount : " + EntryCount);
        for (int i = 0; i < SampleEntrys.Count; i++)
        {
            str.Append(SampleEntrys[i].ToString());
        }

        return str.ToString();
    }
}

/// <summary>
/// 分为video sample entry和audio sample entry。
/// video track中为video sample entry，type域为vide；
/// audio track中为audio sample entry，type域为sund；
/// </summary>
public class SampleEntry : Box
{
    /// <summary>
    /// 保留位，占6个字节
    /// </summary>
    public byte[] Reserved;

    /// <summary>
    /// 占2个字节
    /// </summary>
    public ushort DataReferenceIndex;

    public override void ReadContent(BinaryReader br)
    {
        Reserved = br.ReadBytes(6);
        DataReferenceIndex = GetUint16(br);

        ulong i = (ulong)headerLength + 8;
        while (i < Size)
        {
            br.ReadByte();
            i++;
        }
    }

    public override string ToString()
    {
        StringBuilder str = new StringBuilder();
        str.Append(base.ToString());

        str.AppendLine("  Reserved : " + string.Join(",", Reserved));
        str.AppendLine("  DataReferenceIndex : " + DataReferenceIndex);

        return str.ToString();
    }
}