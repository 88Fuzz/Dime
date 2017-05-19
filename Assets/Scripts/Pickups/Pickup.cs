using UnityEngine;

public abstract class Pickup : MonoBehaviour
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
        //TODO I don't think this check should be here anymore, now that I know how to change the physics collisions of layers
        if(LayerUtils.CompareLayerWithLayerMask(collider.gameObject.layer, playerLayer))
        {
            PickedUp(collider.gameObject.GetComponent<Player>());
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
    protected abstract void PickedUp(Player player);
}