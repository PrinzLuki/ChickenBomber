using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] Slider masterVolumeSlider;
    [SerializeField] Toggle toggleMusic;
    [SerializeField] Toggle toggleSfx;

    private void Start()
    {
        CheckPlayerSettings();
    }

    public void MasterVolumeChangeValue()
    {
        SaveData.PlayerSettings.masterVolume = masterVolumeSlider.value;
        SavePlayerSettings();
    }

    #region Saving
    private void CheckPlayerSettings()
    {
        LoadPlayerSettings();
        if (!SaveData.PlayerSettings.volumeEdited)
        {
            CreateNewPlayerSettings();
            SetUpUiStuff();
        }
        else
        {
            SetUpUiStuff();
        }
    }

    void SetUpUiStuff()
    {
        masterVolumeSlider.value = SaveData.PlayerSettings.masterVolume;
        toggleMusic.isOn = SaveData.PlayerSettings.toggleMusic;
        toggleSfx.isOn = SaveData.PlayerSettings.toggleSfx;
    }

    /// <summary>
    /// Creates new playerProfile and saves it
    /// </summary>
    private void CreateNewPlayerSettings()
    {
        SaveData.PlayerSettings.masterVolume = masterVolumeSlider.maxValue / 2;
        SaveData.PlayerSettings.toggleMusic = true;
        SaveData.PlayerSettings.toggleSfx = true;
        SavePlayerSettings();
    }

    private void SavePlayerSettings()
    {
        SaveData.PlayerSettings.volumeEdited = true;
        SerializationManager.Save("playerSettings", SaveData.PlayerSettings);
    }

    private void LoadPlayerSettings()
    {
        SaveData.PlayerSettings = (PlayerSettings)SerializationManager.Load(Application.persistentDataPath + "/saves/playerSettings.cutebirds");
    }
    #endregion

    public void ToggleMusic()
    {
        SaveData.PlayerSettings.toggleMusic = toggleMusic.isOn;
        SavePlayerSettings();
    }

    public void ToggleSFX()
    {
        SaveData.PlayerSettings.toggleSfx = toggleSfx.isOn;

        SavePlayerSettings();
    }

}
