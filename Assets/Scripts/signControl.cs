using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class signControl : MonoBehaviour
{
    //-------------Sign In------------//
    public GameObject username_signIn;
    public GameObject password_signIn;

    //-------------Sign Up------------//
    public GameObject username_signUp;
    public GameObject password_signUp;
    public GameObject passwordConfirm_signUp;

    //-------------Button-------------//
    public GameObject signIn_but;
    public GameObject signUp_but;
    public GameObject confirm_but;
    public GameObject cancel_but;


    bool isSignIn = false;
    string username;
    string password;
    string confirmPassword;

    public TMP_Text systemInformation;
    public GameObject systemNotificationPanel;
    public GameObject systemPanel;


    bool isLogged = false;
    public Transform cameraTransform;
    public GameObject[] points;
    public float moveSpeed = 1;
    public float rotateSpeed;
    int i = 0;
    float distance;
    float time = 0;



    // Start is called before the first frame update
    void Start()
    {
        signIn_but.SetActive(true);
        signUp_but.SetActive(true);
        username_signIn.SetActive(false);
        password_signIn.SetActive(false);
        username_signUp.SetActive(false);
        password_signUp.SetActive(false);
        passwordConfirm_signUp.SetActive(false);
        confirm_but.SetActive(false);
        cancel_but.SetActive(false);
        systemNotificationPanel.SetActive(false);

        isSignIn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isLogged)
        {
            systemInformation.GetComponent<TMPro.TextMeshProUGUI>().color = Color.green;
            systemInformation.GetComponent<TMPro.TextMeshProUGUI>().text = "You have Successfully logged in our system! You will move into the Characters Selection Session in " + (int)(3 - time) + "s";
            time += Time.deltaTime;
            if (time >= 3)
            {
                systemPanel.transform.position = new Vector3(0, 100, 0);
                systemNotificationPanel.SetActive(false);
                cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, Quaternion.LookRotation(points[i].transform.position - cameraTransform.position), rotateSpeed * Time.deltaTime);
                distance = Vector3.Distance(cameraTransform.position, points[i].transform.position);
                cameraTransform.position = Vector3.MoveTowards(cameraTransform.position, points[i].transform.position, Time.deltaTime * moveSpeed);
                if (distance < 0.1f && i < points.Length - 1)
                {
                    i++;


                }
                else if (distance < 0.1f && i == points.Length - 1)
                {
                    SceneManager.LoadScene(1);
                }
            }
            
        }
    }

    public void clickSignIn()
    {
        signIn_but.SetActive(false);
        signUp_but.SetActive(false);
        username_signIn.SetActive(true);
        password_signIn.SetActive(true);
        username_signUp.SetActive(false);
        password_signUp.SetActive(false);
        passwordConfirm_signUp.SetActive(false);
        confirm_but.SetActive(true);
        cancel_but.SetActive(true);

        isSignIn = true;
    }

    public void clickSignUp()
    {
        signIn_but.SetActive(false);
        signUp_but.SetActive(false);
        username_signIn.SetActive(false);
        password_signIn.SetActive(false);
        username_signUp.SetActive(true);
        password_signUp.SetActive(true);
        passwordConfirm_signUp.SetActive(true);
        confirm_but.SetActive(true);
        cancel_but.SetActive(true);


    }

    public void clickConfirm()
    {
        //Sign In
        if (isSignIn)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                systemNotificationPanel.SetActive(true);
                systemInformation.GetComponent<TMPro.TextMeshProUGUI>().color = Color.red;
                systemInformation.GetComponent<TMPro.TextMeshProUGUI>().text = "Username or Password is empty! Please enter your username and password...";
            }
            else
            {
                if (GetComponent<UserController>().Login(username, password) == 1)
                {
                    systemNotificationPanel.SetActive(true);
                    systemInformation.GetComponent<TMPro.TextMeshProUGUI>().color = Color.red;
                    systemInformation.GetComponent<TMPro.TextMeshProUGUI>().text = "This user does not exist! Please enter an available username...";
                    Debug.Log("用户不存在");
                }
                else if (GetComponent<UserController>().Login(username, password) == 2)
                {
                    systemNotificationPanel.SetActive(true);
                    systemInformation.GetComponent<TMPro.TextMeshProUGUI>().color = Color.red;
                    systemInformation.GetComponent<TMPro.TextMeshProUGUI>().text = "The password is wrong! Please enter your password again...";
                    Debug.Log("密码错误");
                }
                else if (GetComponent<UserController>().Login(username, password) == 0)
                {
                    systemNotificationPanel.SetActive(true);
                    systemInformation.GetComponent<TMPro.TextMeshProUGUI>().color = Color.green;
                    systemInformation.GetComponent<TMPro.TextMeshProUGUI>().text = "You have Successfully logged in our system! You will move into the Characters Selection Session in "+(int)(3-time)+"s";
                    Debug.Log("成功登录");
                    //SceneManager.LoadScene(1);
                    isLogged = true;

                }
            }

        }
        else
        {
            //Sign Up
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                systemNotificationPanel.SetActive(true);
                systemInformation.GetComponent<TMPro.TextMeshProUGUI>().color = Color.red;
                systemInformation.text = "Username or Password is empty! Please enter your username and password...";
            }
            else
            {
                if (!password.Equals(confirmPassword))
                {
                    systemNotificationPanel.SetActive(true);
                    systemInformation.GetComponent<TMPro.TextMeshProUGUI>().color = Color.red;
                    systemInformation.text = "Two passwords are not same! Please check your password...";

                }
                else
                {
                    systemNotificationPanel.SetActive(true);
                    GetComponent<UserController>().Register(username, password);
                    systemInformation.GetComponent<TMPro.TextMeshProUGUI>().color = Color.green;
                    systemInformation.text = "You have successfully register an account, you can use the account to log in our system...";
                    SceneManager.LoadScene(0);
                }

            }

        }
    }

    public void clickCancel()
    {
        systemNotificationPanel.SetActive(false);
        SceneManager.LoadScene(0);
    }

    public void saveUsername(string text)
    {
        username = text;
    }

    public void savePassword(string text)
    {
        password = text;
    }

    public void saveConfirmPassword(string text)
    {
        confirmPassword = text;
    }

    public void checkPasswordIsValid(string passward)
    {

    }


}
