using UnityEngine;

public class ObjectClick : MonoBehaviour
{
    public float moveSpeed;

    void Update()
    {
        Vector3 move = Vector3.zero;

        if (MonitorZoomController.isZoomComplete && MonitorZoomController.CurrentZoomIndex == 1)
        {

            if (Input.GetKey(KeyCode.W))
            {
                move += Vector3.up;
            }
            if (Input.GetKey(KeyCode.S))
            {
                move += Vector3.down;
            }
            if (Input.GetKey(KeyCode.A))
            {
                move += Vector3.left;
            }
            if (Input.GetKey(KeyCode.D))
            {
                move += Vector3.right;
            }

            if (Input.GetKeyDown(KeyCode.T))
            {
                float detectionRadius = 3.2f; //周囲の検出半径
                Vector3 center = transform.position;

                Collider[] hits = Physics.OverlapSphere(center, detectionRadius);

                int eventLayer = LayerMask.NameToLayer("Event");

                foreach (var hit in hits)
                {
                    if (hit.gameObject.layer == eventLayer)
                    {
                        Debug.Log("Eventレイヤーのオブジェクトを削除: " + hit.gameObject.name);
                        Destroy(hit.gameObject);
                    }
                }
            }
        }

        transform.Translate(move.normalized * moveSpeed * Time.deltaTime, Space.World);
    }
}