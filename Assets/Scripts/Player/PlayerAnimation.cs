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
        //一時停止スキル発動中は
        if (InventoryItemSpawn.IsFrameActive) return;

        //現在フレームの移動方向を保持
        int currentDirection = -1;

        //移動キーのどれが押されているかを取得する
        bool up = PlayerManager.Instance.movementKeys.up;       //Wキー
        bool down = PlayerManager.Instance.movementKeys.down;   //Sキー
        bool left = PlayerManager.Instance.movementKeys.left;   //Aキー
        bool right = PlayerManager.Instance.movementKeys.right; //Dキー

        // 入力の相殺処理　XORで処理
        bool vertical = up ^ down;   //WキーとSキーのどちらかが押されてるときtrue
        bool horizontal = left ^ right; //AキーとDキーのどちらかが押されている時true

        //プレイヤーの入力キーによってアニメーションを変更する
        // 斜めアニメーション上下キーと左右キーがどちらも一つ押されている時
        if (vertical && horizontal)
        {
            //if (up && right) currentDirection = 4;      // 右上
            //else if (down && right) currentDirection = 5; // 右下
            //else if (down && left) currentDirection = 6;  // 左下
            //else if (up && left) currentDirection = 7;    // 左上

            //斜め方向まだないから1フレーム前の方向を保持する
            currentDirection = oldDirection;

            //斜め方向を追加したら消す処理
            if (currentDirection == -1) {
                if (right) currentDirection = 1;    //右
                else if (left) currentDirection = 2;//左
            }


        }
        //上下方向の移動
        else if (vertical)
        {
            if (up) currentDirection = 3;       //上
            else if (down) currentDirection = 0;//下
        }
        //左右方向の移動
        else if (horizontal)
        {
            if (right) currentDirection = 1;    //右
            else if (left) currentDirection = 2;//左
        }

        //プレイヤーが移動中のみアニメーションを再生
        //プレイヤーが移動しているまたは現在のスプライトが動作しているなら
        if (currentDirection != -1 || (currentSprite != 0 && currentSprite != 3))
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

