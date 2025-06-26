using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public bool useFade = true; // シーン遷移時のフェードON/OFF
    public bool fadeOnStart = true; // シーン開始時のフェードON/OFF
    //public Fade fade; // フェードコンポーネント

    private void Start()
    {
        // `Fade` がシーン内にあるか確認し、取得
        //if (fade == null)
        //{
        //    fade = FindObjectOfType<Fade>();
        //    if (fade == null)
        //    {
        //        Debug.LogError("Fade コンポーネントが見つかりません！シーンに追加してください。");
        //    }
        //}

        //// フェード処理を実行するか選択可能に
        //if (fadeOnStart && fade != null)
        //{
        //    StartCoroutine(FadeInAtStart());
        //}
    }

    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1f;

        //if (useFade && fade != null)
        //{
        //   StartCoroutine(FadeAndLoadScene(sceneName));
        //}
        //else
        //{
            SceneManager.LoadScene(sceneName);
        //}
    }

    //private IEnumerator FadeAndLoadScene(string sceneName)
    //{
    //    if (fade != null)
    //    {
    //        yield return fade.FadeIn(1f); // フェードアウト
    //    }

    //    SceneManager.LoadScene(sceneName);
    //    yield return new WaitForSeconds(0.5f);

    //    // シーン読み込み後に新しい `ChangeScene` を取得
    //    ChangeScene newChangeScene = FindObjectOfType<ChangeScene>();

    //    if (newChangeScene != null && newChangeScene.fadeOnStart && newChangeScene.fade != null)
    //    {
    //        newChangeScene.fade.FadeOut(1f); // 新しいシーンの `fadeOnStart` を参照
    //    }
    //}

    //private IEnumerator FadeInAtStart()
    //{
    //    if (fade != null)
    //    {
    //        yield return fade.FadeOut(1f); // シーン開始時のフェードイン処理
    //    }
    //}

    public void GameEnd()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
