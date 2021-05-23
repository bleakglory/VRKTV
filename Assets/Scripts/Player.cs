using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public GameObject player;
    public GameObject Instrument;

    public Transform leftHand;
    public Transform rightHand;

    public Transform handRotation;
    public GameObject MenuCanvas;
    public GameObject ReturnCanvas;
    public GameObject TipCanvas;
    public GameObject LrcCanvas;
    public ParticleSystem MovePointEFX;
    public int height;
    public int resPoint;
    public LineRenderer PlayerMoveLineRender;

    public LineRenderer UILineRender;
    public GameObject input;
    public GameManager GM;

    //private Transform forward;

    private bool _openMove;
    private bool _isCatchInstru;
    private GameObject _instrument;
    float Rotate;
    private Vector3[] _path;

    void Start()
    {
        _isCatchInstru = false;
    }


    private void Update()
    {
        if (!(MenuCanvas.activeInHierarchy || ReturnCanvas.activeInHierarchy))
        {
            if (OVRInput.GetDown(OVRInput.Button.One))
            {
                _openMove = !_openMove;
            }

            if (_openMove)
            {
                openPlayerMove();

                PlayerMoveLineRender.enabled = true;
                MovePointEFX.Play();
            }
            else
            {
                PlayerMoveLineRender.enabled = false;
                MovePointEFX.Simulate(0);
            }
            UILineRender.enabled = false;
            input.SetActive(false);

            if (rightHand.GetComponent<Instrument>().HasPlayed && !rightHand.GetComponent<AudioSource>().isPlaying)
            {
                Instrument.SetActive(false);
                LrcCanvas.SetActive(true);
                TipCanvas.SetActive(true);
                //GM.isStart = true;
            }
            else
            {
                Instrument.SetActive(true);
            }
        }

       else 
        {
            if (!UILineRender.enabled)
                UILineRender.enabled = true;
            if (!input.activeInHierarchy)
                input.SetActive(true);

            LrcCanvas.SetActive(false);
            TipCanvas.SetActive(false);
        }

    }

    //LateUpdate: let camera move with canvas
    void LateUpdate()
    {
        MenuCanvas.transform.position = new Vector3(transform.TransformPoint(Vector3.forward * 2).x, 
            transform.TransformPoint(Vector3.forward * 2).y + 40, 
            transform.TransformPoint(Vector3.forward * 2).z);

        ReturnCanvas.transform.position = new Vector3(transform.TransformPoint(Vector3.forward * 2).x, 
            transform.TransformPoint(Vector3.forward * 2).y + 40, 
            transform.TransformPoint(Vector3.forward * 2).z);

        TipCanvas.transform.position = new Vector3(transform.TransformPoint(Vector3.forward * 2).x,
    transform.TransformPoint(Vector3.forward * 2).y + 40,
    transform.TransformPoint(Vector3.forward * 2).z);

        LrcCanvas.transform.position = new Vector3(transform.TransformPoint(Vector3.forward * 2).x,
    transform.TransformPoint(Vector3.forward * 2).y + 40,
    transform.TransformPoint(Vector3.forward * 2).z);

        if (_isCatchInstru && !rightHand.GetComponent<Instrument>().HasPlayed)
        {
            catchInstrument(_instrument);
        }

    }

    public static Vector3 GetBezierPoint(float t, Vector3 start, Vector3 center, Vector3 end)
    {
        return (1 - t) * (1 - t) * start + 2 * t * (1 - t) * center + t * t * end;
    }

    void openPlayerMove()
    {
        var startPoint = rightHand.position;
        //var endPoint = new Vector3(2*transform.forward.normalized.x, transform.position.y - 0.5f, 2*transform.forward.normalized.z);
        var moveDistance = 4 + 6 * (handRotation.localRotation.x) / 45;
        if (moveDistance <= 1)
        {
            moveDistance = 1;
        }
        var endPoint = rightHand.TransformPoint(Vector3.down * moveDistance);
        endPoint = new Vector3(endPoint.x, transform.position.y - 0.4f, endPoint.z);
        var bezierControlPoint = (startPoint + endPoint) * 0.5f + (Vector3.up * height);

        _path = new Vector3[resPoint];
        for (int i = 0; i < resPoint; i++)
        {
            var t = (i + 1) / (float)resPoint;
            _path[i] = GetBezierPoint(t, startPoint, bezierControlPoint, endPoint);
        }

        PlayerMoveLineRender.positionCount = _path.Length;
        PlayerMoveLineRender.SetPositions(_path);
        MovePointEFX.transform.position = endPoint;
        if (OVRInput.GetDown(OVRInput.Button.Up) && MovePointEFX.GetComponent<MoveJudgement>().canMove)
        {
            Debug.Log("Move To the Point !!!!!");
            transform.position = new Vector3(endPoint.x, transform.position.y, endPoint.z);
        }

        if (OVRInput.GetDown(OVRInput.Button.Down) && MovePointEFX.GetComponent<MoveJudgement>().canCatch)
        {
           _instrument = MovePointEFX.GetComponent<MoveJudgement>().Instrument;
            _isCatchInstru = true;
        }
        // 45 - -45
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

    void catchInstrument(GameObject obj)
    {
        _instrument.transform.position =
        new Vector3(leftHand.transform.position.x, leftHand.transform.position.y,
        leftHand.transform.TransformPoint(Vector3.forward * 0.1f).z);
        _instrument.transform.forward = transform.forward;
        _instrument.transform.rotation = Quaternion.Euler(leftHand.transform.rotation.x, leftHand.transform.rotation.y, leftHand.transform.rotation.z + 55);
    }

}
