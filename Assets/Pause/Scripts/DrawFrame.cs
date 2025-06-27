using UnityEngine;

public class DrawFrame : MonoBehaviour
{
    public RectTransform targetObject; // 枠を表示したい対象オブジェクト
    private LineRenderer lineRenderer;

    void Start()
    {
        // LineRendererを動的に追加
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 5; // 四角形の4辺 + 始点に戻るための点
        lineRenderer.loop = false; // ループしない（手動で枠を閉じる）
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // シンプルなシェーダー
        lineRenderer.startColor = Color.red; // 枠線の色
        lineRenderer.endColor = Color.red;

        // 初期位置を設定
        DrawSquareFrame();
    }

    void Update()
    {
        // 毎フレーム、対象オブジェクトの位置に追従
        if (targetObject != null)
        {
            DrawSquareFrame();
        }
    }

    void DrawSquareFrame()
    {
        // RectTransformから位置とサイズを取得
        Vector3[] corners = new Vector3[4];
        targetObject.GetWorldCorners(corners);
        Debug.Log("左下: " + corners[0] + ", 右上: " + corners[2]);

        // 各コーナーの位置を設定
        lineRenderer.SetPosition(0, corners[0]); // 左下
        lineRenderer.SetPosition(1, corners[1]); // 左上
        lineRenderer.SetPosition(2, corners[2]); // 右上
        lineRenderer.SetPosition(3, corners[3]); // 右下
        lineRenderer.SetPosition(4, corners[0]); // 左下（始点に戻る）
    }
}
