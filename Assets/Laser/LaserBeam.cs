using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ReflectingLaser : MonoBehaviour
{
    public int maxReflections = 5;
    public float maxDistance = 100f;
    public LayerMask reflectLayers;
    public LayerMask stopLayers;

    private LineRenderer lr;

    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 1;
    }

    void Update()
    {
        Vector3 origin = transform.position;
        Vector3 direction = transform.forward;

        lr.positionCount = 1;
        lr.SetPosition(0, origin);

        // レーザーの色切り替え
        lr.material.color = InventoryItemSpawn.IsFrameActive ? Color.cyan : Color.red;

        int points = 1;

        for (int i = 0; i < maxReflections; i++)
        {
            Ray ray = new Ray(origin, direction);
            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, reflectLayers | stopLayers))
            {
                origin = hit.point;
                lr.positionCount = points + 1;
                lr.SetPosition(points, origin);
                points++;

                // RopeCuttable を取得して切断
                RopeCuttable rope = hit.collider.GetComponent<RopeCuttable>();
                if (rope != null)
                {
                    rope.Cut();
                }

                // 反射処理
                if (((1 << hit.collider.gameObject.layer) & reflectLayers) != 0)
                {
                    direction = Vector3.Reflect(direction, hit.normal);
                }
                else
                {
                    // 通常モードのみ落下を発生させる
                    if (!InventoryItemSpawn.IsFrameActive)
                    {
                        Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
                        if (rb != null)
                        {
                            rb.isKinematic = false;
                            rb.useGravity = true;
                        }
                    }
                    break;
                }
            }
            else
            {
                origin += direction * maxDistance;
                lr.positionCount = points + 1;
                lr.SetPosition(points, origin);
                break;
            }
        }
    }
}
