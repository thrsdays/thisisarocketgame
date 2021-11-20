using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    [SerializeField] Transform worldCamera;
    [SerializeField] GameObject Rocket;

    bool isRocketMoving;

    Vector3 rocketPosition;

    private float zoomValue;

    private bool isDragging;

    [SerializeField] private float cameraDist;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(cameraDist);
        zoomValue = -10;
        worldCamera.LookAt(Rocket.transform.position, Vector3.up);
    }

    // Update is called once per frame
    void Update()
    {
        cameraDist = (worldCamera.position - Rocket.transform.position).magnitude;
        Debug.Log("worldCam distance:" + cameraDist);
        if (Rocket.transform.hasChanged)
        {
            worldCamera.position = Rocket.transform.position + new Vector3(0, 0, -50);
            worldCamera.LookAt(Rocket.transform.position, Vector3.up);
        }

        if (Input.mouseScrollDelta.y != 0)
        {
            zoomValue += Input.mouseScrollDelta.y * .1f;
            Debug.Log(worldCamera.position.normalized);
            //if(CameraDistance < 0)
            //    worldCamera.position = 
        }

        #region Camera Control
        // Right Mouse Button
        if (Input.GetMouseButton(1))
        {
            //worldCamera.RotateAround(rocketPosition, Vector3.up, 1f);

            //Debug.Log(Vector3.Distance(rocketPosition, worldCamera.position));
        }
        
        if (Input.GetMouseButtonUp(1))
        {
            //Debug.Log("Right Up");
        }

        // Left Mouse Button
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("Left Down");
        }
        else if (Input.GetMouseButtonUp(0))
        {
            //Debug.Log("Left Up");
        }

        #endregion
    }


}
