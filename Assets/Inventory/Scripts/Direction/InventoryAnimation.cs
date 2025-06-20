using UnityEngine;

public class InventoryAnimation : MonoBehaviour
{//ダイヤルアニメーション
    private bool isRotated = false;
    private Quaternion originalRotation;
    private Quaternion targetRotation;
    private float rotationSpeed = 5f;

    void Start()
    {
        originalRotation = transform.rotation;
        targetRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, 0, 90));//zに90度回転
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            isRotated = !isRotated;
        }

        Quaternion desiredRotation = isRotated ? targetRotation : originalRotation;
        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.deltaTime * rotationSpeed);
    }
}
