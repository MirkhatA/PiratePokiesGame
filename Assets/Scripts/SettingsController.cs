using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    public Image soundImage;
    public Image musicImage;
    public Image vibrationImage;
    
    public Sprite on;
    public Sprite off;
    
    private const string SOUND_PREF = "Sound";
    private const string MUSIC_PREF = "Music";
    private const string VIBRATION_PREF = "Vibration";
    
    private void Start()
    {
        // Инициализация изображений в зависимости от сохраненных настроек
        UpdateSoundIcon();
        UpdateMusicIcon();
        UpdateVibrationIcon();
    }
    
    public void ToggleSound()
    {
        bool isSoundOn = PlayerPrefs.GetInt(SOUND_PREF, 1) == 1;
        
        isSoundOn = !isSoundOn;
        
        PlayerPrefs.SetInt(SOUND_PREF, isSoundOn ? 1 : 0);
        
        soundImage.sprite = isSoundOn ? on : off;

        Debug.Log("Sound " + (isSoundOn ? "On" : "Off"));
    }

    public void ToggleMusic()
    {
        bool isMusicOn = PlayerPrefs.GetInt(MUSIC_PREF, 1) == 1;
        
        isMusicOn = !isMusicOn;
        
        PlayerPrefs.SetInt(MUSIC_PREF, isMusicOn ? 1 : 0);
        
        musicImage.sprite = isMusicOn ? on : off;

        Debug.Log("Music " + (isMusicOn ? "On" : "Off"));
    }

    public void ToggleVibration()
    {
        bool isVibrationOn = PlayerPrefs.GetInt(VIBRATION_PREF, 1) == 1;
        
        isVibrationOn = !isVibrationOn;
        
        PlayerPrefs.SetInt(VIBRATION_PREF, isVibrationOn ? 1 : 0);
        
        vibrationImage.sprite = isVibrationOn ? on : off;

        Debug.Log("Vibration " + (isVibrationOn ? "On" : "Off"));
    }

    private void UpdateSoundIcon()
    {
        bool isSoundOn = PlayerPrefs.GetInt(SOUND_PREF, 1) == 1;
        soundImage.sprite = isSoundOn ? on : off;
    }

    private void UpdateMusicIcon()
    {
        bool isMusicOn = PlayerPrefs.GetInt(MUSIC_PREF, 1) == 1;
        musicImage.sprite = isMusicOn ? on : off;
    }

    private void UpdateVibrationIcon()
    {
        bool isVibrationOn = PlayerPrefs.GetInt(VIBRATION_PREF, 1) == 1;
        vibrationImage.sprite = isVibrationOn ? on : off;
    }
}

