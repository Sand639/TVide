using UnityEngine;
using System.Collections;

public class NoiseCanvasController : MonoBehaviour
{
    public GameObject noiseRawImage;
    public float duration = 0.5f;

    public void ShowNoise()
    {
        StartCoroutine(ShowRoutine());
    }

    private IEnumerator ShowRoutine()
    {
        noiseRawImage.SetActive(true);
        yield return new WaitForSeconds(duration);
        noiseRawImage.SetActive(false);
    }
}
