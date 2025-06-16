using UnityEngine;
using System.Collections;

public class ArmAnimation : MonoBehaviour
{
    private VibrationAnimation Va;

    public Transform pointA;
    public Transform pointB;
    public float moveDuration = 1.0f;
    public float waitAtB = 2.0f;
    private bool isMoving = false;

    void Start()
    {
        Va = FindObjectOfType<VibrationAnimation>();
    }

    void Update()
    {
        if (!isMoving&&Va.isAnimation)
        {
            StartCoroutine(MoveToBAndBack());
        }
    }

    IEnumerator MoveToBAndBack()
    {
        isMoving = true;

        // pointA‚©‚çpointB‚ÉˆÚ“®‚·‚é
        yield return StartCoroutine(MoveBetweenPoints(pointA.position, pointB.position, moveDuration));

        // B‚Å‘Ò‹@
        yield return new WaitForSeconds(waitAtB);

        //pointB‚©‚ç‚¢‚Ç‚¤‚·‚é
        yield return StartCoroutine(MoveBetweenPoints(pointB.position, pointA.position, moveDuration));

        isMoving = false;
    }

    IEnumerator MoveBetweenPoints(Vector3 start, Vector3 end, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            transform.position = Vector3.Lerp(start, end, t);
            yield return null;
        }
        transform.position = end;
    }
}
