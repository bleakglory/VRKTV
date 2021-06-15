using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundAdjust : MonoBehaviour
{    
    public AudioSource[] MusicList;
    public AudioSource PlayerRecord;

    public Slider PlayerRecordController;
    public Slider AccompanyMusicController;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        AccompanyMusicAdjust();
        PlayerRecordAdjust();
    }

    #region
    public void AccompanyMusicAdjust()
    {
        MusicList[PlayerPrefs.GetInt("Music") - 1].volume = AccompanyMusicController.value/10;
    }
    #endregion

    public void PlayerRecordAdjust()
    {
        PlayerRecord.volume = PlayerRecordController.value;
    }
}
