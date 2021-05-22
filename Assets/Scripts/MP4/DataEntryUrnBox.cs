using System.IO;
using System.Text;
/// <summary>
/// data reference box下会包含若干个ur或urn，这些box组成一个表，用来定位track数据。type域为urn；
/// </summary>
public class DataEntryUrnBox : FullBox
{
    /// <summary>
    /// 必须
    /// </summary>
    public string Name = string.Empty;

    /// <summary>
    /// 可选
    /// </summary>
    public string Location = string.Empty;

    public override void ReadContent(BinaryReader br)
    {
        int length = (int)Size - headerLength;
        if (length > 0)
        {
            Name = GetString(br, length);
        }
    }

    public override string ToString()
    {
        StringBuilder str = new StringBuilder();
        str.Append(base.ToString());

        str.AppendLine(" Name : " + Name);
        str.AppendLine(" Location : " + Location);

        return str.ToString();
    }
}