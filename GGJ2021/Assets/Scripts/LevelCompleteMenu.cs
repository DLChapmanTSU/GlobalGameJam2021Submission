using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompleteMenu : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] SceneFade sceneFade;
    [SerializeField] AudioSource buttonSource;

    //Transitions to the next level when the button is pressed
    public void OnNextPress()
    {
        buttonSource.Play();

        if (SceneManager.GetActiveScene().buildIndex == 8)
        {
            sceneFade.SetIndex(0);
            Time.timeScale = 1.0f;
            anim.SetTrigger("Fade");
        }
        else
        {
            sceneFade.SetIndex(SceneManager.GetActiveScene().buildIndex + 1);
            Time.timeScale = 1.0f;
            anim.SetTrigger("Fade");
        }
    }

    //Returns to the main menu when the quit button is pressed
    public void OnQuitPress()
    {
        buttonSource.Play();
        sceneFade.SetIndex(0);
        Time.timeScale = 1.0f;
        anim.SetTrigger("Fade");
    }
}
