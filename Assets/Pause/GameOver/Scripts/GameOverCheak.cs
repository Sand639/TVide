using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverChack : MonoBehaviour
{
    [SerializeField] private Canvas GameOverCanvas;

    [SerializeField] private PlayerHP playerHP;

    private bool isDisplayed = false;

    void Start()
    {
        if (GameOverCanvas != null)
        {
            GameOverCanvas.enabled = false;
        }
    }

    void Update()
    {
        if (playerHP != null && playerHP.GameOver && !isDisplayed)
        {
            ShowGameOver();

          //  if (Input.GetKeyDown(KeyCode.T))
          //  {
         //       SceneManager.LoadScene("Title");
         //   }

        }

    }

    private void ShowGameOver()
    {
        if (GameOverCanvas != null)
        {
            GameOverCanvas.enabled = true;
        }

        isDisplayed = true;

        Time.timeScale = 0f;
    }


}
