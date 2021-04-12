using UnityEngine;
using UnityEngine.UI;

public class SettingToggleDisplay : MonoBehaviour
{
    public Text musicText;
    public Text sfxText;

    private SettingsManager settingsManager;

    void Awake()
    {
        settingsManager = Object.FindObjectOfType<SettingsManager>();
        settingsManager.onChange.AddListener(OnSettingsUpdate);
        OnSettingsUpdate();
    }

    void OnSettingsUpdate()
    {
        musicText.text = "Music " + SettingsManager.music.ToString();
        sfxText.text = "Sound " + SettingsManager.sfx.ToString();
    }
}
