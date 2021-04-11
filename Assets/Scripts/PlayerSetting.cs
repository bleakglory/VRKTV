using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetting : MonoBehaviour
{
    public GameObject[] characterList;
    void Start()
    {
        characterList[PlayerPrefs.GetInt("player")].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
