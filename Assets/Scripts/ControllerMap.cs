using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class ControllerMap
{
    public Button AttackButton { get; set; }
				  
    public float LeftStickX { get { return controllerState.ThumbSticks.Left.X; } }
    public float LeftStickY { get { return controllerState.ThumbSticks.Left.Y; } }
    public float RightStickX { get { return controllerState.ThumbSticks.Right.X; } }
    public float RightStickY { get { return controllerState.ThumbSticks.Right.Y; } }

    private GamePadState controllerState;
    private GamePadState prevState;
    private PlayerIndex player;

    public enum Button { A, B, X, Y, dLeft, dRight, dUp, dDown, L1, L2, R1, R2 }

    /// <summary>
    /// A delegate for all controller button press functions.
    /// </summary>
    /// <param name="timePressed">The Time.time recorded when the button press was triggered.</param>
    /// <param name="buttonPressed">The actual button pressed on the xinput controller.</param>
    public delegate void ButtonPressed(float timePressed, Button buttonPressed);

    /// <summary>
    /// Triggers once when button mapped to the gods attack move is pressed.
    /// </summary>
    public event ButtonPressed attackPressed;

    /// <summary>
    /// Default controls.
    /// </summary>
    /// <param name="playerNumber">Physical Controller Number.</param>
    public ControllerMap(PlayerIndex playerNumber)
    {
        this.player = playerNumber;
        this.AttackButton = Button.A;

        Init();
    }

    /// <summary>
    /// Constuct with given mapping.
    /// </summary>
    /// <param name="playerNumber">Physical Controller Number.</param>
    /// <param name="AttackButton">Button that will trigger god attacks.</param>
    public ControllerMap(PlayerIndex playerNumber, Button AttackButton)
    {
        this.player = playerNumber;
        this.AttackButton = AttackButton;

        Init();
    }

    /// <summary>
    /// Constuct a by value copy of an existing ControllerMap.
    /// </summary>
    /// <param name="existingMap">Existing ControllerMap.</param>
    /// <param name="playerNumber">Physical Controller Number for the new map.</param>
	public ControllerMap(ControllerMap existingMap, PlayerIndex playerNumber)
    {
		this.player = playerNumber;
        this.AttackButton = existingMap.AttackButton;

        Init();
    }

    private void Init()
    {
        prevState = controllerState;
    }

    public void Update()
    {
        //Poll controller
        controllerState = GamePad.GetState(player);

        //Attack Pressed
        if (checkPressed(AttackButton) && !checkPressed(AttackButton, prevState))
		{
            if (attackPressed != null)
			{
                attackPressed(Time.time, AttackButton);
			}
		}

        //Debug.Log("Left Stick: X=" + controllerState.ThumbSticks.Left.X + " Y=" + controllerState.ThumbSticks.Left.Y);

        //Last: Set previous controller state to current state
        prevState = controllerState;
    }

    public bool checkPressed(Button toCheck)
    {
        return checkPressed(toCheck, controllerState);
    }

    private static bool checkPressed(Button toCheck, GamePadState controller)
    {
        bool rtn = false;

        switch(toCheck)
        {
            case Button.A:
                if (controller.Buttons.A == ButtonState.Pressed)
				{
					rtn = true;
				}
				
                break;
				
            case Button.B:
                if (controller.Buttons.B == ButtonState.Pressed)
				{
					rtn = true;
				}
				
                break;
				
            case Button.X:
                if (controller.Buttons.X == ButtonState.Pressed)
				{
					rtn = true;
				}
				
                break;
				
            case Button.Y:
                if (controller.Buttons.Y == ButtonState.Pressed)
				{
					rtn = true;
				}
				
                break;
				
            case Button.dLeft:
                if (controller.DPad.Left == ButtonState.Pressed)
				{
					rtn = true;
				}
                
				break;
				
            case Button.dRight:
                if (controller.DPad.Right == ButtonState.Pressed)
				{
					rtn = true;
				}
                
				break;
				
            case Button.dUp:
                if (controller.DPad.Up == ButtonState.Pressed)
				{
					rtn = true;
				}
                
				break;
				
            case Button.dDown:
                if (controller.DPad.Down == ButtonState.Pressed)
				{
					rtn = true;
				}
                
				break;
				
            case Button.L1:
                if (controller.Buttons.LeftShoulder == ButtonState.Pressed)
				{
					rtn = true;
				}
                
				break;

            case Button.R1:
                if (controller.Buttons.RightShoulder == ButtonState.Pressed)
				{
					rtn = true;
				}
				
				break;
				
            case Button.L2:
                if (controller.Triggers.Left > 0.5)
				{
					rtn = true;
				}
                
				break;
				
            case Button.R2:
                if (controller.Triggers.Right > 0.5)
				{
					rtn = true;
				}
                
				break;
        }
		
        return rtn;
    }
}
