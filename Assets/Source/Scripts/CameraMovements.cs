using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraMovements : MonoBehaviour
{
    private Transform _target;
    public BaseCameraMargin margin;
    public Vector2 offset;

    private Transform _transform;

    private void Start()
    {
        this._transform = transform;
        Screen.SetResolution(1920, 1080, true);
    }

    private void Awake()
    {
        _target = GameObject.FindGameObjectWithTag("Player").transform;
    }


    private void Update()
    {
        if (!Screen.fullScreen)
        {
            Screen.SetResolution(1920, 1080, true);
        }

        var pos = _target.position;
        _transform.position = this.margin.limitCamera(new Vector3(pos.x, pos.y, _transform.position.z),
            Camera.main!.orthographicSize) + (Vector3)offset;
    }

    public void setBound(GameObject map)
    {
        // var config = map.GetComponent<SuperTiled2Unity.SuperMap>();
        // if map has a component of type CameraMargins then set the camera limits
        var mapMargin = map.GetComponent<BaseCameraMargin>();
        if (mapMargin == null)
            mapMargin = map.AddComponent<EmptyCameraMargin>();
        this.margin = mapMargin;
    }
}