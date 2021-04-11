using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    public GameObject systemInformation;
    


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

        isSignIn = false;
    }

    // Update is called once per frame
    void Update()
    {
        
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
        if (isSignIn)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                systemInformation.GetComponent<TMPro.TextMeshProUGUI>().text = "username or password is empty";
            }
            else
            {
                if (GetComponent<UserController>().Login(username, password) == 1)
                {
                    systemInformation.GetComponent<TMPro.TextMeshProUGUI>().text = "user dont exist";
                    Debug.Log("用户不存在");
                }
                else if (GetComponent<UserController>().Login(username, password) == 2)
                {
                    systemInformation.GetComponent<TMPro.TextMeshProUGUI>().text = "wrong pwd";
                    Debug.Log("密码错误");
                }
                else if (GetComponent<UserController>().Login(username, password) == 0)
                {
                    systemInformation.GetComponent<TMPro.TextMeshProUGUI>().text = "succ log in";
                    Debug.Log("成功登录");
                    SceneManager.LoadScene(1);

                }
            }

        }
        else
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                systemInformation.GetComponent<TMPro.TextMeshProUGUI>().text = "username or password is empty";
            }
            else
            {
                if (!password.Equals(confirmPassword))
                {
                    systemInformation.GetComponent<TMPro.TextMeshProUGUI>().text = "two pwd are not same"+" "+password+"   cp:"+confirmPassword;

                }
                else
                {
                    GetComponent<UserController>().Register(username, password);
                    systemInformation.GetComponent<TMPro.TextMeshProUGUI>().text = "succ register";
                    SceneManager.LoadScene(0);
                }

            }

        }
    }

    public void clickCancel()
    {
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


}
