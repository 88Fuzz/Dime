using UnityEngine;

/*
 * This code is based off of https://github.com/DMeville/Unity3d-Dungeon-Generator
 * TODO add more stuff here
 */
public class GridGenerator : MonoBehaviour
{
    public Vector3 generatorSize = new Vector3(1f, 1f, 1f);
    public GameObject[] gridCells;

    private static EditorDebugInfo gizmoDebug = EditorDebugInfo.DEBUG;

    public GameObject gameObjectContainer;
    public Bounds bounds;

    public void OnDrawGizmos()
    {
        if (gizmoDebug == EditorDebugInfo.INFO)
        {
            if (bounds == null) return;
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(bounds.center, bounds.size);
        }
        else if(gizmoDebug == EditorDebugInfo.DEBUG)
        {
            if (gameObjectContainer == null)
            {
                return;
            }
            for (int i = 0; i < gameObjectContainer.transform.childCount; i++)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(gameObjectContainer.transform.GetChild(i).transform.position, Vector3.one * Grid.CELL_SCALE);
            }
        }
    }


    [ContextMenu("Generate Grid")]
    public void GenerateGrid()
    {
        if (gridCells != null && gridCells.Length != 0)
        {
            for (int i = 0; i < gridCells.Length; i++)
            {
                DestroyImmediate(gridCells[i]);
            }
        }

        if (gameObjectContainer == null)
        {
            gameObjectContainer = new GameObject("GridCells");
            gameObjectContainer.transform.parent = transform;
        }

        gridCells = new GameObject[(int)(generatorSize.x * generatorSize.y * generatorSize.z)];
        int indexCount = 0;
        for (int i = 0; i < generatorSize.x; i++)
        {
            for (int j = 0; j < generatorSize.y; j++)
            {
                for (int k = 0; k < generatorSize.z; k++)
                {
                    gridCells[indexCount++] = CreateCell(i * Grid.CELL_SCALE,
                                j * Grid.CELL_SCALE,
                                k * Grid.CELL_SCALE);
                }
            }
        }
    }

    /*
     * Converts the (x,y,z) coordinate to a 1d number assuming x is the most significant bit, and z being the least significant.
     * TODO this should be moved to a utils package instead of sitting in here not being used.
     */
    private int Get1dGridCoordinate(int x, int y, int z)
    {
        return (int)((x * generatorSize.y * generatorSize.z) + (y * generatorSize.z) + z);
    }

    private GameObject CreateCell(int i, int j, int k)
    {
        int halfGridScale = Grid.CELL_SCALE / 2;
        GameObject cell = new GameObject(string.Format("Cell - ({0}, {1}, {2})", i, j, k));
        //One must round to int because of floating point percision :(
        cell.transform.position = new Vector3(i + halfGridScale, j + halfGridScale, k);
        cell.transform.parent = gameObjectContainer.transform;

        return cell;
    }

    [ContextMenu("Assign GridPositions")]
    public void AssignGridPositionsToList()
    {

        Vector3 min = new Vector3(gameObjectContainer.transform.GetChild(0).transform.position.x,
                                        gameObjectContainer.transform.GetChild(0).transform.position.y,
                                        gameObjectContainer.transform.GetChild(0).transform.position.z);

        Vector3 max = new Vector3(gameObjectContainer.transform.GetChild(0).transform.position.x,
                                        gameObjectContainer.transform.GetChild(0).transform.position.y,
                                        gameObjectContainer.transform.GetChild(0).transform.position.z);

        for (int i = 0; i < gameObjectContainer.transform.childCount; i++)
        {
            //gridCells.Add(gameObjectContainer.transform.GetChild(i).gameObject);
            Vector3 pos = gameObjectContainer.transform.GetChild(i).transform.position;

            if (pos.x < min.x) min.x = pos.x;
            if (pos.y < min.y) min.y = pos.y;
            if (pos.z < min.z) min.z = pos.z;

            if (pos.x > max.x) max.x = pos.x;
            if (pos.y > max.y) max.y = pos.y;
            if (pos.z > max.z) max.z = pos.z;
        }

        Vector3 size = new Vector3(0.5f * Grid.CELL_SCALE, 0.5f * Grid.CELL_SCALE, 0.5f * Grid.CELL_SCALE);
        bounds = new Bounds((min + max) / 2f, ((max + size) - (min - size)));
    }

    [ContextMenu("Cycle Gizmo Mode")]
    public void CycleGizmoToDraw()
    {
        gizmoDebug++;
        if (gizmoDebug == EditorDebugInfo.MAX)
            gizmoDebug = 0;
    }

    //TODO, I don't think this method is needed??
    public void RecalculateBounds()
    {

        Vector3 min = new Vector3(gridCells[0].transform.position.x,
                                  gridCells[0].transform.position.y,
                                  gridCells[0].transform.position.z);

        Vector3 max = new Vector3(gridCells[0].transform.position.x,
                                  gridCells[0].transform.position.y,
                                  gridCells[0].transform.position.z);

        for (int i = 0; i < gridCells.Length; i++)
        {
            Vector3 pos = gridCells[i].transform.position;

            if (pos.x < min.x) min.x = pos.x;
            if (pos.y < min.y) min.y = pos.y;
            if (pos.z < min.z) min.z = pos.z;

            if (pos.x > max.x) max.x = pos.x;
            if (pos.y > max.y) max.y = pos.y;
            if (pos.z > max.z) max.z = pos.z;
        }

        //Debug.Log("Voxel::RecalculateBounds() | " + min + " : " + max);

        Vector3 size = new Vector3(0.5f * Grid.CELL_SCALE, 0.5f * Grid.CELL_SCALE, 0.5f * Grid.CELL_SCALE);
        bounds = new Bounds((min + max) / 2f, ((max + size) - (min - size)));
    }
}
