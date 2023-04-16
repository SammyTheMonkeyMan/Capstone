using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public float timeToRespawn;

    public int gemsCollected;

    public string sceneToLoad;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RespawnPlayer()
    {
        StartCoroutine(RewspawnCo());
    }

    private IEnumerator RewspawnCo()
    {
        PlayerController.instance.gameObject.SetActive(false);

        yield return new WaitForSeconds(timeToRespawn - 1 / UIController.instance.fadeSpeed);

        UIController.instance.FadeOut();

        yield return new WaitForSeconds(1 / UIController.instance.fadeSpeed + 0.2f);

        UIController.instance.FadeIn();

        PlayerController.instance.gameObject.SetActive(true);

        PlayerController.instance.theSR.flipX = false;

        PlayerController.instance.transform.position = CheckpointController.instance.spawnPoint;
        CameraController.instance.transform.position = new Vector3(PlayerController.instance.transform.position.x, PlayerController.instance.transform.position.y, CameraController.instance.transform.position.z);
        //PlayerController.instance.knockBackCounter = 0;

        //PlayerHealthController.instance.currentHealth = PlayerHealthController.instance.maxHealth;
        //UIController.instance.UpdateHealthDisplay();
    }

    public void EndLevel()
    {
        StartCoroutine(EndLevelCo());
    }

    private IEnumerator EndLevelCo()
    {
        UIController.instance.levelCompleteText.SetActive(true);
        PlayerController.instance.stopControl = true;
        CameraController.instance.stopFollow = true;

        yield return new WaitForSeconds(3);

        UIController.instance.FadeOut();

        yield return new WaitForSeconds(1 / UIController.instance.fadeSpeed + 0.3f);

        SceneManager.LoadScene(sceneToLoad);
    }
}
