using UnityEngine;

/*
 * The longer the fire button is held down, the faster the bullets will shoot.
 */
[CreateAssetMenu(fileName = "DecreaseShootDelayModifier", menuName = "ScriptableObjects/Shooting/ShootDelayModifier/DecreaseShootDelayModifier")]
//TODO should this pickup also make the player's shooting delay stat go to the maximum value?
public class DecreaseShootDelayModifier : ShootDelayModifier
{
    private static readonly int SHOTS_FIRE_BEFORE_INCREASING_RATE = 5;
    //TODO all this state information stored here won't be good for multiple players :(
    int fireCount;
    float currentDelay;
    float valueChange;
    float minValue;

    public override void InitModifier()
    {
        ActionManager actionManager = Singleton<ActionManager>.Instance;
        actionManager.RegisterStartButtonListener(InputButton.PRIMARY_ATTACK, FirePressed);
        actionManager.RegisterEndButtonListener(InputButton.PRIMARY_ATTACK, FireReleased);

        fireCount = 0;
        currentDelay = PlayerStats.GetCurrentValue(PlayerStats.Stat.SHOOT_DELAY);
        valueChange = PlayerStats.GetChangeValue(PlayerStats.Stat.SHOOT_DELAY);
        minValue = PlayerStats.GetMinValue(PlayerStats.Stat.SHOOT_DELAY);
    }

    /*
     * The longer the fire button is held down, the faster the bullets will shoot.
     */
    public override float GetShootDelay(ShootingManager shootingManager)
    {
        if(++fireCount > SHOTS_FIRE_BEFORE_INCREASING_RATE)
        {
            fireCount = 0;
            currentDelay -= valueChange;
            if (currentDelay < minValue)
                currentDelay = minValue;
        }

        return currentDelay;
    }

    public void FirePressed(InputButton inputButton)
    {
        ResetData();
    }

    public void FireReleased(InputButton inputButton)
    {
        ResetData();
    }

    private void ResetData()
    {
        fireCount = 0;
        currentDelay = PlayerStats.GetCurrentValue(PlayerStats.Stat.SHOOT_DELAY);
    }
}