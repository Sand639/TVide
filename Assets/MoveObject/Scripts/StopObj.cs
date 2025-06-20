using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// MoveObj を継承し、スケール変化と停止機能を持つオブジェクト。
/// StopObj を持たない MoveObj 派生を一括停止／解除する。
/// </summary>
[RequireComponent(typeof(Collider))]
public class StopObj : MoveObj
{
    [Header("スケール＆ライフタイム設定")]
    [Tooltip("拡大にかける時間（秒）")]
    public float scaleUpTime = 1f;
    [Tooltip("待機にかける時間（秒）")]
    public float waitTime = 2f;
    [Tooltip("縮小にかける時間（秒）")]
    public float scaleDownTime = 1f;
    [Tooltip("拡大後の目標スケール")]
    public Vector3 targetScale = Vector3.one;

    private float timer = 0f;

    private enum State { ScalingUp, Waiting, ScalingDown, Done }
    private State state = State.ScalingUp;

    // この StopObj が停止させている MoveObj とそのカウント
    private Dictionary<MoveObj, int> stopCounts = new();

    void Awake()
    {
        Collider col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    public override void Init() { }

    /// <summary>
    /// スケーリングと消滅管理
    /// </summary>
    public override void Move()
    {
        timer += Time.deltaTime;

        switch (state)
        {
            case State.ScalingUp:
                if (scaleUpTime > 0f)
                {
                    float t = Mathf.Clamp01(timer / scaleUpTime);
                    transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, t);
                }
                else
                {
                    transform.localScale = targetScale;
                }

                if (timer >= scaleUpTime)
                {
                    state = State.Waiting;
                    timer = 0f;
                }
                break;

            case State.Waiting:
                if (timer >= waitTime)
                {
                    state = State.ScalingDown;
                    timer = 0f;
                }
                break;

            case State.ScalingDown:
                if (scaleDownTime > 0f)
                {
                    float t = Mathf.Clamp01(timer / scaleDownTime);
                    transform.localScale = Vector3.Lerp(targetScale, Vector3.zero, t);
                }
                else
                {
                    transform.localScale = Vector3.zero;
                }

                if (timer >= scaleDownTime)
                {
                    state = State.Done;
                    Destroy(gameObject);
                }
                break;

            case State.Done:
                break;
        }
    }

    public override void MoveStop()
    {
        // 停止時は何もしない
    }

    /// <summary>
    /// Trigger 進入時に MoveObj を停止
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        MoveObj mo = other.GetComponent<MoveObj>();
        if (mo != null && mo.GetComponent<StopObj>() == null && mo != this)
        {
            if (!stopCounts.ContainsKey(mo)) stopCounts[mo] = 0;
            stopCounts[mo]++;

            // global 停止カウント増加
            mo.IncrementGlobalStop();
        }
    }

    /// <summary>
    /// Trigger 離脱時に停止解除判定
    /// </summary>
    void OnTriggerExit(Collider other)
    {
        MoveObj mo = other.GetComponent<MoveObj>();
        if (mo != null && stopCounts.ContainsKey(mo))
        {
            stopCounts[mo]--;
            mo.DecrementGlobalStop();

            if (stopCounts[mo] <= 0)
            {
                stopCounts.Remove(mo);
            }
        }
    }

    /// <summary>
    /// 自身の破棄時に関係していたすべての対象に停止解除
    /// </summary>
    void OnDestroy()
    {
        foreach (var kv in stopCounts)
        {
            MoveObj mo = kv.Key;
            int count = kv.Value;

            for (int i = 0; i < count; i++)
            {
                mo.DecrementGlobalStop();
            }
        }
        stopCounts.Clear();
    }
}