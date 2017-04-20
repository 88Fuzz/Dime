using System.Collections.Generic;
using UnityEngine;

//TODO figure out if this needs to be a monoBehaviour
public class Grid
{
    public static readonly int CELL_SCALE = 10;
    //Unity doesn't support Sets, so we have to use a map :(
    private IDictionary<IntVector3, bool> grid;

    public Grid()
    {
        grid = new Dictionary<IntVector3, bool>();
    }

    public bool IsGridPositionTaken(Vector3 position)
    {
        IntVector3 intVector3 = new IntVector3(position);
        return grid.ContainsKey(intVector3);
    }

    public bool AddGridPosition(Vector3 position)
    {
        IntVector3 intVector3 = new IntVector3(position);
        if (grid.ContainsKey(intVector3))
            return false;

        grid.Add(intVector3, true);
        return true;
    }

    public bool RemoveGridPosition(Vector3 position)
    {
        IntVector3 intVector3 = new IntVector3(position);
        return grid.Remove(intVector3);
    }
}