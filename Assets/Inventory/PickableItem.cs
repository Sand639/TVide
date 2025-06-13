using UnityEngine;

public class PickableItem : MonoBehaviour
{
    public Texture itemTexture;
    public GameObject itemPrefab;
    public Camera renderCamera;

    void Update()
    {
        if (renderCamera != null && Input.GetMouseButtonDown(0))
        {
            Ray ray = renderCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.gameObject == gameObject)
            {
                TryPickup();
            }
        }
    }

    void OnMouseDown()
    {
        TryPickup();
    }

    void TryPickup()
    {
        InventoryManager inventory = FindObjectOfType<InventoryManager>();
        if (inventory != null)
        {
            //アイテムがある場合は拾えないようにする
            if (inventory.HasAnyItem())
            {
                Debug.Log("すでにアイテムを所持しています。");
                return;
            }

            bool added = inventory.AddSceneItemToInventory(itemTexture, itemPrefab, gameObject);
            if (added)
            {
                gameObject.SetActive(false); 
            }
        }
    }

}
