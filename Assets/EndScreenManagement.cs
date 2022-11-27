using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class EndScreenManagement : MonoBehaviour
{
    public Button restartButton;
    public Button toMainMenuButton;
    public Button toLevelSelectionButton;

    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

}
