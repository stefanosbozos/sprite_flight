public interface ICanAttack
{
    public static float minTimeToAttack = 1.5f;
    public static float maxTimeToAttack = 5.0f;

    public void Attack();
    public bool IsReadyToAttack();

}