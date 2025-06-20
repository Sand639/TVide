using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SelectFade : MonoBehaviour
{
    public CanvasGroup canvasGroup;

    public float fadeDuration = 0.5f;

    private bool isVisible = false;
    private bool isFading = false;

    void Start()
    {
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    void Update()
    {
        // Eキーでフェードイン／アウト
        if (Input.GetKeyDown(KeyCode.E) && !isFading)
        {
            if (isVisible)
                StartCoroutine(FadeOut());
            else
                StartCoroutine(FadeIn());
        }

        // Enterキーで強制的に表示にする
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StopAllCoroutines(); // フェード中でも即座に止める
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            isVisible = false;
            isFading = false;
        }
    }

    IEnumerator FadeIn()
    {
        isFading = true;
        float elapsed = 0f;

        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        while (elapsed < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 1f;
        isVisible = true;
        isFading = false;
    }

    IEnumerator FadeOut()
    {
        isFading = true;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        isVisible = false;
        isFading = false;
    }
}
