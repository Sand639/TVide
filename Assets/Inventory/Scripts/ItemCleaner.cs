using UnityEngine;

public class ItemCleaner : MonoBehaviour
{
    [Header("アイテムタグ設定")]
    public string itemTag = "InventoryItem";

    // 一つだけアイテムを削除する
    public void RemoveOneItem()
    {
        GameObject[] items = GameObject.FindGameObjectsWithTag(itemTag);

        if (items.Length > 0)
        {
            // 最初のアイテムを削除
            Destroy(items[0]);
            Debug.Log("1つのアイテムを削除しました。");
        }
        else
        {
            Debug.Log("アイテムが見つかりません。");
        }
    }
}
