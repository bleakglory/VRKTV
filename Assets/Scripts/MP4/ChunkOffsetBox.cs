using System.Collections.Generic;
using System.IO;
using System.Text;
/// <summary>
/// type域为stco表示32位；type域为co64表示64位；
/// stco定义了每个chunk在媒体流中的位置，sample的偏移可以根据其他box推算出来。
/// 位置有两种可能，32位和64位的，在一个表中只会有一种可能，这个位置是在整个文件中的，
/// 而不是在任何box中的，这样做就可以直接在文件中找到媒体位置，而不用解释box。
/// 需要注意的是一旦前面的box有了任何改变，这张表都要重新建立，因为位置信息已经改变了。
/// </summary>
public class ChunkOffsetBox : FullBox
{
    /// <summary>
    /// 占四个字节
    /// </summary>
    public uint EntryCount;

    /// <summary>
    /// 若type为stco使用，占EntryCount*4个字节
    /// </summary>
    public List<uint> ChunkOffset32;

    /// <summary>
    /// 若type为co64使用，占EntryCount*8个字节
    /// </summary>
    public List<ulong> ChunkOffset64;

    public override void ReadContent(BinaryReader br)
    {
        EntryCount = GetUint32(br);
        if (Type.Equals("stco"))
        {
            ChunkOffset32 = new List<uint>();
            for (int i = 0; i < EntryCount; i++)
            {
                ChunkOffset32.Add(GetUint32(br));
            }
        }
        else
        {
            ChunkOffset64 = new List<ulong>();
            for (int i = 0; i < EntryCount; i++)
            {
                ChunkOffset64.Add(GetUint64(br));
            }
        }
    }

    public override string ToString()
    {
        StringBuilder str = new StringBuilder();
        str.Append(base.ToString());

        str.AppendLine("  EntrySize : " + EntryCount);
        if (Type.Equals("stco"))
        {
            str.AppendLine("  ChunkOffset : " + string.Join(",", ChunkOffset32));
        }
        else
        {
            str.AppendLine("  ChunkOffset : " + string.Join(",", ChunkOffset64));
        }

        return str.ToString();
    }
}