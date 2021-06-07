using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using UnityEngine.UI;

public class tettttt : MonoBehaviour
{
	// 储存获取到的图片
	List<Texture2D> allTex2d = new List<Texture2D>();
	public Image image;
	// Use this for initialization
	void Start()
	{
		load();
	}

    void Update()
    {
		//image.GetComponent<Texture2D>().a
    }

    void load()
	{
		List<string> filePaths = new List<string>();
		string imgtype = "*.BMP|*.JPG|*.GIF|*.PNG";
		string[] ImageType = imgtype.Split('|');
		for (int i = 0; i < ImageType.Length; i++)
		{
			//获取d盘中a文件夹下所有的图片路径
			string[] dirs = Directory.GetFiles(@"C:\Users\HP\Desktop\1", ImageType[i]);
			for (int j = 0; j < dirs.Length; j++)
			{
				filePaths.Add(dirs[j]);
			}
		}

		for (int i = 0; i < filePaths.Count; i++)
		{
			Texture2D tx = new Texture2D(100, 100);
			tx.LoadImage(getImageByte(filePaths[i]));
			allTex2d.Add(tx);
		}
	}

	/// <summary>
	/// 根据图片路径返回图片的字节流byte[]
	/// </summary>
	/// <param name="imagePath">图片路径</param>
	/// <returns>返回的字节流</returns>
	private static byte[] getImageByte(string imagePath)
	{
		FileStream files = new FileStream(imagePath, FileMode.Open);
		byte[] imgByte = new byte[files.Length];
		files.Read(imgByte, 0, imgByte.Length);
		files.Close();
		return imgByte;
	}
 


	private Sprite[] textureToSprite(Texture2D[] texttures)
    {
		string[] dirs = Directory.GetFiles(@"C:\Users\HP\Desktop\1");
		Sprite[] sprite = new Sprite[dirs.Length];
		for(int i = 0; i < dirs.Length; i++)
        {
            sprite[i]= Sprite.Create(texttures[i], new Rect(0, 0, texttures[i].width, texttures[i].height), new Vector2(0.5f, 0.5f));
		}
		return sprite;
	}
}
