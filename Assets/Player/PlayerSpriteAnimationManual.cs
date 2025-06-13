using UnityEngine;

public class PlayerSpriteAnimationDirectional : MonoBehaviour
{
    public Sprite[] sprites; // スプライトシートを1枚ずつ分割してInspectorに設定（6x4 = 24枚）
    public SpriteRenderer spriteRenderer;

    public float frameRate = 10f; // 1秒あたりのフレーム数

    private int currentFrame = 0;
    private float timer = 0f;
    private int currentRow = -1;

    private bool isMoving = false;

    void Update()
    {
        int newRow = -1;

       // if (!FrameAndBlockSpawner.IsFrameActive)
       if (!InventoryItemSpawn.IsFrameActive)
        {
            if (MonitorZoomController.isZoomComplete && MonitorZoomController.CurrentZoomIndex == 0)
            {
                if (Input.GetKey(KeyCode.S))
                {
                    newRow = 0;// 下
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    newRow = 1; // 右
                }
                else if (Input.GetKey(KeyCode.A))
                {
                    newRow = 2; // 左
                }
                else if (Input.GetKey(KeyCode.W))
                {
                    newRow = 3; // 上
                }
            }
        }

            if (newRow != -1)
        {
            if (newRow != currentRow)
            {
                currentRow = newRow;
                currentFrame = 0;
                timer = 0f;
            }

            isMoving = true;
            Animate();
        }
        else
        {
            isMoving = false;
            timer = 0f;
        }
    }

    void Animate()
    {
        if (!isMoving || currentRow == -1) return;

        timer += Time.deltaTime;

        if (timer >= 1f / frameRate)
        {
            timer -= 1f / frameRate;
            currentFrame = (currentFrame + 1) % 6;

            int index = currentRow * 6 + currentFrame;
            if (index >= 0 && index < sprites.Length)
            {
                spriteRenderer.sprite = sprites[index];
            }
        }
    }
}
