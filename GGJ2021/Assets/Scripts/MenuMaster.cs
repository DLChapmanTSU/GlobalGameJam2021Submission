using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MenuMaster : MonoBehaviour
{
    [SerializeField] GameObject logo;
    [SerializeField] GameObject prompt;
    [SerializeField] GameObject menu;
    [SerializeField] Animator anim;
    [SerializeField] SceneFade sceneFade;
    [SerializeField] AudioMixer mixer;
    [SerializeField] Toggle toggle;
    bool onStartScreen;

    // Start is called before the first frame update
    void Start()
    {
        //Loads the players settings from PlayerPrefs
        if (PlayerPrefs.GetInt("SoundToggle", 0) == 0)
        {
            toggle.isOn = false;
            mixer.SetFloat("MasterVolume", 0.0f);
        }
        else
        {
            toggle.isOn = true;
            mixer.SetFloat("MasterVolume", -80.0f);
        }

        logo.SetActive(true);
        prompt.SetActive(true);
        menu.SetActive(false);
        onStartScreen = true;

        Time.timeScale = 1.0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {
        //Quits the game if escape is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Quit");
            Application.Quit();
        }
        else if (Input.anyKeyDown && onStartScreen == true)
        {
            onStartScreen = false;
            logo.SetActive(false);
            prompt.SetActive(false);
            menu.SetActive(true);
        }
    }

    public void OnStart()
    {
        sceneFade.SetIndex(SceneManager.GetActiveScene().buildIndex + 1);
        anim.SetTrigger("Fade");
    }

    public void OnSoundToggle()
    {
        //Turns sound either on or off when the sound toggle box is clicked
        float volume;
        mixer.GetFloat("MasterVolume", out volume);
        if (volume == 0.0f)
        {
            mixer.SetFloat("MasterVolume", -80.0f);
            PlayerPrefs.SetInt("SoundToggle", 1);
        }
        else
        {
            mixer.SetFloat("MasterVolume", 0.0f);
            PlayerPrefs.SetInt("SoundToggle", 0);
        }
    }
}
