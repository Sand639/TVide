using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerHpUi : MonoBehaviour
{
    [SerializeField] private Sprite fullHeart;  //普通のハート
    [SerializeField] private Sprite halfHeart;  //半分のハート
    [SerializeField] private GameObject heartImagePrefab;  // Imageプレハブ

    [SerializeField] private float heartSize = 100f;          // ハートのサイズ（正方形）
    [SerializeField] private Vector2 startPosition = new Vector2(20f, -20f); // ハートの設置位置
    [SerializeField] private float heartSpacing = 20f;         // ハート間のスペース

    //ハートを保持しておくリスト
    private List<GameObject> heartObjects = new List<GameObject>();

    void Start()
    {
        RebuildUI();  // 最初にUI構築
    }

    private void Update()
    {
        if (PlayerManager.Instance.isChangeHp)
        {
            PlayerManager.Instance.isChangeHp = false;  //HPの変更フラグを下げる
            RebuildUI();  // HPが変わったらUIを再構築
        }
    }

    //ハートを生成する関数
    private void RebuildUI()
    {
        // 既存のハートを破棄
        foreach (var obj in heartObjects)
        {
            Destroy(obj);   //ハートを削除
        }
        heartObjects.Clear();   //リストをクリア

        // プレイヤーのHPを取得
        int hp = PlayerManager.Instance.hp;
        int count = hp / 2 + hp % 2;    //ハートの数

        //ハートの数分ハートを作成する
        for (int i = 0; i < count; i++)
        {
            GameObject heart = Instantiate(heartImagePrefab, transform);  // 親はこのUIオブジェクト
            Image img = heart.GetComponent<Image>();

            //ハートの種類を決定
            if (hp >= 2)    //HP2以上なら普通のハートを生成
            {
                img.sprite = fullHeart; //普通のハートをimageに代入
                hp -= 2;    //HPを減らす
            }
            else //HPが1なら半分のハートを生成
            {
                img.sprite = halfHeart; //半分のハートをimageに代入
                hp = 0;     //HPを0にする
            }

            // 配置処理
            RectTransform rt = heart.GetComponent<RectTransform>(); //ハートのポジションを取得
            rt.sizeDelta = new Vector2(heartSize, heartSize);  // サイズ設定

            // 座標計算（X方向に横並び）
            float x = startPosition.x + i * (heartSize + heartSpacing); // X方向にずらしながら生成する
            float y = startPosition.y; //yはそのまま
            rt.anchoredPosition = new Vector2(x, y);    //新しいハートのポジションを代入

            heartObjects.Add(heart);    //ハートを作成
        }
    }



}
