using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region param
    public Canvas menuCanv;
    public Canvas recordCanv;
    public AudioSource[] musicList;
    public GameObject[] backgrounds;
    public AudioSource[] StrongthenSound;
    private int Frequency = 16000; //录音频率
    private int BitRate = 16; //比特率
    private int MicSecond = 3;  //每隔3秒，保存一下录音数据
    public bool isStart;

    private bool isPlay;

    private string path = "";
    [SerializeField]
    private AudioSource au;
    private string[] _songName = new string[5];
    #endregion

    void Start()
    {
        path = Application.persistentDataPath + "/Users";
        au = GetComponent<AudioSource>();
        backgrounds[PlayerPrefs.GetInt("Background")].SetActive(true);
        menuCanv.gameObject.SetActive(true);
        recordCanv.gameObject.SetActive(false);
        isStart = false;
        isPlay = false;

        for (int i = 0; i < StrongthenSound.Length - 1; i++)
        {
            StrongthenSound[i] = au;
        }

        _songName[0] = "老男孩";
        _songName[1] = "年少有为";
        _songName[2] = "起风了";
        _songName[3] = "十年";
        _songName[4] = "水星记";
    }

    private void Update()
    {
        if(isStart == true)
        {
            musicPlay();
        }

        if(isPlay == true)
        {
            if(!musicList[PlayerPrefs.GetInt("Music") - 1].isPlaying)
            {
                OnStopClick();
                GetComponent<MusicVisualization>().thisAudioSource = null;
                recordCanv.gameObject.SetActive(true);
                isPlay = false;
            }
        }

    }
    void musicPlay()
    {
        musicList[PlayerPrefs.GetInt("Music") - 1].Play();
        GetComponent<MusicVisualization>().thisAudioSource = musicList[PlayerPrefs.GetInt("Music") - 1];
        GetComponent<Lrc>().enabled = true;
        GetComponent<Lrc>().songPath = _songName[PlayerPrefs.GetInt("Music") - 1];
        isStart = false;
        isPlay = true;
        OnStartClick();
    }
    #region buttons
    public void Quit()
    {
        SceneManager.LoadScene(1);
    }
    public void save()
    {
        WavFromClip(path + "/test.wav", au.clip); //将录音保存为wav
    }

    public void playRecord()
    {
        musicList[PlayerPrefs.GetInt("Music") - 1].Play();
        OnPlayClick();
    }

    public void backToMenu()
    {
        musicList[PlayerPrefs.GetInt("Music") - 1].Stop();
        recordCanv.gameObject.SetActive(false);
        menuCanv.gameObject.SetActive(true);
    }


    public void one()
    {
        startMusic(1);
    }

    public void two()
    {
        startMusic(2);
    }

    public void three()
    {
        startMusic(3);
    }

    public void four()
    {
        startMusic(4);
    }

    public void five()
    {
        startMusic(5);
    }

    public void six()
    {
        startMusic(6);
    }

    public void seven()
    {
        startMusic(7);
    }

    public void eight()
    {
        startMusic(8);
    }

    public void nine()
    {
        startMusic(9);
    }

    public void ten()
    {
        startMusic(10);
    }
    #endregion

    void startMusic(int num)
    {
        PlayerPrefs.SetInt("Music", num);
        menuCanv.gameObject.SetActive(false);
        isStart = true;
    }

    #region microphone
    //开始录音
    void OnStartClick()
    {
        au.Stop();
        au.loop = false;
        au.mute = true;
        au.clip = Microphone.Start(null, true, MicSecond, Frequency);
    }

    //停止录音....
    void OnStopClick()
    {
        if (!Microphone.IsRecording(null))
            return;
        Microphone.End(null);
        au.Stop();
    }

    //播放录音....
    void OnPlayClick()
    {
        if (Microphone.IsRecording(null))
            return;
        if (au.clip == null)
            return;
        au.mute = false;
        au.loop = false;
        au.Play();
        foreach (AudioSource au in StrongthenSound)
        {
            au.Play();
        }

    }

    public void WavFromClip(string WavPosition, AudioClip clip)
    {
        if (Microphone.IsRecording(null))
            return;
        Microphone.End(null);

        using (FileStream fs = CreateEmpty(WavPosition))
        {
            ConvertAndWrite(fs, au.clip);
            WriteHeader(fs, au.clip); //wav文件头
        }
    }

    private FileStream CreateEmpty(string filepath)
    {
        FileStream fileStream = new FileStream(filepath, FileMode.Create);
        byte emptyByte = new byte();

        for (int i = 0; i < 44; i++) //为wav文件头留出空间
        {
            fileStream.WriteByte(emptyByte);
        }

        return fileStream;
    }

    private void ConvertAndWrite(FileStream fileStream, AudioClip clip)
    {

        float[] samples = new float[clip.samples];
        clip.GetData(samples, 0);

        Int16[] intData = new Int16[samples.Length];

        Byte[] bytesData = new Byte[samples.Length * 2];

        int rescaleFactor = 32767; //to convert float to Int16  

        for (int i = 0; i < samples.Length; i++)
        {
            intData[i] = (short)(samples[i] * rescaleFactor);
            Byte[] byteArr = new Byte[2];
            byteArr = BitConverter.GetBytes(intData[i]);
            byteArr.CopyTo(bytesData, i * 2);
        }
        fileStream.Write(bytesData, 0, bytesData.Length);
    }


    private void WriteHeader(FileStream stream, AudioClip clip)
    {
        int hz = clip.frequency;
        int channels = clip.channels;
        int samples = clip.samples;

        stream.Seek(0, SeekOrigin.Begin);

        Byte[] riff = System.Text.Encoding.UTF8.GetBytes("RIFF");
        stream.Write(riff, 0, 4);

        Byte[] chunkSize = BitConverter.GetBytes(stream.Length - 8);
        stream.Write(chunkSize, 0, 4);

        Byte[] wave = System.Text.Encoding.UTF8.GetBytes("WAVE");
        stream.Write(wave, 0, 4);

        Byte[] fmt = System.Text.Encoding.UTF8.GetBytes("fmt ");
        stream.Write(fmt, 0, 4);

        Byte[] subChunk1 = BitConverter.GetBytes(16);
        stream.Write(subChunk1, 0, 4);

        UInt16 two = 2;
        UInt16 one = 1;

        Byte[] audioFormat = BitConverter.GetBytes(one);
        stream.Write(audioFormat, 0, 2);

        Byte[] numChannels = BitConverter.GetBytes(channels);
        stream.Write(numChannels, 0, 2);

        Byte[] sampleRate = BitConverter.GetBytes(hz);
        stream.Write(sampleRate, 0, 4);

        Byte[] byteRate = BitConverter.GetBytes(hz * channels * 2); // sampleRate * bytesPerSample*number of channels, here 44100*2*2  
        stream.Write(byteRate, 0, 4);

        UInt16 blockAlign = (ushort)(channels * 2);
        stream.Write(BitConverter.GetBytes(blockAlign), 0, 2);

        UInt16 bps = 16;
        Byte[] bitsPerSample = BitConverter.GetBytes(bps);
        stream.Write(bitsPerSample, 0, 2);

        Byte[] datastring = System.Text.Encoding.UTF8.GetBytes("data");
        stream.Write(datastring, 0, 4);

        Byte[] subChunk2 = BitConverter.GetBytes(samples * channels * 2);
        stream.Write(subChunk2, 0, 4);

    }
    #endregion
}
