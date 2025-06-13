using System.Collections;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public delegate void ShakeEventHandler();
    public static event ShakeEventHandler OnShake;

    bool isShaking = false;
    public float shakeDuration;
    public float shakeMagnitude;
    float shakeTimeRemaining;
    public float minX = -15f;
    public float minY = 0f;

    GameObject charaObj;
    Vector3 targetCameraPos;
    public float lerpSpeed = 7.0f;
    bool isCameraTilt = false;

    void Start()
    {
        targetCameraPos = Vector3.zero;
        charaObj = GameObject.Find("character_test2_0");
    }

    void Update()
    {
        CameraLeap();

        if (MonitorZoomController.isZoomComplete &&
        MonitorZoomController.CurrentZoomIndex == 0 &&
        Input.GetKeyDown(KeyCode.Return))
        {
            StartShake();
        }

        if (isShaking)
        {
            ApplyShake();
        }
    }

    void CameraLeap()
    {
        if (MonitorZoomController.isZoomComplete && MonitorZoomController.CurrentZoomIndex == 0)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                isCameraTilt = !isCameraTilt;
            }
        }

        if (isCameraTilt)
        {
            targetCameraPos.x = charaObj.transform.position.x;
            targetCameraPos.y = charaObj.transform.position.y + 6.0f;
            targetCameraPos.z = charaObj.transform.position.z - 15.0f;
        }
        else
        {
            targetCameraPos.x = charaObj.transform.position.x;
            targetCameraPos.y = charaObj.transform.position.y - 6.0f;
            targetCameraPos.z = charaObj.transform.position.z - 15.0f;
        }

        Vector3 newPos = Vector3.Lerp(transform.position, targetCameraPos, Time.deltaTime * lerpSpeed);
        newPos.x = charaObj.transform.position.x;

        if (newPos.x < minX) newPos.x = minX;
        if (newPos.y < minY) newPos.y = minY;

        transform.position = newPos;

        transform.LookAt(charaObj.transform.position);
        Vector3 euler = transform.rotation.eulerAngles;
        euler.y = 0f;
        if (!isCameraTilt && euler.x > 10f && euler.x < 180f) euler.x = 10f;
        transform.rotation = Quaternion.Euler(euler);
    }

    void StartShake()
    {
        isShaking = true;
        shakeTimeRemaining = shakeDuration;
        OnShake?.Invoke();
    }

    void ApplyShake()
    {
        if (shakeTimeRemaining > 0)
        {
            transform.position += Random.insideUnitSphere * shakeMagnitude;
            shakeTimeRemaining -= Time.deltaTime;
        }
        else
        {
            isShaking = false;
        }
    }

    public void TriggerShakeForSeconds(float duration)
    {
        StartCoroutine(ShakeForSeconds(duration));
    }

    IEnumerator ShakeForSeconds(float duration)
    {
        isShaking = true;
        float timer = duration;
        while (timer > 0f)
        {
            transform.position += Random.insideUnitSphere * shakeMagnitude;
            timer -= Time.deltaTime;
            yield return null;
        }
        isShaking = false;
    }

    void OnEnable()
    {
        WeatherEvent.OnWindEventStart += StartWindShake;
    }

    void OnDisable()
    {
        WeatherEvent.OnWindEventStart -= StartWindShake;
    }

    void OnMagnetDestroyedHandler()
    {
        TriggerShakeForSeconds(3f);
    }

    void StartWindShake(float duration)
    {
        StartCoroutine(DelayedWindShake(duration));
    }

    IEnumerator DelayedWindShake(float duration)
    {
        yield return new WaitForSeconds(0.2f);
        isShaking = true;

        float timer = duration;
        while (timer > 0f)
        {
            transform.position += Random.insideUnitSphere * shakeMagnitude;
            timer -= Time.deltaTime;
            yield return null;
        }

        isShaking = false;
    }
}
