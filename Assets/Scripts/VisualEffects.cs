using UnityEngine;

public class VisualEffects : MonoBehaviour
{
    [SerializeField] private ParticleSystem m_thrusterVFX;
    [SerializeField] private ParticleSystem m_smokeVFX;
    [SerializeField] private GameObject m_takeDamageVFX;
    [SerializeField] private GameObject m_explosionVFX;

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
        GameObject explosion = Instantiate(m_explosionVFX, position, rotation);
        Destroy(explosion, 0.5f);
    }

    public void TakeDamageVFX(Vector2 pointOfcontact, Quaternion rotation)
    {
        GameObject damageEffect = Instantiate(m_takeDamageVFX, pointOfcontact, rotation);
        Destroy(damageEffect, 3f);
    }

}