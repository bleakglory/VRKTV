using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class playRecord : MonoBehaviour
{

    public Image recordPlayer;
    String path;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void recordPlay()
    {
        //if (Time.time - lastTime > 5) {
        //    int i = 0;
        //    recordPlayer.GetComponent<Image>().sprite = textureToSprite(getTextures(path))[i];
        //    i++;
            
        //}
        
    }

    public List<Texture2D> getTextures(String path)
    {
        List<Texture2D> textures = new List<Texture2D>();
        string[] files = Directory.GetFiles(path);
        foreach (string file in files)
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            fs.Seek(0, SeekOrigin.Begin);
            byte[] bytes = new byte[fs.Length];
            try
            {
                fs.Read(bytes, 0, bytes.Length);

            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
            fs.Close();
            fs.Dispose();

            int width = 2048;
            int height = 2048;
            Texture2D texture = new Texture2D(width, height);
            texture.LoadImage(bytes);
            textures.Add(texture);
        }

        return textures;
    }

    public List<Sprite> textureToSprite(List<Texture2D> textures)
    {

        
        List<Sprite> sprites = new List<Sprite>();
        foreach(Texture2D texture in textures)
        {
            
            sprites.Add(Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)));
        }
        return sprites;
    }
}
