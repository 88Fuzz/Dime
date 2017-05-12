using UnityEngine.UI;
using UnityEngine;

public class PlayerHittable : Hittable
{
    public Slider healthSlider;

    private PlayerStats playerStats;

    public new void Awake()
    {
        base.Awake();

        maxHealth = PlayerStats.GetCurrentValue(PlayerStats.Stat.HEALTH);
    }

    protected override void HitAction()
    {
        base.HitAction();
        healthSlider.value = currentHealth / maxHealth;
    }

    //The PlayerStats singleton's maxHealth property should be updated somewhere else first before this method is called.
    public void UpdateMaxHealth()
    {
        //TODO what is the player's health gets moved to 0. They should die or something. Fix that shit.
        maxHealth = PlayerStats.GetCurrentValue(PlayerStats.Stat.HEALTH);
        Debug.Log("Health is increasing " + maxHealth);
    }
}