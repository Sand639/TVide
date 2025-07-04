using UnityEngine;

public class WarpDoor : MonoBehaviour
{
    public Transform warpDestination;
    public bool requiresKey = false;
    public Texture lockedTexture;
    public Texture unlockedTexture;

    private Renderer rend;
    private bool playerInRange = false;

    void Start()
    {
        rend = GetComponent<Renderer>();
        UpdateTexture();
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!requiresKey || PlayerKeyInventory.HasKey)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                player.transform.position = warpDestination.position;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    void OnValidate()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null && renderer.sharedMaterial != null)
        {
            renderer.sharedMaterial.mainTexture = requiresKey ? lockedTexture : unlockedTexture;
        }
    }

    void UpdateTexture()
    {
        if (rend != null)
        {
            rend.material.mainTexture = requiresKey ? lockedTexture : unlockedTexture;
        }
    }
}
