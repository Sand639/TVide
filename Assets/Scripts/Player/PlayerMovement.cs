using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //プレイヤーのRigidBody
    private Rigidbody rb;

    //移動の方向
    private Vector3 moveDir;

    //プレイヤーの移動のみvelocity
    private Vector3 playerVelocity;
    //別のオブジェクトのvelocity 数を増やしたら簡単で使いやすいと思う　多分たすだけ
    [HideInInspector] public Vector3 externalVelocity;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

    }

    void Update()
    {
        //毎回移動方向をリセット
        moveDir = new Vector3(0.0f, 0.0f, 0.0f);

        if (PlayerManager.Instance.movementKeys.up)     //Wキー
        {
            moveDir.z += 1.0f;    //奥方向の移動値を加算
        }
        if (PlayerManager.Instance.movementKeys.down)   //Sキー
        {
            moveDir.z += -1.0f;   //手前方向の移動値を加算
        }
        if (PlayerManager.Instance.movementKeys.left)   //Aキー
        {
            moveDir.x += -1.0f;   //左方向の移動値を加算
        }
        if (PlayerManager.Instance.movementKeys.right)  //Dキー
        {
            moveDir.x += 1.0f;    //右方向の移動値を加算
        }

        // drag設定（必要なら）
        if (PlayerManager.Instance.movementMode == MovementMode.Inertia)
            rb.linearDamping = 0f; // 減衰強め
        else
            rb.linearDamping = 0f; // 即止まる

    }

    private void FixedUpdate()
    {
        Vector3 dir = moveDir.normalized;
        float targetSpeed = PlayerManager.Instance.moveSpeed;   //最大速度

        switch (PlayerManager.Instance.movementMode)
        {
            case MovementMode.ConstantSpeed:
            default:    //通常移動

                if (dir != Vector3.zero)    //移動してないなら力を加えない
                {
                    // 現在速度の movePower 方向成分を取得
                    float currentSpeed = Vector3.Dot(playerVelocity, dir);

                    // 現在の移動方向への速度が目標より遅いときのみ加速
                    if (currentSpeed < targetSpeed)
                    {
                        //現在の速度と目標速度の差分を取得
                        float speedDiff = targetSpeed - currentSpeed;

                        // 力 = 差分 × 加速度係数（任意）
                        float accelerationFactor = 0.2f; // 調整可
                        Vector3 force = dir * speedDiff * accelerationFactor;

                        //力を加える
                        playerVelocity += force;
                    }
                }
                else
                {
                    // プレイヤー入力による速度を自然減衰させる（摩擦）
                    playerVelocity = Vector3.Lerp(playerVelocity, Vector3.zero, 0.2f);
                }

                break;
            case MovementMode.Inertia:

                playerVelocity += dir * targetSpeed * Time.fixedDeltaTime;

                break;
        }



        // 合成してセット（Y方向は元のrb.velocityを保持）
        Vector3 velocity = playerVelocity + externalVelocity;
        velocity.y = rb.linearVelocity.y;
        rb.linearVelocity = velocity;

        //Vector3 finalVelocity = new Vector3(0.0f,0.0f,0.0f);

        //最終的なVelocityを加算させる
        //rb.linearVelocity = finalVelocity;

        //ジャンプ

    }


}