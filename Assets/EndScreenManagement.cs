using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class EndScreenManagement : MonoBehaviour
{
    public Button restartButton;
    public Button toLevelSelectionButton;
    public Button toNextLevelButton;
    public Image nextLevelImage;
    public Image nextLevelImageBackground;

    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

}
