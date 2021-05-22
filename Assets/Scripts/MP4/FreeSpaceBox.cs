using System.Text;
/// <summary>
/// free space box中的内容是无关紧要的，可以被忽略。
/// 该box被删除后，不会对播放产生任何影响，它的type域可以是free或skip
/// </summary>
public class FreeSpaceBox : Box
{
    public override string ToString()
    {
        StringBuilder str = new StringBuilder();
        str.Append(base.ToString());
        str.AppendLine("  ContentLength : " + (Size - (ulong)headerLength));
        return str.ToString();
    }
}