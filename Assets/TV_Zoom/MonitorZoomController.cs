using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MonitorZoomController : MonoBehaviour
{
    public static bool isZoomComplete = false;
    public static int CurrentZoomIndex { get; private set; } = -1;

    public Transform[] monitorMasks;
    public Transform[] monitors;

    public float[] monitorMaskZoomScales;
    public float[] monitorZoomScales;

    public float zoomSpeed = 5f;

    private int currentZoomIndex = -1;
    private Vector3[] originalScales;
    private Vector3[] originalPositions;

    public Vector3 zoomTargetPosition = Vector3.zero;
    private bool isZooming = false;

    public Material[] monitorMaterials;
    public float defaultNoiseStrength = 0.271f;
    public float defaultScanlineStrength = 0.226f;

    public RawImage[] monitorRawImages;
    public Vector2[] zoomedSizes;
    public Vector2[] originalSizes;
    private Vector2[] originalRawImagePos;
    public Vector2[] zoomedRawImagePos;

    public GameObject zoomBlockCanvas;
    public float hideDelayAfterZoomStart = 0.1f;

    void Start()
    {
        int count = monitorMasks.Length;
        originalScales = new Vector3[count];
        originalPositions = new Vector3[count];
        originalSizes = new Vector2[count];
        originalRawImagePos = new Vector2[count];

        for (int i = 0; i < count; i++)
        {
            originalScales[i] = monitorMasks[i].localScale;
            originalPositions[i] = monitorMasks[i].localPosition;
            if (monitorRawImages != null && i < monitorRawImages.Length)
            {
                originalSizes[i] = monitorRawImages[i].rectTransform.sizeDelta;
                originalRawImagePos[i] = monitorRawImages[i].rectTransform.anchoredPosition;
            }
        }

        if (zoomBlockCanvas != null)
        {
            zoomBlockCanvas.SetActive(false);
        }
    }

    void Update()
    {
        if (isZooming) return;

        for (int i = 0; i < monitorMasks.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                if (currentZoomIndex != -1 && currentZoomIndex != i)
                {
                    return;
                }
                HandleZoom(i);
            }
        }
    }

    void HandleZoom(int index)
    {
        isZooming = true;

        if (currentZoomIndex == index)
        {
            if (zoomBlockCanvas != null)
            {
                zoomBlockCanvas.SetActive(true);
                StartCoroutine(HideZoomBlockCanvasAfterDelay(hideDelayAfterZoomStart));
            }

            StartCoroutine(ZoomTo(monitorMasks[index], Vector3.one, originalPositions[index], () => {
                isZooming = false;
                isZoomComplete = true;
                SetNoiseStrength(index, defaultNoiseStrength);
                SetScanlineStrength(index, defaultScanlineStrength);
            }));
            StartCoroutine(ZoomScaleOnly(monitors[index], Vector3.one));

            if (monitorRawImages != null && index < monitorRawImages.Length)
            {
                StartCoroutine(ZoomSize(monitorRawImages[index].rectTransform, originalSizes[index], originalRawImagePos[index]));
            }

            currentZoomIndex = -1;
            CurrentZoomIndex = -1;
        }
        else
        {
            monitorMasks[index].SetAsLastSibling();

            Vector3 maskTargetScale = Vector3.one * monitorMaskZoomScales[index];
            Vector3 monitorTargetScale = Vector3.one * monitorZoomScales[index];

            SetNoiseStrength(index, 0f);
            SetScanlineStrength(index, 0f);

            if (zoomBlockCanvas != null)
                zoomBlockCanvas.SetActive(true);

            StartCoroutine(HideZoomBlockCanvasAfterDelay(hideDelayAfterZoomStart));

            StartCoroutine(ZoomTo(monitorMasks[index], maskTargetScale, zoomTargetPosition, () => {
                isZooming = false;
                isZoomComplete = true;
            }));
            StartCoroutine(ZoomScaleOnly(monitors[index], monitorTargetScale));

            if (monitorRawImages != null && index < monitorRawImages.Length && index < zoomedSizes.Length && index < zoomedRawImagePos.Length)
            {
                StartCoroutine(ZoomSize(monitorRawImages[index].rectTransform, zoomedSizes[index], zoomedRawImagePos[index]));
            }

            currentZoomIndex = index;
            CurrentZoomIndex = index;
        }
    }

    IEnumerator HideZoomBlockCanvasAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (zoomBlockCanvas != null)
            zoomBlockCanvas.SetActive(false);
    }

    IEnumerator ZoomTo(Transform target, Vector3 targetScale, Vector3 targetPosition, System.Action onComplete)
    {
        while (Vector3.Distance(target.localScale, targetScale) > 0.01f ||
               Vector3.Distance(target.localPosition, targetPosition) > 0.01f)
        {
            target.localPosition = Vector3.Lerp(target.localPosition, targetPosition, Time.deltaTime * zoomSpeed);
            target.localScale = Vector3.Lerp(target.localScale, targetScale, Time.deltaTime * zoomSpeed);
            yield return null;
        }

        target.localScale = targetScale;
        target.localPosition = targetPosition;

        if (onComplete != null) onComplete();
    }

    IEnumerator ZoomScaleOnly(Transform target, Vector3 targetScale)
    {
        while (Vector3.Distance(target.localScale, targetScale) > 0.01f)
        {
            target.localScale = Vector3.Lerp(target.localScale, targetScale, Time.deltaTime * zoomSpeed);
            yield return null;
        }

        target.localScale = targetScale;
    }

    IEnumerator ZoomSize(RectTransform rectTransform, Vector2 targetSize, Vector2 targetAnchoredPos)
    {
        while (Vector2.Distance(rectTransform.sizeDelta, targetSize) > 0.5f ||
               Vector2.Distance(rectTransform.anchoredPosition, targetAnchoredPos) > 0.01f)
        {
            rectTransform.sizeDelta = Vector2.Lerp(rectTransform.sizeDelta, targetSize, Time.deltaTime * zoomSpeed);
            rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, targetAnchoredPos, Time.deltaTime * zoomSpeed);
            yield return null;
        }
        rectTransform.sizeDelta = targetSize;
        rectTransform.anchoredPosition = targetAnchoredPos;
    }

    void SetNoiseStrength(int index, float value)
    {
        if (monitorMaterials != null && index >= 0 && index < monitorMaterials.Length && monitorMaterials[index] != null)
        {
            monitorMaterials[index].SetFloat("_NoiseStrength", value);
        }
    }

    void SetScanlineStrength(int index, float value)
    {
        if (monitorMaterials != null && index >= 0 && index < monitorMaterials.Length && monitorMaterials[index] != null)
        {
            monitorMaterials[index].SetFloat("_ScanlineStrength", value);
        }
    }
}
