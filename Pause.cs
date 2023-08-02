using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public GameObject PausePanel;
    public static bool isGamePaused = true;
    private void Awake() {
        Time.timeScale = 0;
        isGamePaused = true;
    }
    private void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(isGamePaused){
                ContinueGame();
            } else {
                PauseGame();
            }
        }
    }
    public void PauseGame(){
        PausePanel.SetActive(true);
        Time.timeScale = 0;
        isGamePaused = true;
    }
    public void ContinueGame(){
        PausePanel.SetActive(false);
        Time.timeScale = 1;
        isGamePaused = false;
    }
}
