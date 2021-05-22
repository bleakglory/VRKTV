using System.IO;
using System.Text;
/// <summary>
/// data reference box下会包含若干个ur或urn，这些box组成一个表，用来定位track数据。type域为url；
/// flags值不是固定的，但是有一个特殊的值，0x000001用来表示当前media的数据和moov包含的数据一致；
/// </summary>
public class DataEntryUrlBox : FullBox
{
    /// <summary>
    /// 
    /// </summary>
    public string Location = string.Empty;

    public override void ReadContent(BinaryReader br)
    {
        int length = (int)Size - headerLength;
        if (length > 0)
        {
            Location = GetString(br, length);
        }
    }

    public override string ToString()
    {
        StringBuilder str = new StringBuilder();
        str.Append(base.ToString());

        str.AppendLine("  Location : " + Location);

        return str.ToString();
    }
}