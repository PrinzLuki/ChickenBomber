using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundFileType
{
    Hit,
    Launched,
    Destroyed
}
[System.Serializable]
public class SoundFile
{
    [SerializeField] SoundFileType soundFileType;
    [SerializeField] AudioClip soundClip;

    public bool IsOfType(SoundFileType typeToCompare)
    {
        return soundFileType == typeToCompare;
    }
    public AudioClip GetAudioClip()
    {
        return soundClip;
    }
}
