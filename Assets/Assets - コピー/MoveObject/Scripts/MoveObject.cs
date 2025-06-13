using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    private bool move = true;

    // オブジェクトが削除されるまでの時間（秒）
    public float lifeTime = 5f;

    // スケールの増加にかける時間（秒）
    public float scaleUpTime = 1f;

    // スケールの減少にかける時間（秒）
    public float scaleDownTime = 1f;

    // 目標とするスケール
    public Vector3 targetScale = Vector3.one;

    // このオブジェクトが停止させているオブジェクトとその重複カウント
    private Dictionary<GameObject, int> stoppedObjects = new Dictionary<GameObject, int>();

    // このオブジェクトが他のオブジェクトによって停止されている回数
    private int beingStoppedCount = 0;

    private float elapsedTime = 0f;
    private enum ScaleState { ScalingUp, Waiting, ScalingDown, Done }
    private ScaleState currentState = ScaleState.ScalingUp;

    public void Start()
    {
        Init();
    }


    public void Update()
    {
        Move();
    }

    public virtual void Init()
    {
        transform.localScale = Vector3.zero;
    }

    public virtual void Move()
    {
        // 派生クラスではそれぞれのクラスの動きを書く
        elapsedTime += Time.deltaTime;

        switch (currentState)
        {
            case ScaleState.ScalingUp:
                if (scaleUpTime > 0f)
                {
                    float t = Mathf.Clamp01(elapsedTime / scaleUpTime);
                    transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, t);
                    if (t >= 1f)
                    {
                        currentState = ScaleState.Waiting;
                        elapsedTime = 0f;
                    }
                }
                else
                {
                    transform.localScale = targetScale;
                    currentState = ScaleState.Waiting;
                    elapsedTime = 0f;
                }
                break;

            case ScaleState.Waiting:
                float waitTime = lifeTime - scaleUpTime - scaleDownTime;
                if (elapsedTime >= waitTime)
                {
                    currentState = ScaleState.ScalingDown;
                    elapsedTime = 0f;
                }
                break;

            case ScaleState.ScalingDown:
                if (scaleDownTime > 0f)
                {
                    float t = Mathf.Clamp01(elapsedTime / scaleDownTime);
                    transform.localScale = Vector3.Lerp(targetScale, Vector3.zero, t);
                    if (t >= 1f)
                    {
                        currentState = ScaleState.Done;
                        Destroy(gameObject);
                    }
                }
                else
                {
                    transform.localScale = Vector3.zero;
                    currentState = ScaleState.Done;
                    Destroy(gameObject);
                }
                break;

            case ScaleState.Done:
                // 何もしない
                break;
        }

    }

    public void SetMove(bool b)
    {
        move = b;
    }

    public bool GetMove()
    {
        return move;
    }

    public virtual void OnTriggerEnter(Collider c)
    {
        if (c != null)
        {
            GameObject other = c.gameObject;

            // このオブジェクトが他のオブジェクトを停止させる
            MoveObject[] components = other.GetComponents<MoveObject>();
            foreach (MoveObject component in components)
            {
                if (component != this)
                {
                    component.IncrementBeingStoppedCount();
                    if (!stoppedObjects.ContainsKey(other))
                    {
                        stoppedObjects[other] = 1;
                    }
                    else
                    {
                        stoppedObjects[other]++;
                    }
                }
            }
        }
    }

    public virtual void OnTriggerExit(Collider c)
    {
        if (c != null)
        {
            GameObject other = c.gameObject;

            // このオブジェクトが他のオブジェクトを再開させる
            MoveObject[] components = other.GetComponents<MoveObject>();
            foreach (MoveObject component in components)
            {
                if (component != this)
                {
                    component.DecrementBeingStoppedCount();
                    if (stoppedObjects.ContainsKey(other))
                    {
                        stoppedObjects[other]--;
                        if (stoppedObjects[other] <= 0)
                        {
                            stoppedObjects.Remove(other);
                        }
                    }
                }
            }
        }
    }

    private void OnDestroy()
    {
        foreach (var kvp in stoppedObjects)
        {
            GameObject obj = kvp.Key;
            int count = kvp.Value;

            // オブジェクトが存在するかチェック
            if (obj != null)
            {
                MoveObject[] components = obj.GetComponents<MoveObject>();
                foreach (MoveObject component in components)
                {
                    if (component != this)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            component.DecrementBeingStoppedCount();
                        }
                    }
                }
            }
        }
    }

    public void IncrementBeingStoppedCount()
    {
        beingStoppedCount++;
        if (beingStoppedCount == 1)
        {
            SetMove(false);
        }
    }

    public void DecrementBeingStoppedCount()
    {
        beingStoppedCount--;
        if (beingStoppedCount <= 0)
        {
            beingStoppedCount = 0;
            SetMove(true);
        }
    }
}