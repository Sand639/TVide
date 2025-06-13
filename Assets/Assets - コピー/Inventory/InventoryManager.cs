using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public RawImage[] inventorySlots;
    public Texture[] itemTextures;
    public GameObject[] gimmickPrefabs;
    public Button[] slotButtons;

    private List<GameObject> sceneItemsToRespawn = new List<GameObject>();

    public bool HasAnyItem()
    {
        foreach (var prefab in gimmickPrefabs)
        {
            if (prefab != null) return true;
        }
        return false;
    }

    public bool AddSceneItemToInventory(Texture itemTexture, GameObject itemPrefab, GameObject sceneObject)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i].texture == null)
            {
                inventorySlots[i].texture = itemTexture;
                gimmickPrefabs[i] = itemPrefab;
                itemTextures[i] = itemTexture;

                if (slotButtons != null && i < slotButtons.Length)
                {
                    slotButtons[i].interactable = true;
                }

                sceneItemsToRespawn.Add(sceneObject);
                return true;
            }
        }

        Debug.Log("インベントリがいっぱいです！");
        return false;
    }


    public GameObject GetFirstAvailableItemPrefab()
    {
        foreach (var prefab in gimmickPrefabs)
        {
            if (prefab != null)
            {
                return prefab;
            }
        }
        return null;
    }

    public void ConsumeFirstAvailableItem()
    {
        for (int i = 0; i < gimmickPrefabs.Length; i++)
        {
            if (gimmickPrefabs[i] != null)
            {
                gimmickPrefabs[i] = null;
                itemTextures[i] = null;
                inventorySlots[i].texture = null;
                if (slotButtons[i] != null) slotButtons[i].interactable = false;
                break;
            }
        }
    }

    public void RespawnSceneItem(GameObject itemPrefab)
    {
        foreach (var obj in sceneItemsToRespawn)
        {
            if (obj != null && obj.name.Contains(itemPrefab.name))
            {
                obj.SetActive(true);
                sceneItemsToRespawn.Remove(obj);
                break;
            }
        }
    }
}
