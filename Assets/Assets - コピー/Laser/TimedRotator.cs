using UnityEngine;
using System.Collections;

public class TimedRotator : MonoBehaviour
{
    public Vector3 rotationAxis = Vector3.up;     // ‰ñ“]Ž²
    public float rotationAngle = 90f;             // ˆê‰ñ‚Ì‰ñ“]Šp“x
    public float intervalSeconds = 2f;            // ŠÔŠui•bj
    public float smoothDuration = 1f;             // ‚È‚ß‚ç‚©‚É‰ñ‚·ŽžŠÔ
    public bool useSmoothRotation = false;        // ƒXƒ€[ƒY‰ñ“]Ø‘Ö
    public bool pingPongRotation = false;         // s‚Á‚½‚è—ˆ‚½‚è‚ðŒJ‚è•Ô‚·
    public int stepsBeforeReturn = 3;             // s‚Á‚Ä–ß‚é‚Ì‰ñ”

    private float timer = 0f;
    private bool isRotating = false;
    private int rotationStep = 0;
    private int direction = 1; // 1: ³‰ñ“], -1: ‹t‰ñ“]

    void Update()
    {
        if (isRotating) return; // ‰ñ“]’†‚È‚ç‰½‚à‚µ‚È‚¢

        timer += Time.deltaTime;
        if (timer >= intervalSeconds)
        {
            timer = 0f;
            float actualAngle = rotationAngle * direction;

            if (useSmoothRotation)
            {
                StartCoroutine(RotateSmoothly(rotationAxis, actualAngle));
            }
            else
            {
                transform.Rotate(rotationAxis, actualAngle);
                UpdateRotationStep();
            }
        }
    }

    IEnumerator RotateSmoothly(Vector3 axis, float angle)
    {
        isRotating = true;

        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = startRotation * Quaternion.AngleAxis(angle, axis);
        float elapsed = 0f;

        while (elapsed < smoothDuration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, elapsed / smoothDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = endRotation;
        isRotating = false;
        UpdateRotationStep();
    }

    void UpdateRotationStep()
    {
        if (!pingPongRotation) return;

        rotationStep++;
        if (rotationStep >= stepsBeforeReturn)
        {
            rotationStep = 0;
            direction *= -1;
        }
    }
}
