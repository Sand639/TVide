using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class ManagerController : MonoBehaviour
{
    public GameObject ClearManager;
    public GameObject PauseManager;
    public GameObject GameOverManager;

    public GameObject ClearCanvas;
    public GameObject PauseCanvas;
    public GameObject GameOverCanvas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ClearManager.SetActive(false);
        PauseManager.SetActive(true);
        GameOverManager.SetActive(false);

        ClearCanvas.SetActive(false);
        PauseCanvas.SetActive(false);
        GameOverCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

 
    public void SetClearManagerActive(bool active)
    {
        
        ClearManager.SetActive(active);
        if (active == true)
        {
            SetPauseManagerActive(false);
            SetGameOverManagerActive(false);

            Time.timeScale = 0f;
            ClearCanvas.SetActive(active);
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
