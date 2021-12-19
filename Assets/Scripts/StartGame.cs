using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed)
        {
            startMainGame();
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            startMainGame();
        }
    }

    void startMainGame()
    {
        SceneManager.LoadScene("WaveGame");
    }
}
