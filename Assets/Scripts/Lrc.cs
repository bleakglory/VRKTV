using System.Collections;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;

using System.Text.RegularExpressions;

public class Lrc : MonoBehaviour
{
    public TMP_Text lrcText;
    public double offest = 0.0;
    public String songPath;
    public Lrc LRCs;

    void Start()
    {

        //songPath = "lemontree.lrc"; //定义歌的文件名
        //                            // songPath = "夜空中最亮的  songPath = "lemontree.lrc"; //定义歌的文件名星 - 邓紫棋.lrc";
        //songPath = "十年.lrc"; //定义歌的文件名

        //songPath = "年少有为.lrc"; //定义歌的文件名
        //songPath = "老男孩.lrc"; //定义歌的文件名
        //songPath = "起风了.lrc"; //定义歌的文件名
        //songPath = "后来.lrc"; //定义歌的文件名
        //songPath = "水星记.lrc"; //定义歌的文件名
#if UNITY_EDITOR
        Lrc lrc = Lrc.InitLrc(Application.streamingAssetsPath + "/" + songPath + ".lrc");
#elif UNITY_ANDROID
        //Lrc lrc = Lrc.InitLrc("jar:file://" + Application.persistentDataPath + "//IRC//" + songPath + ".lrc");
        Lrc lrc = Lrc.InitLrc(Application.streamingAssetsPath + "//" + songPath + ".lrc");
#endif

        StartCoroutine(ShowLrc(lrc.LrcWord)); //显示歌词

    }

    IEnumerator ShowLrc(Dictionary<double, string> dic)
    {
        //lrcText.text = dic.First.Value;
        //yield return new WaitForSeconds(dic.First.Key);
        double previousKey = 0.0;
        foreach (KeyValuePair<double, string> kvp in dic) //获取第一个key
        {
            previousKey = kvp.Key;
            break;
        }

        foreach (KeyValuePair<double, string> kvp in dic)
        {
            float ts = 0.0f;
            ts = (float)(kvp.Key - previousKey - offest);  //偏移量
            yield return new WaitForSeconds(ts);
            lrcText.text = kvp.Value;
            previousKey = kvp.Key;
        }

    }

    /// <summary>
    /// 歌曲
    /// </summary>
    public string Title { get; set; }
        /// <summary>
        /// 艺术家
        /// </summary>
        public string Artist { get; set; }
        /// <summary>
        /// 专辑
        /// </summary>
        public string Album { get; set; }
        /// <summary>
        /// 歌词作者
        /// </summary>
        public string LrcBy { get; set; }
        /// <summary>
        /// 偏移量
        /// </summary>
        public string Offset { get; set; }

        /// <summary>
        /// 歌词
        /// </summary>
        public Dictionary<double, string> LrcWord = new Dictionary<double, string>();


        /// <summary>
        /// 获得歌词信息
        /// </summary>
        /// <param name="LrcPath">歌词路径</param>
        /// <returns>返回歌词信息(Lrc实例)</returns>
        ////初始化歌词，用Dictionary<double, string> dicword来储存歌词，表示时间和歌词，把lrc里面的作者专辑等属性放入类属性中
        public static Lrc InitLrc(string LrcPath)
        {
            Lrc lrc = new Lrc();
            //用Dictionary<double, string> dicword来储存歌词，表示时间和歌词
            Dictionary<double, string> dicword = new Dictionary<double, string>();

            using (FileStream fs = new FileStream(LrcPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                string line;
                //读取歌词的细节
                //这里的编码方式取决于lrc文件的编码方式，最好都用gb2312编码
                using (StreamReader sr = new StreamReader(fs, System.Text.Encoding./*GetEncoding("gb2312")*/Default))//英文歌
           // using (StreamReader sr = new StreamReader(fs, System.Text.Encoding.GetEncoding("gb2312")/*Default*/))//中文歌
            {
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.StartsWith("[ti:"))
                        {
                            lrc.Title = SplitInfo(line);
                        }
                        else if (line.StartsWith("[ar:"))
                        {
                            lrc.Artist = SplitInfo(line);
                        }
                        else if (line.StartsWith("[al:"))
                        {
                            lrc.Album = SplitInfo(line);
                        }
                        else if (line.StartsWith("[by:"))
                        {
                            lrc.LrcBy = SplitInfo(line);
                        }
                        else if (line.StartsWith("[offset:"))
                        {
                            lrc.Offset = SplitInfo(line);
                        }
                        else
                        {
                            try
                            {
                                Regex regexword = new Regex(@".*\](.*)");
                                Match mcw = regexword.Match(line);
                                string word = mcw.Groups[1].Value;
                                Regex regextime = new Regex(@"\[([0-9.:]*)\]", RegexOptions.Compiled);
                                MatchCollection mct = regextime.Matches(line);
                                foreach (Match item in mct)
                                {
                                    double time = TimeSpan.Parse("00:" + item.Groups[1].Value).TotalSeconds;
                                    dicword.Add(time, word);
                                }
                            }
                            catch
                            {
                                continue;
                            }
                        }
                    }
                }
            }
            //把dicword放到LrcWord里面，LrcWord是类属性
            lrc.LrcWord = dicword.OrderBy(t => t.Key).ToDictionary(t => t.Key, p => p.Value);
            return lrc;
        }

        /// <summary>
        /// 处理信息(私有方法)
        /// </summary>
        /// <param name="line"></param>
        /// <returns>返回基础信息</returns>
        static string SplitInfo(string line)
        {
            return line.Substring(line.IndexOf(":") + 1).TrimEnd(']');
        }


    

}
