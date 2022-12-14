using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{

    [Header("Ingame Canvas")]
    public Canvas ingameCanvas;
    [Header("HUD")]
    public Transform hudPanel;
    public TMP_Text ingamePointsText;
    [Header("Pause")]
    public Transform pausePanel;
    [Header("GameOverUI")]
    public Transform gameOverPanel;
    public TMP_Text gameOverPointsText;
    public Image starsImageUI;
    [Header("Victory")]
    public Transform victoryStuff;
    public Transform starShineUI;
    [Header("Lose")]
    public Transform loseStuff;
    [Header("Stars")]
    public Sprite noStar;
    public Sprite oneStar;
    public Sprite twoStar;
    public Sprite threeStar;

    public void SetPointsUI(int points)
    {
        string output = points.ToString();
        if (output.Length > 10)
            output = output.Substring(0, 10);
        ingamePointsText.text = output;
        gameOverPointsText.text = output;
    }

    public IEnumerator IActivateGameOverUI(float timer, bool victoryValue)
    {
        yield return new WaitForSeconds(timer);
        hudPanel.gameObject.SetActive(false);
        gameOverPanel.gameObject.SetActive(true);


        if (!victoryValue)
        {
            loseStuff.gameObject.SetActive(false);
            var endscreenmanager = gameOverPanel.GetComponent<EndScreenManagement>();
            endscreenmanager.toNextLevelButton.interactable = false;
            endscreenmanager.nextLevelImage.color = Color.gray;
            endscreenmanager.nextLevelImageBackground.color = Color.gray;
        }
        else
        {
            starShineUI.gameObject.SetActive(true);
            victoryStuff.gameObject.SetActive(true);
        }
    }


    public void SetStars(int stars)
    {
        switch (stars)
        {
            case 0:
                starsImageUI.sprite = noStar;
                break;
            case 1:
                starsImageUI.sprite = oneStar;
                break;
            case 2:
                starsImageUI.sprite = twoStar;
                break;
            case 3:
                starsImageUI.sprite = threeStar;
                break;
            default:
                Debug.Log("Something gone wrong");
                break;
        }
    }

}
