using UnityEngine;

/*
 * Used to indicate where enemies can be spawned in a room.
 */
public class Spawner : EditorDebug
{
    //In case a spawner is up against a door the player will enter, it can be deactivated so that enemies don't spawn on top of the player.
    public bool active;
    the size needs to take into account the rotation of the room. Somehow!
    public Vector2 size;

    /*
     * Pick a random location within the box for the hittable to spawn and spawn it at that location.
     * Return true if the hittable was spawned. False if not.
     */
    public bool Spawn(Hittable hittable, Room parentRoom)
    {
        if (active)
        {
            Vector3 randomPosition = GetRandomPosition();
            GameObject newObject = Instantiate(hittable.gameObject, randomPosition, Quaternion.identity, null) as GameObject;
            Hittable newHittable = newObject.GetComponent<Hittable>();
            parentRoom.activeEnemies.Add(newHittable);
            if (newHittable)
                newHittable.parentRoom = parentRoom;
        }

        return active;
    }

    private Vector3 GetRandomPosition()
    {
        //TODO figure out what to do with this y position
        return new Vector3(GetSpawnPosition(transform.position.x, size.x), .5f,
            GetSpawnPosition(transform.position.z, size.y));
    }

    private float GetSpawnPosition(float position, float size)
    {
        return position + LevelRandomNumberGenerator.levelRNG.GetValueInRange(-1 * size / 2, size / 2);
    }

    protected override void DrawDebugGizmos()
    {
        Gizmos.DrawWireCube(new Vector3(transform.position.x, transform.position.y + Grid.CELL_SCALE/2, transform.position.z), new Vector3(size.x, Grid.CELL_SCALE, size.y));
    }

    protected override void DrawInfoGizmos()
    {
        //Do nothing
    }
}