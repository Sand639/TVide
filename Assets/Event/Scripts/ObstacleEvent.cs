using UnityEngine;

public class ObstacleEvent : MoveObj

{
    private Rigidbody rb;
    private bool hasDropped = false;
    private Transform playerTransform;
    private bool isPlayerNearby = false;

    public float dropDistance;


    public override void Init()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;

        if (gameObject.CompareTag("WoodBox"))
        {
            Physics.gravity = new Vector3(0, -30f, 0);
        }


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


    void OnDestroy()
    {
        MainCamera.OnShake -= TryDropObject;
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
}


