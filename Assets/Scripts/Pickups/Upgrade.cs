using UnityEngine;

public abstract class Upgrade : MonoBehaviour
{
    public bool pickedUp;

    private int playerLayer;

	public void Awake()
    {
        playerLayer = LayerMask.GetMask("Player");
        pickedUp = false;
	}

    public void OnTriggerEnter(Collider collider)
    {
        if(LayerUtils.CompareLayerWithLayerMask(collider.gameObject.layer, playerLayer))
        {
            UpgradePickedUp(collider.gameObject);
            pickedUp = true;
        }
    }

    public void Activate()
    {
        gameObject.SetActive(true);
        pickedUp = false;
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
        pickedUp = false;
    }

    /*
     * Method called when the player picks up the current pickup.
     */
    protected abstract void UpgradePickedUp(GameObject player);
}