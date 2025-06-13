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

        if (goalcheck.goal==true)
        {
            // "T"キーを押したらタイトルシーンへ遷移
            if (Input.GetKeyDown(KeyCode.T))
            {
                SceneManager.LoadScene("Title");
            }

        }
    }
}