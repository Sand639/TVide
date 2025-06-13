using UnityEngine;

public class MirrorRotator : MonoBehaviour
{
    public float rotationSpeed = 50f;

    void Update()
    {
       // float h = Input.GetAxis("Horizontal");
       // transform.Rotate(Vector3.up, h * rotationSpeed * Time.deltaTime);
        Vector3 euler = transform.eulerAngles;
        euler.x = 0f;
        transform.rotation = Quaternion.Euler(euler);
    }
}
