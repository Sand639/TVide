using System.Collections;
using UnityEngine;

public class CameraCoroutine : MonoBehaviour
{
    public Transform pointA; // ズームイン位置
    public Transform pointB; // ズームアウト位置
    public float duration = 1.0f; // 移動時間

    public ChannelManager channelManager; // UI入力管理用

    private Coroutine moveCoroutine;
    public bool isMovingToA = true; // 次にpointAへ
    public bool isMoving = false;

    private void Start()
    {
        // 最初にカメラを現在位置にセットするなどは不要
    }

    // カメラを移動開始
    public void MoveCamera()
    {
        if (isMoving) return;

        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        moveCoroutine = StartCoroutine(MoveToTarget());
    }

    private IEnumerator MoveToTarget()
    {
        isMoving = true;

        Transform target = isMovingToA ? pointA : pointB;
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        // ChannelManager に今の状態を伝える
        if (isMovingToA)
        {
            // pointAに行くときは←→無効, Enter有効
            channelManager.SetInput(false, true);
        }
        else
        {
            // pointBに行くときは←→有効, Enter無効
            channelManager.SetInput(true, false);
        }

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            transform.position = Vector3.Lerp(startPosition, target.position, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 最終補正
        transform.position = target.position;

        isMovingToA = !isMovingToA; // 次は逆に移動
        isMoving = false;
    }
}
