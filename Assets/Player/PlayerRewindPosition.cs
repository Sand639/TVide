using UnityEngine;
using System.Collections.Generic;

public class PlayerRewindPosition : MonoBehaviour
{
    public float rewindTime = 1f;

    private Queue<Vector3> positionHistory = new Queue<Vector3>();
    private float recordInterval = 0.1f;
    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= recordInterval)
        {
            timer = 0f;
            positionHistory.Enqueue(transform.position);
            if (positionHistory.Count > Mathf.CeilToInt(rewindTime / recordInterval))
            {
                positionHistory.Dequeue();
            }
        }
    }

    public void RewindToPastPosition()
    {
        if (positionHistory.Count > 0)
        {
            transform.position = positionHistory.Peek();
        }
    }
}
