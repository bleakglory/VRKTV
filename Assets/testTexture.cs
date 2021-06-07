using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Drawing;

public class testTexture : MonoBehaviour
{
    private float fps = 10.0f;
    private float time = 0;
    private int currentIndex = 0;
    private Object[] texObject;
    public bool isCanDraw = false;
    private string path;
    Texture[] frameTex;

    void Start()
    {
        
        LoadTexture(texObject, "Texture");
    }
    void Update()
    {

    }
    void OnGUI()
    {
        int length = frameTex.Length;
        this.GetComponent<Renderer>().material.mainTexture = frameTex[currentIndex];
        time += Time.deltaTime;

        if (time >= 1.0f / fps)
        {
            currentIndex++;
            time = 0;
            if (currentIndex >= length - 1)
            {
                //播放一遍
                //currentIndex = length - 1;
                //循环播放
                currentIndex = 0;

            }
        }
    }

    void LoadTexture(Object[] texObj, string path)
    {
        texObj = Resources.LoadAll(path);
        frameTex = new Texture[texObj.Length];
        texObj.CopyTo(frameTex, 0);

    }

   

}   
