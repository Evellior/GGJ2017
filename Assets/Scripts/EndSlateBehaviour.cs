using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class EndSlateBehaviour : MonoBehaviour {

    public Canvas PoseidonCanvas;
    public Canvas NeptuneCanvas;

    void Start() {
        PoseidonCanvas.enabled = false;
        NeptuneCanvas.enabled = false;
    }
    public void EnablePosedion() {
        PoseidonCanvas.enabled = true;
        NeptuneCanvas.enabled = false;
    }

    public void EnableNeptune() {
        PoseidonCanvas.enabled = false;
        NeptuneCanvas.enabled = true;
    }

    public void LoadMainMenu() {
        SceneManager.LoadScene(0);
    }

    public void Restart() {
        SceneManager.LoadScene(1);
    }

    public void AlsoQuitGame() {
        Application.Quit();
    }
}
