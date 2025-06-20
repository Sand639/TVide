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
    private float[] originalRawImagePosX;
    public float[] zoomedRawImagePosX;

    void Start()
    {
        int count = monitorMasks.Length;
        originalScales = new Vector3[count];
        originalPositions = new Vector3[count];
        originalSizes = new Vector2[count];
        originalRawImagePosX = new float[count];

        for (int i = 0; i < count; i++)
        {
            originalScales[i] = monitorMasks[i].localScale;
            originalPositions[i] = monitorMasks[i].localPosition;
            if (monitorRawImages != null && i < monitorRawImages.Length)
            {
                originalSizes[i] = monitorRawImages[i].rectTransform.sizeDelta;
                originalRawImagePosX[i] = monitorRawImages[i].rectTransform.anchoredPosition.x;
            }
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
                    // 他のモニターにズーム中なら無視
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
            SetNoiseStrength(index, defaultNoiseStrength);
            SetScanlineStrength(index, defaultScanlineStrength);

            StartCoroutine(ZoomTo(monitorMasks[index], Vector3.one, originalPositions[index], () => {
                isZooming = false;
                isZoomComplete = true;
            }));
            StartCoroutine(ZoomScaleOnly(monitors[index], Vector3.one));

            if (monitorRawImages != null && index < monitorRawImages.Length)
            {
                StartCoroutine(ZoomSize(monitorRawImages[index].rectTransform, originalSizes[index], false, originalRawImagePosX[index]));
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

            StartCoroutine(ZoomTo(monitorMasks[index], maskTargetScale, zoomTargetPosition, () => {
                isZooming = false;
                isZoomComplete = true;
            }));
            StartCoroutine(ZoomScaleOnly(monitors[index], monitorTargetScale));

            if (monitorRawImages != null && index < monitorRawImages.Length && index < zoomedSizes.Length && index < zoomedRawImagePosX.Length)
            {
                StartCoroutine(ZoomSize(monitorRawImages[index].rectTransform, zoomedSizes[index], false, zoomedRawImagePosX[index]));
            }

            currentZoomIndex = index;
            CurrentZoomIndex = index;
        }
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

    IEnumerator ZoomSize(RectTransform rectTransform, Vector2 targetSize, bool preserveY, float targetPosX)
    {
        Vector2 initialSize = rectTransform.sizeDelta;
        float targetY = preserveY ? initialSize.y : targetSize.y;

        while (Vector2.Distance(rectTransform.sizeDelta, targetSize) > 0.5f || Mathf.Abs(rectTransform.anchoredPosition.x - targetPosX) > 0.01f)
        {
            Vector2 newSize = Vector2.Lerp(rectTransform.sizeDelta, targetSize, Time.deltaTime * zoomSpeed);
            if (preserveY) newSize.y = targetY;
            rectTransform.sizeDelta = newSize;
            rectTransform.anchoredPosition = new Vector2(Mathf.Lerp(rectTransform.anchoredPosition.x, targetPosX, Time.deltaTime * zoomSpeed), rectTransform.anchoredPosition.y);
            yield return null;
        }
        rectTransform.sizeDelta = preserveY ? new Vector2(targetSize.x, targetY) : targetSize;
        rectTransform.anchoredPosition = new Vector2(targetPosX, rectTransform.anchoredPosition.y);
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
