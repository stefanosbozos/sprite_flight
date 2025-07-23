using UnityEngine;

public class FighterShip : Enemy
{
    void Start()
    {
        EnemyAI.GenerateDistanceFromPlayer();
        EnemyStats.SetHealth(100);
        floatingHealthBar.UpdateStatusBar(EnemyStats.GetHealth, EnemyStats.maxHealth);
    }

    void Update()
    {
        FollowPlayer(m_targetPosition.position);
        Vfx.ThrustersEmitFire(m_rb.linearVelocity.sqrMagnitude);
    }

    protected override void FollowPlayer(Vector3 targetPosition)
    {
        // Finds the distance from the player and maintains a constant distance
        if (targetPosition != null)
        {
            EnemyAI.SetDistanceFromPlayer(Vector3.Distance(transform.position, targetPosition));
            Vector3 direction = (transform.position - targetPosition).normalized;

            if (EnemyAI.DistanceFromPlayer < EnemyAI.DistanceFromPlayerLimit)
            {
                // Get away from the player
                m_rb.AddForce(direction * EnemyStats.movement_speed, ForceMode2D.Force);
            }
            else
            {
                // Go to the player's position
                m_rb.AddForceAtPosition(-direction * EnemyStats.movement_speed, targetPosition);
            }
        }
    }

}