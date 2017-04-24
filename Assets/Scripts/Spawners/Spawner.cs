using System;
using UnityEngine;

public class Spawner : EditorDebug
{
    public bool active;
    public Vector2 size;

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
        Gizmos.DrawWireCube(transform.position, new Vector3(size.x, Grid.CELL_SCALE, size.y));
        throw new NotImplementedException();
    }

    protected override void DrawInfoGizmos()
    {
        //Do nothing
    }
}