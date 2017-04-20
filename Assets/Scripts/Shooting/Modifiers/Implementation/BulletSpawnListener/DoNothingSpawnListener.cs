using UnityEngine;

/*
 * Does nothing when a bullet is spawned.
 */
[CreateAssetMenu(fileName = "DoNothingSpawnListener", menuName = "ScriptableObjects/Bullets/BulletSpawnListener/DoNothingSpawnListener")]
public class DoNothingSpawnListener : BulletSpawnListener
{
    public override void OnBulletSpawn(Bullet bullet)
    {
        //Do nothing.
    }
}