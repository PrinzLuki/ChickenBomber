using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : Singleton<SoundManager>
{
    [Header("MainSoundSettings")]
    [SerializeField] protected AudioMixerGroup mainAudioGroup;
    [SerializeField] protected AudioMixerGroup sfxAudioGroup;

    public void SetMasterAudioGroupVolume(float volume)
    {
        LinearToDecibel(ref volume);
        mainAudioGroup.audioMixer.SetFloat("MasterVolume", volume);
    }

    public void SetSfxAudioGroupVolume(float volume)
    {
        LinearToDecibel(ref volume);
        sfxAudioGroup.audioMixer.SetFloat("SfxVolume",volume);
    }

    protected void LinearToDecibel(ref float linearValue)
    {
        if (linearValue != 0)
            linearValue = 20.0f * Mathf.Log10(linearValue);
        else
            linearValue = -80.0f;
    }
}
