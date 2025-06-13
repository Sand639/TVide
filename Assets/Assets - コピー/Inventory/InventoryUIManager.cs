using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryUIManager : MonoBehaviour
{
    public static InventoryUIManager Instance;

    public List<Image> slotImages = new List<Image>();
    public Sprite emptySprite;
    public Sprite[] itemSprites;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // 起動時は全スロットを空画像に
        foreach (var img in slotImages)
        {
            img.sprite = emptySprite;
        }
    }

    //public void UpdateSlot(int index, int spriteIndex)
    //{
    //    if (index < 0 || index >= slotImages.Count) return;

    //    if (spriteIndex == -1 || itemSprites.Length == 0)
    //    {
    //        slotImages[index].sprite = emptySprite;
    //    }
    //    else
    //    {
    //        slotImages[index].sprite = itemSprites[0]; // 強制的に最初の画像を使う！
    //    }
    //}

    public void UpdateSlot(int index, int spriteIndex)
    {
        if (index < 0 || index >= slotImages.Count) return;

        if (spriteIndex == -1 || itemSprites.Length == 0 || spriteIndex >= itemSprites.Length)
        {
            slotImages[index].sprite = emptySprite;
        }
        else
        {
            slotImages[index].sprite = itemSprites[spriteIndex]; // 選択されたスプライトを描画
        }
    }

}
