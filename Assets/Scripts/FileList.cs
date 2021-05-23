using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileList : MonoBehaviour
{
    private string path = "";
    private string currentUser = null;
    private List<string> Files = new List<string>();
    // Start is called before the first frame update
    void Start()
    {
        path = Application.persistentDataPath + "/Users";
        //if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        Files.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentUser != GetComponent<UserController>().GetCurrentUser())
        {
            currentUser = GetComponent<UserController>().GetCurrentUser();
            if (currentUser != null)
            {
                RefreshFiles();
            }
        }
    }
    public List<string> GetFiles()
    {
        return Files;
    }
    private void RefreshFiles()
    {
        Files.Clear();
        FileInfo[] files = new DirectoryInfo(path + '/' + currentUser).GetFiles();
        foreach (FileInfo f in files)
        {
            Files.Add(f.Name.Split('.')[0]);
        }
    }
}
