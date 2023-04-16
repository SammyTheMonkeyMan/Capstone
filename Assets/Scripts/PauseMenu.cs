using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance;

    public GameObject pauseScreen;
    public bool paused;

    public GameObject mainPauseScreen;
    private bool mainPause;

    public GameObject settingsScreen;
    private bool settings;

    private PlayerInputActions playerInputActions;

    //public Slider brightnessSlider, musicSlider, SFXSlider;
    [SerializeField] public AudioMixer mixer;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.ActionMap.Enable();
        mainPause = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInputActions.ActionMap.Pause.WasPressedThisFrame() //Input.GetKeyDown(KeyCode.Escape))
        ){
            paused = !paused;
        }

        pauseScreen.SetActive(paused);

        if (paused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }

        mainPauseScreen.SetActive(mainPause);
        settingsScreen.SetActive(settings);
    }

    public void Resume()
    {
        paused = !paused;
    }

    public void LevelSelect()
    {
        SceneManager.LoadScene("Level_Select");
        Time.timeScale = 1;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Main_Menu");
        Time.timeScale = 1;
    }

    public void Settings()
    {
        settings = true;
        mainPause = false;
    }

    public void MainPause()
    {
        mainPause = true;
        settings = false;
    }

    public void ChangeBrightness(float sliderValue)
    {
        Screen.brightness = sliderValue; //brightnessSlider.value;
    }

    public void ChangeMusicVolume(float sliderValue)
    {
        mixer.SetFloat("Music", Mathf.Log10(sliderValue) * 20);
    }

    public void ChangeSFXVolume(float sliderValue)
    {
        mixer.SetFloat("SFX", Mathf.Log10(sliderValue) * 20);
    }
}
