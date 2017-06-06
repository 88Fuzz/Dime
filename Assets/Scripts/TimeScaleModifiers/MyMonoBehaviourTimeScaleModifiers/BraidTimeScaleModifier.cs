using UnityEngine;

/*
 * The BraidTimeScaleModifier slows the entity down everything the closer they get to the BraidRing.
 */
[CreateAssetMenu(fileName = "BraidTimeScaleModifier", menuName = "ScriptableObjects/TimeScale/MyMonoBehaviourTimeScaleModifier/BraidTimeScaleModifier")]
public class BraidTimeScaleModifier : MyMonoBehaviourTimeScaleModifier
{
    public BraidRing braidRing;
    public Range affectedDistance;
    public float minimumTimeScale;

    public override float ModifyTimeScale(MyMonoBehaviour entity, float timeScale)
    {
        if (!braidRing || !braidRing.gameObject.activeInHierarchy)
            return timeScale;

        //TODO the internet tells me doing a sqrt is time consuming. Is it being used too much here?
        float distanceToRing = Vector3.Distance(entity.GetPosition(), braidRing.transform.position);
        float timeScaleModifier = 1;
        if (distanceToRing < affectedDistance.min)
            timeScaleModifier = minimumTimeScale;
        else if (distanceToRing < affectedDistance.max)
            timeScaleModifier = Mathf.Lerp(minimumTimeScale, 1, (distanceToRing - affectedDistance.min) / affectedDistance.GetDifference());

        return timeScale * timeScaleModifier;
    }
}
