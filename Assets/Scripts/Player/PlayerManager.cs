using UnityEngine;

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


    //[Header("移動速度")]
    //[SerializeField] private float moveSpeed = 15.0f;
    //[Header("ジャンプの強さ")]
    //[SerializeField] private float jumpPower = 10.5f;
    //[Header("重力加速度")]
    //[SerializeField] private static float gravity = 9.81f;
    //[Header("プレイヤーの最大HP")]
    //[SerializeField] private static int maxHp = 5;  
    ////プレイヤーの現在のHP
    //private int hp = maxHp;

    private void Awake()
    {
        //シングルトンインスタンス
        if (Instance == null) Instance = this;
        else Destroy(gameObject);


    }

    void Start()
    {
        
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
}
