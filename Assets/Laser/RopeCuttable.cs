using UnityEngine;

public class RopeCuttable : MonoBehaviour
{
    public GameObject ropeObject; // Ropeオブジェクト（Cylinderなど）
    public Rigidbody blockRigidbody; // 紐につながれてるブロック

    private bool isCut = false;

    public void Cut()
    {
        if (isCut) return;

        isCut = true;

        if (ropeObject != null)
            Destroy(ropeObject); // 紐を非表示に

        if (blockRigidbody != null)
        {
            blockRigidbody.isKinematic = false;
            blockRigidbody.useGravity = true;
        }
    }
}
