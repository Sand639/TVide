using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MoveObj 継承クラス：複数地点を巡回する動くオブジェクト。
/// 往復（Ping‑Pong）またはループ移動を選択でき、
/// プレイヤーが乗ると一緒に移動します。
/// 初期位置はポイントリストの最初に設定されます。
/// </summary>
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))] // 必須：物理衝突のため
public class MoveProp : MoveObj
{
    [Header("移動設定")]
    [Tooltip("移動するTransformポイントを順番に指定")]
    public List<Transform> points = new List<Transform>();

    [Tooltip("1秒間の移動距離")]
    public float speed = 1f;

    [Tooltip("チェック = 往復移動（Ping‑Pong）、未チェック = ループ移動")]
    public bool pingPong = true;

    // 現在のポイントインデックスおよび方向
    private int currentIndex = 0;
    private int direction = 1;

    // 前フレームの位置と移動差分
    private Vector3 lastPosition;
    private Vector3 deltaMove;

    private void Awake()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;

        Collider col = GetComponent<Collider>();
        if (col.isTrigger)
        {
            Debug.LogWarning("MovePropのColliderは Trigger にしないでください。Triggerでは物理衝突が発生せずOnCollision系イベントは呼ばれません。");
        }
    }

    public override void Init()
    {
        if (points == null || points.Count == 0)
        {
            Debug.LogError($"[{name}] points に少なくとも 1つは Transform を設定してください。");
            enabled = false;
            return;
        }

        transform.position = points[0].position;
        currentIndex = 0;
        direction = 1;

        lastPosition = transform.position;
        deltaMove = Vector3.zero;
    }

    public override void Move()
    {
        lastPosition = transform.position;

        MoveAlongPoints();

        deltaMove = transform.position - lastPosition;
    }

    public override void MoveStop()
    {
        deltaMove = Vector3.zero;
    }

    private void MoveAlongPoints()
    {
        if (points.Count == 0) return;

        Transform target = points[currentIndex];
        Vector3 dirVec = (target.position - transform.position).normalized;
        float step = speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, target.position) <= step)
        {
            transform.position = target.position;
            UpdateIndex();
        }
        else
        {
            transform.position += dirVec * step;
        }
    }

    private void UpdateIndex()
    {
        if (pingPong)
        {
            if (currentIndex == points.Count - 1) direction = -1;
            else if (currentIndex == 0) direction = 1;
            currentIndex += direction;
        }
        else
        {
            currentIndex = (currentIndex + 1) % points.Count;
        }
    }

    /// <summary>
    /// 現在のフレームでの移動量（差分）を返す
    /// </summary>
    public Vector3 GetDeltaMove()
    {
        return deltaMove;
    }
}
