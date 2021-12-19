using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interpolator
{
	private float timeInit;
	private float timeDiff;
	
	private float posInit;
	private float posDiff;
	
	private bool active;
	
	public Interpolator(float a, float b, float t)
	{
		posInit = a;
		posDiff = b - a;
		
		timeInit = Time.time;
		timeDiff = t;
		
		active = true;
	}
	
	private float interpolate()
	{
		float t;
		
		if (isActive)
		{
			t = (Time.time - timeInit) / timeDiff;
		}
		else
		{
			t = 1.0f;
		}
		
		return posInit + posDiff * t;
	}
	
	public float currentPosition
	{
		get
		{
			return interpolate();
		}
	}
	
	public bool isActive
	{
		get
		{
			if (Time.time >= timeInit + timeDiff)
			{
				active = false;
			}
			
			return active;
		}
	}
}
