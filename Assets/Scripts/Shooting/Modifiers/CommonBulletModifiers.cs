using System.Collections.Generic;
using UnityEngine;

/*
 * List of common Modifiers for bullets that will always be used.
 */
[CreateAssetMenu(fileName = "CommonBulletModifiers", menuName = "ScriptableObjects/Bullets/CommonBulletModifiers")]
public class CommonBulletModifiers : ScriptableObject 
{
    public List<BulletHitListener> commonHitListeners;
}