using UnityEngine;
using System.Collections;

public class PickupSpawner : MonoBehaviour
{
    public Pickup[] pickups;
    public float waitSpawnTime = .2f;

    private int playerLayer;
    private bool shouldActivateUpgrades;
    private bool upgradesActive;
    private bool playerInTheWay;

    public void Awake()
    {
        playerLayer = LayerMask.GetMask("Player");
        playerInTheWay = false;
    }
    
    public void FixedUpdate()
    {
        if(upgradesActive)
        {
            foreach(Pickup pickup in pickups)
            {
                if(pickup.pickedUp)
                {
                    DeactivateUpgrades();
                    break;
                }
            }
        }
    }

    public bool Collected()
    {
        return !upgradesActive;
    }

    public void DeactivateUpgrades()
    {
        shouldActivateUpgrades = false;
        upgradesActive = false;

        foreach (Pickup pickup in pickups)
        {
            pickup.Deactivate();
            Destroy(pickup.gameObject);
        }
    }

    public void ActivateUpgrades()
    {
        upgradesActive = true;
        StartCoroutine(WaitToSpawn());
    }

    public void OnTriggerEnter(Collider collider)
    {
        if (LayerUtils.CompareLayerWithLayerMask(collider.gameObject.layer, playerLayer))
            playerInTheWay = true;
    }

    public void OnTriggerExit(Collider collider)
    {
        if(LayerUtils.CompareLayerWithLayerMask(collider.gameObject.layer, playerLayer))
        {
            playerInTheWay = false;
            if (shouldActivateUpgrades)
                EnableUpgrades();
        }
    }

    private void EnableUpgrades()
    {
        upgradesActive = true;
        shouldActivateUpgrades = false;
        foreach (Pickup pickup in pickups)
        {
            pickup.Activate();
        }
    }

    private IEnumerator WaitToSpawn()
    {
        yield return new WaitForSeconds(waitSpawnTime);

        if (playerInTheWay)
            shouldActivateUpgrades = true;
        else
            EnableUpgrades();
    }
}