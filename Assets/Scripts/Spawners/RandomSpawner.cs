using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class RandomSpawner : MonoBehaviour
{
    private Vector3 spawnCenter;
    private Vector3 spawnSize;

    public void Awake()
    {
        //TODO I don't think this needs to be a box collider. Why not just make some new object that has a width and height... Like a vector2??
        BoxCollider boxCollider = GetComponent<BoxCollider>();

        spawnCenter = transform.position + boxCollider.center;
        spawnSize = boxCollider.size;
    }

    public void Spawn(GameObject gameObject, RoomController roomController, int count)
    {
        for(int i = 0; i < count; i++)
        {
            Vector3 randomPosition = GetRandomPosition();
            GameObject newObject = Instantiate(gameObject, randomPosition, Quaternion.identity, null) as GameObject;
            Hittable hittable = newObject.GetComponent<Hittable>();
            if (hittable)
                hittable.roomController = roomController;
        }
    }

    private Vector3 GetRandomPosition()
    {
        //TODO figure out what to do with this y position
        return new Vector3(GetRandomFloatInRange(spawnCenter.x, spawnSize.x), .5f,
            GetRandomFloatInRange(spawnCenter.z, spawnSize.z));
    }

    private float GetRandomFloatInRange(float position, float offset)
    {
        float randomOffset = (Random.value * offset / 2) - (offset / 2);
        return position + randomOffset;
    }
}