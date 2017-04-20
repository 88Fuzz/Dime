using UnityEngine;

public class CollisionAttack : MonoBehaviour
{
    public LayerMask targetLayer;
    public float attackDamage;

    public void OnCollisionStay(Collision collision)
    {
        if(LayerUtils.CompareLayerWithLayerMask(collision.gameObject.layer,targetLayer))
        {
            Hittable hittable = collision.gameObject.GetComponent<Hittable>();
            if (hittable)
                hittable.Hit(attackDamage);
        }
    }
}