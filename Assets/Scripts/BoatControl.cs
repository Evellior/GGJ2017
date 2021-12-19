using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class BoatControl : MonoBehaviour
{
    public PlayerIndex PlayerNumber;
	public GameObject canvasController;

    private Rigidbody boatBody;
    private Transform boatState;
	
	private float theta = 0;
	private float gravity = -9.8f;

    ControllerMap controller;

    // Use this for initialization
	void Start()
	{
        boatBody = GetComponent<Rigidbody>();
        boatState = GetComponent<Transform>();

        controller = new ControllerMap(PlayerNumber);
	}

    private void tilt(float stickX, float stickY)
    {
        float tiltScale = 1f;
        //body.MoveRotation(Quaternion.Euler(new Vector3(controller.RightStickX * tiltScale, tiltLock.y, controller.RightStickY * tiltScale)));
        boatState.up = new Vector3(0f * tiltScale, 1f, 0f * tiltScale);
    }

    private void move(float stickX, float stickY)
    {
        float forceMultiplier = 80f;
		float angleMultiplier = 10f;
		
		theta += stickX * angleMultiplier;
		
		Vector3 rot = boatState.rotation.eulerAngles;

		rot.y = theta;
		
		boatState.rotation = Quaternion.Euler(rot);

        boatBody.velocity = new Vector3(stickY * Mathf.Sin(Mathf.Deg2Rad * theta) * forceMultiplier, gravity, stickY * Mathf.Cos(Mathf.Deg2Rad * theta) * forceMultiplier);
		
		boatBody.velocity = Vector3.ClampMagnitude(boatBody.velocity, 60.0f);
		
		gravity -= 9.8f;
		
		if (stickX >= 0.1f || stickX <= -0.1f || stickY >= 0.1f || stickY <= -0.1f)
		{
			GetComponent<Animator>().speed = 1.0f;
		}
		else
		{
			GetComponent<Animator>().speed = 0.0f;
		}
    }

    // Update is called once per frame
    void Update()
	{
        //Poll the controller first
        controller.Update();
        
        tilt(controller.RightStickX, controller.RightStickY);
        move(controller.LeftStickX, controller.LeftStickY);

		if (!Mathf.Approximately(boatBody.angularVelocity.magnitude, 0.0f))
		{
			boatBody.rotation.SetLookRotation(boatBody.angularVelocity);
		}
		else
		{
			boatBody.rotation.SetLookRotation(new Vector3(0.0f, 0.0f, 1.0f));
		}
    
		if (boatState.position.y <= 0.0f)
		{
			Vector3 pos = boatState.position;
			
			pos.y = 0.0f;
			
			boatState.position = pos;
		}
	
        //Extra Gravity
        boatBody.AddForce(0f, -9.8f * 2, 0f);
	}
	
	void OnCollisionEnter(Collision collision)
	{
		foreach (ContactPoint contact in collision.contacts)
		{
			if (contact.otherCollider.gameObject.name == "Plane")
			{
				gravity = 0.0f;
				
				break;
			}
            if (contact.otherCollider.gameObject.transform.parent.gameObject.name == "Border Rocks")
            {
                if (PlayerNumber == PlayerIndex.One)
                {
                    canvasController.GetComponent<EndSlateBehaviour>().EnablePosedion();
                }
                else if (PlayerNumber == PlayerIndex.Two)
                {
                    canvasController.GetComponent<EndSlateBehaviour>().EnableNeptune();
                }

                break;
            }
        }
	}
}
