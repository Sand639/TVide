using UnityEngine;

/// <summary>
/// プレイヤーが範囲内に入ったらcanvas1を描画、
/// 範囲内にいてHキーを押したらcanvas2を描画、
/// 範囲外に出たら両方とも非表示にするスクリプト。
/// 
/// 範囲の中心点を任意のTransformで指定可能。
/// </summary>
public class BoardController : MonoBehaviour
{
    [Header("範囲の中心点（未設定なら自分自身）")]
    [SerializeField] private Transform centerPointTransform;

    [Header("プレイヤーを検知する半径")]
    [SerializeField] private float detectionRadius = 5f;

    [Header("プレイヤーのTag")]
    [SerializeField] private string playerTag = "Player";

    [Header("範囲内で表示するCanvas")]
    [SerializeField] private GameObject canvas1;

    [Header("Hキーを押すと表示するCanvas")]
    [SerializeField] private GameObject canvas2;

    private GameObject player;
    private bool isPlayerInRange = false;

    private void Start()
    {
        // 最初は両方とも非表示にしておく
        if (canvas1 != null) canvas1.SetActive(false);
        if (canvas2 != null) canvas2.SetActive(false);

        // centerPointTransform が null なら自分自身を使う
        if (centerPointTransform == null)
        {
            centerPointTransform = this.transform;
        }
    }

    private void Update()
    {
        FindPlayer();

        if (player != null)
        {
            float distance = Vector3.Distance(centerPointTransform.position, player.transform.position);
            isPlayerInRange = distance <= detectionRadius;

            if (isPlayerInRange)
            {
                if (canvas1 != null) canvas1.SetActive(true);

                if (Input.GetKeyDown(KeyCode.H))
                {
                    if (canvas2 != null) canvas2.SetActive(true);
                }
            }
            else
            {
                if (canvas1 != null) canvas1.SetActive(false);
                if (canvas2 != null) canvas2.SetActive(false);
            }
        }
    }

    private void FindPlayer()
    {
        if (player == null)
        {
            GameObject foundPlayer = GameObject.FindGameObjectWithTag(playerTag);
            if (foundPlayer != null)
            {
                player = foundPlayer;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        Vector3 centerPos = centerPointTransform != null ? centerPointTransform.position : transform.position;
        Gizmos.DrawWireSphere(centerPos, detectionRadius);
    }
}
