using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameClearController : MonoBehaviour
{
    public GameObject canvas; // ゲームクリア画面のキャンバス

    
    public TextMeshProUGUI[] menuItems; // メニュー項目（タイトルに戻る、終了する）
    public int Index = 0;
    public float normalScale = 1f; // 通常サイズ
    public float selectedScale = 1.2f; // 選択中のサイズ

    public string nextStage;
    void Start()
    {
        if (canvas != null)
        //if (gameObject != null)
        {
            //canvas.SetActive(false); // 初期状態では非表示
            //gameObject.SetActive(false); // 初期状態では非表示
        }

        if (menuItems == null || menuItems.Length == 0)
        {
            Debug.LogWarning("menuItemsが設定されていません。UIを確認してください！");
        }
        else
        {
            UpdateSelect(); // 初期選択状態を設定
        }
    }

    //public void TriggerGameClear()
    //{
    //    // ゲームクリア時の処理
    //    if (canvas != null)
    //    //if (gameObject != null)
    //    {
    //        canvas.SetActive(true); // ゲームクリア画面を表示
    //        //gameObject.SetActive(true); // ゲームクリア画面を表示

    //    }

    //    Debug.Log("ゲームクリア！メニューが動作中");
    //}

    void Update()
    {
        // 左右キーで選択を移動
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Index = (Index - 1 + menuItems.Length) % menuItems.Length;
            Debug.Log("現在のインデックス (左): " + Index);
            UpdateSelect();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Index = (Index + 1) % menuItems.Length;
            Debug.Log("現在のインデックス (右): " + Index);
            UpdateSelect();
        }

        // ENTERキーで選択を実行
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ExecuteSelect();
        }
    }

    public void ReturnToTitle()
    {
        Time.timeScale = 1f;
        Debug.Log("タイトルに戻る処理を実行");
        SceneManager.LoadScene("Title"); // タイトルシーンをロード
    }

    public void NextStage()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(nextStage); // 次のシーンをロード
    }

    public void QuitGame()
    {
        Debug.Log("ゲーム終了処理を実行");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    void UpdateSelect()
    {
        if (menuItems == null || menuItems.Length == 0) return;

        for (int i = 0; i < menuItems.Length; i++)
        {
            menuItems[i].transform.localScale = (i == Index) ?
                Vector3.one * selectedScale : Vector3.one * normalScale;
        }

        Debug.Log("選択状態更新: インデックス " + Index);
    }

    void ExecuteSelect()
    {
        if (menuItems == null || menuItems.Length == 0)
        {
            Debug.LogWarning("menuItemsが設定されていません");
            return;
        }

        if (Index < 0 || Index >= menuItems.Length)
        {
            Debug.LogWarning("Indexが範囲外です: " + Index);
            return;
        }

        string selectedText = menuItems[Index].text;
        Debug.Log("現在選択中のテキスト: " + selectedText);

        switch (Index)
        {
            case 0:
                ReturnToTitle();
                break;

            case 1:
                //NextStage();
                break;

            case 2:
                QuitGame();
                break;

            default:
                Debug.LogWarning("未対応の選択肢: " + selectedText);
                break;
        }
    }
}
