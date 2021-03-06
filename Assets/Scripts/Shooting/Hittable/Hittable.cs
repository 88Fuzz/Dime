﻿using UnityEngine;
using System.Collections;

public class Hittable : MonoBehaviour
{
    [System.Serializable]
    public struct HittableSpawnArea
    {
        public Vector3 halfDimentions;
    }

    public HittableKilledAction[] killActions;
    public HittableHitAction[] hitActions;
    public HittableSpawnArea hittableSpawnArea;
    public Room parentRoom = null;
    public float maxHealth;
    public float ySpawnOffset;
    //The special charge is the amount it will charge the player's special meter when the hittable is killed
    public float specialCharge;
    public Color hitColor = Color.red;
    public int hitColorFlashCount;
    public float hitFlashSpeed;
    public float hitColorFlashDuration;
    public bool invulnerableWhenHit;
    public Renderer materialRenderer;

    protected bool invulnerable;
    protected float currentHealth;
    private Color baseColor;
    private MyMonoBehaviour myMonoBehaviour;

    public void Awake()
    {
        Singleton<EventManager>.Instance.PublishEvent(EventManager.EventName.HittableSpawned);
        invulnerable = false;
        currentHealth = maxHealth;
        myMonoBehaviour = GetComponent<MyMonoBehaviour>();
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

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    protected virtual void HealAction()
    {
        //TODO there should probably be Heal Listeners to go along with the development practices of this game
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

        foreach(HittableHitAction action in hitActions)
        {
            action.HittableHasBeenHit(this);
        }
    }

    protected virtual void DeadAction()
    {
        Singleton<EventManager>.Instance.PublishEvent(EventManager.EventName.HittableDestroyed);
        foreach(HittableKilledAction action in killActions)
        {
            action.HittableHasBeenKilled(this);
        }

        if(myMonoBehaviour)
            myMonoBehaviour.MyDestroy();
        else
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
