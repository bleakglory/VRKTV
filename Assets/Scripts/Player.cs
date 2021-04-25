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
    public ParticleSystem MovePoint;
    public int height;
    public int resPoint;
    public LineRenderer lineRender;
    float Rotate;
    Vector3[] _path;
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    private void Update()
    {
        if (!(Menucanvas.activeInHierarchy || Returncanvas.activeInHierarchy))
        {
            playerMove();
        }
    }

    public static Vector3 GetBezierPoint(float t, Vector3 start, Vector3 center, Vector3 end)
    {
        return (1 - t) * (1 - t) * start + 2 * t * (1 - t) * center + t * t * end;
    }

    void playerMove()
    {
        var startPoint = startTrans.position;
        var endPoint = new Vector3(startTrans.position.x, transform.position.y , startTrans.position.z + 2);
        Debug.Log("end: " + endPoint);
        Debug.Log("start: " + startPoint);
        var bezierControlPoint = (startPoint + endPoint) * 0.5f + (Vector3.up * height);

        _path = new Vector3[resPoint];
        for (int i = 0; i < resPoint; i++)
        {
            var t = (i + 1) / (float)resPoint;
            _path[i] = GetBezierPoint(t, startPoint, bezierControlPoint, endPoint);
        }

        lineRender.positionCount = _path.Length;
        lineRender.SetPositions(_path);
        MovePoint.transform.position = endPoint;
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            transform.position = endPoint;
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
