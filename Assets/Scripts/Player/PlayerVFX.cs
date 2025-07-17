using UnityEngine;

public class PlayerVFX : MonoBehaviour
{
    public ParticleSystem ThrustersFX;
    public ParticleSystem ShipSmoke;
    public GameObject GunMuzzles;
    public GameObject DeathFX;
    public GameObject TakeDamageFX;

    void Start()
    {
        // Just to make sure that they will not appear when the game starts.
        GunMuzzleFlash(false);
        ThrustersEmitFire(0);
    }

    public void ThrustersEmitFire(float emissionRate)
    {
        var emission = ThrustersFX.emission;
        emission.rateOverTime = emissionRate;
    }

    public void GunMuzzleFlash(bool activate)
    {
        GunMuzzles.SetActive(activate);
    }

    public void EmitSmoke(bool emit)
    {
        if (emit)
        {
            ShipSmoke.Play();
        }
        else
        {
            ShipSmoke.Stop();
            ShipSmoke.Clear();
        }
    }

    public void ExplodeVFX(Vector3 position, Quaternion rotation)
    {
        GameObject explosion = Instantiate(DeathFX, position, rotation);
        Destroy(explosion, 0.5f);
    }

    public void SparksVFX(Vector2 pointOfcontact, Quaternion rotation)
    {
        GameObject damageEffect = Instantiate(TakeDamageFX, pointOfcontact, rotation);
        Destroy(damageEffect, 3f);
    }

}