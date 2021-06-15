using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System.IO;

public class UserRecord : MonoBehaviour
{
    public AudioSource Audio;
    public GameObject[] MVList;
    public Image[] RecordButtonImage;
    private string[] _songName = new string[5];
    private string path;
    void Start()
    {
        path = Application.persistentDataPath + "/Users" + "/" + PlayerPrefs.GetString("currentUser") + "/Audios";
        
        _songName[0] = "老男孩";
        _songName[1] = "年少有为";
        _songName[2] = "起风了";
        _songName[3] = "十年";
        _songName[4] = "水星记";
        foreach (GameObject obj in MVList)
        {
            obj.SetActive(false);
        }

        for (int i = 0; i < _songName.Length; i++)
        {
            if(!Directory.Exists(path + "/" + _songName[i] + ".wav"))
            {
                RecordButtonImage[i].color = Color.red;
            }
            else
            {
                Debug.Log("Have!!!!");
                RecordButtonImage[i].color = Color.white;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region

    public void ChooseRecordOne()
    {
        if (RecordButtonImage[0].color == Color.white)
        {
            playRecord(path + "/" + _songName[0] + ".wav");
            MVList[0].SetActive(true);
            MVList[0].GetComponent<VideoPlayer>().Play();
        }
    }

    public void ChooseRecordTwo()
    {
        if (RecordButtonImage[1].color == Color.white)
        {
            playRecord(path + "/" + _songName[1] + ".wav");
            MVList[1].SetActive(true);
            MVList[1].GetComponent<VideoPlayer>().Play();
        }
    }

    public void ChooseRecordThree()
    {
        if (RecordButtonImage[2].color == Color.white)
        {
            playRecord(path + "/" + _songName[2] + ".wav");
            MVList[2].SetActive(true);
            MVList[2].GetComponent<VideoPlayer>().Play();
        }
    }

    public void ChooseRecordFour()
    {
        if (RecordButtonImage[3].color == Color.white)
        {
            playRecord(path + "/" + _songName[3] + ".wav");
            MVList[3].SetActive(true);
            MVList[3].GetComponent<VideoPlayer>().Play();
        }
    }

    public void ChooseRecordFive()
    {
        if (RecordButtonImage[4].color == Color.white)
        {
            playRecord(path + "/" + _songName[4] + ".wav");
            MVList[4].SetActive(true);
            MVList[4].GetComponent<VideoPlayer>().Play();
        }
    }

    #endregion
    void playRecord(string filePath)
    {
        StartCoroutine(LoadAudio(filePath));
    }
    public IEnumerator LoadAudio(string recordPath)
    {
        // www 加载音频
        WWW www = new WWW(recordPath);
        yield return www;
        var clipTemp = www.GetAudioClip();
        Audio.clip = clipTemp;

        //yield return new WaitForSeconds(5);
        //aud.loop = true;

        //播放音频
        Audio.Play();
    }

}
