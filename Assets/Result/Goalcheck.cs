using UnityEngine;

public class Goalcheck : MonoBehaviour
{
    //public GameObject clearUI;

    public GameObject Manager;  

    public bool goal = false;
    public bool pause = false;

    void Start()
    {
        
        goal = false;
        pause = false;
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            goal = true;
            //pause = true;
            Debug.Log("ÉSÅ[ÉãÇ…è’ìÀÅI");

            
            ManagerController script = Manager.GetComponent<ManagerController>();
            script.SetClearManagerActive(true);
                //clearUI.SetActive(true);
            
        }
    }
}
