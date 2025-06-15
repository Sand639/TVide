using UnityEngine;

//プレイヤーのスプライトアニメーションをさせるためのスクリプト
public class PlayerAnimation : MonoBehaviour
{
    [Header("プレイヤーの画像を保持する変数")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    //プレイヤーのスプライト画像を保持する変数
    private Sprite[] sprites;

    [Header("次のスプライトに移動するための遅延時間")]
    [SerializeField] private float delayTime = 0.1f;

    //現在のスプライトに移動してから経過した時間
    private float timer = 0f;
    //現在のスプライトが左から何枚目か
    private int currentSprite = 0;
    // 0:下　1:右　2:左 3:上    アニメーション順
    private int oldDirection = 0;



    private void Awake()
    {

        //画像を読み込んで自動でスプライトを割り当てる
        sprites = Resources.LoadAll<Sprite>("Sprites/Player");

    }

    void Update()
    {
        //現在フレームの移動方向を保持
        int currentDirection = -1;

        //プレイヤーの入力キーによってアニメーションを変更する
        switch (PlayerManager.Instance.movementKey)
        {
            case PLAYER_MOVEMENT.UP:    //上入力
                currentDirection = 3;
                break;
            case PLAYER_MOVEMENT.LEFT:  //左入力
                currentDirection = 2;
                break;
            case PLAYER_MOVEMENT.DOWN:  //下入力
                currentDirection = 0;
                break;
            case PLAYER_MOVEMENT.RIGHT: //右入力
                currentDirection = 1;
                break;
            case PLAYER_MOVEMENT.NONE:  //入力無し
            default:
                break;
        }

        //プレイヤーが移動中のみアニメーションを再生
        //プレイヤーが移動しているまたは現在のスプライトが動作しているなら
        if (currentDirection != -1 || (currentSprite != 0　&& currentSprite != 3))
        {
            //1フレーム前の移動方向と現在の移動方向が違う且つプレイヤーが移動している時
            if (currentDirection != oldDirection && currentDirection != -1)
            {
                //1フレーム前の移動方向を上書き
                oldDirection = currentDirection;
            }

            // 現在のスプライトに移動してから経過した時間を加算
            timer += Time.deltaTime;
            //delayTime秒過ぎたら次のスプライトに移動する
            if (timer >= delayTime)
            {
                //timerをリセット
                timer -= delayTime;
                //次のスプライトに移動させる
                ++currentSprite;
                //アニメーションが6枚で一周のため0～5の範囲で循環させる
                currentSprite = currentSprite % 6;
                //移動方向によってスプライトが上から何番目かを掛けてあげる
                spriteRenderer.sprite = sprites[oldDirection * 6 + currentSprite];
            }
        }
        else
        {
            //プレイヤーが移動していないならtimerを0にする
            timer = 0f;

        }
    }

}
