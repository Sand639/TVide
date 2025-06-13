using UnityEngine;

public class RainEvent : MonoBehaviour
{
    public FloatingObject[] floatingObjects;  // インスペクターで設定
    public Transform pointA;                  // 移動先A
    public Transform pointB;                  // 移動先B
    public float moveSpeed = 1.2f;

    private Transform targetPoint = null;
    private bool isMoving = false;
    private Collider cubeCollider;

    void Start()
    {
        cubeCollider = GetComponent<Collider>();
        if (cubeCollider != null)
        {
            cubeCollider.enabled = false;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            if (!isMoving)
            {
                float distToA = Vector3.Distance(transform.position, pointA.position);
                float distToB = Vector3.Distance(transform.position, pointB.position);
                targetPoint = (distToA < distToB) ? pointB : pointA;
                isMoving = true;

                // pointA に戻るときに沈ませる
                if (targetPoint == pointA)
                {
                    foreach (var obj in floatingObjects)
                    {
                        obj.StartSinking();
                    }
                }
            }
        }

        if (isMoving && targetPoint != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, moveSpeed * Time.deltaTime);



            if (Vector3.Distance(transform.position, targetPoint.position) < 0.01f)
            {//目的地に到着したら通知
                isMoving = false;
                transform.position = targetPoint.position;
                targetPoint = null;

                if (Vector3.Distance(transform.position, pointA.position) < 0.01f)
                {
                    foreach (var obj in floatingObjects)
                    {
                        obj.ResetFloating();
                    }
                }
            }


        }
    }
}
