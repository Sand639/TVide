using UnityEngine;

public class EventObject : MoveObj
{
    private Rigidbody rb;
    private bool hasDropped = false;
    private Transform playerTransform;
    private bool isPlayerNearby = false;

    public float dropDistance;

    

    void OnDestroy()
    {
        MainCamera.OnShake -= TryDropObject;
    }

    public override void Init()     //Startの代わり
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogWarning("オブジェクトが見つかりません。");
        }

        MainCamera.OnShake += TryDropObject;
    }

    public override void Move()     //Updateの代わり
    {
        if (GetMove())
        {
            rb.constraints &= ~RigidbodyConstraints.None;
            rb.constraints = RigidbodyConstraints.FreezePositionX |
                             RigidbodyConstraints.FreezePositionZ |
                             RigidbodyConstraints.FreezeRotationX |
                             RigidbodyConstraints.FreezeRotationY |
                             RigidbodyConstraints.FreezeRotationZ;
            if (!hasDropped && playerTransform != null)
            {
                float distance = Vector3.Distance(transform.position, playerTransform.position);
                isPlayerNearby = distance <= dropDistance;
            }
        }
        else
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

        void TryDropObject()
    {
        if (!hasDropped && isPlayerNearby)
        {
            DropObject();
        }
    }

    void DropObject()
    {
        rb.isKinematic = false;
        hasDropped = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (hasDropped && collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

}


