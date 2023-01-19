using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    //public Image heart1, heart2, heart3;
    //public Image[] hearts;
    //public Sprite heartEmpty, heartFull, heartHalf;

    //public Text gemText;

    public Image fadeScreen; 
    public float fadeSpeed;
    private bool shouldFadeIn, shouldFadeOut;

    //public GameObject levelCompleteText;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //UpdateGemDisplay();
        FadeIn();
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldFadeIn)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 0, fadeSpeed * Time.deltaTime));
            if (fadeScreen.color.a == 0)
            {
                shouldFadeIn = false;
            }
        }
        else if (shouldFadeOut)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 1, fadeSpeed * Time.deltaTime));
            if (fadeScreen.color.a == 1)
            {
                shouldFadeOut = false;
            }
        }
    }

    public void FadeOut()
    {
        shouldFadeOut = true;
    }

    public void FadeIn()
    {
        shouldFadeIn = true;
    }

    /*public void UpdateHealthDisplay()
    {
        for (int i = 0; i < 3; i++)
        {
            if (i < PlayerHealthController.instance.currentHealth / 2 - 0.5)
            {
                hearts[i].sprite = heartFull;
            }
            else if (i < (float)PlayerHealthController.instance.currentHealth / 2)
            {
                hearts[i].sprite = heartHalf;
            }
            else
            {
                hearts[i].sprite = heartEmpty;
            }
        }

        for (int i = 0; i < 3; i++)
        {
            if (i < PlayerHealthController.instance.currentHealth)
            {
                hearts[i].sprite = heartFull;
            }
            else
            {
                hearts[i].sprite = heartEmpty;
            }
        }

        switch (PlayerHealthController.instance.currentHealth)
        {
            case 6:
                heart1.sprite = heartFull;
                heart2.sprite = heartFull;
                heart3.sprite = heartFull;

                break;

            case 5:
                heart1.sprite = heartFull;
                heart2.sprite = heartFull;
                heart3.sprite = heartHalf;

                break;

            case 4:
                heart1.sprite = heartFull;
                heart2.sprite = heartFull;
                heart3.sprite = heartEmpty;

                break;

            case 3:
                heart1.sprite = heartFull;
                heart2.sprite = heartHalf;
                heart3.sprite = heartEmpty;

                break;

            case 2:
                heart1.sprite = heartFull;
                heart2.sprite = heartEmpty;
                heart3.sprite = heartEmpty;

                break;

            case 1:
                heart1.sprite = heartHalf;
                heart2.sprite = heartEmpty;
                heart3.sprite = heartEmpty;

                break;

            case 0:
                heart1.sprite = heartEmpty;
                heart2.sprite = heartEmpty;
                heart3.sprite = heartEmpty;

                break;

            default:
                heart1.sprite = heartEmpty;
                heart2.sprite = heartEmpty;
                heart3.sprite = heartEmpty;

                break;
        }
    }*/

    /*public void UpdateGemDisplay()
    {
        gemText.text = LevelManager.instance.gemsCollected.ToString();
    }*/
}
