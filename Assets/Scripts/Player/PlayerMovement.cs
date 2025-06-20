using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //プレイヤーのRigidBody
    private Rigidbody rb;

    //移動の方向
    private Vector3 movePower;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //毎回移動方向をリセット
        movePower = new Vector3(0.0f, 0.0f, 0.0f);

        if (PlayerManager.Instance.movementKeys.up)     //Wキー
        {
            movePower.z += 1.0f;    //奥方向の移動値を加算
        }
        if (PlayerManager.Instance.movementKeys.down)   //Sキー
        {
            movePower.z += -1.0f;   //手前方向の移動値を加算
        }
        if (PlayerManager.Instance.movementKeys.left)   //Aキー
        {
            movePower.x += -1.0f;   //左方向の移動値を加算
        }
        if (PlayerManager.Instance.movementKeys.right)  //Dキー
        {
            movePower.x += 1.0f;    //右方向の移動値を加算
        }
    }

    private void FixedUpdate()
    {




        Vector3 finalVelocity = new Vector3(0.0f,0.0f,0.0f);

        //最終的なVelocityを加算させる
        rb.linearVelocity = finalVelocity;

        //ジャンプ

    }
}
