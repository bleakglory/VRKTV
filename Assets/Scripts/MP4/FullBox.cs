using System;
using System.IO;
using System.Text;
/// <summary>
/// box header相对box多了version和flags字段
/// </summary>
public class FullBox : Box
{
    /// <summary>
    /// 用来指定该box的文件的格式
    /// full box的特有字段
    /// </summary>
    public byte Version;

    /// <summary>
    /// 标志图
    /// full box的特有字段
    /// </summary>
    public byte[] Flags = new byte[3];

    public override void ReadHeader(BinaryReader br)
    {
        base.ReadHeader(br);
        ReadFullHeader(br);
    }

    /// <summary>
    /// 读取fullbox相对box额外的信息
    /// </summary>
    /// <param name="br"></param>
    public void ReadFullHeader(BinaryReader br)
    {
        Version = br.ReadByte();
        Flags = br.ReadBytes(3);
        headerLength += 4;
    }

    public override string ToString()
    {
        StringBuilder str = new StringBuilder();
        str.Append(base.ToString());
        str.AppendLine("  Version : " + Version);
        str.AppendLine("  Flags : " + BitConverter.ToString(Flags));
        return str.ToString();
    }

    public virtual void Copy(FullBox box)
    {
        base.Copy(box);
        Version = box.Version;
        Flags = box.Flags;
    }
}