using System.Collections.Generic;
using System.IO;
using System.Text;
/// <summary>
/// 解释如何定位媒体信息，是一个container box。type域为dinf；
/// 一般包含一个dref box，即data reference box。
/// </summary>
public class DataInformationBox : Box
{
    /// <summary>
    /// 用来设置当前box描述信息的data_entry
    /// </summary>
    public DataReferenceBox DataReference = new DataReferenceBox();

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
            if (box.Type.Equals("dref"))
            {
                DataReference.Copy(box);
                DataReference.ReadFullHeader(br);
                DataReference.ReadContent(br);
            }
            else
            {
                box.ReadContent(br);
                Boxs.Add(box);
            }
            i += box.Size;
        }
    }

    public override string ToString()
    {
        StringBuilder str = new StringBuilder();
        str.Append(base.ToString());

        str.Append(DataReference.ToString());

        for (int i = 0; i < Boxs.Count; i++)
        {
            str.Append(Boxs[i].ToString());
        }

        return str.ToString();
    }
}