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
        if (Input.GetKeyDown(KeyCode.I)&&isAnimation==false)
        {
            StartCoroutine(ZoomOutAndReturn());
        }

        //isAnimaitionが真になったらアニメーションとカメラの振動を実行させる
        if (isAnimation)
        {
            shakeElapsed += Time.deltaTime;
            shakeTimer += Time.deltaTime;

            if (shakeTimer >= shakeFrequency)
            {
                shakeTimer = 0f;
                Vector2 shakeOffset = Random.insideUnitCircle * shakeIntensity;
                mainCamera.transform.localPosition = originalPosition + new Vector3(shakeOffset.x, shakeOffset.y, 0);
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
        //カメラをひく
        yield return StartCoroutine(SmoothZoom(mainCamera.orthographicSize, zoomOutSize, zoomDuration));

        // 振動開始
        shakeElapsed = 0f;
        shakeTimer = 0f;
        isAnimation = true;

        //カメラをとめる
        yield return new WaitForSeconds(waitTime);
        //カメラを元に戻す
        yield return StartCoroutine(SmoothZoom(mainCamera.orthographicSize, originalSize, zoomDuration));

        isAnimation = false;

    }


    //カメラの補間用の関数
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
