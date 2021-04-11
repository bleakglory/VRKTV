using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackgroundSelect : MonoBehaviour
{
    public Sprite[] BackgroundList;
    public Image current;
    private int selectedIndex = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void nextCharacter()
    {
        if (selectedIndex == BackgroundList.Length - 1)
        {
            selectedIndex = 0;
        }
        else
        {
            selectedIndex++;
        }
        current.sprite = BackgroundList[selectedIndex];
        PlayerPrefs.SetInt("Background", selectedIndex);
    }
    public void previosCharacter()
    {

        if (selectedIndex == -1)
        {
            selectedIndex = BackgroundList.Length - 1;
        }
        else
        {
            selectedIndex--;
        }
        current.sprite = BackgroundList[selectedIndex];
        PlayerPrefs.SetInt("Background", selectedIndex);
    }

    public void clickConfirm()
    {
        SceneManager.LoadScene(2);
    }

    public void clickCancel()
    {
        SceneManager.LoadScene(3);
    }
}
