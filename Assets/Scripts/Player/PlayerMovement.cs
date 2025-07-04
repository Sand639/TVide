using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerMovement : MonoBehaviour
{
    //プレイヤーのRigidBody
    private Rigidbody rb;

    //移動の方向
    private Vector3 moveDir;

    //プレイヤーの移動のみの速度
    private Vector3 playerVelocity;
    //縦方向の速度　(重力やジャンプなど)
    private float verticalVelocity = 0f;
    //その他の速度
    //ノックバックの速度
    private Vector3 knockbackVelocity;


    [Header("ジャンプ設定")]
    public Transform groundCheck;
    public float groundCheckDistance = 0.3f;
    public LayerMask groundLayer;

    [Header("地面レイヤー設定")]
    private bool isGrounded;

    [Header("除外レイヤー設定")]
    public LayerMask excludedGroundLayer;


    //プレイヤーがノックバック中かどうか
    private bool isKnockback = false;




    void Start()
    {
        rb = GetComponent<Rigidbody>(); //RigidBodyの取得

    }

    //毎フレーム呼ばれるためプレイヤーのキー入力を直ぐに反映させる
    void Update()
    {
        /*******************************************************
        *  フラグ判定処理
        ******************************************************/

        //プレイヤー停止中　または　ノックバック中ならこの後の処理を行わない
        if (PlayerManager.Instance.isStop || isKnockback)
        {
            //プレイヤー停止中ならプレイヤーの動きを停止させる
            rb.isKinematic = PlayerManager.Instance.isStop;
            //Debug.Log("停止中");
            return;
        }

        rb.isKinematic = false; //プレイヤーを動かせるようにする

        /*******************************************************
        *  キー入力判定処理
        ******************************************************/

        //毎回移動方向をリセット
        moveDir = new Vector3(0.0f, 0.0f, 0.0f);

        //移動入力

        //Z方向の移動はカメラ切替(3D)のスキルが有効な時のみ入力可能
        if (GameManager.Instance.isCameraSkill) 
        {
            if (PlayerManager.Instance.movementKeys.up)     //Wキー
            {
                moveDir.z += 1.0f;    //奥方向の移動値を加算
                //Debug.Log("Wキー");
            }
            if (PlayerManager.Instance.movementKeys.down)   //Sキー
            {
                moveDir.z += -1.0f;   //手前方向の移動値を加算
                //Debug.Log("Sキー");
            }
        }
        if (PlayerManager.Instance.movementKeys.left)   //Aキー
        {
            moveDir.x += -1.0f;   //左方向の移動値を加算
            //Debug.Log("Aキー");
        }
        if (PlayerManager.Instance.movementKeys.right)  //Dキー
        {
            moveDir.x += 1.0f;    //右方向の移動値を加算
            //Debug.Log("Dキー");
        }

        // ジャンプ入力
        // 雨イベント中はジャンプ力低下、空中ジャンプ許可
        if ((isGrounded || PlayerManager.Instance.isRain) && Input.GetKeyDown(KeyCode.Space))
        {
            float jump = 0.0f;
            //Debug.Log("ジャンプ中");

            //雨発生中のジャンプ力を下げる
            if (PlayerManager.Instance.isRain) { jump = PlayerManager.Instance.jumpPower * 0.5f; }
            else jump = PlayerManager.Instance.jumpPower;

            //縦方向の速度にジャンプの速度を加算する
            verticalVelocity = jump;
        }

    }

    //パソコンの性能やフレームレートによって差がでないようにFixedUpdateで値変更処理を行う
    private void FixedUpdate()
    {
        /*******************************************************
        *  フラグ判定処理
        ******************************************************/

        //プレイヤーが停止中ならこの後の処理を行わない
        if (PlayerManager.Instance.isStop) { return; }


        /*******************************************************
        *  地面判定
        ******************************************************/


        isGrounded = false;

        Vector3 boxHalfExtents = new Vector3(0.69f, 0.05f, 0.69f); // 横に広く、縦に薄い
        Vector3 boxCenter = groundCheck.position - Vector3.up * (groundCheckDistance * 0.5f);

        Collider[] hitColliders = Physics.OverlapBox(boxCenter, boxHalfExtents, Quaternion.identity, groundLayer);

        foreach (var col in hitColliders)
        {
            if (((1 << col.gameObject.layer) & excludedGroundLayer) == 0)
            {
                isGrounded = true;
                Debug.Log("OverlapBox 地面着地中: " + col.name);
                break;
            }
        }
        if (!isGrounded)
        {
            Debug.Log("OverlapBox 空中判定中");
        }

        // groundCheck の位置を中心に、groundCheckDistance の半径で球体（OverlapSphere）を作り、範囲内のレイヤーを取得
        //Collider[] hits = Physics.OverlapSphere(groundCheck.position, groundCheckDistance, groundLayer);


        //isGrounded = false; //毎回地面着地フラグをリセットする
        //Debug.Log("OverlapSphereのヒット数: " + hits.Length);

        //foreach (var hit in hits)   //取得した全てのレイヤーを調べる
        //{
        //    // 除外対象のレイヤーでなければ（= 通常の地面であれば）isGrounded を true にする
        //    if (((1 << hit.gameObject.layer) & excludedGroundLayer) == 0)
        //    {
        //        Debug.Log("ヒット対象: " + hit.gameObject.name + " Layer: " + LayerMask.LayerToName(hit.gameObject.layer));
        //        isGrounded = true;  //地面着地フラグを立てる
        //        break;  //一つでも地面レイヤーがあったならfor文から抜ける
        //    }
        //}

        /*******************************************************
        *  重力処理
        ******************************************************/

        //地面に着地中
        if (isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;  // 地面に押し付ける
            //Debug.Log("地面着地中");
        }
        else
        {
            verticalVelocity -= PlayerManager.Instance.jumpGravity * Time.fixedDeltaTime;  // 上昇中
           // Debug.Log("空中");
        }
        //else if (verticalVelocity > 0f)
        //{   // 上昇中（ジャンプ中）→ 少しだけ重力
        //    verticalVelocity -= PlayerManager.Instance.jumpGravity * Time.fixedDeltaTime;  // 上昇中
        //    Debug.Log("ジャンプ中");
        //}
        //else
        //{   // 落下中 → 強い重力
        //    verticalVelocity -= PlayerManager.Instance.fallGravity * Time.fixedDeltaTime;  // 落下中
        //    Debug.Log("落下中");
        //}

        /*******************************************************
         *  移動処理 
         ******************************************************/

        Vector3 dir = moveDir.normalized;                       //念のため方向を正規化
        //最大速度 通常移動 + 風の速度
        float targetSpeed = PlayerManager.Instance.moveSpeed + PlayerManager.Instance.windSpeed;

        if (!isKnockback)//ノックバック中はプレイヤーの移動速度を加算させない
        {

            //現在の移動モードによって処理を変更
            switch (PlayerManager.Instance.movementMode)
            {
                case MovementMode.NormalMode:   //通常移動
                default:    //デフォルトで通常移動

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

                    break;
                case MovementMode.Inertia:  //一旦モード変更できるかを試すために作成したやつ

                    playerVelocity += dir * targetSpeed * Time.fixedDeltaTime;

                    break;
            }

        }


        // 壁との接触判定用
        RaycastHit hitRay;
        bool isTouchingWall = Physics.Raycast(transform.position, dir, out hitRay, 0.6f, groundLayer);

        if (isTouchingWall)
        {
            // 壁にぶつかっている方向の成分を除外
            Vector3 wallNormal = hitRay.normal;
            dir = Vector3.ProjectOnPlane(dir, wallNormal); // 壁面に沿った移動方向に修正
        }

        /*******************************************************
        *  最終的な速度の計算
        ******************************************************/

        //Debug.Log("計算前velocity: " + rb.linearVelocity);

        //移動の速度とその他の速度を加算する
        Vector3 velocity = playerVelocity + knockbackVelocity;
        //縦方向の速度を加算する
        velocity.y = verticalVelocity;
        //最終的な速度をRigidBodyに代入する
        rb.linearVelocity = velocity;


        //Debug.Log("最終的なvelocity: " + velocity);

        /*******************************************************
        *  速度の減衰計算
        ******************************************************/

        if (isGrounded) //地面にいる時の減衰をかける
        {
            playerVelocity = Vector3.Lerp(playerVelocity, Vector3.zero, PlayerManager.Instance.groundDrag);
        }
        else //空中にいる時の減衰をかける
        {
            playerVelocity = Vector3.Lerp(playerVelocity, Vector3.zero, PlayerManager.Instance.airDrag);
        }


        /*******************************************************
        *  その他の処理
        ******************************************************/

        HitFallLine();  //プレイヤーを移動させた後、落下していないかを判定する
    }

    private void HitFallLine()   //プレイヤーが落下判定化を走査し、復活させるか判断する関数
    {
        //プレイヤーが落下判定のラインを超えているなら
        if (transform.position.y < PlayerManager.Instance.fallLine)
        {
            Respawn();  //プレイヤーを復活
        }
    }

    //プレイヤーを保存している復活地点に復活させる関数
    public void Respawn()
    {
        //プレイヤーの座標を復活地点へ移動させる
        transform.position = PlayerManager.Instance.respawnPoint.position;

        //プレイヤーの速度をリセットさせる
        ResetVelocity();
    }

    //プレイヤーの速度とその他の速度をリセットさせる
    private void ResetVelocity()
    {
        ResetPlayerVelocity();  //プレイヤーの速度をリセットする
        ResetOthersVelocity();  //その他の速度をリセットする
    }

    //プレイヤーの速度のみをリセットさせる
    private void ResetPlayerVelocity()
    {
        rb.linearVelocity = Vector3.zero;   //RigidBodyの速度をリセットする
        playerVelocity = Vector3.zero;      //プレイヤーの移動キーによる速度をリセットする
        verticalVelocity = 0f;              //ジャンプ(重力含む)の速度をリセットする
    }

    //その他の速度を全てリセットする
    private void ResetOthersVelocity()
    {
        knockbackVelocity = Vector3.zero;   //ノックバックの速度をリセットする
    }

    //ノックバックの呼出し関数
    public void KnockBack(Vector3 direction, float force)
    {
        if (!isKnockback)   //ノックバックフラグがfalseの時にノックバックを開始する
        {
            isKnockback = true;     //ノックバック中のフラグを立てる
            ResetPlayerVelocity();  //プレイヤーの速度を全部リセットする
            knockbackVelocity = direction * force;  //ノックバックの速度を計算する
            //ノックバックの処理を開始させる
            StartCoroutine(KnockbackCoroutine(PlayerManager.Instance.knockbackDuration));
        }
    }

    //ノックバックの処理
    private IEnumerator KnockbackCoroutine(float duration)
    {
        yield return new WaitForSeconds(duration);  //ノックバックを継続する時間待つ
        knockbackVelocity = Vector3.zero;   //ノックバック終了後、ノックバックの速度をリセットする
        isKnockback = false;    //ノックバック中のフラグを下げる
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Vector3 size = new Vector3(1.38f, 0.1f, 1.38f); // 2倍の値（Boxの全体サイズ）
            Vector3 center = groundCheck.position - Vector3.up * (groundCheckDistance * 0.5f);
            Gizmos.DrawWireCube(center, size);
        }
    }

}

