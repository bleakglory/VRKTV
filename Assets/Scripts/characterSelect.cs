using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class characterSelect : MonoBehaviour
{
    public GameObject[] characterList;
    private int selectedIndex = 0;
    bool isConfirmCharacter = false;
    public Transform cameraTransform;
    public GameObject[] points;
    public float moveSpeed = 1;
    public float rotateSpeed;
    int i = 0;
    float distance;
    public GameObject characterPanel;
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
        if (isConfirmCharacter)
        {
            systemNotification.SetActive(true);
            systemInformation.GetComponent<TMPro.TextMeshProUGUI>().color = Color.green;
            systemInformation.GetComponent<TMPro.TextMeshProUGUI>().text = "You have Successfully select a character! You will move into the BackGround Selection Session in " + (int)(3 - time) + "s";
            time += Time.deltaTime;
            if (time >= 3)
            {
                systemNotification.SetActive(false);
                characterPanel.transform.position = new Vector3(0,100,0);

                cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, Quaternion.LookRotation(points[i].transform.position - cameraTransform.position), rotateSpeed * Time.deltaTime);
                distance = Vector3.Distance(cameraTransform.position, points[i].transform.position);
                cameraTransform.position = Vector3.MoveTowards(cameraTransform.position, points[i].transform.position, Time.deltaTime * moveSpeed);
                if (distance < 0.1f && i < points.Length - 1)
                {
                    i++;


                }
                else if (distance < 0.1f && i == points.Length - 1)
                {
                    SceneManager.LoadScene(2);// load into chooseBG scene
                }
            }
            
        }
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

        isConfirmCharacter = true;
        
        //SceneManager.LoadScene(3);
    }

    public void clickLobbyCancel()
    {
        SceneManager.LoadScene(0);
        //SceneManager.LoadScene(0);
    }
}
