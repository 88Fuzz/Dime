using UnityEngine;

/*
 * Collection of Managers used by the Player.
 */
public class Player : MonoBehaviour 
{
    public ShootingManager shootingManager;
    public BulletManager bulletManager;
    public SpecialManager specialManager;
    public PlayerHittable hittable;

    public void Awake()
    {
        specialManager = ScriptableObject.CreateInstance<SpecialManager>();
    }
}