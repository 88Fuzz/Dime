using UnityEngine;

/*
 * Creates a Floor generator on awake so that the Singleton instance of the FloorGenerator can be set for the rest of the game.
 * Floor generator needs to be set
 */
public class FloorGeneratorCreator : MonoBehaviour
{
    public FloorGenerator floorGenerator;
    public void Awake()
    {
        FloorGenerator newFloorGenerator = Instantiate(floorGenerator);
        newFloorGenerator.name = "FloorGenerator";
        //FloorGenerator floorGenerator = gameObject.AddComponent<FloorGenerator>();
        Singleton<FloorGenerator>.SetInstance(newFloorGenerator);

        Destroy(gameObject);
    }
}