using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource[] soundEffects;
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //string e = soundEffects[1].ToString();
       // Debug.Log(e);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlaySFX(string sfxToPlay)
    {
        for (int i = 0; i < soundEffects.Length; i++)
        {
            if (soundEffects[i].ToString() == sfxToPlay + " (UnityEngine.AudioSource)")
            {
                soundEffects[i].Stop();
                soundEffects[i].pitch = Random.Range(0.9f, 1.1f);
                soundEffects[i].Play();
            }
        }
    }
}
