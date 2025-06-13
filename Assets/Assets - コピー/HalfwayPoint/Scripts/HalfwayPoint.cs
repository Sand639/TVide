using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HalfwayPoint : MonoBehaviour
{
    public GameObject respornPoint;
    public Transform newRespornPoint;
    private bool usePoint = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //usePoint = false;
    }

    // Update is called once per frame
    void Update()
    {
        OnTriggerEnter(null);
    }

    protected void OnTriggerEnter(Collider collision)
    {

        if (collision != null)
        {
            HitPlayer(collision);
            
        }
    }


    protected void HitPlayer(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!usePoint)
            {
                respornPoint.transform.position = newRespornPoint.transform.position;
                usePoint = true;
            }
        }
    }
}
