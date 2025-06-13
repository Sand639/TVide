using UnityEngine;

[System.Serializable]

public class RespawnableItem
{
    public GameObject prefab;
    public Vector3 position;
    public Quaternion rotation;

    public RespawnableItem(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        this.prefab = prefab;
        this.position = position;
        this.rotation = rotation;
    }
}
