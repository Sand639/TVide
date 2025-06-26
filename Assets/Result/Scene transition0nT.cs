using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    Goalcheck goalcheck;

    void Start()
    {
        goalcheck = FindObjectOfType<Goalcheck>();
    }


    void Update()
    {

        if (goalcheck.goal==true||goalcheck.pause == true)
        {
            // "T"キーを押したらタイトルシーンへ遷移
            if (Input.GetKeyDown(KeyCode.T))
            {
                Time.timeScale = 1f; // 時間停止を解除してからシーン遷移
                SceneManager.LoadScene("Title");
            }

        }
    }
}