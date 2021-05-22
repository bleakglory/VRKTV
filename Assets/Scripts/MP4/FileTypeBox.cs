using System.IO;
using System.Text;
/// <summary>
/// 文件类型，占24个字节，type域为ftyp
/// </summary>
public class FileTypeBox : Box
{
    /// <summary>
    /// 文件类型标识符，例如mp42，占四个字节（int）
    /// </summary>
    public string MajorBrand;

    /// <summary>
    /// major brand的次版本标识，占四个字节（int）
    /// </summary>
    public string MinorVersion;

    /// <summary>
    /// 兼容类型，占八个字节
    /// </summary>
    public string CompatibleBrands;

    public override void ReadContent(BinaryReader br)
    {
        MajorBrand = GetString(br, 4);
        MinorVersion = GetString(br, 4);
        CompatibleBrands = GetString(br, 8);
    }

    public override string ToString()
    {
        StringBuilder str = new StringBuilder();
        str.Append(base.ToString());

        str.AppendLine("  MajorBrand : " + MajorBrand);
        str.AppendLine("  MinorVersion : " + MinorVersion);
        str.AppendLine("  CompatibleBrands : " + CompatibleBrands);

        return str.ToString();
    }
}