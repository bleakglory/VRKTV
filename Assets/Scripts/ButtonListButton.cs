using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonListButton : MonoBehaviour
{
    [SerializeField]
    private Text myText;
    [SerializeField]
    private ButtonListContent buttonControl;

    private string myTextString;

    public void SetText(string textString)
    {
        myTextString = textString;
        myText.text = textString;
    }

    public void Onclick()
    {
        buttonControl.ButtonClicked(myTextString);
        
    }
}
