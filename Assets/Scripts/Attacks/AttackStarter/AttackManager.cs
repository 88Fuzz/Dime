using UnityEngine;

/*
 * AttackStarter is used to start an attack, this is typically firing a bullet but could be anything
 * that requires triggering an attack.
 */
public abstract class AttackManager : ScriptableObject
{
    /*
     * Start an attack, this is typically firing a bullet but could be anything
     * that requires triggering an attack.
     */
    public abstract MyMonoBehaviour StartAttack(Transform spawnPosition);

    /*
     * In the case where an attack is tied to a parent attacker, this will be called
     * when the parent attacker is killed.
     */
    public abstract void EndAttack(MyMonoBehaviour myMonoBehaviour);
}