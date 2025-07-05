using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Scriptable Objects/Enemy")]
public class Enemy : ScriptableObject
{
    [Header("Vitals")]
    [SerializeField]
    private int health;
    [SerializeField]
    private bool hasShield;
    [SerializeField]
    private int shield;

    [Header("Movement")]
    [SerializeField]
    private float movement_speed;

}
