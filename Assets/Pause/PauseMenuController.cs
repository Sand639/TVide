using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public GameObject canvas; // ポーズ画面のキャンバス
    private Goalcheck goalcheck;

    // UI関連の設定
    public TextMeshProUGUI[] menuItems;
    public int Index = 0;
    public float normalScale = 1f; // 通常サイズ
    public float selectedScale = 1.2f; // 選択中のサイズ
    public bool canbasActive=false;
    
    void Start()
    {
        // 初期設定
        if (canvas != null)
        {
            //canvas.SetActive(false); // ポーズ画面を非表示
            Debug.Log("キャンバス初期状態: 非表示");
        }

        Time.timeScale = 1f; // 通常速度で動作
        goalcheck = FindObjectOfType<Goalcheck>();
        Debug.Log("TimeScale初期設定: " + Time.timeScale);

        if (menuItems == null || menuItems.Length == 0)
        {
            Debug.LogWarning("menuItemsが設定されていません。UIを確認してください！");
        }
        else
        {
            UpdateSelect(); // UI選択の初期状態を設定
        }
    }

    void Update()
    {
        // Escキーでポーズ状態を切り替える
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        // UI操作: 上下キーで選択移動
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (menuItems != null && menuItems.Length > 0)
            {
                Index = (Index - 1 + menuItems.Length) % menuItems.Length;
                UpdateSelect(); // 選択状態を更新
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (menuItems != null && menuItems.Length > 0)
            {
                Index = (Index + 1) % menuItems.Length;
                UpdateSelect(); // 選択状態を更新
            }
        }

        // Pキーで選択を実行
        else if (Input.GetKeyDown(KeyCode.P))
        {
            ExecuteSelect(); // 選択項目の実行
        }
    }

    public void TogglePause()
    {
        // ポーズ画面の状態を切り替える処理
        if (canvas != null)
        {
            canvas.SetActive(!canbasActive); // キャンバス表示を更新
            canbasActive = !canbasActive;
            goalcheck.pause = canbasActive; // ポーズ状態を更新
            Time.timeScale = canbasActive ? 0f : 1f; // ゲーム時間を停止/再開

            Debug.Log("ポーズ状態切り替え: " + (canbasActive ? "オン" : "オフ"));
            Debug.Log("キャンバス表示状態: " + canvas.activeSelf);
            Debug.Log("時間スケール: " + Time.timeScale);
            Debug.Log("goalcheck.pauseの状態: " + goalcheck.pause);
        }
    }

    public void ResumeGame()
    {
        // 「ゲームに戻る」ボタンを押した場合の処理
        if (canvas.activeSelf)
        {
            canvas.SetActive(false); // ポーズ画面を非表示
            canbasActive = false;
            Time.timeScale = 1f; // ゲーム再開
            goalcheck.pause = false; // ポーズ解除

            Debug.Log("ポーズ解除: ゲームが再開されました！");
            Debug.Log("キャンバス状態: " + canvas.activeSelf);
            Debug.Log("時間スケール: " + Time.timeScale);
        }
    }

    public void ReturnToTitle()
    {
        // 「タイトルに戻る」ボタンを押した場合の処理
        Debug.Log("タイトルに戻る処理を実行");
        Time.timeScale = 1f;
        SceneManager.LoadScene("Title");
    }

    public void QuitGame()
    {
        // 「終了する」ボタンを押した場合の処理
        
        Debug.Log("ゲーム終了処理を実行");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    void UpdateSelect()
    {
        // UIの選択状態を更新
        if (menuItems == null || menuItems.Length == 0) return;

        for (int i = 0; i < menuItems.Length; i++)
        {
            if (menuItems[i] == null) continue;
            menuItems[i].transform.localScale = (i == Index) ?
                Vector3.one * selectedScale : Vector3.one * normalScale;
        }
    }

    void ExecuteSelect()
    {
        // menuItemsが設定されているか確認
        if (menuItems == null || menuItems.Length == 0)
        {
            Debug.LogWarning("menuItemsが設定されていません");
            return; // 処理を中断
        }

        if (Index < 0 || Index >= menuItems.Length)
        {
            Debug.LogWarning("Indexが範囲外です: " + Index);
            return; // 処理を中断
        }

        string selectedText = menuItems[Index].text;
        Debug.Log("現在選択中のテキスト: " + selectedText);

        switch (Index)
        {
            case 0:
                ResumeGame();
                break;
            case 1:
                ReturnToTitle();
                break;
            case 2:
                QuitGame();
                break;

            default:
                Debug.Log("未対応の選択肢: " + selectedText);
                break;
        }
    }
}

//using UnityEngine;

//public class PauseMenuController : MonoBehaviour
//{
//    public GameObject canvas; // キャンバス（ポーズ用UI）の参照
//    private Goalcheck goalcheck;

//    void Start()
//    {
//        // ゲーム開始時はキャンバスを非表示にし、通常動作にする
//        if (canvas != null)
//        {
//            canvas.SetActive(false);
//        }
//        Time.timeScale = 1f; // ゲーム開始時は通常速度で動作

//        goalcheck = FindObjectOfType<Goalcheck>();

//    }

//    void Update()
//    {
//        // Escキーが押されたら、キャンバスの表示状態をトグルする
//        if (Input.GetKeyDown(KeyCode.Escape))
//        {
//            if (canvas != null)
//            {
//                goalcheck.pause = true;

//                // 現在のキャンバスの状態を反転する
//                canvas.SetActive(!canvas.activeSelf);

//                // キャンバスが表示されていればゲームを停止、非表示なら再開
//                if (canvas.activeSelf)
//                {
//                    // キャンバスが表示された＝ポーズ中 → ゲームを一時停止（時間停止）
//                    Time.timeScale = 0f;
//                }
//                else
//                {
//                    // キャンバスが非表示になった＝ポーズ解除 → ゲームを再開
//                    Time.timeScale = 1f;
//                }

//                Debug.Log("キャンバスは現在 " + (canvas.activeSelf ? "表示" : "非表示") +
//                          " （時間 " + (canvas.activeSelf ? "停止" : "再開") + "）");
//            }
//        }
//    }
//}

