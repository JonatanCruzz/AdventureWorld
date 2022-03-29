using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovements : MonoBehaviour
{
    Transform target;
    // Bound object to set limits of the camera
    public GameObject bound;

    public float LX, LY, RX, RY;
    public CameraMargin margin;
    private void Start()
    {
        Screen.SetResolution(1920, 1080, true);

    }
    void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }


    void Update()
    {
        if (!Screen.fullScreen)
        {
            Screen.SetResolution(1920, 1080, true);
        }



        if (Input.GetKey("escape")) Application.Quit();

        transform.position = new Vector3(
            Mathf.Clamp(target.position.x, LX, RX),
            Mathf.Clamp(target.position.y, RY, LY),
            transform.position.z
            );

    }

    public void setBound(GameObject map)
    {
        SuperTiled2Unity.SuperMap config = map.GetComponent<SuperTiled2Unity.SuperMap>();
        // if map has a component of type CameraMargins then set the camera limits
        var mapMargin = map.GetComponent<CameraMargin>();
        if (mapMargin == null)
            mapMargin = map.AddComponent<CameraMargin>();

        this.margin = mapMargin;

        float cameraSize = Camera.main.orthographicSize;


        LX = map.transform.position.x + cameraSize - mapMargin.left;
        LY = map.transform.position.y - cameraSize + mapMargin.top;
        RX = map.transform.position.x + config.m_Width - cameraSize + mapMargin.right;
        RY = map.transform.position.y - config.m_Height + cameraSize - mapMargin.bottom;


    }
}
