using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CameraCoroutine : MonoBehaviour
{
    public UnityEvent changeSelectFalse;
    public UnityEvent changeSelectTrue;

    public Transform pointA; // 移動先の1つ目の位置
    public Transform pointB; // 移動先の2つ目の位置
    public float duration = 1.0f; // 移動にかける時間

    private Coroutine moveCoroutine;
    private bool isMovingToA = true; // 初期状態でpointAに向かう
    private bool isMoving = false; // カメラが移動中かどうか

    // カメラを移動させるメソッド
    public void MoveCamera()
    {
        if (isMoving) // 移動中の場合は再呼び出しを無視
        {
            return;
        }

        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        moveCoroutine = StartCoroutine(MoveToTarget());
    }

    private IEnumerator MoveToTarget()
    {
        isMoving = true; // 移動を開始するのでフラグを立てる
        if (isMovingToA)
        {
            changeSelectFalse.Invoke();
        }
        Transform target = isMovingToA ? pointA : pointB;
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, target.position, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = target.position;

        if(!isMovingToA)
        {
            changeSelectTrue.Invoke();
        }
        

        isMovingToA = !isMovingToA; // 次回は逆のポイントに移動

        isMoving = false; // 移動が完了したらフラグを戻す
    }
}
