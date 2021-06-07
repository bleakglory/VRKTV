using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine.Video;

public class GameManager : MonoBehaviour
{
    #region param
    public Canvas menuCanv;
    public Canvas recordCanv;
    public TMP_Text txt;
    public VideoPlayer[] VideoList;
    public AudioSource[] MusicList;
    public GameObject[] backgrounds;
    public AudioSource[] StrongthenSound;
    public Instrument instrument;
    private int Frequency = 48000; //录音频率
    private int BitRate = 16; //比特率
    private int MicSecond = 300;  
    public bool isStart;

    private bool isPlay;

    private string path = "";
    [SerializeField]
    private AudioSource au;
    private string[] _songName = new string[5];
    private string[] devices;
    private bool _isHaveMicrophone;
    private bool _startSoundBack;
    private string currentUser = null;
    #endregion

    void Start()
    {
        currentUser = PlayerPrefs.GetString("CurrentUser");
        path = Application.persistentDataPath + "/Users" + "/" + currentUser + "/Audios";
        au = GetComponent<AudioSource>();
        backgrounds[PlayerPrefs.GetInt("Background")].SetActive(true);
        menuCanv.gameObject.SetActive(true);
        recordCanv.gameObject.SetActive(false);
        isStart = false;
        isPlay = false;
        _startSoundBack = false;

        for (int i = 0; i < StrongthenSound.Length - 1; i++)
        {
            StrongthenSound[i] = au;
        }

        devices = Microphone.devices;

        if (devices.Length > 0)

        {

            _isHaveMicrophone = true;

            foreach ( string text in devices)
            {
                txt.text += "Microphone Checked:" + devices[0] + "\n";
            }
            
            UnityEngine.Debug.Log(devices[0]);
        }

        else
        {

            _isHaveMicrophone = false;

            txt.text = "No Microphone";

        }

        _songName[0] = "老男孩";
        _songName[1] = "年少有为";
        _songName[2] = "起风了";
        _songName[3] = "十年";
        _songName[4] = "水星记";
    }

    private void Update()
    {
        if(isStart == true && instrument.HasPlayed)
        {
            musicPlay();
        }

        if(isPlay == true)
        {
            if(! VideoList[PlayerPrefs.GetInt("Music") - 1].isPlaying)
            {
                OnStopClick();
                GetComponent<MusicVisualization>().thisAudioSource = null;
                recordCanv.gameObject.SetActive(true);
                isPlay = false;
            }
        }
        //if (Microphone.GetPosition(devices[0]) > 500)
        //{
        //    _startSoundBack = true;
        //}

        //if ( !_startSoundBack)
        //{
        //    foreach (AudioSource au in StrongthenSound)
        //    {
        //        au.Play();
        //        au.timeSamples = Microphone.GetPosition(devices[0]);
        //        au.Play();
        //    }

        //    au.Play();
        //    au.timeSamples = Microphone.GetPosition(devices[0]);
        //    au.Play();

        //    _startSoundBack = true;
        //}

    }
    void musicPlay()
    {
        VideoList[PlayerPrefs.GetInt("Music") - 1].Play();
        GetComponent<MusicVisualization>().thisAudioSource = MusicList[PlayerPrefs.GetInt("Music") - 1];
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

        string wav = path + "/"+ _songName[PlayerPrefs.GetInt("Music") - 1] + ".wav";
        using (Process proc = new Process())
        {
            proc.StartInfo.FileName = Application.streamingAssetsPath + @"SimpleDenoise.exe";
            proc.StartInfo.Arguments = string.Format(@" ""{0}""", wav);

            proc.StartInfo.CreateNoWindow = true;

            proc.StartInfo.UseShellExecute = false;

            proc.StartInfo.RedirectStandardOutput = true;
            proc.Start();
            proc.Close();
        }
    }

    public void playRecord()
    {
        VideoList[PlayerPrefs.GetInt("Music") - 1].Play();
        OnPlayClick();
    }

    public void backToMenu()
    {
        VideoList[PlayerPrefs.GetInt("Music") - 1].Stop();
        recordCanv.gameObject.SetActive(false);
        menuCanv.gameObject.SetActive(true);
        instrument.gameObject.SetActive(true);
        instrument.HasPlayed = false;
        instrument.Guitar.transform.position = instrument.OriPos.position;
        instrument.GetComponent<SkinnedMeshRenderer>().enabled = true;
        instrument.GetComponent<BoxCollider>().enabled = true;
        instrument.MicrophoneModel.SetActive(false);
    }

    public void chooseMusicButton(Button bn)
    {
        String buttonText = bn.GetComponentInChildren<Text>().text;
        if (buttonText.Equals("老男孩"))
        {
            one();
        }else if (buttonText.Equals("年少有为")) { two(); }
        else if (buttonText.Equals("起风了")) { three(); }
        else if (buttonText.Equals("十年")) { four(); }
        else if (buttonText.Equals("水星记")) { five(); }
    }
    //老男孩
    public void one()
    {
        startMusic(1);
    }
    //年少有为
    public void two()
    {
        startMusic(2);
    }
    //起风了
    public void three()
    {
        startMusic(3);
    }
    //十年
    public void four()
    {
        startMusic(4);
    }
    //水星记
    public void five()
    {
        startMusic(5);
    }

    //public void six()
    //{
    //    startMusic(6);
    //}

    //public void seven()
    //{
    //    startMusic(7);
    //}

    //public void eight()
    //{
    //    startMusic(8);
    //}

    //public void nine()
    //{
    //    startMusic(9);
    //}

    //public void ten()
    //{
    //    startMusic(10);
    //}
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
        if ((_isHaveMicrophone == false) || (Microphone.IsRecording(devices[0])))
        {
            return;
        }

        //au.Stop();
        //au.loop = false;
        //au.mute = true;
        au.clip = Microphone.Start(devices[0], true, MicSecond, Frequency);

        foreach (AudioSource au in StrongthenSound)
        {
            au.Play();
            au.timeSamples = Microphone.GetPosition(devices[0]);
            au.Play();
        }

        au.timeSamples = Microphone.GetPosition(devices[0]);
        au.Play();

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
