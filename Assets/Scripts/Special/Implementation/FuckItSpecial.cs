using UnityEngine;

/*
 * When activated has a chance to one hit kill the player.
 */
[CreateAssetMenu(fileName = "FuckItSpecial", menuName = "ScriptableObjects/Specials/Implementation/FuckItSpecial")]
public class FuckItSpecial : SpecialAction
{
    public float percentChance;

    /*
     * When activated has a chance to one hit kill the player.
     */
    public override void DoAction(SpecialManager specialManager)
    {
        float chance = RandomNumberGeneratorUtils.unityRNG.GetRandomPercent();
        if (chance < percentChance)
        {
            PlayerHittable hittable = specialManager.player.hittable;
            hittable.Hit(hittable.GetCurrentHealth() * 3);
        }
    }
}