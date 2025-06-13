using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_Base : MoveObject
{
    //[System.NonSerialized]
    public float speed = 2f;             // 移動速度
    //public float walkTime = 2f;          // 歩く時間
    public float timer;                 // タイマー
    public int direction = 1;           // 移動方向（1:右, -1:左）

    // Start is called before the first frame update
    public void Start()
    {
        //timer = walkTime;
        //Attack();
    }

    // Update is called once per frame
    public void Update()
    {
        if (MonitorZoomController.isZoomComplete && MonitorZoomController.CurrentZoomIndex == 0)
        {
            // 移動
            Move();

            //当たり判定処理
            OnCollisionEnter(null);

            // タイマー更新
            TimerUpdate();
          
        }
    }


    public override void Move()
    {
        // 移動
        if (GetMove())
        {
            transform.Translate(Vector2.right * direction * speed * Time.deltaTime);
        }
    }


    public void TimerUpdate()
    {
        // タイマー更新
        //timer -= Time.deltaTime;
    }

    // スプライトの向きを反転（2D用）
    public virtual void FlipSprite()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    // エネミーの方向転換
    public virtual void Flip()
    {
        direction *= -1; // 方向転換
        FlipSprite(); // 見た目も反転（必要なら）   
    }

    //playerと接触したら
    protected virtual void OnCollisionEnter(Collision collision)
    {
        
        if (collision != null)
        {
            HitPlayer(collision);
            HitWall(collision);
        }
    }


    protected virtual void HitPlayer(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerTest player = collision.gameObject.GetComponent<PlayerTest>();
            if (player != null)
            {
                Vector3 knockDirection = (collision.transform.position - transform.position).normalized;
                float knockForce = 10.0f;
                player.KnockBack(knockDirection, knockForce);

            }
            Destroy(gameObject);
        }
    }
    

    protected virtual void HitWall(Collision collision)
    {
        if(collision.gameObject.CompareTag("Wall")||collision.gameObject.CompareTag("WoodBox"))
        {
            Flip();
        }
    }


}