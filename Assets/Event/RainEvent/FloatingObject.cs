using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    [Header("浮上設定")]
    public float floatHeight = 2f;
    public float floatSpeed = 1f;
    public Transform rainSurface;

    [Header("揺れ設定")]
    public float bobbingSpeed = 0.5f;
    public float bobbingAmount = 0.1f;

    [Header("沈む設定")]
    public float sinkSpeed = 0.5f;

    private bool isFloating = false;
    private bool isBobbing = false;
    private bool isSinking = false;

    private Vector3 targetPosition;
    private Vector3 basePosition;
    private Vector3 initialPosition;
    private float bobTimer = 0f;

    private Rigidbody rb;
    private float originalDrag;
    private float originalAngularDrag;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody>();

        originalDrag = rb.linearDamping;
        originalAngularDrag = rb.angularDamping;

        initialPosition = transform.position; // 初期位置を保存
        basePosition = transform.position;
    }

    void Update()
    {
        // 浮上処理
        if (isFloating)
        {
            float maxY = rainSurface != null ? rainSurface.position.y : float.MaxValue;
            float targetY = Mathf.Min(targetPosition.y, maxY);

            transform.position = Vector3.MoveTowards(
                transform.position,
                new Vector3(transform.position.x, targetY, transform.position.z),
                floatSpeed * Time.deltaTime
            );

            rb.useGravity = false;
            rb.linearDamping = 5f;
            rb.angularDamping = 5f;

            if (Mathf.Abs(transform.position.y - targetY) < 0.01f)
            {
                isFloating = false;
                isBobbing = true;
                basePosition = transform.position;
                bobTimer = 0f;
            }
        }

        // 揺れ処理
        if (isBobbing)
        {
            bobTimer += Time.deltaTime * bobbingSpeed;
            float offsetY = Mathf.Sin(bobTimer) * bobbingAmount;
            transform.position = new Vector3(basePosition.x, basePosition.y + offsetY, basePosition.z);
        }

        // 沈む処理
        if (isSinking)
        {
            transform.position -= new Vector3(0, sinkSpeed * Time.deltaTime, 0);
        }
    }

    public void StartFloating()
    {
        if (!isFloating && !isBobbing && !isSinking)
        {
            targetPosition = new Vector3(transform.position.x, transform.position.y + floatHeight, transform.position.z);
            isFloating = true;
        }
    }

    public void StartSinking()
    {
        isFloating = false;
        isBobbing = false;
        isSinking = true;
        rb.useGravity = false;
    }

    public void ResetFloating()
    {
        isFloating = false;
        isBobbing = false;
        isSinking = false;

        rb.useGravity = true;
        rb.linearDamping = originalDrag;
        rb.angularDamping = originalAngularDrag;

        transform.position = initialPosition;
    }
}
