using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ReflectingLaser : MonoBehaviour
{
    public int maxReflections = 1;
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
        if (MonitorZoomController.isZoomComplete && MonitorZoomController.CurrentZoomIndex == 0)
        {
            Vector3 origin = transform.position;
            Vector3 direction = transform.forward;

            lr.positionCount = 1;
            lr.SetPosition(0, origin);

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

                    // Ç‡ÇµReflectëwÇ»ÇÁíµÇÀï‘ÇÈ
                    if (((1 << hit.collider.gameObject.layer) & reflectLayers) != 0)
                    {
                        direction = Vector3.Reflect(direction, hit.normal);
                    }
                    else
                    {
                        // ReflectëwÇ∂Ç·Ç»Ç¢ÅÅé~Ç‹ÇÈ Å® Rigidbodyóéâ∫
                        Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
                        if (rb != null)
                        {
                            rb.isKinematic = false;
                            rb.useGravity = true;
                        }
                        break;
                    }
                }
                else
                {
                    // ÉqÉbÉgÇ»Çµ Å® Ç‹Ç¡Ç∑ÇÆêLÇŒÇ∑
                    origin += direction * maxDistance;
                    lr.positionCount = points + 1;
                    lr.SetPosition(points, origin);
                    break;
                }
            }
        }
    }
}
