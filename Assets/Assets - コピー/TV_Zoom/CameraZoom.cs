using UnityEngine;
using System.Collections;

public class CameraZoom : MonoBehaviour
{
    public Camera mainCamera;

    public Transform monitor1Position;
    public Transform monitor2Position;
    public Transform monitor3Position;
    public Transform monitor4Position;

    public Camera monitor1Camera;
    public Camera monitor2Camera;
    public Camera monitor3Camera;
    public Camera monitor4Camera;

    public RenderTexture rt1;
    public RenderTexture rt2;
    public RenderTexture rt3;
    public RenderTexture rt4;

    public GameObject noiseEffectUI; // ノイズUI
    public float noiseDuration = 0.5f; // ノイズ表示時間

    public float zoomSpeed = 5f;
    public float zoomInSize = 5f;
    public float zoomOutSize = 10f;

    private Vector3 originalPosition;
    private Coroutine currentCoroutine;
    private int currentZoomIndex = 0;

    //フラグ追加
    //Zoomが終わったらフラグを立てる（playerの操作）
    public static bool isZoomedInOnMonitor1 = false;
    public static bool isZoomedInOnMonitor2 = false;
    private bool isZooming = false;

    void Start()
    {
        originalPosition = mainCamera.transform.position;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) HandleZoom(1, monitor1Position);
        if (Input.GetKeyDown(KeyCode.Alpha2)) HandleZoom(2, monitor2Position);
        if (Input.GetKeyDown(KeyCode.Alpha3)) HandleZoom(3, monitor3Position);
        if (Input.GetKeyDown(KeyCode.Alpha4)) HandleZoom(4, monitor4Position);
    }

    private void HandleZoom(int index, Transform target)
    {
        if (isZooming) return;
        if (currentZoomIndex == 0)
        {
            StartNewZoomCoroutine(target.position, zoomInSize);
            currentZoomIndex = index;
            StartCoroutine(SwitchCameraAfterDelay(index, 1f));
        }
        else if (currentZoomIndex == index)
        {
            StartNewZoomCoroutine(originalPosition, zoomOutSize);
            currentZoomIndex = 0;

            monitor1Camera.enabled = false;
            monitor2Camera.enabled = false;
            monitor3Camera.enabled = false;
            monitor4Camera.enabled = false;

            monitor1Camera.targetTexture = rt1;
            monitor2Camera.targetTexture = rt2;
            monitor3Camera.targetTexture = rt3;
            monitor4Camera.targetTexture = rt4;

            mainCamera.enabled = true;
        }
        else
        {
            Debug.Log("ズーム中は他のモニターに切り替えできません！");
        }
    }

    private IEnumerator SwitchCameraAfterDelay(int index, float delay)
    {
        yield return new WaitForSeconds(delay);

        // ノイズを一時的に表示
        if (noiseEffectUI != null)
        {
            noiseEffectUI.SetActive(true);
            yield return new WaitForSeconds(noiseDuration);
            noiseEffectUI.SetActive(false);
        }

        // メインカメラ無効化
        mainCamera.enabled = false;

        monitor1Camera.enabled = false;
        monitor2Camera.enabled = false;
        monitor3Camera.enabled = false;
        monitor4Camera.enabled = false;

        if (index == 1)
        {
            monitor1Camera.targetTexture = null;
            monitor1Camera.enabled = true;
            isZoomedInOnMonitor1 = true;//ここでフラグを立てる
            isZoomedInOnMonitor2 = false ;
        }
        else if (index == 2)
        {
            monitor2Camera.targetTexture = null;
            monitor2Camera.enabled = true;
            isZoomedInOnMonitor1 = false;
            isZoomedInOnMonitor2 = true;
        }
        else if (index == 3)
        {
            monitor3Camera.targetTexture = null;
            monitor3Camera.enabled = true;
            isZoomedInOnMonitor1 = false;
        }
        else if (index == 4)
        {
            monitor4Camera.targetTexture = null;
            monitor4Camera.enabled = true;
            isZoomedInOnMonitor1 = false;
        }
    }

    private void StartNewZoomCoroutine(Vector3 targetPosition, float targetSize)
    {
        isZooming = true;

        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        currentCoroutine = StartCoroutine(ZoomCamera(targetPosition, targetSize));
    }

    private IEnumerator ZoomCamera(Vector3 targetPosition, float targetSize)
    {
        float originalZ = mainCamera.transform.position.z;

        while (Vector3.Distance(mainCamera.transform.position, targetPosition) > 0.1f ||
               Mathf.Abs(mainCamera.orthographicSize - targetSize) > 0.1f)
        {
            Vector3 newPosition = new Vector3(
                Mathf.Lerp(mainCamera.transform.position.x, targetPosition.x, zoomSpeed * Time.deltaTime),
                Mathf.Lerp(mainCamera.transform.position.y, targetPosition.y, zoomSpeed * Time.deltaTime),
                originalZ
            );

            mainCamera.transform.position = newPosition;
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, targetSize, zoomSpeed * Time.deltaTime);

            yield return null;
        }

        mainCamera.transform.position = new Vector3(targetPosition.x, targetPosition.y, originalZ);
        mainCamera.orthographicSize = targetSize;
        isZooming = false;
    }
}
