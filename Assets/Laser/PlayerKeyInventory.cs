using UnityEngine;

public class PlayerKeyInventory : MonoBehaviour
{
    public static bool HasKey = false;

    // 一時的にキーを拾う用
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Key"))
        {
            HasKey = true;
            Destroy(other.gameObject); // 拾ったら消す
        }
    }
}
