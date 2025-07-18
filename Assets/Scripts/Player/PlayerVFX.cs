using UnityEngine;

public class PlayerVFX : MonoBehaviour
{
    [SerializeField] private PlayerStatsSO m_playerStats;
    [SerializeField] private ParticleSystem m_thrusterVFX;
    [SerializeField] private ParticleSystem m_smokeVFX;

    void Start()
    {
        // Just to make sure that they will not appear when the game starts.
        ThrustersEmitFire(0);
    }

    public void ThrustersEmitFire(float emissionRate)
    {
        var emission = m_thrusterVFX.emission;
        emission.rateOverTime = emissionRate;
    }

    public void EmitSmoke(bool emit)
    {
        if (emit)
        {
            m_smokeVFX.Play();
        }
        else
        {
            m_smokeVFX.Stop();
            m_smokeVFX.Clear();
        }
    }

    public void ExplodeVFX(Vector3 position, Quaternion rotation)
    {
        GameObject explosion = Instantiate(m_playerStats.ExplodeFX, position, rotation);
        Destroy(explosion, 0.5f);
    }

    public void SparksVFX(Vector2 pointOfcontact, Quaternion rotation)
    {
        GameObject damageEffect = Instantiate(m_playerStats.TakeDamageFX, pointOfcontact, rotation);
        Destroy(damageEffect, 3f);
    }

}