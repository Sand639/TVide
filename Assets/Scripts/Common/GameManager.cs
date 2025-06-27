using UnityEngine;

public class GameManager : MonoBehaviour
{
    // シングルトンインスタンス用プロパティ（外部からアクセス可能）
    public static GameManager Instance { get; private set; }

    //Qボタンのカメラ切替(3D/2D)のスキルが有効かどうか
    [HideInInspector] public bool isCameraSkill = false;

    private void Awake()
    {
        //シングルトンインスタンス
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        isCameraSkill = false;
    }

    void Update()
    {
        if (MonitorZoomController.isZoomComplete && MonitorZoomController.CurrentZoomIndex == 0)
        {
            //Qボタンが押された時且つ、カメラが切り替えられる範囲内なら
            if (Input.GetKeyDown(KeyCode.Q) && PlayerManager.Instance.canCameraChange)
                isCameraSkill = !isCameraSkill;        //カメラスキルで3Dと2Dを切り替える
        }
    
    
    
    }


}
