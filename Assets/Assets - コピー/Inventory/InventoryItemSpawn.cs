using UnityEngine;

public class InventoryItemSpawn : MonoBehaviour
{
    public static bool IsFrameActive = false;

    [Header("プレハブ設定")]
    public GameObject framePrefab;

    [Header("生成位置設定")]
    public Transform playerCamera;
    public float frameSpawnDistance = 3.0f;

    [Header("枠の移動設定")]
    public float frameMoveSpeed = 8.0f;

    [Header("ブロック生成オフセット")]
    public float blockOffsetZ = 12.5f;

    [Header("インベントリ参照")]
    public InventoryManager inventoryManager;

    private GameObject currentFrame;

    void Update()
    {
        if (MonitorZoomController.isZoomComplete && MonitorZoomController.CurrentZoomIndex == 0)
        {
            HandleFrameToggle();
            HandleFrameMovement();
            HandleBlockPlacement();
        }
    }

    void HandleFrameToggle()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentFrame == null)
            {
                Vector3 spawnPos = playerCamera.position + playerCamera.forward * frameSpawnDistance;
                Quaternion spawnRot = Quaternion.LookRotation(playerCamera.forward);

                currentFrame = Instantiate(framePrefab, spawnPos, spawnRot);
                IsFrameActive = true;
            }
            else
            {
                Destroy(currentFrame);
                currentFrame = null;
                IsFrameActive = false;
            }
        }
    }

    void HandleFrameMovement()
    {
        if (currentFrame == null) return;

        Vector3 move = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) move += Vector3.up;
        if (Input.GetKey(KeyCode.S)) move += Vector3.down;
        if (Input.GetKey(KeyCode.A)) move += Vector3.left;
        if (Input.GetKey(KeyCode.D)) move += Vector3.right;

        currentFrame.transform.Translate(move.normalized * frameMoveSpeed * Time.deltaTime, Space.World);
    }

    void HandleBlockPlacement()
    {
        if (currentFrame != null && Input.GetKeyDown(KeyCode.Return))
        {
            if (inventoryManager != null && inventoryManager.HasAnyItem())
            {
                Vector3 blockPos = currentFrame.transform.position + currentFrame.transform.forward * blockOffsetZ;
                Quaternion blockRot = currentFrame.transform.rotation;

                GameObject itemPrefab = inventoryManager.GetFirstAvailableItemPrefab();
                if (itemPrefab != null)
                {
                    Instantiate(itemPrefab, blockPos, blockRot);

                    // 使用後に元のオブジェクトを再表示
                    inventoryManager.RespawnSceneItem(itemPrefab);

                    inventoryManager.ConsumeFirstAvailableItem();
                }
            }
            else
            {
                Debug.Log("インベントリにアイテムがありません！");
            }
        }
    }
}
