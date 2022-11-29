using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagmentComponent : MonoBehaviour
{
    [SerializeField] SoundFile[] soundFiles;

    public AudioClip GetAudioClipFromSoundFileOfType(SoundFileType soundFileType)
    {
        var soundFile = GetSoundFileOfType(soundFileType);
        if (soundFile == null) return null;
        {
            return soundFile.GetAudioClip();
        }
    }
    SoundFile GetSoundFileOfType(SoundFileType soundFileType)
    {
        SoundFile soundFileOfType = null;

        foreach (var soundFile in soundFiles)
        {
            if (!soundFile.IsOfType(soundFileType)) continue;
            soundFileOfType = soundFile;
            
        }

        return soundFileOfType;
    }
}
