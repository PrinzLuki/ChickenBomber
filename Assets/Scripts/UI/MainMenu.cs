using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] SceneAsset levelSelectionScene;

    public void OpenLevelSelection()
    {
        SceneManager.LoadScene(levelSelectionScene.name);
    }
}
