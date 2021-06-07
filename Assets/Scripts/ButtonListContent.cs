using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonListContent : MonoBehaviour
{
    [SerializeField]
    private GameObject buttonTemplate;
    [SerializeField]
    private Dropdown select_singer;
    [SerializeField]
    private Dropdown select_type;



    private List<GameObject> buttons;

    //private List<string> stringlist;
    string[] songs_info;
    private void Start()
    {

        songs_info = new String[5];
        songs_info[0] = "老男孩,筷子兄弟,怀旧";
        songs_info[1] = "年少有为,李荣浩,抒情";
        songs_info[2] = "起风了,周深,青春";
        songs_info[3] = "十年,陈奕迅,经典";
        songs_info[4] = "水星记,郭顶,抒情";
        // songs_info = System.IO.File.ReadAllLines("Assets/songs.txt");

        //for (int i = 0; i < songs_info.Length; i++) { Debug.Log("song: " + songs_info[i]); }
        // Debug.Log("song"+songs_info.Length);
        buttons = new List<GameObject>();
        for (int i = 0; i < songs_info.Length; i++)
        {
            GameObject button = Instantiate(buttonTemplate) as GameObject;
            button.SetActive(true);
            button.GetComponent<ButtonListButton>().SetText(songs_info[i].Split(',')[0]);
            buttons.Add(button);
            Debug.Log(button.ToString());
        }

            for (int j = 0; j<buttons.Count; j++)
            {
                buttons[j].transform.SetParent(buttonTemplate.transform.parent, false);
            }

        //choose_singer("xiaohong");

    }

    //public void namesearch()
    //{
    //    buttons = new List<GameObject>();
        
    //    foreach (ButtonListButton button in buttonTemplate.transform.parent.GetComponentsInChildren<ButtonListButton>())
    //    {
    //        Destroy(button.gameObject);
    //    }

    //    for (int i = 0; i < songs_info.Length; i++)
    //    {
    //        if (songs_info[i].Split(',')[0].Equals(enter_name.text))
    //        {
    //            GameObject button = Instantiate(buttonTemplate) as GameObject;
    //            button.SetActive(true);
    //            button.GetComponent<ButtonListButton>().SetText(songs_info[i].Split(',')[0]);
    //            buttons.Add(button);
    //        }
            
    //        for (int j = 0; j < buttons.Count; j++)
    //        {
    //            buttons[j].transform.SetParent(buttonTemplate.transform.parent, false);
    //        }
    //    }
    //}
    
    public void enter_singer()
    {

        buttons = new List<GameObject>();
        if (buttons.Count > 0)
        {
            foreach (GameObject button in buttons)
            {
                Destroy(button.gameObject);
            }
            
            buttons.Clear();
            
        }
        choose_singer(select_singer.options[select_singer.value].text);
    }

    public void enter_type()
    {
        buttons = new List<GameObject>();
        if (buttons.Count > 0)
        {
            foreach (GameObject button in buttons)
            {
                Destroy(button.gameObject);
            }

            buttons.Clear();

        }
        choose_type(select_type.options[select_type.value].text);
    }

    private void choose_singer(string singer)
    {
        foreach(ButtonListButton button in buttonTemplate.transform.parent.GetComponentsInChildren<ButtonListButton>())
        {
            Destroy(button.gameObject);
        }

        for (int i = 0; i < songs_info.Length; i++)
        {
            Debug.Log("select singer: "+ (songs_info[i].Split(',')[1])+" singger: "+singer);
            if ((songs_info[i].Split(',')[1].Equals(singer)||singer.Equals("anyone"))&& ((songs_info[i].Split(',')[2].Equals(select_type.options[select_type.value].text))||(select_type.options[select_type.value].text.Equals("anytype"))))
            {   
                GameObject button = Instantiate(buttonTemplate) as GameObject;
                button.SetActive(true);
                button.GetComponent<ButtonListButton>().SetText(songs_info[i].Split(',')[0]);
                buttons.Add(button);
            }
            //else if(singer.Equals("anyone"))
            //{
            //    GameObject button = Instantiate(buttonTemplate) as GameObject;
            //    button.SetActive(true);
            //    button.GetComponent<ButtonListButton>().SetText(songs_info[i].Split(',')[0]);
            //    buttons.Add(button);
            //}

            for (int j = 0; j < buttons.Count; j++)
            {

                buttons[j].transform.SetParent(buttonTemplate.transform.parent, false);
            }
        }
    }

    private void choose_type(string type)
    {
        
        foreach (ButtonListButton button in buttonTemplate.transform.parent.GetComponentsInChildren<ButtonListButton>())
        {
            Destroy(button.gameObject);
        }

        for (int i = 0; i < songs_info.Length; i++)
        {
            if ((songs_info[i].Split(',')[2].Equals(type)||type.Equals("anytype"))&&(songs_info[i].Split(',')[1].Equals(select_singer.options[select_singer.value].text)||(select_singer.options[select_singer.value].text.Equals("anyone"))))
            {

                GameObject button = Instantiate(buttonTemplate) as GameObject;
                button.SetActive(true);
                button.GetComponent<ButtonListButton>().SetText(songs_info[i].Split(',')[0]);
                buttons.Add(button);
            }
            //else if (type.Equals("anytype"))
            //{
            //    GameObject button = Instantiate(buttonTemplate) as GameObject;
            //    button.SetActive(true);
            //    button.GetComponent<ButtonListButton>().SetText(songs_info[i].Split(',')[0]);
            //    buttons.Add(button);
            //}

            for (int j = 0; j < buttons.Count; j++)
            {

                buttons[j].transform.SetParent(buttonTemplate.transform.parent, false);
            }
        }
    }

    public void ButtonClicked(string myTextString)
    {
        //finish choosing the music
        Debug.Log(myTextString);
    }


}
