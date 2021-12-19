using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinkingBehaviour : MonoBehaviour
{
    private float timeSinking;

    // Use this for initialization
    void Start()
    {
        timeSinking = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(timeSinking>15)
        {
            //You Lose!!1!
        }
    }

    void OnTriggerEnter(Collider other)
    {
        timeSinking += Time.deltaTime;
        //Debug.Log("Been sinking for " + timeSinking + " seconds!");
    }

    void OnTriggerExit(Collider other)
    {

    }
}