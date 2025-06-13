using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneN : MonoBehaviour
{
    public bool useFade = true; // フェードを使うかどうか

    [SerializeField]
    private FadeManager.FadeType fadeType = FadeManager.FadeType.Black; // フェードの種類

    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1f;

        if (useFade)
        {
            FadeManager fadeManager = Object.FindFirstObjectByType<FadeManager>();
            if (fadeManager != null)
            {
                fadeManager.LoadSceneWithFade(sceneName, fadeType);
            }
            else
            {
                Debug.LogWarning("FadeManager が見つかりません。通常のシーン遷移を行います。");
                SceneManager.LoadScene(sceneName);
            }
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    public void GameEnd()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
