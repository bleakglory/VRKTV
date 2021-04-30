using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject player;
    public GameObject leftHand;
    public GameObject rightHand;

    public Transform startTrans;
    public GameObject Menucanvas;
    public GameObject Returncanvas;
    public ParticleSystem MovePointEFT;
    public int height;
    public int resPoint;
    public LineRenderer lineRender;

    public LineRenderer line;
    public GameObject input;
    public GameManager GM;

    private Transform forward;

    float Rotate;
    Vector3[] _path;
    void Start()
    {

    }


    private void Update()
    {
        if (!(Menucanvas.activeInHierarchy || Returncanvas.activeInHierarchy))
        {
            playerMove();
            line.enabled = false;
            input.SetActive(false);
        }

       else 
        {
            if (!line.enabled)
                line.enabled = true;
            if (!input.activeInHierarchy)
                input.SetActive(true);
        }
    }

    //LateUpdate: let camera move with canvas
    void LateUpdate()
    {
        Menucanvas.transform.position = new Vector3(transform.TransformPoint(Vector3.forward * 2).x, transform.TransformPoint(Vector3.forward * 2).y + 40, transform.TransformPoint(Vector3.forward * 2).z);
        Returncanvas.transform.position = new Vector3(transform.TransformPoint(Vector3.forward * 2).x, transform.TransformPoint(Vector3.forward * 2).y + 40, transform.TransformPoint(Vector3.forward * 2).z);
    }

    public static Vector3 GetBezierPoint(float t, Vector3 start, Vector3 center, Vector3 end)
    {
        return (1 - t) * (1 - t) * start + 2 * t * (1 - t) * center + t * t * end;
    }

    void playerMove()
    {
        var startPoint = startTrans.position;
        //var endPoint = new Vector3(2*transform.forward.normalized.x, transform.position.y - 0.5f, 2*transform.forward.normalized.z);
        var endPoint = startTrans.TransformPoint(Vector3.down * 4);
        endPoint = new Vector3(endPoint.x, transform.position.y - 0.4f, endPoint.z);
        var bezierControlPoint = (startPoint + endPoint) * 0.5f + (Vector3.up * height);

        _path = new Vector3[resPoint];
        for (int i = 0; i < resPoint; i++)
        {
            var t = (i + 1) / (float)resPoint;
            _path[i] = GetBezierPoint(t, startPoint, bezierControlPoint, endPoint);
        }

        lineRender.positionCount = _path.Length;
        lineRender.SetPositions(_path);
        MovePointEFT.transform.position = endPoint;
        if (OVRInput.GetDown(OVRInput.Button.One) && MovePointEFT.GetComponent<MoveJudgement>().canMove)
        {
            transform.position = new Vector3(endPoint.x, transform.position.y, endPoint.z);
        }
        //if(Mathf.Abs(leftHand.transform.position.y - lastLeftHandPos.y) > 0.001f || Mathf.Abs(rightHand.transform.position.y - lastRightHandPos.y) > 0.001f )
        //{
        //    Rotate = player.transform.eulerAngles.y % 360;
        //    if (Mathf.Abs(Rotate) <= 90)
        //    {
        //        currentForward.z = (float)(Mathf.Abs(Rotate));
        //        currentForward.x = (float)(Mathf.Abs(Rotate));
        //    }
        //    else if(Mathf.Abs(Rotate) >= 270)
        //    {
        //        currentForward.z = (float)(360 - Mathf.Abs(Rotate));
        //        currentForward.x = - (float)(360 - Mathf.Abs(Rotate));
        //    }
        //    else if(Mathf.Abs(Rotate) > 90 && Mathf.Abs(Rotate) < 180)
        //    {
        //        currentForward.z = - (float)(180 - Mathf.Abs(Rotate));
        //        currentForward.x = (float)(180 - Mathf.Abs(Rotate));
        //    }
        //    else
        //    {
        //        currentForward.z = - (float)(270 - Mathf.Abs(Rotate));
        //        currentForward.x = - (float)(270 - Mathf.Abs(Rotate));
        //    }
        //    transform.Translate(new Vector3(currentForward.x,0,0).normalized * Time.deltaTime , null);
        //    transform.Translate(new Vector3(0, 0, currentForward.z).normalized * Time.deltaTime, null);
        //}
        //lastRightHandPos = rightHand.transform.position;
        //lastLeftHandPos = leftHand.transform.position;
    }


}
