using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    public enum FadeType { Black, White, CustomColor, Circle }

    public CanvasGroup fadeCanvasGroup;
    public Image fadeImage;
    public Material circleFadeMaterial; // 円形フェード用マテリアル
    public float fadeDuration = 1f;
    public Color customFadeColor = Color.black;

    private FadeType currentFadeType = FadeType.Black; // 初期値を Black に設定

    private void Start()
    {
        // シーン開始時に現在のフェード種類でフェードイン
        StartCoroutine(FadeIn(currentFadeType));
    }

    public void LoadSceneWithFade(string sceneName, FadeType fadeType)
    {
        currentFadeType = fadeType; // 修正: フェード種類を更新
        StartCoroutine(FadeOut(sceneName, fadeType));
    }

    private IEnumerator FadeIn(FadeType fadeType)
    {
        SetFadeColor(fadeType);
        if (fadeType == FadeType.Circle)
        {
            yield return CircleFade(1, 0);
        }
        else
        {
            yield return AlphaFade(1, 0);
        }
    }

    private IEnumerator FadeOut(string sceneName, FadeType fadeType)
    {
        SetFadeColor(fadeType);
        if (fadeType == FadeType.Circle)
        {
            yield return CircleFade(0, 1);
        }
        else
        {
            yield return AlphaFade(0, 1);
        }

        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator AlphaFade(float start, float end)
    {
        float timer = 0;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(start, end, timer / fadeDuration);
            yield return null;
        }
        fadeCanvasGroup.alpha = end;
    }

    private IEnumerator CircleFade(float start, float end)
    {
        if (circleFadeMaterial == null)
        {
            Debug.LogError("Circle Fade Material が設定されていません！");
            yield break;
        }

        float timer = 0;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float fadeValue = Mathf.Lerp(start, end, timer / fadeDuration);
            circleFadeMaterial.SetFloat("_Fade", fadeValue);
            Debug.Log("Circle Fade Value: " + fadeValue); // 値が変わるか確認！
            yield return null;
        }
        circleFadeMaterial.SetFloat("_Fade", end);
        Debug.Log("Final Fade Value: " + end);
    }


    private void SetFadeColor(FadeType fadeType)
    {
        if (fadeImage == null) return;

        switch (fadeType)
        {
            case FadeType.Black:
                fadeImage.material = null;
                fadeImage.color = Color.black;
                break;
            case FadeType.White:
                fadeImage.material = null;
                fadeImage.color = Color.white;
                break;
            case FadeType.CustomColor:
                fadeImage.material = null;
                fadeImage.color = customFadeColor;
                break;
            case FadeType.Circle:
                if (circleFadeMaterial != null)
                {
                    fadeImage.material = circleFadeMaterial; // マテリアル適用
                    circleFadeMaterial.SetFloat("_Fade", 0f); // 初期値をしっかり設定
                }
                else
                {
                    Debug.LogError("Circle Fade Material が設定されていません！");
                }
                break;
        }
    }
}
