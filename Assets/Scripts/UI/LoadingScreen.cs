using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] float speedToLoad = 0.5f;
    [SerializeField] GameObject loadingDisplay;
    [SerializeField] Animator fadeImgAnim;
    [SerializeField] SceneAsset sceneToLoad;
    [SerializeField] Image loadingBar;
    [SerializeField] Color cameraBgColor;
    string sceneNameToLoad;

    public static event Action OnLoadScene;

    private void Start()
    {
        OnlyFadeOut();

        if (sceneToLoad == null) return;

        sceneNameToLoad = sceneToLoad.name;
    }

    public void SetSceneToLoad(string sceneToLoad)
    {
        this.sceneNameToLoad = sceneToLoad;
    }

    public void FadeInAndOut()
    {
        StartCoroutine(StartFade());
    }

    public void OnlyFadeOut()
    {
        fadeImgAnim.SetTrigger("OnlyOut");
    }

    public void OnlyFadeIn()
    {
        fadeImgAnim.SetTrigger("OnlyIn");
    }

    readonly WaitForEndOfFrame wait = new();
    IEnumerator StartFade()
    {
        fadeImgAnim.SetTrigger("Start");
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        Camera.main.backgroundColor = cameraBgColor;
        loadingDisplay.SetActive(true);
        fadeImgAnim.SetTrigger("Out");
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(StartLoadingScreen());
    }

    IEnumerator StartLoadingScreen()
    {
        float loadingValue = 0;
        while (loadingValue < 1)
        {
            loadingValue += speedToLoad * Time.deltaTime;
            loadingBar.fillAmount = Mathf.Lerp(0, 1, loadingValue);
            yield return wait;
        }
        OnlyFadeIn();
        yield return new WaitForSeconds(1f);

        if (sceneNameToLoad != string.Empty)
        {
            SceneManager.LoadSceneAsync(sceneNameToLoad);
        }

        OnLoadScene?.Invoke();
    }
}
