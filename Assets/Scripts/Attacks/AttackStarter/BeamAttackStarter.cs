using UnityEngine;

[CreateAssetMenu(fileName = "BeamShooter", menuName = "ScriptableObjects/Enemy/AttackManager/BeamShooter")]
public class BeamShooter : AttackManager
{
    public Beam beam;
    public override void EndAttack(MyMonoBehaviour myMonoBehaviour)
    {
        //TODO implement
    }

    public override MyMonoBehaviour StartAttack(Transform spawnPosition)
    {
        return Instantiate(beam, spawnPosition.position, spawnPosition.rotation, null) as MyMonoBehaviour;
    }
}