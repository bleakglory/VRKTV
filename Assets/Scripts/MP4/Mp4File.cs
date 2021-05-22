using System.Collections.Generic;
using System.IO;
using System.Text;

public class Mp4File
{
    private string filePath;
    private string fileName;

    /// <summary>
    /// File Type
    /// </summary>
    public FileTypeBox Ftyp = new FileTypeBox();

    /// <summary>
    /// Media Data
    /// </summary>
    public List<MediaDataBox> Mdats = new List<MediaDataBox>();

    /// <summary>
    /// Movie
    /// </summary>
    public MovieBox Moov = new MovieBox();

    /// <summary>
    /// Free
    /// </summary>
    public FreeSpaceBox Free;

    /// <summary>
    /// Meta
    /// </summary>
    public MetaBox Meta;

    /// <summary>
    /// 其他
    /// </summary>
    public List<Box> Boxs = new List<Box>();

    public bool Open(string file)
    {
        this.filePath = file;
        this.fileName = Path.GetFileNameWithoutExtension(fileName);

        FileStream fs = new FileStream(this.filePath, FileMode.Open, FileAccess.Read);
        BinaryReader br = new BinaryReader(fs);

        br.BaseStream.Seek(0, SeekOrigin.Begin);
        while (br.PeekChar() > -1)
        {
            Box box = new Box();
            box.ReadHeader(br);
            switch (box.Type)
            {
                case "ftyp":
                    Ftyp.Copy(box);
                    Ftyp.ReadContent(br);
                    break;
                case "mdat":
                    MediaDataBox mdat = new MediaDataBox();
                    mdat.Copy(box);
                    mdat.ReadContent(br);
                    Mdats.Add(mdat);
                    break;
                case "free":
                case "skip":
                    Free = new FreeSpaceBox();
                    Free.Copy(box);
                    Free.ReadContent(br);
                    break;
                case "meta":
                    Meta = new MetaBox();
                    Meta.Copy(box);
                    Meta.ReadFullHeader(br);
                    Meta.ReadContent(br);
                    break;
                case "moov":
                    Moov.Copy(box);
                    Moov.ReadContent(br);
                    break;
                default:
                    box.ReadContent(br);
                    Boxs.Add(box);
                    break;
            }
        }

        br.Close();
        fs.Close();

        return true;
    }

    public override string ToString()
    {
        StringBuilder str = new StringBuilder();
        str.Append(Ftyp.ToString());
        for (int i = 0; i < Mdats.Count; i++)
        {
            str.Append(Mdats[i].ToString());
        }
        if (Free != null)
        {
            str.Append(Free.ToString());
        }
        if (Meta != null)
        {
            str.Append(Meta.ToString());
        }
        str.Append(Moov.ToString());

        // 其他
        for (int i = 0; i < Boxs.Count; i++)
        {
            str.Append(Boxs[i].ToString());
        }
        return str.ToString();
    }
}