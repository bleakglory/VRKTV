using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject player;
    public GameObject leftHand;
    public GameObject rightHand;
    Vector3 lastLeftHandPos;
    Vector3 lastRightHandPos;
    Vector3 currentForward;
    float Rotate;
    void Start()
    {
        Vector3 lastLeftHandPos = leftHand.transform.position;
        Vector3 lastRightHandPos = rightHand.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        playerMove();
        //cameraRotate();
    }

    private void Update()
    {
        
    }

    void playerMove()
    {
        if(Mathf.Abs(leftHand.transform.position.y - lastLeftHandPos.y) > 0.001f || Mathf.Abs(rightHand.transform.position.y - lastRightHandPos.y) > 0.001f )
        {
            Rotate = player.transform.eulerAngles.y % 360;
            if (Mathf.Abs(Rotate) <= 90)
            {
                currentForward.z = (float)(Mathf.Abs(Rotate));
                currentForward.x = (float)(Mathf.Abs(Rotate));
            }
            else if(Mathf.Abs(Rotate) >= 270)
            {
                currentForward.z = (float)(360 - Mathf.Abs(Rotate));
                currentForward.x = - (float)(360 - Mathf.Abs(Rotate));
            }
            else if(Mathf.Abs(Rotate) > 90 && Mathf.Abs(Rotate) < 180)
            {
                currentForward.z = - (float)(180 - Mathf.Abs(Rotate));
                currentForward.x = (float)(180 - Mathf.Abs(Rotate));
            }
            else
            {
                currentForward.z = - (float)(270 - Mathf.Abs(Rotate));
                currentForward.x = - (float)(270 - Mathf.Abs(Rotate));
            }
            transform.Translate(new Vector3(currentForward.x,0,0).normalized * Time.deltaTime , null);
            transform.Translate(new Vector3(0, 0, currentForward.z).normalized * Time.deltaTime, null);
        }
        lastRightHandPos = rightHand.transform.position;
        lastLeftHandPos = leftHand.transform.position;
    }

    void cameraRotate()
    {
        Vector3 charge = player.transform.position - transform.position;
        charge.y += 180;
        while (charge.x > 10 || charge.y > 10 || charge.z > 10)
        {
            transform.Rotate(charge, Space.Self);
        }
    }
}
