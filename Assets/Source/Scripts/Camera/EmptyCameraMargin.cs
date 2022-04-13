using UnityEngine;

public class EmptyCameraMargin : BaseCameraMargin
{
    public override void DoDrawGizmo()
    {
        // Gizmos.DrawWireCube(transform.position, Vector3.one);
    }

    public override Vector3 limitCamera(Vector3 position, float cameraSize)
    {
        return position;
    }
}