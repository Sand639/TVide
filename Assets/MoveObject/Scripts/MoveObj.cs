using UnityEngine;

// ベースクラス
public class MoveObj : MonoBehaviour
{
    private bool move = true;

    // 他の StopObj から停止されている数
    private int globalStopCount = 0;

    void Start()
    {
        Init();
    }

    void Update()
    {
        if (MonitorZoomController.isZoomComplete && MonitorZoomController.CurrentZoomIndex == 0)
        {
            if (move)
            {
                Move();       // 動いていい状態なら移動系処理
            }
            MoveStop();       // 止まっていても毎フレーム動く処理（演出等）
        }
    }

    // 初期化
    public virtual void Init()
    {
    }

    // 動きが止められていないときの処理（移動系の処理）
    public virtual void Move()
    {
    }

    // 動きが止められているときでも動く処理（移動系以外の処理）
    public virtual void MoveStop()
    {
    }

    public void SetMove(bool m)
    {
        move = m;
    }

    public bool GetMove()
    {
        return move;
    }

    // 外部から停止指示を受けた時（StopObj から呼ばれる）
    public void IncrementGlobalStop()
    {
        globalStopCount++;
        if (globalStopCount == 1)
        {
            SetMove(false);  // 最初に停止されたときのみ停止
        }
    }

    // 外部から停止解除を受けた時（StopObj から呼ばれる）
    public void DecrementGlobalStop()
    {
        globalStopCount = Mathf.Max(0, globalStopCount - 1);
        if (globalStopCount == 0)
        {
            SetMove(true);   // 全ての StopObj からの停止が解除されたときのみ再開
        }
    }
}