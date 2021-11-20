using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLookAt : MonoBehaviour
{
    Transform mainSubject;

    // Start is called before the first frame update
    void Start()
    {
        //mainSubject = GameObject.Find("Rocket").transform;
        //transform.LookAt(mainSubject);
    }

    // Update is called once per frame
    void Update()
    {
        // Stick object to look orthographically
        //transform.position = mainSubject.position + new Vector3(0, 0, -5);
        //transform.LookAt(mainSubject);
    }
}
