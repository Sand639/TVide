using UnityEngine;
using System.Collections;

public class VibrationAnimation : MonoBehaviour
{
    public Camera mainCamera;
    private float originalSize;
    private Vector3 originalPosition;

    public float zoomOutSize = 7f;
    public float zoomDuration = 0.5f;
    public float waitTime = 3f;

    public float shakeIntensity = 0.1f;
    public float shakeFrequency = 0.02f;
    public float shakeDuration = 2f;

    private float shakeTimer = 0f;
    private float shakeElapsed = 0f;

    public bool isAnimation = false;
    private bool isFading = false;

    [Header("Canvasのフェード処理を呼び出す")]
    public CanvasFade canvasFade;

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        originalSize = mainCamera.orthographicSize;
        originalPosition = mainCamera.transform.localPosition;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && !isAnimation && !isFading)
        {
            Debug.Log("通知：振動アニメーション開始");

            if (canvasFade != null)
                canvasFade.FadeOutAll();

            StartCoroutine(ZoomOutAndReturn());
        }

        if (isAnimation)
        {
            shakeElapsed += Time.deltaTime;
            shakeTimer += Time.deltaTime;

            if (shakeTimer >= shakeFrequency)
            {
                shakeTimer = 0f;
                float shakeOffsetY = Random.Range(-shakeIntensity, shakeIntensity);
                mainCamera.transform.localPosition = originalPosition + new Vector3(0, shakeOffsetY, 0);
            }

            if (shakeElapsed >= shakeDuration)
            {
                mainCamera.transform.localPosition = originalPosition;
            }
        }
        else
        {
            mainCamera.transform.localPosition = originalPosition;
        }
    }

    IEnumerator ZoomOutAndReturn()
    {
        yield return StartCoroutine(SmoothZoom(mainCamera.orthographicSize, zoomOutSize, zoomDuration));

        shakeElapsed = 0f;
        shakeTimer = 0f;
        isAnimation = true;

        yield return new WaitForSeconds(waitTime);

        isFading = true;
        if (canvasFade != null)
        {
            canvasFade.FadeInAll(() =>
            {
                isFading = false;
            });
        }

        yield return StartCoroutine(SmoothZoom(mainCamera.orthographicSize, originalSize, zoomDuration));

        isAnimation = false;
        Debug.Log("通知：振動アニメーション終了");
    }

    IEnumerator SmoothZoom(float fromSize, float toSize, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            mainCamera.orthographicSize = Mathf.Lerp(fromSize, toSize, elapsed / duration);
            yield return null;
        }
        mainCamera.orthographicSize = toSize;
    }
}
