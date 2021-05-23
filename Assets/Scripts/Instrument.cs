using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instrument : MonoBehaviour
{
    public bool HasPlayed;
    public GameObject Guitar;
    public Transform OriPos;
    public GameObject MicrophoneModel;

    [SerializeField]
    private bool _startCountDown;
    [SerializeField]
    private float _timer;

    private Vector3 _handStartPoint;

    void Start()
    {
        _timer = -0.1f;
        _startCountDown = false;
        HasPlayed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_startCountDown)
        {
            _timer += Time.deltaTime;
            if (_timer > 2)
            {
                _startCountDown = false;
                _timer = -0.1f;
            }

            if (_handStartPoint.y - transform.position.y > 0.01f)
            {
                if (!GetComponent<AudioSource>().isPlaying)
                {
                    GetComponent<AudioSource>().Play();
                    HasPlayed = true;
                    MicrophoneModel.SetActive(true);
                    GetComponent<SkinnedMeshRenderer>().enabled = false;
                    GetComponent<BoxCollider>().enabled = false;
                }
            }
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Instrument"))
        {
            _handStartPoint = transform.position;
            _startCountDown = true;
        }
    }

    //private void OnTriggerExit(Collider other)
    //{

    //    if (other.CompareTag("Instrument"))
    //    {
    //        if (_startCountDown)
    //        {
    //            if (!GetComponent<AudioSource>().isPlaying)
    //            {
    //                GetComponent<AudioSource>().Play();
    //                Debug.Log("Play Music !!!");
    //            }
    //        }
    //    }
    //}
}
