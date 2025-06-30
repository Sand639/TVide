using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class TitleManager : MonoBehaviour
{

    private Coroutine StartCoroutine;
    private Coroutine NoiseCoroutine;

    public UnityEvent changeSceneTutorial;
    public UnityEvent changeSceneGame;
    public UnityEvent zoomEvent;
    

    public int Channel = 0;

    //channelの数字の意味
    // 0 ->タイトル
    // 1 ->チュートリアル
    // 2 ->ゲーム

    public GameObject NoiseCanvas;
    public GameObject FadeCanvas;

    public GameObject TitleCanvas;
    public GameObject TutorialCanvas;
    public GameObject GameCanvas;

    public GameObject StartText;
    public GameObject ChangeChannelInText;
    public GameObject ChangeChannelOutText;


    public bool powerFlag = false;
    public bool selectChannel = false;

    public bool useChange = false;

    public float duration = 1.0f; // チャンネル切り替えの時間
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        NoiseCanvas.SetActive(false);
        FadeCanvas.SetActive(false);
        TitleCanvas.SetActive(false);
        TutorialCanvas.SetActive(false);
        GameCanvas.SetActive(false);
        //StartText.SetActive(false);
        ChangeChannelInText.SetActive(false);
        ChangeChannelOutText.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            
        }

        if (!useChange && selectChannel && powerFlag)
        {
            StartText.SetActive(false);
            ChangeChannelInText.SetActive(false);
            ChangeChannelOutText.SetActive(true);
        }
        else if (!useChange && !selectChannel && powerFlag)
        {
            StartText.SetActive(false);
            ChangeChannelInText.SetActive(true);
            ChangeChannelOutText.SetActive(false);
        }
        
        if (Input.GetKey(KeyCode.A))
        {
            //FadeCanvas.SetActive(true);
            if (!useChange && selectChannel)
            {
                changeScene();
            }
        }
        

        if(Input.GetKeyDown(KeyCode.Space))
        {
            if (!powerFlag)
            {
                StartCoroutine = StartCoroutine(StartPower());
                
            }
            else
            {
                // イベントを実行
                if (zoomEvent != null)
                {
                    zoomEvent.Invoke();
                }
            }
            
        }

        if (!useChange)
        {
            ChangeChannel();
        }
    }

    public void changeScene()
    {
        // イベントを実行
        if (Channel == 1)
        {
            if (changeSceneTutorial != null)
            {
                changeSceneTutorial.Invoke();
            }

        }
        else if(Channel == 2)
        {
            if (changeSceneGame != null)
            {
                changeSceneGame.Invoke();
            }

        }
         
    }

   

    public void SelectChannel(bool Select)
    {
        selectChannel = Select;
    }

    private void ChangeChannel()
    {
        if(selectChannel)
        {
            

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                NoiseCoroutine = StartCoroutine(StartNoise());
                Channel--;
                if (Channel == -1)
                {
                    Channel = 2;
                }
            }

            if(Input.GetKeyDown(KeyCode.RightArrow))
            {
                NoiseCoroutine = StartCoroutine(StartNoise());
                Channel++;
                if (Channel == 3)
                {
                    Channel = 0;
                }
            }
            
            
        }
    }


    public IEnumerator StartNoise()
    {
        //selectChannel = false;
        useChange = true;
        NoiseCanvas.SetActive(true);
        TitleCanvas.SetActive(false);
        TutorialCanvas.SetActive(false);
        GameCanvas.SetActive(false);
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        NoiseCanvas.SetActive(false);
        FadeCanvas.SetActive(false);
        if (Channel == 0)
        {
            TitleCanvas.SetActive(true);
            TutorialCanvas.SetActive(false);
            GameCanvas.SetActive(false);
        }
        else if (Channel == 1)
        {
            TitleCanvas.SetActive(false);
            TutorialCanvas.SetActive(true);
            GameCanvas.SetActive(false);
        }
        else if (Channel == 2)
        {
            TitleCanvas.SetActive(false);
            TutorialCanvas.SetActive(false);
            GameCanvas.SetActive(true);
        }
        useChange = false;
        //selectChannel = true;
    }
    
    
    public IEnumerator StartPower()
    {
        //useChange = true;
        NoiseCanvas.SetActive(true);
        
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        NoiseCanvas.SetActive(false);
        powerFlag = true;
        TitleCanvas.SetActive(true);

        // イベントを実行
        if (zoomEvent != null)
        {
            zoomEvent.Invoke();
        }

        //useChange = false;
    }
}
