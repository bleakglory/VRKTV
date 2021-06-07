using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class testPassword : MonoBehaviour
{
    public Toggle showIcon;
    public TMP_InputField input;
    private bool isShow = true;


    private string passwordRule = @"^(?![0-9]+$)(?![a-zA-Z]+$)[a-zA-Z\d]{8,15}$"; //密码必须有数字与字母混合组成的8-15位数
    // Start is called before the first frame update
    void Start()
    {
        showIcon.onValueChanged.AddListener(isShowPassword);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    


    public void checkPasswordIsValid(string s)
    {
        Regex regex = new Regex(passwordRule);
        if (regex.IsMatch(s))
        {
            Debug.Log("该密码符合规则");
            
        }
        else
        {
            Debug.Log("该密码不符合规则");
            
        }
        
    }

    public void isShowPassword(bool isShown)
    {
        input.Select();
        input.contentType = isShown ? TMP_InputField.ContentType.Standard : TMP_InputField.ContentType.Password;
        
    }

    
}
