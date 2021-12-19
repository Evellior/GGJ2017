using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxControl : MonoBehaviour
{
	public float rotationSpeed = 0.5f;
	
	private float theta;

	// Use this for initialization
	void Start()
	{
		theta = Random.value * 2.0f * Mathf.PI;
	}
	
	// Update is called once per frame
	void Update()
	{
		theta += rotationSpeed * Time.deltaTime;
		
		theta = Mathf.Repeat(theta, 2.0f * Mathf.PI);
		
		GetComponent<Transform>().rotation = Quaternion.Euler(-90.0f, Mathf.Rad2Deg * theta, 0.0f);
	}
}
