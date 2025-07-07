using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class ManagerController : MonoBehaviour
{
    // シングルトンインスタンス用プロパティ（外部からアクセス可能）
    public static ManagerController Instance { get; private set; }

    [Header("各キャンバスのマネージャー")]
    [Tooltip("ポーズメニュー画面マネージャー")]
    public GameObject PauseManager;
    [Tooltip("ゲームクリア画面のマネージャー")]
    public GameObject GameClearManager;
    [Tooltip("ゲームオーバー画面のマネージャー")]
    public GameObject GameOverManager;

    [Header("各キャンバス")]
    [Tooltip("ポーズメニュー画面")]
    public GameObject PauseCanvas;
    [Tooltip("ゲームクリア画面")]
    public GameObject GameClearCanvas;
    [Tooltip("ゲームオーバー画面")]
    public GameObject GameOverCanvas;


    private void Awake()
    {
        //シングルトンインスタンス
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameClearManager.SetActive(false);
        PauseManager.SetActive(true);
        GameOverManager.SetActive(false);

        GameClearCanvas.SetActive(false);
        PauseCanvas.SetActive(false);
        GameOverCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

 
    public void SetClearManagerActive(bool active)
    {
        
        GameClearManager.SetActive(active);
        if (active == true)
        {
            SetPauseManagerActive(false);
            SetGameOverManagerActive(false);

            Time.timeScale = 0f;
            GameClearCanvas.SetActive(active);
        }
        
    }

    public void SetPauseManagerActive(bool active)
    {
        PauseManager.SetActive(active);
        if (active == true)
        {
            SetClearManagerActive(false);
            SetGameOverManagerActive(false);

            Time.timeScale = 0f;
            PauseCanvas.SetActive(active);
        }

    }

    public void SetGameOverManagerActive(bool active)
    {
        GameOverManager.SetActive(active);
        if (active == true)
        {
            SetPauseManagerActive(false);
            SetClearManagerActive(false);

            Time.timeScale = 0f;
            GameOverCanvas.SetActive(active);
        }

    }


}
