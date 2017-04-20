using UnityEngine;

public abstract class BulletProperties : ScriptableObject
{
    public abstract void Initialize();

    public abstract void OnHitObject(GameObject obj);

    public abstract void OnHitEnemy(Hittable hittable);

    public abstract void OnEnemyKill(Hittable hittable);

    public abstract Vector3 GetInitialVelocity();

    public abstract Vector3 GetVolocityChange(Vector3 currentVelocity);

    public abstract GameObject GetBulletModel();

    public abstract float GetDamage();
}