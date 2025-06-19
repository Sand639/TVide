using UnityEngine;

public class PickableItem : MonoBehaviour
{
    public Texture itemTexture;
    public GameObject itemPrefab;
    public Camera renderCamera;
    public int monitorIndex = 0; // インスペクタで設定する

    void Update()
    {
        if (renderCamera != null && Input.GetMouseButtonDown(0))
        {
            // 今ズームしてるモニターが自分のモニターか？
            if (MonitorZoomController.CurrentZoomIndex != monitorIndex) return;

            Ray ray = renderCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.gameObject == gameObject)
            {
                TryPickup();
            }
        }
    }

    void OnMouseDown()
    {
        if (MonitorZoomController.CurrentZoomIndex != monitorIndex) return;
        TryPickup();
    }

    void TryPickup()
    {
        InventoryManager inventory = FindObjectOfType<InventoryManager>();
        if (inventory != null)
        {
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
