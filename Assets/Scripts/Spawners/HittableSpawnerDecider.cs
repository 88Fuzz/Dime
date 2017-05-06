using UnityEngine;

/*
 * Returns the set of enemies that should be spawned in the room.
 */
[CreateAssetMenu(fileName = "HittableSpawnerDecider", menuName = "ScriptableObjects/Misc/HittableSpawnerDecider")]
public class HittableSpawnerDecider : ScriptableObject
{
    public int numberOfEnemies;
    public Hittable enemy;

    /*
     * Returns the set of enemies that should be spawned in the room.
     */
    public Hittable[] GetHittablesForRoom()
    {
        //TODO constantly creating arrays here is possibly expensive? Maybe have a set of arrays that are free that you can use?
        Hittable[] hittables = new Hittable[numberOfEnemies];
        Debug.Log("TRYING TO SPAWN THIS MANY ENEMIES " + numberOfEnemies);
        for(int i = 0; i < numberOfEnemies; i++)
            hittables[i] = enemy;

        return hittables;
    }
}