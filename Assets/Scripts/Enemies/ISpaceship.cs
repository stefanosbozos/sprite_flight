public interface ISpaceship
{
    public static float minDistanceFromPlayer = 5.0f;
    public static float maxDistanceFromPlayer = 15.0f;

    public void EngageWithPlayer();

    public void Thrust();
}