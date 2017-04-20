using UnityEngine.UI;
using UnityEngine;

public class PlayerHittable : Hittable
{
    public Slider healthSlider;

    private PlayerStats playerStats;

    public new void Awake()
    {
        base.Awake();

        maxHealth = PlayerStats.maxHealth;
    }

    protected override void HitAction()
    {
        base.HitAction();
        healthSlider.value = currentHealth / maxHealth;
    }

    //The PlayerStats singleton's maxHealth property should be updated somewhere else first before this method is called.
    public void UpdateMaxHealth()
    {
        maxHealth = PlayerStats.maxHealth;
        Debug.Log("Health is increasing " + maxHealth);
    }
}