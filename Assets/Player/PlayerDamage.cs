using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    [SerializeField] private int Damage = 1;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {

            PlayerHP playerHP = collision.gameObject.GetComponent<PlayerHP>();

            HitStopManager.instance.StartHitStop(0.3f);//ここでヒットストップ

            if (!playerHP.Invincible)
            {
                playerHP.HP -= Damage;
                playerHP.Invincible = true;
                Destroy(this.gameObject);
            }
        }
    }

}
