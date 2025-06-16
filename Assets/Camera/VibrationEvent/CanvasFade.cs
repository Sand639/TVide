using UnityEngine;
using System.Collections;

public class CanvasFade : MonoBehaviour
{
    public CanvasGroup[] canvasGroups;

    [Header("フェードアウト時間（透明にする）")]
    public float fadeOutDuration = 0.3f;

    [Header("フェードイン時間（元に戻す）")]
    public float fadeInDuration = 1f;

    public void FadeOutAll()
    {
        foreach (CanvasGroup cg in canvasGroups)
        {
            if (cg != null)
                StartCoroutine(FadeCanvas(cg, cg.alpha, 0f, fadeOutDuration));
        }
    }

    public void FadeInAll(System.Action onComplete = null)
    {
        StartCoroutine(FadeInCoroutine(onComplete));
    }

    IEnumerator FadeInCoroutine(System.Action onComplete)
    {
        foreach (CanvasGroup cg in canvasGroups)
        {
            if (cg != null)
                yield return StartCoroutine(FadeCanvas(cg, cg.alpha, 1f, fadeInDuration));
        }

        onComplete?.Invoke();
    }

    IEnumerator FadeCanvas(CanvasGroup canvasGroup, float fromAlpha, float toAlpha, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(fromAlpha, toAlpha, elapsed / duration);
            canvasGroup.alpha = alpha;
            yield return null;
        }
        canvasGroup.alpha = toAlpha;
    }
}
