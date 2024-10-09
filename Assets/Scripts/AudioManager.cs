using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    
    public AudioSource audioSource;
    
    private const string MUSIC_PREF = "Music";
    
    private void Awake()
    {
        Application.targetFrameRate = 120;
        
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Update()
    {
        bool isMusicOn = PlayerPrefs.GetInt(MUSIC_PREF, 1) == 1;
        if (isMusicOn)
        {
            audioSource.volume = 0.5f;
        }
        else
        {
            audioSource.volume = 0f;
        }
    }
}
