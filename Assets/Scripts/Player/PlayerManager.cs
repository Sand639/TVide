using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

//プレイヤーの移動方向に関する構造体
public struct MovementKeys
{
    public bool up;     //奥方向
    public bool down;   //前方向
    public bool left;   //左方向
    public bool right;  //右方向
}

//プレイヤーの移動モード
public enum MovementMode
{
    NormalMode,     // 通常モード
    Inertia         // 慣性（減衰）モード
}

//プレイヤー関連の変数を管理するスクリプト
//レベルデザイン時に使う変数は全部ここに入れる
public class PlayerManager : MonoBehaviour
{
    // シングルトンインスタンス用プロパティ（外部からアクセス可能）
    public static PlayerManager Instance { get; private set; }

    //プレイヤー関連のスクリプトを保持
    private Goalcheck goalcheck;    //ゴール判定確認クラス



    //現在プレイヤーがどの移動キーを入力しているか
    [HideInInspector] public MovementKeys movementKeys;

    //プレイヤーが現在どの移動モードか
    [HideInInspector] public MovementMode movementMode = MovementMode.Inertia;

    /*******************************************************
    * 移動系に与える力
    ******************************************************/
    [Header("移動速度")]
    public float moveSpeed = 15.0f;
    [Header("ジャンプの強さ")]
    public float jumpPower = 10.5f;

    /*******************************************************
    * 重力加速度
    ******************************************************/
    [Header("通常時にかかる重力加速度")]
    public float jumpGravity = 15f;   // 通常の重力
    [Header("落下時にかかる重力加速度")]
    public float fallGravity = 20.0f;   // 落下時に強くする

    /*******************************************************
    * 減衰値
    ******************************************************/
    [Header("地面に着地してるときの速度減衰値(摩擦力)")]
    public float groundDrag = 0.2f;
    [Header("空中にいる時の速度減衰値(空気抵抗力)")]
    public float airDrag = 0.07f;

    /*******************************************************
    * 復活地点の設定
    ******************************************************/
    [Header("落下判定になるY座標の値")]
    public float fallLine = -10;        // 落下判定になるy座標
    [Header("プレイヤーの復活地点 / EmptyObjectなどで場所を指定してあげる")]
    public Transform respawnPoint;      // リスポーンポイント

    /*******************************************************
    * プレイヤーのHP
    ******************************************************/
    [Header("プレイヤーの最大HP")]
    [SerializeField] private static int maxHp = 10;
    //プレイヤーの現在のHP
    private int hp = maxHp;

    /*******************************************************
    * プレイヤー関連のフラグ
    ******************************************************/
    //カメラ切替(3D/2D)のスキルを使えるかどうか
    [HideInInspector] public bool canCameraChange = false;
    //プレイヤーの更新を停止させるかどうか
    [HideInInspector] public bool isStop= false;
    //ゲームオーバーかどうかを判定する
    private bool isGameOver = false;


    /*******************************************************
    * 風に関する変数
    ******************************************************/
    //風が発生中かどうか
    [HideInInspector] public bool isWind = false;
    //風のスピード
    [HideInInspector] public float windSpeed = 0f;
    //風の継続する時間
    private float windDuration = 0f;

    /*******************************************************
    * 雨に関する変数
    ******************************************************/
    //雨が発生中かどうか
    [HideInInspector] public bool isRain = false;
    //雨の継続する時間
    private float rainDuration = 0f;

    /*******************************************************
    * ノックバックに関する変数
    ******************************************************/
    [Header("ノックバックの継続時間")]
    public float knockbackDuration = 0.3f;

    private void Awake()
    {
        //シングルトンインスタンス
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

    }

    private void Start()
    {
        goalcheck = FindObjectOfType<Goalcheck>();  //ゴールクラスを見つける
    }

    void Update()
    {
        //画面が違う または ゴールした　または　スキル使用中なら更新を止める
        if (InventoryItemSpawn.IsFrameActive || MoveStopCursor.IsFrameActive || goalcheck.goal ||
            !(MonitorZoomController.isZoomComplete && MonitorZoomController.CurrentZoomIndex == 0))
        {
            isStop = true;  //プレイヤーの更新処理を止める
            return;
        }
        else
        {
            isStop = false; //プレイヤーの更新処理を開始
        }

        //プレイヤーが入力した移動キーを保持する
        movementKeys.up = Input.GetKey(KeyCode.W);
        movementKeys.down = Input.GetKey(KeyCode.S);
        movementKeys.left = Input.GetKey(KeyCode.A);
        movementKeys.right = Input.GetKey(KeyCode.D);

        //天候系のタイマー処理
        UpdateWeatherTimers();
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
        if (hp <= 0) isGameOver = true;
    }

    //何かのオブジェクトのisTriggerに当たった時に呼ばれる関数
    private void OnTriggerEnter(Collider other)
    {
        //CameraChangeRangeタグと当たっている時カメラの切替スキルをONにする
        if (other.CompareTag("CameraChangeRange"))
        {
            //カメラの切替スキル使用可能
            canCameraChange = true;
        }
    }

    //何かのタグから抜けた時
    private void OnTriggerExit(Collider other)
    {
        //CameraChangeRangeタグから離れた時カメラの切替スキルをOFFにする
        if (other.CompareTag("CameraChangeRange"))
        {
            //カメラの切替スキル使用不可
            canCameraChange = false;
        }
    }

    //風を発生させる関数
    public void ApplyWindBoost(float duration, float boostSpeed)
    {
        windDuration = duration;    //風の継続時間を代入
        windSpeed = boostSpeed;     //風の速さを代入
        isWind = true;              //風発生中のフラグを立てる
    }

    //雨を発生させる関数
    public void ApplyRainFloat(float duration)
    {
        rainDuration = duration;    //雨の継続時間を代入
        isRain = true;              //雨発生中のフラグを立てる
    }

    //天候系のタイマーを計算する関数
    private void UpdateWeatherTimers()
    {
        if (isWind) //風発生中
        {
            windDuration -= Time.deltaTime; //風の継続時間を減らしていく
            if (windDuration <= 0f) //風の継続時間が終わったら
            {
                isWind = false; //風発生中のフラグを下げる
                windSpeed = 0f; //風の速さを0にする
            }
        }

        if (isRain) //雨発生中
        {
            rainDuration -= Time.deltaTime; //雨の継続時間を減らしていく
            if (rainDuration <= 0f) //雨の継続時間が終わったら
            {
                isRain = false; //雨発生中のフラグを下げる
            }
        }
    }
}
