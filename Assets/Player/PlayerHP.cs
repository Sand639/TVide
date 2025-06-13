using System.Collections;
using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    [SerializeField] private int MaxHP = 10;
    public int HP;

    [SerializeField] private Vector2 HitPointHeartPos = new Vector2(-890, 460);
    private Vector3 HitPointPositionBase;

    [SerializeField] private int HitPointHeartPosRange = 100;
    private int HitPointPositionRange;
    [SerializeField] private bool Mirror = false;

    [SerializeField] private GameObject FullHeart;
    [SerializeField] private GameObject HalfHeart;
    [SerializeField] private Transform Canvas;

    private GameObject[] HPGO = new GameObject[10];

    [SerializeField] private float InvincibleMaxTime = 2.0f;
    private float InvincibleTime = 0.0f;
    public bool Invincible = false;
    private bool InvincibleReset = false;

    public bool GameOver = false;
    [SerializeField] private GameObject GameOverUI;

    private Renderer playerRenderer;

    void Start()
    {
        HP = MaxHP;
        InvincibleTime = 0.0f;
        Invincible = false;

        playerRenderer = GetComponent<Renderer>();
        if (playerRenderer == null)
        {
            playerRenderer = GetComponentInChildren<Renderer>();
        }

        HitPointPositionRange = Mirror ? -HitPointHeartPosRange : HitPointHeartPosRange;

        for (int i = 0; i < 10; i += 2)
        {
            HitPointPositionBase = new Vector3(HitPointHeartPos.x + HitPointPositionRange * (i / 2), HitPointHeartPos.y, 0.0f);
            if (Mirror)
            {
                HPGO[i] = Instantiate(FullHeart, Canvas);
                HPGO[i].transform.localPosition = HitPointPositionBase;
                HPGO[i + 1] = Instantiate(HalfHeart, Canvas);
                HPGO[i + 1].transform.localPosition = HitPointPositionBase;
            }
            else
            {
                HPGO[i] = Instantiate(HalfHeart, Canvas);
                HPGO[i].transform.localPosition = HitPointPositionBase;
                HPGO[i + 1] = Instantiate(FullHeart, Canvas);
                HPGO[i + 1].transform.localPosition = HitPointPositionBase;
            }
        }

        if (GameOverUI != null)
        {
            GameOverUI.SetActive(false);
        }
    }

    void Update()
    {
        // ハートUI更新
        for (int i = 0; i < 10; i++)
        {
            HPGO[i].SetActive(HP > i);
        }

        // 無敵時間処理
        if (Invincible)
        {
            if (!InvincibleReset)
            {
                InvincibleTime = InvincibleMaxTime;
                InvincibleReset = true;
                StartCoroutine(FlashWhileInvincible());
            }

            InvincibleTime -= Time.deltaTime;

            if (InvincibleTime <= 0.0f)
            {
                Invincible = false;
                InvincibleReset = false;
                if (playerRenderer != null)
                {
                    playerRenderer.enabled = true;
                }
            }
        }

        // ゲームオーバー処理
        if (HP <= 0 && !GameOver)
        {
            GameOver = true;
            if (GameOverUI != null)
            {
                GameOverUI.SetActive(true);
            }
        }
    }


    private IEnumerator FlashWhileInvincible()
    {
        float flashInterval = 0.2f;

        if (playerRenderer != null)
        {
            playerRenderer.enabled = false;
        }

        while (Invincible)
        {
            yield return new WaitForSeconds(flashInterval);

            if (playerRenderer != null)
            {
                playerRenderer.enabled = !playerRenderer.enabled;
            }
        }

        // 最後に表示状態に戻す
        if (playerRenderer != null)
        {
            playerRenderer.enabled = true;
        }
    }

}
