using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System;

public class UserController : MonoBehaviour
{
    // Start is called before the first frame update
    private Dictionary<string, Dictionary<string, string>> users = new Dictionary<string, Dictionary<string, string>>();
    private Dictionary<string, string> infos = new Dictionary<string, string>() {
        {"password",""}
    };
    private string path=null;
    //private string path = System.Environment.CurrentDirectory+@"\Users";
    void Start()
    {
        path = Application.persistentDataPath + "/Users";

        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        DirectoryInfo[] dirs = new DirectoryInfo(path).GetDirectories();
        foreach (DirectoryInfo d in dirs)
        {
            users.Add(d.Name, GetUserInfo(d.Name + "/info.txt"));
        }
    }
    private Dictionary<string,string> GetUserInfo(string userpath)
    {
        Dictionary<string, string> userinfo = new Dictionary<string, string>();
        string userInfoPath = path + "/" + userpath;
        try
        {
            using (StreamReader sr=new StreamReader(userInfoPath))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] strs = line.Split(',');
                    userinfo.Add(strs[0], strs[1]);
                }
            }
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
        }
        return userinfo;
    }
    private string MD5Encrypt(string str)
    {
        byte[] result = Encoding.Default.GetBytes(str);
        MD5 md5 = new MD5CryptoServiceProvider();
        byte[] md5pwd = md5.ComputeHash(result);
        return Convert.ToBase64String(md5pwd);
    }
    public int Login(string username,string password)
    {
        // User doesn't exist
        if (!UserExist(username)) return 1;
        // Check password
        string md5pwd = MD5Encrypt(password);
        // Wrong password
        if (!Comparepwd(username,md5pwd)) return 2;
        // Successful login
        return 0;
    }
    public int Register(string username,string password)
    {
        // User exist, ask for changing username
        if (UserExist(username)) return 1;
        string md5pwd = MD5Encrypt(password);
        Dictionary<string, string> userinfo = new Dictionary<string, string>();
        foreach(string info in infos.Keys)
        {
            userinfo.Add(info, infos[info]);
        }
        userinfo["password"] = md5pwd;
        users.Add(username, userinfo);
        SaveUser(username, userinfo);

        return 0;
    }
    private bool UserExist(string username)
    {
        if (users.ContainsKey(username)) return true;
        else return false;
    }
    private bool Comparepwd(string username,string pwd)
    {
        if (users[username]["password"].Equals(pwd)) return true;
        else return false;
    }
    private void SaveUser(string username, Dictionary<string, string> userinfo)
    {
        string userFolderPath = path + "/" + username;
        string userInfoPath = userFolderPath + "/info.txt";
        if (!Directory.Exists(userFolderPath)) Directory.CreateDirectory(userFolderPath);
        if (!Directory.Exists(userFolderPath + "/Videos")) Directory.CreateDirectory(userFolderPath + "/Videos");
        if (!Directory.Exists(userFolderPath + "/Audios")) Directory.CreateDirectory(userFolderPath + "/Audios");
        if (!Directory.Exists(userFolderPath + "/Cache")) Directory.CreateDirectory(userFolderPath + "/Cache");
        if (!File.Exists(userInfoPath)) File.Create(userInfoPath).Dispose();
        using (StreamWriter sw = new StreamWriter(userInfoPath))
        {
            foreach (string s in userinfo.Keys)
            {
                sw.WriteLine(s + ',' + userinfo[s]);
            }
        }
    }
}
