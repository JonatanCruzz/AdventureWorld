using UnityEngine;

public abstract class BaseCameraMargin : MonoBehaviour
{
    public string MapName = "";
    public Color gizmoColor = Color.green;
    public bool drawSolid = true;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmoColor;
        DoDrawGizmo();
    }

    public abstract void DoDrawGizmo();

    public abstract Vector3 limitCamera(Vector3 position, float cameraSize);
}