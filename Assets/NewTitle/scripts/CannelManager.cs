using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// UI の Image を左右キーで切り替え、
/// Enter でシーンを切り替え、
/// 上下キーでカメラ移動（MoveCamera）を呼び出す
/// </summary>
public class ChannelManager : MonoBehaviour
{
    [Header("【画像切替用設定】")]
    [SerializeField] private Image targetImage;

    [Header("【切替画像リスト】")]
    [SerializeField] private List<Sprite> sprites;

    [Header("【各画像に対応するシーン名】")]
    [SerializeField] private List<string> sceneNames;

    [Header("【砂嵐エフェクト設定】")]
    [SerializeField] private GameObject noiseOverlay;
    [SerializeField, Range(0.1f, 2.0f)]
    private float noiseDuration = 0.3f;

    public int currentIndex = 0;
    private bool isSwitching = false;

    [Header("【外部制御用フラグ】")]
    public bool inputEnabled = true;      // ←→入力を許可するか
    public bool allowSceneChange = false; // Enter入力を許可するか

    [Header("【カメラコントローラ】")]
    [SerializeField]
    private CameraCoroutine cameraCoroutine; // 上下キーで呼ぶ

    private void Start()
    {
        targetImage.sprite = sprites[currentIndex];
        noiseOverlay.SetActive(false);
    }

    private void Update()
    {
        // ←→ 入力は inputEnabled が true のときだけ許可
        if (inputEnabled)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                StartCoroutine(SwitchImageWithNoise(1));
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                StartCoroutine(SwitchImageWithNoise(-1));
            }

        }


        // Enter 入力は allowSceneChange が true のときだけ許可
        if (allowSceneChange && Input.GetKeyDown(KeyCode.Return))
        {
            LoadCurrentScene();
        }

        // 上下キーは常にカメラ移動を呼ぶ
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            cameraCoroutine.MoveCamera();
        }
    }


    private IEnumerator SwitchImageWithNoise(int direction)
    {
        if (isSwitching) yield break;

        isSwitching = true;
        noiseOverlay.SetActive(true);

        yield return new WaitForSeconds(noiseDuration);

        currentIndex += direction;
        if (currentIndex >= sprites.Count) currentIndex = 0;
        if (currentIndex < 0) currentIndex = sprites.Count - 1;

        targetImage.sprite = sprites[currentIndex];

        noiseOverlay.SetActive(false);
        isSwitching = false;
    }

    private void LoadCurrentScene()
    {
        string sceneName = sceneNames[currentIndex];
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    public void SetInput(bool enableInput, bool enableSceneChange)
    {
        inputEnabled = enableInput;
        allowSceneChange = enableSceneChange;
    }
}
