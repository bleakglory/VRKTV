using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;

public class CameraRecorder : MonoBehaviour
{

    //External Plugin Function Definition
    //Pack the accumulated frames and samples in a video with a width and height size that will be recorded at videopath
    class PackVideoClass
    {
        public string videoPath;
        public int width, height;
        public float frameCount, duration;
        public IntPtr[] colors;
        [DllImport("CameraRecorder")]
        private static extern void PackVideo(
        string videoPath,
        int width,
        int height,
        System.IntPtr[] colors,
        float frameCount,
        float duration);
        public PackVideoClass(string videoPath,int width,int height,IntPtr[] colors,float frameCount,float duration)
        {
            this.videoPath = videoPath;
            this.width = width;
            this.height = height;
            this.colors = colors;
            this.frameCount = frameCount;
            this.duration = duration;
        }
        public void Packvideo()
        {
            Debug.Log("startpacktime:" + DateTime.Now.TimeOfDay.ToString());
            PackVideo(videoPath, width, height, colors, frameCount, duration);
            Debug.Log("endpacktime:" + DateTime.Now.TimeOfDay.ToString());
        }
    }
    


    public Camera videoSource;

    private static bool recording = false;
    public static bool Trigger = false;
    private static float startingTime = 0;
    private string path = null;
    private string currentUser = null;
    private static List<GCHandle> frames;  //Acumulated frames
    private static List<GCHandle> samples; //Acummulated samples



    void Start()
    {
        path = Application.persistentDataPath + "/Users";
        frames = new List<GCHandle>();
        samples = new List<GCHandle>();
    }
    void Update()
    {
        currentUser = GetComponent<UserController>().GetCurrentUser();
    }

    void FixedUpdate()
    {
        //Debug.Log("Trigger:" + Trigger);
        if (Trigger && videoSource)
        {

            Trigger = false;
            recording = !recording;
            if (recording)
            {
                Debug.Log("Starting the recording");
                startingTime = Time.time;
            }
            else
            {
                Debug.Log("Stoping the recording");
                //Debug.Log("videopath:" + Application.dataPath + "/CameraRecorder/RecordedVideo.mp4");
                float duration = Time.time - startingTime;
                string videoName = path + "/" + currentUser + "/Videos" + "/Video_" + DateTime.Now.ToString("MM_dd_HH_mm_ss");
                //Create a C friendly Array for the frames and then Call the C Function to Pack the video
                int frameCount = frames.Count;
                if (frameCount > 0)
                {
                    System.IntPtr[] framesArray = new System.IntPtr[frameCount];
                    for (int i = 0; i < frameCount; i++)
                        framesArray[i] = frames[i].AddrOfPinnedObject();
                    Debug.Log("width:" + videoSource.pixelWidth + " height:" + videoSource.pixelHeight);
                    PackVideoClass pack = new PackVideoClass(
                        //Application.dataPath + "/CameraRecorder/RecordedVideo.mp4",
                        videoName,
                        videoSource.pixelWidth,
                        videoSource.pixelHeight,
                        framesArray,
                        frameCount,
                        duration);
                    Thread th = new Thread(pack.Packvideo);
                    th.Start();
                    /*
                    PackVideo(
                        Application.dataPath + "/CameraRecorder/RecordedVideo.avi",
                        videoSource.pixelWidth,
                        videoSource.pixelHeight,
                        framesArray,
                        frameCount,
                        duration);
                    */
                }

                //Free the accumulated frames
                frames.Clear();

                Debug.Log("FrameCount " + frameCount + " duration " + duration);
            }
        }
    }

    void LateUpdate()
    {
        if (videoSource && recording)
        {
            //Add the last Rendered Frame of the videoSource to the accumulated Frames

            //Render the Camera Frame into a 2D texture
            RenderTexture rt = new RenderTexture(videoSource.pixelWidth, videoSource.pixelHeight, 24);
            videoSource.targetTexture = rt;
            Texture2D screenShot = new Texture2D(videoSource.pixelWidth, videoSource.pixelHeight, TextureFormat.ARGB32, false);
            videoSource.Render();
            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, videoSource.pixelWidth, videoSource.pixelHeight), 0, 0);
            videoSource.targetTexture = null;
            RenderTexture.active = null;
            Destroy(rt);

            //Add the rendered frame as pixels to the accumulated frames 
            Color32[] pixels = screenShot.GetPixels32();
            GCHandle pixelsHandle = GCHandle.Alloc(pixels, GCHandleType.Pinned);
            frames.Add(pixelsHandle);
        }
    }

}