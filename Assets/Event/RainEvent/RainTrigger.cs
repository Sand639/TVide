using UnityEngine;
using System.Collections.Generic;

public class RainTrigger : MonoBehaviour
{
    private Dictionary<Collider, float> contactTime = new Dictionary<Collider, float>();
    public float requiredTime = 0.2f;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player")) return;

        if (!contactTime.ContainsKey(other))
        {
            contactTime[other] = 0f;
        }

        contactTime[other] += Time.deltaTime;

        if (contactTime[other] >= requiredTime)
        {
            FloatingObject floating = other.GetComponent<FloatingObject>();
            if (floating != null)
            {
                floating.StartFloating();
                contactTime.Remove(other); //•‚‚©‚¹‚é
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (contactTime.ContainsKey(other))
        {
            contactTime.Remove(other);
        }
    }
}
