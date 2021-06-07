using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelSearch : MonoBehaviour
{
    
    public int currentPage=0;
    GameObject[] panelgroup=new GameObject [2];
    public GameObject p0;
    public GameObject p1;

    // Start is called before the first frame update
    void Start()
    {
        panelgroup[0] = p0;
        panelgroup[1] = p1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OpenPanel()
    {
        if (panelgroup[currentPage] != null)
        {
            panelgroup[currentPage].SetActive(true);
            //panelgroup[currentPage].SetActive(true);
        }
    }

    public void click_history()
    {
        currentPage = 1;
        closeAllPages();
        panelgroup[1].SetActive(true);
        Debug.Log(panelgroup[1].active);
    }

    void closeAllPages()
    {
        for(int i = 0; i < panelgroup.Length; i++)
        {
            panelgroup[i].SetActive(false);
        }
    }
}
