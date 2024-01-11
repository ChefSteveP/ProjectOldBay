using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadScene : MonoBehaviour
{
    public Animator animator;

    public static int levelToLoad;
    // Start is called before the first frame update
    public static void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    // public void LoadLevel(int levelIndex)
    // {
    //     //call FadeToLevel with levelIndex
    //     FadeToLevel(levelIndex);
    // }
    
    public void FadeToLevel(int levelIndex) {
        Time.timeScale = 1f;
        levelToLoad = levelIndex;
        animator.SetTrigger("FadeOut");
    }    

    public void OnFadeComplete() {
        SceneManager.LoadScene(levelToLoad);
    }
}
