using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class TestController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(DateTime.Now.ToString("MM_dd_HH_mm_ss"));
        Mp4File mp4 = new Mp4File();
        if (mp4.Open(@"C:\Users\vrlab\Documents\GitHub\VRKTV\Assets\CameraRecorder\RecordedVideo.mp4"))
        {
            Debug.Log("mp4true");
            Debug.Log(Environment.CurrentDirectory);
            FileStream filestream = new FileStream(Environment.CurrentDirectory + "\\" + "mp4information.txt", FileMode.Create);
            StreamWriter streamWriter = new StreamWriter(filestream, Encoding.Default);
            streamWriter.Write(mp4.ToString());
            streamWriter.Flush();
            streamWriter.Close();
            filestream.Close();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        /*
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("MergeFile:~");
            String[] filelist = new String[] { "Assets/Music/houlai.wav", "Assets/Music/shinian.wav" };
            String outputfilename = "Assets/Music/mergeaudiofile.wav";
            String tempdir = "Assets/Music/temp";
            WAVFile.MergeAudioFiles(filelist, outputfilename, tempdir);
        }
        */
        if (Input.GetKeyDown(KeyCode.Space)||OVRInput.GetDown(OVRInput.RawButton.A))
        {
            Debug.Log("space!!!");
            CameraRecorder.Trigger = true;
        }
        
    }
}