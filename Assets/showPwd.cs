using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class showPwd : MonoBehaviour
{
    public Toggle showIcon;
    public TMP_InputField input;
    private bool isShow = true;
    // Start is called before the first frame update
    void Start()
    {
        showIcon.onValueChanged.AddListener(isShowPassword);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void isShowPassword(bool isShown)
    {
        input.Select();
        input.contentType = isShown ? TMP_InputField.ContentType.Standard : TMP_InputField.ContentType.Password;

    }
}
