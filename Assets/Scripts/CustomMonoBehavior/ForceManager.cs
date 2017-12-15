using UnityEngine;
using System.Collections.Generic;

/*
 * Force manager is used to apply forces that take into account slow motion from the MyMonoBehaviour
 */
public class ForceManager : MyMonoBehaviour
{
    public float frictionPercent;

    private class Force
    {
        private Vector3 force;
        private float timer;
        private float frictionPercent;

        public Force(Vector3 force, float durration, float frictionPercent)
        {
            this.force = force;
            this.frictionPercent = frictionPercent;
            timer = durration;
        }

        public Vector3 CalculateForce(float deltaTime)
        {
            timer -= deltaTime;
            force *= 1 - (frictionPercent * deltaTime);
            return force * deltaTime;
        }

        public bool IsForceFinished()
        {
            return timer <= 0 || force.sqrMagnitude <= 0.05f;
        }
    }

    private Rigidbody rb;
    private List<Force> forces;

    protected override void MyAwake()
    {
        rb = GetComponent<Rigidbody>();
        //TODO object pooling?
        forces = new List<Force>();
    }

    protected override void MyFixedUpdateWithDeltaTime(float myDeltaTime, float timeScale)
    {
        Vector3 netForce = new Vector3(0, 0, 0);
        rb.velocity = Vector3.zero;
        for(int i = forces.Count-1; i >= 0; i--)
        {
            Force currentForce = forces[i];
            netForce += currentForce.CalculateForce(myDeltaTime);
            if(currentForce.IsForceFinished())
                forces.RemoveAt(i);
        }

        rb.AddForce(netForce, ForceMode.Force);
    }

    public void AddForce(Vector3 force, float durration)
    {
        //TODO object pooling?
        forces.Add(new Force(force, durration, frictionPercent));
    }
}