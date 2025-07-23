using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private PlayerStatsSO m_playerStats;

    void Start()
    {
        m_playerStats.SetCurrentHealth(100);
        m_playerStats.SetCurrentShield(50);
    }

}