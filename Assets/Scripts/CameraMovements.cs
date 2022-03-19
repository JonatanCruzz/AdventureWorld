using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovements : MonoBehaviour
{
    Transform target;
    float tLX, tLY, bRX, bRY;

   
    private void Start()
    {
        Screen.SetResolution(800, 800, true);
    }
    void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

  
    void Update()
    {
        if (!Screen.fullScreen || Camera.main.aspect != 1)
        {
            Screen.SetResolution(800, 800, true);
        }

        if (Input.GetKey("escape")) Application.Quit();

        transform.position = new Vector3(
            Mathf.Clamp(target.position.x, tLX, bRX),
            Mathf.Clamp(target.position.y, bRY, tLY),
            transform.position.z
            );
    }

    public void setBound(GameObject map)
    {
        SuperTiled2Unity.SuperMap config = map.GetComponent<SuperTiled2Unity.SuperMap>();
        float cameraSize = Camera.main.orthographicSize;

        tLX = map.transform.position.x + cameraSize;
        tLY = map.transform.position.y - cameraSize;
        bRX = map.transform.position.x + config.m_Width - cameraSize;
        bRY = map.transform.position.y - config.m_Height + cameraSize;

    }
}
