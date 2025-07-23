using UnityEngine;

public class EnemyCollisionSystem : MonoBehaviour
{
    public EnemyStatsSO EnemyStats;
    public GameHUD GameHud;
    private VisualEffects Vfx;

    void Awake()
    {
        GameHud = GetComponentInChildren<GameHUD>();
        Vfx = GetComponent<VisualEffects>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "player_fire")
        {

            Projectile projectile = collision.gameObject.GetComponentInParent<Projectile>();

            if (projectile != null)
            {
                GameHud.UpdateScore(EnemyStats.m_scoreValue);
                Destroy(collision.gameObject);
                Vfx.ExplodeVFX(transform.position, transform.rotation);
                Destroy(gameObject);
            }

        }
    }

}