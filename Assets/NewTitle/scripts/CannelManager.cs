using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// UI の Image を左右キーで切替、Enter でシーン移動、上下でカメラ移動。
/// ヘルプテキストは Text と TMP 両方に対応。
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

    [Header("【カメラコントローラ】")]
    [SerializeField]
    private CameraCoroutine cameraCoroutine;

    [Header("【ヘルプテキスト設定】")]
    [SerializeField] private Text helpTextUI;               // Unity UI Text
    [SerializeField] private TextMeshProUGUI helpTextTMP;   // TMP

    [SerializeField] private string defaultText = "↑↓:ズーム ←→:選択 Enter:決定 H:ヘルプ切替";
    [SerializeField] private string helpTextZoomOut = "ヘルプON: ←→で選択肢を切替";
    [SerializeField] private string helpTextZoomIn = "ヘルプON: Enterで決定";

    private bool isHelpOpen = false; // ヘルプを開いているか

    private int currentIndex = 0;
    private bool isSwitching = false;

    [Header("【外部制御用フラグ】")]
    public bool inputEnabled = true;      // ←→入力を許可するか
    public bool allowSceneChange = false; // Enter入力を許可するか

    private void Start()
    {
        targetImage.sprite = sprites[currentIndex];
        noiseOverlay.SetActive(false);
        UpdateHelpText();
    }

    private void Update()
    {
        // HキーでヘルプON/OFF
        if (Input.GetKeyDown(KeyCode.H))
        {
            isHelpOpen = !isHelpOpen;
            UpdateHelpText();
        }

        // ←→ 入力は inputEnabled が true のときのみ
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

        // Enter 入力は allowSceneChange が true のときのみ
        if (!allowSceneChange && Input.GetKeyDown(KeyCode.Return))
        {
            LoadCurrentScene();
        }

        // 上下キーは常にカメラを動かす
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

    /// <summary>
    /// CameraCoroutine から呼ばれる
    /// inputEnabled / allowSceneChange をセットし、その状態に応じてヘルプを更新
    /// </summary>
    public void SetInput(bool enableInput, bool enableSceneChange)
    {
        inputEnabled = enableInput;
        allowSceneChange = enableSceneChange;
        UpdateHelpText();
    }

    /// <summary>
    /// ヘルプテキストをヘルプON/OFFとカメラの状態に応じて切替。
    /// Text または TMP がセットされている方に表示。
    /// </summary>
    private void UpdateHelpText()
    {
        string textToShow = "";

        if (!isHelpOpen)
        {
            textToShow = defaultText;
        }
        else
        {
            textToShow = allowSceneChange ? helpTextZoomIn : helpTextZoomOut;
        }

        if (helpTextUI != null)
        {
            helpTextUI.text = textToShow;
        }
        if (helpTextTMP != null)
        {
            helpTextTMP.text = textToShow;
        }
    }
}