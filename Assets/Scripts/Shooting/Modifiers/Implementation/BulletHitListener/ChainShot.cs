using UnityEngine;

/*
 * Bullet has a probablility of spawning a second bullet after hitting/killing an enemy.
 */
[CreateAssetMenu(fileName = "ChainShot", menuName = "ScriptableObjects/Bullets/BulletHitListener/ChainShot")]
public class ChainShot : BulletHitListener
{
    [Range(0,100)]
    public float spawnPercent;
    public LayerMask enemyLayer;
    public LayerMask floorLayer;
    public float scanRadius;
    public float verticalOffsetFromFloor;

    /*
     * 2 was chosen here because we only need one new enemy that's NOT the enemy that was already hit.
     * I do not how how the OverlapSphere call works, so we allocate room for 2 enemies to be found.
     * One for the enemy that is currently hit and another for the future enemy to hit.
     */
    private Collider[] colliderResults = new Collider[2];

    /*
     * One raycast is hit here because there should only ever be one floor hit below the enemy.
     */
    private RaycastHit[] rayCastResults = new RaycastHit[1];

    public override bool OnEnemyHit(Bullet bullet, Hittable enemy)
    {
        PossiblyCreateBullet(bullet, enemy.gameObject);
        return true;
    }

    public override bool OnEnemyKill(Bullet bullet, Hittable enemy)
    {
        PossiblyCreateBullet(bullet, enemy.gameObject);
        return true;
    }

    public override void OnObjectHit(Bullet bullet, GameObject obj)
    {
        //Do nothing
    }

    private void PossiblyCreateBullet(Bullet bullet, GameObject hitGameObject)
    {
        float percent = RandomNumberGeneratorUtils.unityRNG.GetValueInRange(0, 100);
        if (percent < spawnPercent)
        {
            Hittable hittable = FindEnemy(bullet.transform, hitGameObject);
            if (hittable)
                CreateBullet(bullet, hittable);
        }
    }

    private Hittable FindEnemy(Transform transform, GameObject hitGameObject)
    {
        int numberFound = Physics.OverlapSphereNonAlloc(transform.position, scanRadius, colliderResults, enemyLayer, QueryTriggerInteraction.Ignore);
        if (numberFound == 0)
            return null;

        for(int i = 0; i < numberFound; i++)
        {
            Collider collider = colliderResults[i];
            if(collider.gameObject == hitGameObject)
                continue;

            return collider.gameObject.GetComponent<Hittable>();
        }

        return null;
    }

    private void CreateBullet(Bullet bullet, Hittable hittable)
    {
        Bullet spawnedBullet = bullet.player.shootingManager.SpawnBulletCopy(bullet);
        Ray ray = new Ray(spawnedBullet.transform.position, Vector3.down);
        int hit = Physics.RaycastNonAlloc(ray, rayCastResults, verticalOffsetFromFloor, floorLayer, QueryTriggerInteraction.Ignore);
        if(hit != 0)
        {
            Vector3 bulletPosition = spawnedBullet.transform.position;
            RaycastHit raycastHit = rayCastResults[0];
            bulletPosition.y = raycastHit.point.y + verticalOffsetFromFloor;

            spawnedBullet.transform.position = bulletPosition;
        }

        Vector3 lookAtPosition = hittable.transform.position;
        lookAtPosition.y = bullet.transform.position.y;
        spawnedBullet.transform.LookAt(lookAtPosition);
        spawnedBullet.CalculateForwardVelocity();
    }
}