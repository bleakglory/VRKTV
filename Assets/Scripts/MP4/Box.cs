using System;
using System.Collections;
using System.IO;
using System.Text;
/// <summary>
/// 由唯一类型标识符和长度定义的面向对象的构建；
/// box中的字节序为网络字节序，也就是大端字节序（Big-Endian）;
/// </summary>
public class Box
{
    /// <summary>
    /// box前四个字节，uint32类型，标识整个box所占用的大小，包括header部分；
    /// 如果box很大（例如存放具体视频数据的mdat box），超过了uint32的最大数值，size就被设置为1，并用type后面的8位uint64的largesize来存放大小；
    /// </summary>
    public ulong Size;

    /// <summary>
    /// box类型，uint类型，占四个字节
    /// </summary>
    public string Type;

    /// <summary>
    /// 若Type为uuid，则header中包含该字段，类型为unsigned int(8)[16]，占16个字节
    /// </summary>
    public string UserType;

    /// <summary>
    /// box header长度
    /// </summary>
    protected int headerLength;

    /// <summary>
    /// 设置父级路径
    /// </summary>
    protected string parentPath = string.Empty;

    /// <summary>
    /// 设置父级路径
    /// </summary>
    /// <returns></returns>
    public string SetParentPath(string value)
    {
        return parentPath = value;
    }

    /// <summary>
    /// 获取路径
    /// </summary>
    /// <returns></returns>
    public string GetPath()
    {
        return Path.Combine(parentPath, this.GetType().Name);
    }

    /// <summary>
    /// 读取box header，获取大小和类型
    /// </summary>
    /// <param name="br"></param>
    /// <returns>box header的长度</returns>
    public virtual void ReadHeader(BinaryReader br)
    {
        Size = GetUint32(br);
        Type = GetString(br, 4).Trim();
        if (Size == 1)
        {
            Size = GetUint64(br);
            headerLength = 16;
        }
        else
        {
            headerLength = 8;
        }
        if (Type.Equals("uuid"))
        {
            UserType = GetString(br, 16).Trim();
            headerLength += 16;
        }
    }

    /// <summary>
    /// 根据长度跳过box内容
    /// </summary>
    /// <param name="br"></param>
    public virtual void ReadContent(BinaryReader br)
    {
        ulong contentLength = Size - (ulong)headerLength;
        ulong i = 0;
        while (i < contentLength)
        {
            try
            {
                br.ReadByte();
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
        str.AppendLine(GetPath());
        if (Size > uint.MaxValue)
        {
            str.AppendLine("  Size : " + 1);
            str.AppendLine("  Type : " + Type);
            str.AppendLine("  Largesize : " + Size);
        }
        else
        {
            str.AppendLine("  Size : " + Size);
            str.AppendLine("  Type : " + Type);
        }
        //str.AppendLine("  ContentLength : " + (Size - (ulong)headerLength));

        return str.ToString();
    }

    public virtual void Copy(Box box)
    {
        Size = box.Size;
        Type = box.Type;
        UserType = box.UserType;
        headerLength = box.headerLength;
        parentPath = box.parentPath;
    }

    public static short GetInt16(BinaryReader br)
    {
        byte[] bytes = br.ReadBytes(2);
        Array.Reverse(bytes);
        return BitConverter.ToInt16(bytes, 0);
    }

    public static ushort GetUint16(BinaryReader br)
    {
        byte[] bytes = br.ReadBytes(2);
        Array.Reverse(bytes);
        return BitConverter.ToUInt16(bytes, 0);
    }

    public static int GetInt32(BinaryReader br)
    {
        byte[] bytes = br.ReadBytes(4);
        Array.Reverse(bytes);
        return BitConverter.ToInt32(bytes, 0);
    }

    public static uint GetUint32(BinaryReader br)
    {
        byte[] bytes = br.ReadBytes(4);
        Array.Reverse(bytes);
        return BitConverter.ToUInt32(bytes, 0);
    }

    public static ulong GetUint64(BinaryReader br)
    {
        byte[] bytes = br.ReadBytes(8);
        Array.Reverse(bytes);
        return BitConverter.ToUInt64(bytes, 0);
    }

    public static int[] GetInt32Array(BinaryReader br, int length)
    {
        int[] array = new int[length];
        for (int i = 0; i < length; i++)
        {
            array[i] = GetInt32(br);
        }
        return array;
    }

    public static uint[] GetUint32Array(BinaryReader br, int length)
    {
        uint[] array = new uint[length];
        for (int i = 0; i < length; i++)
        {
            array[i] = GetUint32(br);
        }
        return array;
    }

    public static ushort[] GetUint16Array(BinaryReader br, int length)
    {
        ushort[] array = new ushort[length];
        for (int i = 0; i < length; i++)
        {
            array[i] = GetUint16(br);
        }
        return array;
    }

    public static string GetString(BinaryReader br, int length)
    {
        byte[] bytes = br.ReadBytes(length);
        return Encoding.ASCII.GetString(bytes).Trim('\0');
    }

    public static byte GetByte(BitArray bitArray, int startIndex, int length)
    {
        byte data = 0;
        for (int i = 0; i < length; i++)
        {
            if (bitArray[startIndex + i])
            {
                data |= (byte)(1 << 8 - (startIndex + i));
            }
        }
        return data;
    }
}