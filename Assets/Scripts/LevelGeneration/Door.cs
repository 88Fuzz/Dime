using UnityEngine;

public class Door : MonoBehaviour
{
    public static readonly float DOOR_SIZE = 10; //In unity units

    public bool isConnected = false;
    public bool isEnterance = false;
    public Door connectedDoor;
    public GameObject door = null;

    private bool doorOpen = false;

    public void Open()
    {
        if (!isConnected || doorOpen)
            return;

        door.SetActive(false);
        doorOpen = true;
        connectedDoor.Open();
    }

    public void Close()
    {
        if (!isConnected || !doorOpen)
            return;

        doorOpen = false;
        door.SetActive(true);
    }

    public void ConnectDoor(GameObject doorObject, Door otherDoor, bool isEnterance)
    {
        this.isEnterance = isEnterance;
        isConnected = true;
        connectedDoor = otherDoor;
        ApplyDoorObject(doorObject);
    }

    public void ApplyDoorObject(GameObject doorObject)
    {
        if (door != null)
            Destroy(door);

        door = Instantiate(doorObject, transform);
        door.transform.position = new Vector3(transform.position.x, transform.position.y + DOOR_SIZE / 2, transform.position.z);
        //TODO figure out what the logic should be for this mother fucker!!! if the door is on the north or south side, the yRotation should be 0.
        //TODO if it is on the east or west side it should be +90. Options on the top of my head
        //1) check if the transform.rotation == 0 or 180 (with in some % error) and if so, add 90
        //2) have some public variable float in the Door class that says how much yRotation to add
        //3) Look at how the original FloorGenerator algorithm does the normalize angle thing. It does something that may be of use here.
        int yRotation = 90;
        door.transform.localEulerAngles = new Vector3(0, yRotation, 0);
    }
}