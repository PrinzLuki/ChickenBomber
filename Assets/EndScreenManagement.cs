using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEditor;

public class EndScreenManagement : MonoBehaviour
{
    public Button restartButton;
    public Button toLevelSelectionButton;
    public Button toNextLevelButton;
    public Image nextLevelImage;
    public Image nextLevelImageBackground;

    [Header("Level Selection Scene")]
    [SerializeField] SceneAsset levelSelectionScene;

    public void LoadLevel(SceneAsset sceneLevel)
    {
        SceneManager.LoadScene(sceneLevel.name);
    }

    public void LoadLevelSelection()
    {
        SceneManager.LoadScene(levelSelectionScene.name);
    }

    public void ReloadScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

}
