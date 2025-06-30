using UnityEngine;
using UnityEngine.Events;


public class CameraManager : MonoBehaviour
{
    public UnityEvent ZoomEvent;
   
    public bool powerFlag = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Zoom()
    {
        // イベントを実行
        if (ZoomEvent != null)
            ZoomEvent.Invoke();
    }

  


}
