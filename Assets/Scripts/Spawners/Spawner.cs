using UnityEngine;

/*
 * Used to indicate where enemies can be spawned in a room.
 */
public class Spawner : EditorDebug
{
    //In case a spawner is up against a door the player will enter, it can be deactivated so that enemies don't spawn on top of the player.
    public bool active;
    public Vector2 size;

    private Range xRange;
    private Range zRange;

    /*
     * The room can be rotated any direction, which would cause the original orientation of the size to not be rotated.
     * This method will orentate the spawn size to align the to rotated room.
     *
     * NOTE: This only works if rooms have been rotated by increments of perfect 90 degrees. I currently don't care to fix
     * that right now. I should probably fix it in the future.
     */
    public void CalculateSpawnArea()
    {
        Vector2 newSize = CalculateRotation(new Vector2(size.x / 2, size.y / 2));
        float absSize = Mathf.Abs(newSize.x);
        xRange = new Range(transform.position.x - absSize, transform.position.x + absSize);
        absSize = Mathf.Abs(newSize.y);
        zRange = new Range(transform.position.z - absSize, transform.position.z + absSize);
    }

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
        return new Vector3(GetSpawnPosition(xRange), .5f,
            GetSpawnPosition(zRange));
    }

    private float GetSpawnPosition(Range range)
    {
        return LevelRandomNumberGenerator.levelRNG.GetValueInRange(range);
    }

    private Vector2 CalculateRotation(Vector2 position)
    {
        //I don't care this method is deprecated because it returns radians and I want the angle in radians
        float rotation = transform.rotation.ToEulerAngles().y;
        float x = position.x * Mathf.Cos(rotation) - position.y * Mathf.Sin(rotation);
        float y = position.x * Mathf.Sin(rotation) + position.y * Mathf.Cos(rotation);

        return new Vector2(x, y);
    }

    protected override void DrawDebugGizmos()
    {
        Gizmos.DrawWireCube(new Vector3(transform.position.x, transform.position.y + Grid.CELL_SCALE/2, transform.position.z),
            new Vector3(xRange.max - xRange.min, Grid.CELL_SCALE, zRange.max - zRange.min));
        //Gizmos.DrawWireCube(new Vector3(transform.position.x, transform.position.y + Grid.CELL_SCALE/2, transform.position.z), new Vector3(size.x, Grid.CELL_SCALE, size.y));
    }

    protected override void DrawInfoGizmos()
    {
        //Do nothing
    }
}