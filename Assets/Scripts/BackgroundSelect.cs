using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackgroundSelect : MonoBehaviour
{
    public Sprite[] BackgroundList;
    public Image current;
    private int selectedIndex = 0;


    bool isBGConfirm = false;

    public Transform cameraTransform;
    public GameObject[] points;
    public float moveSpeed = 1;
    public float rotateSpeed;
    int i = 0;
    float distance;
    
    public GameObject BGPanel;
    public GameObject systemNotification;
    public TMP_Text systemInformation;
    float time = 0;

    // Start is called before the first frame update
    void Start()
    {
        
        systemNotification.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isBGConfirm)
        {
            systemNotification.SetActive(true);
            systemInformation.GetComponent<TMPro.TextMeshProUGUI>().color = Color.green;
            systemInformation.GetComponent<TMPro.TextMeshProUGUI>().text = "You have Successfully select a BackGround! You will move into the KTV Session in " + (int)(3 - time) + "s";
            time += Time.deltaTime;
            if (time >= 3)
            {
                BGPanel.transform.position = new Vector3(0, 100, 0);
                systemNotification.SetActive(false);
                cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, Quaternion.LookRotation(points[i].transform.position - cameraTransform.position), rotateSpeed * Time.deltaTime);
                distance = Vector3.Distance(cameraTransform.position, points[i].transform.position);
                cameraTransform.position = Vector3.MoveTowards(cameraTransform.position, points[i].transform.position, Time.deltaTime * moveSpeed);
                if (distance < 0.1f && i < points.Length - 1)
                {
                    i++;


                }
                else if (distance < 0.1f && i == points.Length - 1)
                {
                    
                    SceneManager.LoadScene(3);// load into KTV scene
                }
            }
                
        }
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
        isBGConfirm = true;
        //SceneManager.LoadScene(2);//load into the KTV scene
    }

    public void clickCancel()
    {
        SceneManager.LoadScene(1);//reload chooseBG scene
    }
}
