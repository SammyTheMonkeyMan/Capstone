using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance;

    public GameObject pauseScreen;
    public bool paused;

    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.ActionMap.Enable();
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
}
