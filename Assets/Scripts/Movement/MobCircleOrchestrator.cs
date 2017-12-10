using System.Collections.Generic;
using UnityEngine;

public class MobCircleOrchestrator : MyMonoBehaviour
{
    public Range numberOfCirclers;
    public MobCircler prefab;
    public float rotationSpeed;
    public float circleDistance;
    public LayerMask targetMask;
    public int scanDistance;

    private List<MobCircler> mobCirclers;
    private Transform targetTransform;
    private float startingAngle;

    protected override void MyAwake()
    {
        int numberOfMobs = (int) LevelRandomNumberGenerator.levelRNG.GetValueInRange(numberOfCirclers);
        //TODO Object pooling
        mobCirclers = new List<MobCircler>(numberOfMobs);
        startingAngle = 0;
        Collider[] colliderResults = new Collider[1];
        if (Physics.OverlapSphereNonAlloc(transform.position, scanDistance, colliderResults, targetMask, QueryTriggerInteraction.Ignore) > 0)
        {
            Collider collider = colliderResults[0];
            targetTransform = collider.gameObject.transform;
        }
        else
        {
            targetTransform = transform;
            Debug.Log("Yo, I can't find a damn target to hit :(");
        }

        for(int i=0; i<numberOfMobs; i++)
        {
            MobCircler mobCircler = Instantiate(prefab, transform);
            mobCircler.lookAtTarget = targetTransform;
            mobCircler.RegisterOnDestroyListener(MobCircleDestroyed);
            mobCirclers.Add(mobCircler);
        }
    }

    protected override void MyFixedUpdateWithDeltaTime(float myDeltaTime, float timeScale)
    {
        if(mobCirclers.Count == 0)
        {
            //Destroy the object as a hittable so that the kill message is sent
            Hittable hittable = GetComponent<Hittable>();
            hittable.Hit(1);
            return;
        }

        startingAngle += rotationSpeed * myDeltaTime;
        //TODO Only set this stuff if the player has moved a bunch?
        SetCircleDistances();
    }

    private void SetCircleDistances()
    {
        Vector3 targetPosition = targetTransform.position;
        float currentAngle = startingAngle;
        float incrementFactor = Mathf.Deg2Rad * 360 / mobCirclers.Count;
        foreach(MobCircler circler in mobCirclers)
        {
            float zOffset = circleDistance * Mathf.Sin(currentAngle);
            float xOffset = circleDistance * Mathf.Cos(currentAngle);

            circler.targetLocation = new Vector3(targetPosition.x + xOffset, targetPosition.y, targetPosition.z + zOffset);
            currentAngle += incrementFactor;
        }
    }

    private void MobCircleDestroyed(GameObject gameObject)
    {
        for (int i = mobCirclers.Count - 1; i >= 0; i--)
        {
            if(mobCirclers[i].gameObject == gameObject)
                mobCirclers.RemoveAt(i);
        }
    }
}