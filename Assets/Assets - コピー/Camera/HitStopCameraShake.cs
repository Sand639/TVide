using UnityEngine;
using System.Collections;

public class HitStopCameraShake : MonoBehaviour
{
    public static HitStopCameraShake instance;

    private void Awake()
    {
        instance = this;
    }

    /// <param name="duration">U“®ŠÔi•bj</param>
    /// <param name="magnitude">U“®‚Ì‹­‚³</param>
    /// 
    public void ShakeDuringHitStop(float duration, float magnitude)
    {
        StartCoroutine(ShakeCoroutine(duration, magnitude));
    }

    private IEnumerator ShakeCoroutine(float duration, float magnitude)
    {
        Vector3 originalPos = transform.position;
        float timer = duration;

        while (timer > 0f)
        {
            transform.position = originalPos + Random.insideUnitSphere * magnitude;
            timer -= Time.unscaledDeltaTime; // Time.timeScale = 0 ‚Å‚à“®ì
            yield return null;
        }

        transform.position = originalPos;
    }
}
