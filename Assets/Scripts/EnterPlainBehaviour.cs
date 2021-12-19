using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterPlainBehaviour : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        Rigidbody body = GetComponent<Rigidbody>();
        body.AddForce(0.0f, 10f, 0.0f);
        body.angularVelocity = new Vector3(body.angularVelocity.x / 2, body.angularVelocity.y, body.angularVelocity.z / 2);
    }

    void OnTriggerExit(Collider other)
    {
    }
}
