using UnityEngine;
/*
 * Will always spawn the numberOfBulletsToSpawn when asked by the BulletManager
 */
[CreateAssetMenu(fileName = "SingleValueBulletSpawner", menuName = "ScriptableObjects/Bullets/BulletSpawnNumberDecider/SingleValueBulletSpawner")]
public class SingleValueBulletSpawner : BulletSpawnNumberDecider
{
    public int numberOfBulletToSpawn = 1;
    public override int GetNumberOfBulletsToSpawn()
    {
        return numberOfBulletToSpawn;
    }
}