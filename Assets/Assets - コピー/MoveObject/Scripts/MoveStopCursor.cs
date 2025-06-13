using UnityEngine;

public class MoveStopCursor : MonoBehaviour
{
    public static bool IsFrameActive = false; // プレイヤー制御用フラグ

    [Header("プレハブ設定")]
    public GameObject StopCursor;
    public GameObject StopArea;

    [Header("生成位置設定")]
    public Transform playerCamera;
    public float frameSpawnDistance = 3.0f;

    [Header("枠の移動設定")]
    public float frameMoveSpeed = 8.0f;

    [Header("ブロック生成オフセット")]
    public float blockOffsetZ = 12.5f; // Zのオフセットの設定

    private GameObject currentFrame;
    private GameObject stopArea;

    public Vector3 scale = Vector3.one;

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
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (currentFrame == null)
            {
                Vector3 spawnPos = playerCamera.position + playerCamera.forward * frameSpawnDistance;
                Quaternion spawnRot = Quaternion.LookRotation(playerCamera.forward);

                currentFrame = Instantiate(StopCursor, spawnPos, spawnRot);
                currentFrame.transform.localScale = scale;
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

    //移動
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

    //枠が生成されている場合、設置可能
    void HandleBlockPlacement()
    {
        if (currentFrame != null && Input.GetKeyDown(KeyCode.Return))
        {
            Vector3 blockPos = currentFrame.transform.position + currentFrame.transform.forward * blockOffsetZ;
            Quaternion blockRot = currentFrame.transform.rotation;

            stopArea = Instantiate(StopArea, blockPos, blockRot);
            //stopArea.transform.localScale = scale;

            //生成されたらカーソルを消す。
            Destroy(currentFrame);
            currentFrame = null;
            IsFrameActive = false;
        }
    }
}
