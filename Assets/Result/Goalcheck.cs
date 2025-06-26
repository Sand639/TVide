using UnityEngine;

public class Goalcheck : MonoBehaviour
{
    public GameObject clearUI;
    public bool goal = false;

    void Start()
    {
        goal = false;
        if (clearUI != null)
        {
            clearUI.SetActive(false); // Å‰‚Í”ñ•\¦‚É‚µ‚Ä‚¨‚­
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            goal = true;
            Debug.Log("ƒS[ƒ‹‚ÉÕ“ËI");

            if (clearUI != null)
            {
                clearUI.SetActive(true);
            }
        }
    }
}
