//using UnityEngine;

//public class ExitGameController : MonoBehaviour
//{
//    public static void QuitGame() // 静的メソッドに変更
//    {
//#if UNITY_EDITOR
//        UnityEditor.EditorApplication.isPlaying = false;
//#else
//        Application.Quit();
//#endif
//    }
//}

using UnityEngine;

public class ExitGameController : MonoBehaviour
{
    void Update()
    {
        //Bボタンが押されたときの動作をチェック
        if (Input.GetKeyDown(KeyCode.B))
        {
            //ゲーム終了処理
            QuitGame();
        }
    }

    void QuitGame()
    {
        //Unityエディター内ではこのコードを使うことで動作を確認可能
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // ビルドされたゲームではこちらを使用
#endif
    }
}
