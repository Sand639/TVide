using UnityEngine;
using System;

//プレイヤーの移動方向に関する列挙体
public enum PLAYER_MOVEMENT
{
    UP,
    LEFT,
    DOWN,
    RIGHT, 
    NONE
}

//プレイヤー関連の変数を管理するスクリプト
//レベルデザイン時に使う変数は全部ここに入れる
public class PlayerManager : MonoBehaviour
{
    // シングルトンインスタンス用プロパティ（外部からアクセス可能）
    public static PlayerManager Instance { get; private set; }

    //プレイヤー関連のスクリプトを保持




    //現在プレイヤーがどの移動キーを入力しているか
    [HideInInspector] public PLAYER_MOVEMENT movementKey = PLAYER_MOVEMENT.NONE;


    [Header("移動速度")]
    [SerializeField] private float moveSpeed = 15.0f;
    [Header("ジャンプの強さ")]
    [SerializeField] private float jumpPower = 10.5f;
    [Header("重力加速度")]
    [SerializeField] private static float gravity = 9.81f;
    [Header("プレイヤーの最大HP")]
    [SerializeField] private static int maxHp = 10;
    //プレイヤーの現在のHP
    private int hp = maxHp;

    //ゲームオーバーかどうかを判定する
    private bool isGameOver = false;

    //カメラ切替(3D/2D)のスキルを使えるかどうか
    [HideInInspector] public bool CameraChange = false;

    private void Awake()
    {
        //シングルトンインスタンス
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

    }

    void Update()
    {
        //プレイヤーが入力した移動キーを保持する
        if (Input.GetKey(KeyCode.W)) movementKey = PLAYER_MOVEMENT.UP;
        else if (Input.GetKey(KeyCode.A)) movementKey = PLAYER_MOVEMENT.LEFT;
        else if (Input.GetKey(KeyCode.S)) movementKey = PLAYER_MOVEMENT.DOWN;
        else if (Input.GetKey(KeyCode.D)) movementKey = PLAYER_MOVEMENT.RIGHT;
        else movementKey = PLAYER_MOVEMENT.NONE;
    }


    //HPの関連の関数
    //回復処理
    public void HealHP(int _heal)
    {
        //HPを回復する   MaxHpを超えたらhpをmaxHpと同じ値にする
        hp = Math.Min(hp + _heal, maxHp);
    }
    //ダメージ処理
    public void DamageHP(int _damage)
    {
        //HPを減らす
        hp = Math.Max(hp - _damage, 0); 

        //HPが0以下になったらゲームオーバー
        if(hp <= 0) isGameOver = true;
    }

    //何かのオブジェクトのisTriggerに当たった時に呼ばれる関数
    private void OnTriggerEnter(Collider other)
    {
        //CameraChangeRangeタグと当たっている時カメラの切替スキルをONにする
        if (other.CompareTag("CameraChangeRange"))
        {
            //カメラの切替スキル使用可能
            CameraChange = true;
        }
    }

    //何かのタグから抜けた時
    private void OnTriggerExit(Collider other)
    {
        //CameraChangeRangeタグから離れた時カメラの切替スキルをOFFにする
        if (other.CompareTag("CameraChangeRange"))
        {
            //カメラの切替スキル使用不可
            CameraChange = false;
        }
    }




}
