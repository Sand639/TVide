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

    private float currentDelayTime = 0.1f;

    //現在のスプライトに移動してから経過した時間
    private float timer = 0f;
    //現在のスプライトが左から何枚目か
    private int currentSprite = 0;
    // 0:右　1:左 2:下 3:上    アニメーション順
    private int oldDirection = 0;

    //一連の動作のスプライト数
    private int spriteAnimNum = 3;

    //ジャンプアニメーションが終了したかどうか
    private bool isJumpAnimEnd = false;

    //ノックバックアニメーションが終了したかどうか
    private bool isKnockbackAnimEnd = false;

    //起き上がりアニメーションが開始されたかどうか
    private bool isGetupAnimStart = false;

    private void Awake()
    {
        //画像を読み込んで自動でスプライトを割り当てる
        sprites = Resources.LoadAll<Sprite>("Sprites/Player");

    }

    void Update()
    {
        //一時停止スキル発動中はアニメーションしない
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
            if (currentDirection == -1)
            {
                if (right) currentDirection = 0;    //右
                else if (left) currentDirection = 1;//左
            }

        }

        ////上下方向の移動
        //else if (vertical)
        //{
        //    if (up) currentDirection = 3;       //上
        //    else if (down) currentDirection = 2;//下
        //}
        //左右方向の移動
        else if (horizontal)
        {
            if (right) currentDirection = 0;    //右
            else if (left) currentDirection = 1;//左
        }

        spriteAnimNum = 3; //アニメーションのスプライト数を3に設定
        currentDelayTime = delayTime;

        //ジャンプ処理のアニメーション割込み
        currentDirection = JumpAnim(currentDirection);

        //ノックバックモーションの割り込み
        //currentDirection = KnockbackAnim(currentDirection);

        //起き上がりアニメーション
        //currentDirection = GetupAnim(currentDirection);


        //プレイヤーが移動中のみアニメーションを再生
        //プレイヤーが移動しているまたは現在のスプライトが動作しているなら
        if (currentDirection != -1 || (currentSprite != 1 % 3))
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
            if (timer >= currentDelayTime)
            {
                //timerをリセット
                timer -= currentDelayTime;
                //次のスプライトに移動させる
                ++currentSprite;
                //アニメーションがspriteAnimNumで一周のため0～(spriteAnimNum - 1)の範囲で循環させる
                currentSprite = currentSprite % (spriteAnimNum - 1);
                //移動方向によってスプライトが上から何番目かを掛けてあげる
                spriteRenderer.sprite = sprites[oldDirection * 5 + currentSprite];

                if (currentSprite == (spriteAnimNum - 1))
                {
                    if (!PlayerMovement.Instance.isGrounded)
                    {
                        //ジャンプアニメーションが終了したらフラグを立てる
                        isJumpAnimEnd = true;
                    }

                    if (isGetupAnimStart)
                    {
                        //起き上がりアニメーション終了フラグをリセット
                        isGetupAnimStart = false;

                        switch (currentDirection)
                        {
                            case 5:
                                oldDirection = 0;
                                break;
                            case 7:
                                oldDirection = 1;
                                break;
                        }
                        currentSprite = 1;

                    }

                    if (PlayerMovement.Instance.isKnockback)
                    {
                        //ノックバックアニメーションが終了したらフラグを立てる
                        isKnockbackAnimEnd = true;

                        //ノックバックが終了したなら起き上がりアニメーション開始
                        //起き上がりアニメーションを開始フラグを立てる
                        isGetupAnimStart = true;
                        currentSprite = 0; //起き上がりアニメーションのスプライトを0にリセット
                    }

                }
            }
        }
        else
        {
            //プレイヤーが移動していないならtimerを0にする
            timer = 0f;
        }
    }

    //ジャンプアニメーションの処理
    private int JumpAnim(int direction)
    {
        //ノックバック中ならジャンプアニメーションはしない
        if (PlayerMovement.Instance.isKnockback) return direction;

        //地面についていない時ジャンプモーション
        if (!PlayerMovement.Instance.isGrounded)
        {
            //ジャンプアニメーションが終了したならアニメーション停止
            if (isJumpAnimEnd) return -1;

            //何も操作がないときに前フレームの方向を取得する
            if (direction == -1)
            {
                //前フレームの方向を取得
                direction = oldDirection;
            }

            spriteAnimNum = 5; //ジャンプのスプライト数を5に設定
            switch (direction)
            {
                case 0: //右ジャンプ
                    return 2;
                case 1: //左ジャンプ
                    return 3;
                case 2:

                    break;
                case 3:

                    break;
                case 5:

                    break;
                case 6:

                    break;
                case 7:

                    break;
                default:
                    break;
            }
        }
        else
        {
            //ジャンプアニメーション終了フラグをリセット
            isJumpAnimEnd = false;
        }

        return direction;
    }

    //ノックバックアニメーションの処理
    private int KnockbackAnim(int direction)
    {
        if (isGetupAnimStart) return direction;

        //ノックバックモーションのアニメーション処理
        if (PlayerMovement.Instance.isKnockback)
        {

            currentDelayTime = delayTime / 5.0f;


            //何も操作がないときに前フレームの方向を取得する
            if (direction == -1)
            {
                //前フレームの方向を取得
                direction = oldDirection;
            }

            spriteAnimNum = 4; //ノックバックのスプライト数を4に設定
            switch (direction)
            {
                case 0: //右ノックバック
                    return 4;
                case 1: //左ノックバック
                    return 6;
                case 2:

                    break;
                case 3:

                    break;
                case 4:

                    break;
                case 5:

                    break;
                case 6:

                    break;
                case 7:

                    break;
                default:
                    break;
            }
        }
        else
        {
            //ノックバックアニメーション終了フラグをリセット
            isKnockbackAnimEnd = false;
        }

        return direction;
    }

    //起き上がりアニメーションの処理
    private int GetupAnim(int direction)
    {
        //起き上がりアニメーションが開始されていないなら何もしない
        if (isGetupAnimStart)
        {
            currentDelayTime = delayTime;

            //何も操作がないときに前フレームの方向を取得する
            if (direction == -1)
            {
                //前フレームの方向を取得
                direction = oldDirection;
            }
            spriteAnimNum = 3; //起き上がりのスプライト数を3に設定
            switch (direction)
            {
                case 0: //右起き上がり
                case 2:
                case 4:
                    return 5;
                case 1: //左起き上がり
                case 3:
                case 6:
                    return 7;
                case 5:
                    break;
                case 7:
                    break;
                default:
                    break;
            }

        }

        return direction;

    }

}

