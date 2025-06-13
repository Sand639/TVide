using UnityEngine;
using System.Collections;

public class Point_Move_Enemy : Enemy_Base
{
    // ウェイポイントの配列（インスペクターで設定）
    public Transform[] waypoints;

    // 現在のウェイポイントのインデックス
    public int currentIndex = 0;


    public override void Move()
    {
        if (GetMove())
        {
            if (waypoints == null || waypoints.Length == 0)
            {
                //return;
            }
            Vector3 targetPosition = waypoints[currentIndex].position;
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                currentIndex += direction;

                if (currentIndex >= waypoints.Length)
                {
                    currentIndex = waypoints.Length - 2;
                    direction = -1;
                }
                else if (currentIndex < 0)
                {
                    currentIndex = 1;
                    direction = 1;
                }
            }
        }// 移動
        else
        {

        }
    }

   

    public override void Flip()
    {

    }
}
