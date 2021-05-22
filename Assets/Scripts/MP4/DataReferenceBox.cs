using System.Collections.Generic;
using System.IO;
using System.Text;
/// <summary>
/// 用来设置当前Box描述信息的data_entry，type域为dref；
/// 包含若干个url或urn，这些box组成一个表，用来定位track数据。
/// 简单的说，track可以被分为若干段，每一段都可以根据url或urn指向的地址来获取数据，
/// sample描述中会用这些片段的序号将这些片段组成一个完成的track。
/// 一般情况下，当数据被完全包含在文件中时，url或urn中的定位字符串是空的。
/// </summary>
public class DataReferenceBox : FullBox
{
    /// <summary>
    /// entry个数
    /// </summary>
    public uint EntryCount;

    public List<DataEntryUrlBox> DataEntryUrls = new List<DataEntryUrlBox>();

    public List<DataEntryUrnBox> DataEntryUrns = new List<DataEntryUrnBox>();

    public override void ReadContent(BinaryReader br)
    {
        EntryCount = GetUint32(br);

        ulong i = (ulong)headerLength + 4;
        while (i < Size)
        {
            FullBox box = new FullBox();
            box.SetParentPath(GetPath());
            box.ReadHeader(br);
            if (box.Type.Equals("url"))
            {
                DataEntryUrlBox dataUrl = new DataEntryUrlBox();
                dataUrl.Copy(box);
                dataUrl.ReadContent(br);
                DataEntryUrls.Add(dataUrl);
            }
            else if (box.Type.Equals("urn"))
            {
                DataEntryUrnBox dataUrn = new DataEntryUrnBox();
                dataUrn.Copy(box);
                dataUrn.ReadContent(br);
                DataEntryUrns.Add(dataUrn);
            }
            else
            {
                box.ReadContent(br);
            }
            i += box.Size;
        }
    }

    public override string ToString()
    {
        StringBuilder str = new StringBuilder();
        str.Append(base.ToString());

        str.AppendLine("  EntryCount : " + EntryCount);

        for (int i = 0; i < DataEntryUrls.Count; i++)
        {
            str.Append(DataEntryUrls[i].ToString());
        }

        for (int i = 0; i < DataEntryUrns.Count; i++)
        {
            str.Append(DataEntryUrns[i].ToString());
        }

        return str.ToString();
    }
}