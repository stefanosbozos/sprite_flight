using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private PlayerStatsSO m_playerStats;
    private DamageSystem m_damageSystem;

    void Awake()
    {
        m_damageSystem = GetComponent<DamageSystem>();
    }

    void Start()
    {
        m_playerStats.SetCurrentHealth(100);
        m_playerStats.SetCurrentShield(50);

        Cursor.lockState = CursorLockMode.Confined;
    }

}