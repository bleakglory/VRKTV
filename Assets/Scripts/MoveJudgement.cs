using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveJudgement : MonoBehaviour
{
    public bool canMove;
    public LineRenderer line;
    public ParticleSystem EFT;
    public GameObject player;
    void Start()
    {
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        //RayCastJudge();
    }

    void RayCastJudge()
    {
        Ray ray = new Ray(player.transform.position, player.transform.forward * 5);
        RaycastHit hitInfo;                                
        if (Physics.Raycast(ray, out hitInfo, 5))         
        {
            canMove = false;
            GetComponent<ParticleSystem>().startColor = Color.red;
            line.SetColors(Color.red, Color.red);
        }
        else
        {
            canMove = true;
            GetComponent<ParticleSystem>().startColor = Color.green;
            line.SetColors(Color.green, Color.green);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Background"))
        {
            canMove = false;
            GetComponent<ParticleSystem>().startColor = Color.red;
            line.SetColors(Color.red, Color.red);
            Debug.Log("In :"+other.tag);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Background"))
        {
            canMove = true;
            GetComponent<ParticleSystem>().startColor = Color.green;
            line.SetColors(Color.green, Color.green);
            Debug.Log("Out :" + other.tag);
        }
    }
}
