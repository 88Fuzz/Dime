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
    //TODO this SpecialAction is only for testing. It should be removed at some point.
    public SpecialAction testingSpecialAction;
    //move this object to BraidSpecial
    public MyMonoBehaviourTimeScaleModifier testingModifier;

    public void Awake()
    {
        specialManager = ScriptableObject.CreateInstance<SpecialManager>();
        specialManager.player = this;

        if (testingSpecialAction)
            specialManager.SpecialAction = testingSpecialAction;
    }
}