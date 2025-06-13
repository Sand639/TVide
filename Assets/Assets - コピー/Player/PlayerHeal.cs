using UnityEngine;

public class PlayerHeal : MonoBehaviour
{
    [SerializeField] private int Heal = 1;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerHP PlayerHP;
            PlayerHP = collision.gameObject.GetComponent<PlayerHP>();
            
            PlayerHP.HP = PlayerHP.HP + Heal;
                
            Destroy(this.gameObject);
            
        }
    }
   }
