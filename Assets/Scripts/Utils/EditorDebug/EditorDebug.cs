using UnityEngine;

/*
 * Collection on information for looking at debug information in the Unity Editor
 */
public abstract class EditorDebug: MonoBehaviour
{
    protected static EditorDebugInfo gizmoDebug = EditorDebugInfo.DEBUG;
    private static readonly Color INFO_COLOR = Color.green;
    private static readonly Color DEBUG_COLOR = Color.red;

    /*
     * Enum for adding debug gizmo information.
     * SILENT - show no editor information.
     * INFO - show basic editor information.
     * DEBUG - show as much editor information as possible.
     */
    protected enum EditorDebugInfo
    {
        SILENT,
        INFO,
        DEBUG,
        MAX
    }

    public void OnDrawGizmos()
    {
        switch (gizmoDebug)
        {
            case EditorDebugInfo.INFO:
                DrawInfo();
                break;
            case EditorDebugInfo.DEBUG:
                DrawDebug();
                break;
            default:
                //Do nothing
                break;
        }
    }

    private void DrawInfo()
    {
        Gizmos.color = INFO_COLOR;
        DrawInfoGizmos();
    }

    private void DrawDebug()
    {
        Gizmos.color = DEBUG_COLOR;
        DrawDebugGizmos();
    }

    protected abstract void DrawInfoGizmos();
    protected abstract void DrawDebugGizmos();
}