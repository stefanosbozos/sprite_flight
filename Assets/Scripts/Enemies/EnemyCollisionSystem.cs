using UnityEngine;

public class EnemyCollisionSystem : MonoBehaviour
{
    public EnemyStatsSO EnemyStats;
    private VisualEffects Vfx;
    private HealthBar floatingHealthBar;

    void Awake()
    {
        Vfx = GetComponent<VisualEffects>();
        floatingHealthBar = GetComponentInChildren<HealthBar>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "laser_blue")
        {

            Projectile projectile = collision.gameObject.GetComponentInParent<Projectile>();

            if (projectile != null)
            {
                Destroy(collision.gameObject);
                TakeDamage(projectile.GetDamage(), collision);
            }

        }
    }

    void TakeDamage(float damageAmount, Collision2D collision)
    {
        EnemyStats.DecreaseHealth(damageAmount);
        floatingHealthBar.UpdateStatusBar(EnemyStats.GetHealth, EnemyStats.maxHealth);     

        Vector2 contactOfdamage = collision.GetContact(0).point;
        Vfx.TakeDamageVFX(contactOfdamage, Quaternion.identity);

        if (EnemyStats.IsDead())
        {
            Vfx.ExplodeVFX(transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

}