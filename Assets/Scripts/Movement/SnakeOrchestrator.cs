using UnityEngine;
using System.Collections.Generic;

public class SnakeOrchestrator : MyMonoBehaviour
{
    public Range numberOfNodes;
    public Snake snake;

    private List<Snake> snakeNodes;

    protected override void MyAwake()
    {
        //TODO object pooling
        int numberOfSnakeNodes = (int)LevelRandomNumberGenerator.levelRNG.GetValueInRange(numberOfNodes);
        snakeNodes = new List<Snake>(numberOfSnakeNodes);
        Snake previousNode = null;
        for(int i = 0; i < numberOfSnakeNodes; i++)
        {
            Snake node = Instantiate(snake, transform);
            snakeNodes.Add(node);
            //for some reason, removing the rigidBody on the Snake makes the call order of Instantiate fail? maybe something is silently crashing somewhere?
            node.RegisterOnDestroyListener(SnakeNodeDestroyed);

            node.Head = previousNode;
            previousNode = node;
        }
    }

    protected override void MyFixedUpdateWithDeltaTime(float myDeltaTime, float timeScale)
    {
        if(snakeNodes.Count == 0)
        {
            //Destroy the object as a hittable so that the kill message is sent
            Hittable hittable = GetComponent<Hittable>();
            hittable.Hit(1);
            return;
        }
    }

    private void SnakeNodeDestroyed(GameObject gameObject)
    {
        for(int i = snakeNodes.Count - 1; i >= 0; i--)
        {
            if(snakeNodes[i].gameObject == gameObject)
            {
                snakeNodes.RemoveAt(i);
                return;
            }
        }
    }
}