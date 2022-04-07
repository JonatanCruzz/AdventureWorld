using UnityEngine;

[AddComponentMenu("Camera Margin/Circular Camera Margin")]
public class CircularCameraMargin : BaseCameraMargin
{
    public float radius = 0;

    public override void DoDrawGizmo()
    {
        if (drawSolid)
            Gizmos.DrawSphere(transform.position, radius);
        else
            Gizmos.DrawWireSphere(transform.position, radius);
    }

    public override Vector3 limitCamera(Vector3 position, float cameraSize)
    {
        var center = transform.position;
        var result =
            new Vector3(
                Mathf.Clamp(position.x, center.x - radius + cameraSize, center.x + radius - cameraSize),
                Mathf.Clamp(position.y, center.y - radius + cameraSize, center.y + radius - cameraSize)
            );
        // result.x = Mathf.Clamp(position.x, -(center.x + radius), center.x + radius);
        // result.y = Mathf.Clamp(position.y, -(center.y + radius), center.y + radius);
        // return result + center;
        return result;
    }
}