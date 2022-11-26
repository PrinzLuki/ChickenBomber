using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class LevelProfile
{
    public LevelData[] levelDatas;
    public bool hasSaveData;
}

[System.Serializable]
public class LevelData{
    public string name;
    public bool unlocked;
    public bool finished;
    public int points;
}
