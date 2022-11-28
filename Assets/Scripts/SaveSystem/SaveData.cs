using UnityEngine;
using System.Collections;

[System.Serializable]
public class SaveData
{
    #region SaveData - Other
    private static SaveData _current;

    public static SaveData Current
    {
        get
        {
            if (_current == null)
            {
                _current = new SaveData();
            }
            return _current;
        }
        set
        {
            if (value != null)
            {
                _current = value;
            }
        }
    }

    #endregion

    #region Player Profile
    private static PlayerSettings _playerSettings;

    public static PlayerSettings PlayerSettings
    {
        get
        {
            if (_playerSettings == null)
            {
                _playerSettings = new PlayerSettings();
            }
            return _playerSettings;
        }
        set
        {
            if (value != null)
            {
                _playerSettings = value;
            }
        }
    }
    #endregion

    #region Level Profile
    private static LevelProfile _levelProfile;

    public static LevelProfile LevelProfile
    {
        get
        {
            if (_levelProfile == null)
            {
                _levelProfile = new LevelProfile();
            }
            return _levelProfile;
        }
        set
        {
            if (value != null)
            {
                _levelProfile = value;
            }
        }
    }
    #endregion
}
