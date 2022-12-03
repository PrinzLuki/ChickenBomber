using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
public enum AudioMixerGroupType
{
    Main,
    Sfx
}
public class InGameSoundManager : SoundManager
{
    public void SpawnSoundObject(AudioClip clipToPlay,Vector3 position,AudioMixerGroupType mixerGroup)
    {
        var go = CreateSoundObject(position, GetAudioMixerGroupOfType(mixerGroup), out AudioSource createdAudioSource);
        createdAudioSource.PlayOneShot(clipToPlay);
        Debug.Log(go.name + clipToPlay.ToString());
        Destroy(go,clipToPlay.length);
    }
    GameObject CreateSoundObject(Vector3 position,AudioMixerGroup audioMixerGroup,out AudioSource createdAudioSource)
    {
        var instance = new GameObject("AudioObject");
        createdAudioSource = instance.AddComponent<AudioSource>();
        instance.transform.position = position;
        createdAudioSource.outputAudioMixerGroup = audioMixerGroup;
        return instance;
    }
    AudioMixerGroup GetAudioMixerGroupOfType(AudioMixerGroupType mixerGroupType)
    {
        switch (mixerGroupType)
        {
            case AudioMixerGroupType.Main:
                return mainAudioGroup;
            case AudioMixerGroupType.Sfx:
                return sfxAudioGroup;

            default: return mainAudioGroup;
        }
    }
}
