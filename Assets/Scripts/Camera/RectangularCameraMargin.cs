using System;
using UnityEngine;

[AddComponentMenu("Camera Margin/Camera Margin")]
public class RectangularCameraMargin : BaseCameraMargin
{
    public float width = 0;
    public float height = 0;
    public Vector2 offset = new Vector2(4, 0);


    public override void DoDrawGizmo()
    {
        if (drawSolid)
            Gizmos.DrawCube(transform.position, new Vector3(width, height, 1));
        else
            Gizmos.DrawWireCube(transform.position, new Vector3(width, height, 0));
    }

    public override Vector3 limitCamera(Vector3 position, float cameraSize)
    {
        var x = transform.position.x;
        var y = transform.position.y;
        var result = new Vector3(
            Mathf.Clamp(position.x, x - (width / 2f) + cameraSize + offset.x, x + (width / 2f) - cameraSize - offset.x),
            Mathf.Clamp(position.y, y - (height / 2f) + cameraSize + offset.y,
                y + (height / 2f) - cameraSize - offset.y),
            position.z
        );
        // Debug.Log("Camera position: " + result + "\n Camera size: " + cameraSize + "\n margin width: " + width +
        // "\n margin height: " + height + "\n objetive position: " + position + "\n margin transform: " + transform.position);
        return result;
    }
}