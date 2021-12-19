using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class FistControl : MonoBehaviour
{
	public PlayerIndex playerNumber;
	
	public float topHeight;
	public float bottomHeight;
	public float punchTime;
	public float retractTime;

	public float angularFrequency = 1;
	
	public GameObject ocean;
	
	private float theta = 0;
	private float radius = 90;
	
	private Animator animator;
	private ControllerMap controller;
	
	private Interpolator height;
	
	private enum FistState
	{
		Idle,
		Punching,
		Retracting
	};
	
	private FistState currentState;
	
	// Use this for initialization
	void Start()
	{
		currentState = FistState.Idle;
		
		theta = Mathf.PI * (float) playerNumber;
		
		animator = GetComponent<Animator>();
		controller = new ControllerMap(playerNumber);
		
		controller.attackPressed += attackPressed;
	}
	
	// Update is called once per frame
	void Update()
	{
		float stepSize = 2.0f * Mathf.PI * angularFrequency * Time.deltaTime;

		controller.Update();

		updateState();
		
		if (currentState == FistState.Idle)
		{
			if (controller.checkPressed(ControllerMap.Button.L1))
			{
				theta += stepSize;
			}
			else if (controller.checkPressed(ControllerMap.Button.R1))
			{
				theta -= stepSize;
			}
		}
		
		theta = Mathf.Repeat(theta, 2.0f * Mathf.PI);
		
		Vector3 newPosition = new Vector3(radius * Mathf.Cos(theta), 0, radius * Mathf.Sin(theta));
		Quaternion newRotation = Quaternion.Euler(0, Mathf.Rad2Deg * Mathf.Atan2(-newPosition.z, newPosition.x), 0);
		
		GetComponent<Transform>().position = newPosition;
		GetComponent<Transform>().rotation = newRotation;

		
		if (height != null)
		{
			newPosition.y = height.currentPosition;
		}
		else
		{
			newPosition.y = topHeight;
		}
		
		GetComponent<Transform>().position = newPosition;
	}
	
	void attackPressed(float timePressed, ControllerMap.Button buttonPressed)
	{
		if (currentState == FistState.Idle)
		{
			animator.Play("FistBall");
		
			height = new Interpolator(GetComponent<Transform>().position.y, bottomHeight, punchTime);
			
			currentState = FistState.Punching;
		}
	}
	
	void updateState()
	{
		if (height != null && !height.isActive)
		{
			switch (currentState)
			{
				case FistState.Idle:
				{
					// Do nothing, we're idle.
					break;
				}
				
				case FistState.Punching:
				{
					// Set to rectracting
					currentState = FistState.Retracting;
					
					animator.Play("Unball");
					
					height = new Interpolator(GetComponent<Transform>().position.y, topHeight, retractTime);
					
					// Make a splishy-splash				
					WaveField water = ocean.GetComponent<WaveField>();
					
					Vector3 worldPosition  = GetComponent<Transform>().position;
					Vector2 position  = new Vector2(worldPosition.x, -worldPosition.z);

					Vector2 target = 5.0f * Random.insideUnitCircle;
					Vector2 waveVelocity = (target - position).normalized;
					
					water.addWave(position, waveVelocity, 17.5f + 2.5f * Random.value);
					
					break;
				}
				
				case FistState.Retracting:
				{
					// Set to idle
					currentState = FistState.Idle;
					
					break;
				}
			}
		}
	}
}
