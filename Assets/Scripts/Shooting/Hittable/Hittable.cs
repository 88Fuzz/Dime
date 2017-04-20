﻿using UnityEngine;
using System.Collections;

public class Hittable : MonoBehaviour
{
    public RoomController roomController;
    public float maxHealth;
    public Color hitColor = Color.red;
    public int hitColorFlashCount;
    public float hitFlashSpeed;
    public float hitColorFlashDuration;
    public bool invulnerableWhenHit;

    protected bool invulnerable;
    protected float currentHealth;
    protected LayerMask enemyLayer;
    private Color baseColor;
    private Renderer materialRenderer;

	public void Awake()
    {
        materialRenderer = GetComponent<Renderer>();
        invulnerable = false;
        currentHealth = maxHealth;
        enemyLayer = LayerMask.GetMask("Enemy");
	}

    public void Heal(float heal)
    {
        if (heal <= 0)
            return;

        HealAction();

        currentHealth += heal;

        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
    }

    protected virtual void HealAction()
    {
        //Do nothing by default
    }
	
    /*
     * Returns true if the Hittable was killed by this attack. Else the Hittable is stil alive.
     */
    public bool Hit(float hitStrength)
    {
        if (invulnerable)
            return false;

        currentHealth -= hitStrength;
        if (currentHealth <= 0)
        {
            DeadAction();
            return true;
        }

        HitAction();
        return false;
    }

    protected virtual void HitAction()
    {
        if(invulnerableWhenHit)
            StartCoroutine(InvulnerableCoroutine(hitColorFlashCount * hitColorFlashDuration));

        if(hitColorFlashCount > 0)
            StartCoroutine(HitColorCoroutine());
    }

    protected virtual void DeadAction()
    {
        if(LayerUtils.CompareLayerWithLayerMask(gameObject.layer, enemyLayer) && roomController)
            roomController.EnemyKilled(1);
        Destroy(gameObject);
    }

    private IEnumerator HitColorCoroutine()
    {
        int flashCount = 0;
        baseColor = materialRenderer.material.color;
        do
        {
            materialRenderer.material.color = hitColor;
            float timer = 0;

            while(timer < hitColorFlashDuration)
            {
                materialRenderer.material.color = Color.Lerp(materialRenderer.material.color, baseColor, hitFlashSpeed * Time.deltaTime);
                timer += Time.deltaTime;
                yield return null;
            }
        } while (++flashCount < hitColorFlashCount);
        materialRenderer.material.color = baseColor;
    }

    private IEnumerator InvulnerableCoroutine(float invulnerableTimer)
    {
        invulnerable = true;
        yield return new WaitForSeconds(invulnerableTimer);
        invulnerable = false;
    }
}