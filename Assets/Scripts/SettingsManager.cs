using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

public class SettingsManager : MonoBehaviour
{
    public static Volume music;
    public static Volume sfx;

    public AudioMixer musicAudioMixer;
    public AudioMixer sfxAudioMixer;

    public UnityEvent onChange;

    void Awake()
    {
        if (PlayerPrefs.HasKey("music"))
        {
            int musicLevel = PlayerPrefs.GetInt("music");
            music = new Volume(musicLevel);
        } else {
            music = new Volume();
        }

        if (PlayerPrefs.HasKey("sfx"))
        {
            int sfxLevel = PlayerPrefs.GetInt("sfx");
            sfx = new Volume(sfxLevel);
        } else {
            sfx = new Volume();
        }
    }

    public void ToggleMusic()
    {
        SettingsManager.music.Increment();
        PlayerPrefs.SetInt("music", SettingsManager.music.ToInt());
        PlayerPrefs.Save();
        onChange.Invoke();
    }

    public void ToggleSFX()
    {
        SettingsManager.sfx.Increment();
        PlayerPrefs.SetInt("sfx", SettingsManager.sfx.ToInt());
        PlayerPrefs.Save();
        onChange.Invoke();
    }

    void Update()
    {
        if (musicAudioMixer != null)
        {
            musicAudioMixer.SetFloat("volume", music.ToFloat());
        }
        if (sfxAudioMixer != null)
        {
            sfxAudioMixer.SetFloat("volume", sfx.ToFloat());
        }
    }
}
