using UnityEngine;

public class Spawner : MonoBehaviour
{
    public bool active;
    public Vector2 size;

    public void Start()
    {

    }

    public void Spawn(GameObject gameObject, RoomController roomController, int count)
    {
        for (int i = 0; i < count; i++)
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
        return new Vector3();
        //TODO have a seeded random and a "don't care" random
        //TODO figure out what to do with this y position
        //return new Vector3(GetRandomFloatInRange(spawnCenter.x, spawnSize.x), .5f,
            //GetRandomFloatInRange(spawnCenter.z, spawnSize.z));
    }
}