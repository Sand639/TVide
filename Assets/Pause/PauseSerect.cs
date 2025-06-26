
//using UnityEngine;
//using TMPro;
//using UnityEngine.SceneManagement;

//public class PauseMenu : MonoBehaviour
//{
//    public GameObject canvas; // ポーズ画面のキャンバスを定義
//    public TextMeshProUGUI[] menuItems;
//    public int Index = 0;

//    public float normalScale = 1f;//デフォルトのscaleを設定しとく
//    public float selectedScale = 1.2f;//選択されているテキストを少し拡大する

//    void Start()
//    {
//        UpdateSelect();
//    }

//    void Update()
//    {//キー操作でインデックスを切り替えするようにする。

//        if (Input.GetKeyDown(KeyCode.UpArrow))
//        {
//            Index = (Index - 1 + menuItems.Length) % menuItems.Length;
//            UpdateSelect();
//        }
//        else if (Input.GetKeyDown(KeyCode.DownArrow))
//        {
//            Index = (Index + 1) % menuItems.Length;
//            UpdateSelect();
//        }
//        else if (Input.GetKeyDown(KeyCode.P))
//        {//Pで実行。
//            ExecuteSelect();
//        }
//    }

//    void UpdateSelect()
//    {
//        for (int i = 0; i < menuItems.Length; i++)
//        {
//            menuItems[i].transform.localScale = (i == Index) ?
//                Vector3.one * selectedScale : Vector3.one * normalScale;
//        }
//    }

//    void ExecuteSelect()
//    {
//        string selectedText = menuItems[Index].text;

//        switch (selectedText)
//        {
//            case "タイトルに戻る":
//                Debug.Log("タイトルに戻るを選択");
//                Time.timeScale = 1f;
//                SceneManager.LoadScene("Title");
//                break;

//            case "ゲームに戻る":
//                Debug.Log("ゲームに戻るの処理が呼び出された");
//                canvas.SetActive(false); // ポーズ画面を非表示
//                Time.timeScale = 1f; // ゲームを再開
//                break;
//        }
//    }

//    void QuitGame()
//    {
//#if UNITY_EDITOR
//    UnityEditor.EditorApplication.isPlaying = false;
//#else
//        Application.Quit();
//#endif
//    }
//}