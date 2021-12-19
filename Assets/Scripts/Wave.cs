using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave
{
	public Vector2 current;
	public Vector2 initial;
	public Vector2 velocity;

	public float energy;
	public float age;
	
	private const float spreadAngle		= 15.0f;
	private const float energyDieOff	= 1.0f;
	private const float waveFreq		= 0.1f;
	
	public Wave(Vector2 pos, Vector2 motion, float newEnergy)
	{
		current		= pos;
		initial		= pos;
		velocity	= motion;
		energy		= newEnergy;
		age			= 0.0f;
	}
	
	public void step(float deltaTime)
	{
		current += velocity * deltaTime;
		energy -= energyDieOff * deltaTime;
		age += deltaTime;
	}
	
	public float influence(Vector2 pos)
	{
		//return 0.005f * energy * Mathf.Max(1.0f - age * 0.125f, 0.0f) * falloffProfile() * widthProfile(current, initial, pos) * angleProfile(current, initial, pos) * arcProfile(current, initial, pos);
		return 0.003f * energy * Mathf.SmoothStep(0.0f, 1.0f, Mathf.Max(1.0f - age * 0.0625f, 0.0f)) * waveProfile(current, initial, pos);
	}
	
	private float waveProfile(Vector2 head, Vector2 tail, Vector2 pos)
	{
		return falloffProfile(head, tail, pos) * widthProfile(head, tail, pos) * angleProfile(head, tail, pos) * arcProfile(head, tail, pos);
	}
	
	private float falloffProfile(Vector2 head, Vector2 tail, Vector2 pos)
	{
		float distance = Mathf.Abs(Vector2.Distance(head, pos));

		return Mathf.SmoothStep(1.0f, 0.0f, distance / 30.0f);
	}
	
	private float widthProfile(Vector2 head, Vector2 tail, Vector2 pos)
	{
		return Mathf.SmoothStep(0.0f, 1.0f, 1.0f / (1.0f + 0.125f * Mathf.Pow(Mathf.Abs(Vector2.Distance(tail, head) - Vector2.Distance(tail, pos)), 1.5f)));
	}
	
	private float angleProfile(Vector2 head, Vector2 tail, Vector2 pos)
	{
		float angle = Mathf.Abs(Vector2.Angle(pos - tail, head - tail));

		if (angle <= spreadAngle)
		{
			return 1.0f;
		}
		else
		{
			return Mathf.SmoothStep(1.0f, 0.0f, (angle - spreadAngle) / spreadAngle);
		}
	}
	
	private float arcProfile(Vector2 head, Vector2 tail, Vector2 pos)
	{
		Vector2 tailPoint = (tail + head) * 0.5f;

		return Convert.ToInt32(Mathf.Abs(Vector2.Distance(tailPoint, pos) - Vector2.Distance(tailPoint, head)) <= 30f);
	}
}
