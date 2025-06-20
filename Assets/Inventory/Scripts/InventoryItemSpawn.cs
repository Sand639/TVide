using UnityEngine;
using System.Collections;

public class InventoryItemSpawn : MonoBehaviour
{
    [Header("生成位置の設定")]
    public Transform playerCamera;
    public float spawnDistance = 3.0f;
    public static bool IsFrameActive = false;

    [Header("アイテム移動設定")]
    public float moveSpeed = 8.0f;

    [Header("インベントリ参照")]
    public InventoryManager inventoryManager;

    private GameObject currentItem;

    void Update()
    {
        if (MonitorZoomController.isZoomComplete && MonitorZoomController.CurrentZoomIndex == 0)
        {
            HandleItemSpawn();
            HandleItemMovement();
            HandleItemPlacement();
        }
    }

    void HandleItemSpawn()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // すでに選択中のアイテムがある場合は縮小して削除
            if (currentItem != null)
            {
                StartCoroutine(ShrinkAndDestroyItem(currentItem, 0.3f));
                currentItem = null;
                IsFrameActive = false;
                return;
            }

            // 新しいアイテムを生成
            if (inventoryManager != null && inventoryManager.HasAnyItem())
            {
                GameObject itemPrefab = inventoryManager.GetFirstAvailableItemPrefab();
                if (itemPrefab != null)
                {
                    Vector3 spawnPos = playerCamera.position + playerCamera.forward * spawnDistance;
                    Quaternion spawnRot = Quaternion.LookRotation(playerCamera.forward);

                    currentItem = Instantiate(itemPrefab, spawnPos, spawnRot);

                    //スケールのアニメーションの実行
                    StartCoroutine(AnimateItemScale(currentItem, 0.5f));

                    IsFrameActive = true;
                }
            }
        }
    }


    void HandleItemMovement()
    {//移動処理
        if (currentItem == null) return;

        Vector3 move = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) move += Vector3.up;
        if (Input.GetKey(KeyCode.S)) move += Vector3.down;
        if (Input.GetKey(KeyCode.A)) move += Vector3.left;
        if (Input.GetKey(KeyCode.D)) move += Vector3.right;

        currentItem.transform.Translate(move.normalized * moveSpeed * Time.deltaTime, Space.World);
    }

    //アイテムの設置と元の位置に再生成する
    void HandleItemPlacement()
    {
        if (currentItem != null && Input.GetKeyDown(KeyCode.Return))
        {
            if (inventoryManager != null && inventoryManager.HasAnyItem())
            {
                // 設置位置と回転を取得
                Vector3 blockPos = currentItem.transform.position + currentItem.transform.forward;
                Quaternion blockRot = currentItem.transform.rotation;

                // インベントリからプレハブ取得
                GameObject itemPrefab = inventoryManager.GetFirstAvailableItemPrefab();
                if (itemPrefab != null)
                {
                    // アイテムを設置
                    Instantiate(itemPrefab, blockPos, blockRot);

                    // アイテムクリーナーを呼び出す(inventoryItemのタグ名がついていたら消す)
                    ItemCleaner cleaner = FindObjectOfType<ItemCleaner>();
                    if (cleaner != null)
                    {
                        cleaner.RemoveOneItem();
                    }

                    // 元の場所に再表示（インベントリ用）
                    inventoryManager.RespawnSceneItem(itemPrefab);

                    // インベントリから消費
                    inventoryManager.ConsumeFirstAvailableItem();


                    // 選択中だったアイテムを削除する
                    Destroy(currentItem);
                    currentItem = null;
                    IsFrameActive = false;

                }
            }
            else
            {
                Debug.Log("インベントリにアイテムがありません！");
            }
        }
    }




    //アイテムを0からデフォルトのサイズまで大きくする
    IEnumerator AnimateItemScale(GameObject item, float duration)
    {
        Vector3 targetScale = item.transform.localScale;
        item.transform.localScale = Vector3.zero;

        float time = 0f;
        while (time < duration)
        {
            item.transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, time / duration);
            time += Time.deltaTime * 1.5f;
            yield return null;
        }

        item.transform.localScale = targetScale;
    }

    //アイテムをデフォルトのサイズから0まで小さくする
    IEnumerator ShrinkAndDestroyItem(GameObject item, float duration)
    {
        Vector3 startScale = item.transform.localScale;
        float time = 0f;

        while (time < duration)
        {
            item.transform.localScale = Vector3.Lerp(startScale, Vector3.zero, time / duration);
            time += Time.deltaTime * 1.5f;
            yield return null;
        }

        item.transform.localScale = Vector3.zero;
        Destroy(item);
    }
}
