using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketManager : MonoBehaviour
{

    // Given the current velocity and orientation of the rocket, produce a trajectory and plot out a line renderer

    ProjectilePhysics rocketProjectilePhysics;
    LineRenderer trajectoryLine;

    [SerializeField] private Transform Camera;

    // Start is called before the first frame update
    void Start()
    {
        rocketProjectilePhysics = GetComponent<ProjectilePhysics>();

        trajectoryLine = gameObject.AddComponent<LineRenderer>();
        trajectoryLine.positionCount = 1000;
        trajectoryLine.startWidth = .1f;
        trajectoryLine.endWidth = .1f;
        trajectoryLine.material.color = new Color(0, 1, 0, .5f);

    }

    // Update is called once per frame
    void Update()
    {
        PlotTrajectory();


    }

    void PlotTrajectory()
    {
        int trajectoryNodes = 1000;

        Vector3 startPos = transform.position;
        Vector3[] points = new Vector3[trajectoryNodes];
        points[0] = startPos;

        Vector3 v = rocketProjectilePhysics.Velocity;

        Vector3 a = rocketProjectilePhysics.Weight / rocketProjectilePhysics.Mass;

        for (int i = 1; i < trajectoryNodes; i++)
        {
            v += a;
            startPos += v;
            points[i] = startPos;
        }

        trajectoryLine.SetPositions(points);

        Debug.Log(trajectoryLine.positionCount + ">" + gameObject.transform + "|" + rocketProjectilePhysics.Velocity);
    }
}
