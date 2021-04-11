using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class characterSelect : MonoBehaviour
{
    public GameObject[] characterList;
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
        if (selectedIndex == characterList.Length - 1)
        {
            characterList[selectedIndex].SetActive(false);
            selectedIndex = 0;
        }
        else
        {
            characterList[selectedIndex].SetActive(false);
            selectedIndex++;
        }
        PlayerPrefs.SetInt("player", selectedIndex);
        characterList[selectedIndex].SetActive(true);
    }
    public void previosCharacter()
    {
        
        if (selectedIndex == -1)
        {
            characterList[selectedIndex].SetActive(false);
            selectedIndex = characterList.Length - 1;
        }
        else
        {
            characterList[selectedIndex].SetActive(false);
            selectedIndex--;
        }
        PlayerPrefs.SetInt("player", selectedIndex);
        characterList[selectedIndex].SetActive(true);
    }

    public void clickLobbyConfirm()
    {
        SceneManager.LoadScene(3);
    }

    public void clickLobbyCancel()
    {
        SceneManager.LoadScene(0);
    }
}
