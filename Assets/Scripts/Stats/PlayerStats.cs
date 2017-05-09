/*
 * class used to hold PlayerStats like movement speed, damage, health, etc.
 */
public class PlayerStats
{
    //TODO these should all have min/max values with those special C# setters and shit.
    //TODO something seems off with these movementSpeed and maxMovementSpeed. Figure it out at some point.
    public static float movementSpeed = 600; //units per second
    public static float maxMovementSpeed = 2000; //units per second
    public static float maxHealth = 100;
    public static float invincibilityCount = 3;

    //Bullet information
    public static float shootDelay = .5f;
    public static float shootDamageMultiplier = 1.0f;
    public static float shootSpeed = 20;
    public static float shootSize = .25f;

    public static float shootCritDamageMultiplier = 2f;
    public static float shootGlanceDamageMultiplier = .5f;

    public static float shootCritChance = 2.0f;
    public static float shootGlanceChance = 5.0f;
}