using System.Collections;
using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    private CharacterController controller;

    private Goalcheck goalcheck;
    private PlayerHP gameOver;
    private RainEvent rainEvent;

    [Header("移動設定")]
    public float speed = 15.0f;
    public KeyCode jumpKey = KeyCode.Space;
    public float jumpPower = 10.5f;
    public float gravity = 9.81f;

    [Header("グラウンドチェック設定")]
    public Transform groundCheckTransform;
    public float groundCheckRadius = 0.14f;
    public LayerMask groundLayer;

    [Header("除外するグラウンドチェック")]
    public LayerMask excludedGroundLayer;

    private float horizontalInput = 0f;
    private float verticalInput = 0f;

    private bool wasGrounded = false;
    private bool canJump = false;
    private float verticalVelocity = 0f;

    // 風の影響
    private bool isWindBoosted = false;
    private float windBoostTimer = 0f;
    private float windExtraSpeed = 0f;

    // 雨の影響（空中ジャンプ可能）
    private bool isFloating = false;
    private float floatTimer = 0f;

    [Header("リスポーンポイント")]
    public float fallLine = -10;        // 落下判定になるy座標
    public Transform respornPoint;      // リスポーンポイント

    private bool isKnockback = false;


    public void ApplyWindBoost(float duration, float boostSpeed)
    {
        StartCoroutine(DelayedWindBoost(duration, boostSpeed));
    }

    private IEnumerator DelayedWindBoost(float duration, float boostSpeed)
    {
        yield return new WaitForSeconds(1f); // 1秒後に風の影響を開始
        isWindBoosted = true;
        windBoostTimer = duration;
        windExtraSpeed = boostSpeed;
    }

    public void ApplyRainFloat(float duration)
    {
        isFloating = true;
        floatTimer = duration;
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();
        goalcheck = FindObjectOfType<Goalcheck>();
        gameOver = FindObjectOfType<PlayerHP>();
        rainEvent = FindObjectOfType<RainEvent>();
    }

    void Update()
    {
        if (InventoryItemSpawn.IsFrameActive)
        {
            controller.Move(Vector3.zero); // 必要ならアニメーション停止などもここで
            return;
        }
        if (MoveStopCursor.IsFrameActive)
        {
            controller.Move(Vector3.zero); // 必要ならアニメーション停止などもここで
            return;
        }

        if (!goalcheck.goal)
        {

            if (MonitorZoomController.isZoomComplete && MonitorZoomController.CurrentZoomIndex == 0)
            {

                if (isKnockback)
                {
                    return; // ノックバック中は操作を無効化
                }
                // 入力処理
                if (isWindBoosted)
                {
                    windBoostTimer -= Time.deltaTime;
                    if (windBoostTimer > 0)
                    {
                        bool noInput = !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D);
                        horizontalInput = noInput ? 0.3f : (Input.GetKey(KeyCode.D) ? 1f : 0f);
                    }
                    else
                    {
                        isWindBoosted = false;
                        windExtraSpeed = 0f;
                    }
                }
                else
                {
                    horizontalInput = Input.GetKey(KeyCode.D) ? 1f : (Input.GetKey(KeyCode.A) ? -1f : 0f);
                }

                verticalInput = GameManager.Instance.isCameraSkill
                    ? (Input.GetKey(KeyCode.W) ? 1f : (Input.GetKey(KeyCode.S) ? -1f : 0f))
                    : 0f;

                Vector3 inputMotion = new Vector3(horizontalInput, 0, verticalInput);

                // グラウンドチェック
                bool isGrounded = false;
                if (groundCheckTransform != null)
                {
                    Vector3 checkPos = groundCheckTransform.position;
                    Collider[] hits = Physics.OverlapSphere(checkPos, groundCheckRadius, groundLayer);

                    foreach (var hit in hits)
                    {
                        if (((1 << hit.gameObject.layer) & excludedGroundLayer) == 0)
                        {
                            isGrounded = true;
                            break;
                        }
                    }
                }
                else
                {
                    isGrounded = controller.isGrounded;
                }

                // 雨イベントのタイマー更新
                if (isFloating)
                {
                    floatTimer -= Time.deltaTime;
                    if (floatTimer <= 0f)
                    {
                        isFloating = false;
                    }
                }

                // 重力処理
                if (isGrounded)
                {
                    if (!wasGrounded)
                    {
                        canJump = true;
                    }

                    verticalVelocity = -1f;
                }
                else
                {//Event中の落下速度を30%
                    float currentGravity = isFloating ? gravity * 0.3f : gravity;
                    verticalVelocity -= currentGravity * Time.deltaTime;

                }

                // ジャンプ処理（雨イベント中はフラグ無効）
                if ((canJump || isFloating) && Input.GetKeyDown(jumpKey)/*&&rainEvent.isInsideTrigger*/)
                {
                    float currentJumpPower = isFloating ? jumpPower * 0.5f : jumpPower; // 雨イベント中はジャンプ力70%
                    verticalVelocity = currentJumpPower;

                    if (!isFloating)
                    {
                        canJump = false;
                    }
                }

                // 移動処理
                Vector3 move = inputMotion.normalized * (speed + windExtraSpeed);
                move.y = verticalVelocity;

                controller.Move(move * Time.deltaTime);
                wasGrounded = isGrounded;

                HitFallLine();
            }
        }
    }

    public void HitFallLine()
    {
        // 移動
        Transform myTransform = this.transform;
        Vector3 pos = myTransform.position;
        if (pos.y < fallLine)
        {
            Respawn();
        }
    }

    public void Respawn()
    {
        Transform myTransform = this.transform;
        myTransform.position = respornPoint.position;
    }

    //ノックバックの処理
    public void KnockBack(Vector3 direction, float force)
    {
        if (!isKnockback)
        {
            StartCoroutine(KnockBackRoutine(direction, force));
        }
    }

    private IEnumerator KnockBackRoutine(Vector3 direction, float force)
    {
        isKnockback = true;

        float knockBackTime = 0.3f;
        float timer = 0.0f;

        while (timer < knockBackTime)
        {
            controller.Move(direction.normalized * force * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }

        isKnockback = false;
    }


}
