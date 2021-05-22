using System;
using System.IO;
using System.Text;
/// <summary>
/// 用来容纳实体数据的box，type域为mdat；
/// 该box包含于文件层，可以有多个，也可以没有（当媒体数据全部为外部文件引用时），用来存储媒体数据。
/// 数据直接跟在box type字段后面，它的结构是由metadata来描述的，metadata通过文件中的绝对偏移来引用媒体数据。
/// </summary>
public class MediaDataBox : Box
{
    /// <summary>
    /// 实体数据
    /// </summary>
    public byte[] Datas;

    public override void ReadContent(BinaryReader br)
    {
        ulong contentLength = Size - (ulong)headerLength;
        Datas = new byte[contentLength];
        ulong i = 0;
        while (i < contentLength)
        {
            try
            {
                Datas[i] = br.ReadByte();
            }
            catch (Exception e)
            {
                break;
            }
            i++;
        }
    }

    public override string ToString()
    {
        StringBuilder str = new StringBuilder();
        str.Append(base.ToString());
        str.AppendLine("  ContentLength : " + Datas.LongLength);

        return str.ToString();
    }
}