using UnityEngine;

public class EditorDebugSwitcher : EditorDebug
{
    [ContextMenu("Cycle Gizmo Mode")]
    public void CycleGizmoToDraw()
    {
        gizmoDebug++;
        if (gizmoDebug == EditorDebugInfo.MAX)
            gizmoDebug = 0;
    }

    protected override void DrawDebugGizmos()
    {
        drawString("DEBUG", transform.position);
    }

    static void drawString(string text, Vector3 worldPos)
    {
        UnityEditor.Handles.BeginGUI();
        var view = UnityEditor.SceneView.currentDrawingSceneView;
        Vector3 screenPos = view.camera.WorldToScreenPoint(worldPos);
        Vector2 size = GUI.skin.label.CalcSize(new GUIContent(text));
        GUI.Label(new Rect(screenPos.x - (size.x / 2), -screenPos.y + view.position.height + 4, size.x, size.y), text);
        UnityEditor.Handles.EndGUI();
    }

    protected override void DrawInfoGizmos()
    {
        drawString("INFO", transform.position);
    }
}