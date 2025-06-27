using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_Base : MoveObj
{
    //[System.NonSerialized]
    public float speed = 2f;             // 移動速度
    public float timer;                 // タイマー
    public int direction = 1;           // 移動方向（1:右, -1:左）

    public override void Move()
    {
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);
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

    protected virtual void HitPlayer(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerManager player = collision.gameObject.GetComponent<PlayerManager>();
            if (player != null)
            {
                Vector3 knockDirection = (collision.transform.position - transform.position).normalized;
                float knockForce = 10.0f;
                player.ApplyKnockBack(knockDirection, knockForce);

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