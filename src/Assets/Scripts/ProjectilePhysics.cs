using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePhysics : MonoBehaviour
{
    // Time Variables
    private static readonly float s_millisecond = .001f;
    private float timeFrameMilliSecond = 0.0f; // Time Elapsed since trajectory start

    // Global Constants
    private const float _gravityEarth = 10f; //9.81 for Earth

    // Gameobject References
    [SerializeField] private GameObject engineGameObject;

    #region Debug Variables
    [SerializeField] bool isVerbose;
    [SerializeField] private Material forceIndicatorMaterial;
    [SerializeField] GameObject debugForceIndicatorGameObject;
    //[SerializeField] LineRenderer trajectoryIndicator;
    private GameObject _weightIndicator;
    private GameObject _thrustIndicator;
    private GameObject _resultantForceIndicator;
    GameObject trailSpheres;
    #endregion

    #region Kinematic Variables
    [Tooltip("Kilogram (kg)")] [SerializeField] public float Mass;
    public Vector3 Velocity;
    public Vector3 Weight;

    float v; // m/s
    float angle;
    float init_h;

    private bool _firstLaunch;
    private bool _isThrustOn;

    private float ThrottlePercentage = 0f;

    [SerializeField] private Transform AirResistanceCam;
    #endregion

    #region Keymap Settings
    private static readonly string s_pitchKeyUp = "w";
    private static readonly string s_pitchKeyDown = "s";
    private static readonly string s_yawKeyLeft = "a";
    private static readonly string s_yawKeyRight = "d";
    private static readonly string s_rollKeyClockwise = "e";
    private static readonly string s_rollKeyAntiClockwise = "q";
    #endregion

    #region MonoBehaviour

    // Time updates at every milisecond
    void Start()
    {

        trailSpheres = new GameObject();
        trailSpheres.name = "Trail";

        v = 10f;

        _firstLaunch = true;
        _isThrustOn = false;

        // Define axis of rotation along the y axis aka 0,0,1,0

        transform.rotation = new Quaternion(0f, 0f, 0f, 0f);

        init_h = gameObject.transform.position.y;

        angle = Mathf.PI/6;

        if (engineGameObject == null) return;

        #region Debug Stuff
        // Spawn Debug GameObjects
        _weightIndicator = new GameObject();
        _thrustIndicator = new GameObject();
        _resultantForceIndicator = new GameObject();

        _weightIndicator.transform.SetParent(debugForceIndicatorGameObject.transform);
        _thrustIndicator.transform.SetParent(debugForceIndicatorGameObject.transform);
        _resultantForceIndicator.transform.SetParent(debugForceIndicatorGameObject.transform);

        _weightIndicator.name = "Weight Indicator";
        _thrustIndicator.name = "Thrust Indicator";
        _resultantForceIndicator.name = "Resultant Indicator";

        Debug.Log("V_0:" + v + "m/s, Angle:" + angle + ", y_0:" + init_h + "m" + ">>" + gameObject.transform.position);
        #endregion

        Velocity = new Vector3(0f, 0f, 0f);
        Weight = new Vector3(0f, -Mass * _gravityEarth * s_millisecond * s_millisecond, 0f); // _weight force points downwards at all times

    }

    // Update is called once per frame
    void Update()
    {

        // Milisecond Time Activity Loop
        if (Time.time > timeFrameMilliSecond)
        {
            timeFrameMilliSecond += s_millisecond;

            // Only start updating position when Spacebar has been pressed and thrust is on
            if (!_firstLaunch) // If launched
            {
                UpdatePosition();
                //Debug.Log(transform.rotation);
                DrawDebugForces();
            }
        }

        #region Roll, Pitch, Yaw
        Vector3 rotationDelta = new Vector3();

        // Pitch Up --------------------
        if (Input.GetKey(s_pitchKeyUp))
        {
            rotationDelta.x += .1f;
        }
        // Pitch Down --------------------
        else if (Input.GetKey(s_pitchKeyDown))
        {
            rotationDelta.x -= .1f;
        }
        // Roll Clockwise --------------------
        else if (Input.GetKey(s_rollKeyAntiClockwise))
        {
            rotationDelta.y += .1f;
        }
        // Roll Anti-Clockwise --------------------
        else if (Input.GetKey(s_rollKeyClockwise))
        {
            rotationDelta.y -= .1f;
        }
        // Yaw Left --------------------
        else if (Input.GetKey(s_yawKeyLeft))
        {
            rotationDelta.z += .1f;
        }
        // Yaw Right --------------------
        else if (Input.GetKey(s_yawKeyRight))
        {
            rotationDelta.z -= .1f;
        }

        Quaternion rotationQuaternion = Quaternion.Euler(rotationDelta.x, rotationDelta.y, rotationDelta.z);

        if(rotationDelta.magnitude != 0)
        {
            transform.rotation *= rotationQuaternion;
            //Velocity = rotationQuaternion * Velocity;
        }

        #endregion

        #region Throttle Control
        // Throttle Up --------------------
        if (Input.GetKey("left shift"))
        {
            //ThrottlePercentage = 1;
            if ((ThrottlePercentage + .01f) >= 1)
            {
                ThrottlePercentage = 1;
            }
            else
            {
                ThrottlePercentage += .01f;
            }
        }
        // Throttle Down --------------------
        else if (Input.GetKey("left ctrl"))
        {
            //ThrottlePercentage = 0;
            if ((ThrottlePercentage - .01f) <= 0)
            {
                ThrottlePercentage = 0;
            }
            else
            {
                ThrottlePercentage -= .01f;
            }
        }
        #endregion

        #region Launch Keys
        if (Input.GetKeyDown("space"))
        {
            _isThrustOn = !_isThrustOn; // Toggle thrust

            if (_firstLaunch)
            {
                ThrottlePercentage = 1f;
                // Turn on gravity upon launch
                _firstLaunch = false; // The only way to turn on thrust
            }

            Debug.Log("Toggle Engines");
        }
        #endregion
    }

    #endregion

    #region Runtime Loop Update Functions
    void UpdatePosition()
    {

        Vector3 rocketPos = transform.position;

        //Vector3 drag = CalculateDrag();

        // Given weight and thrust, calculate current velocity
        Vector3 _thrust_n = new Vector3(5f * s_millisecond * s_millisecond, 15f * s_millisecond * s_millisecond, 0f); // _weight force points downwards at all times
        //Vector3 _acceleration_ms2 = ((_thrust_n * ThrottlePercentage) + Weight - drag) / Mass;
        Vector3 _acceleration_ms2 = ((_thrust_n * ThrottlePercentage) + Weight) / Mass;

        Velocity += _acceleration_ms2;

        float twr = _thrust_n.magnitude / Weight.magnitude;

        //Debug.Log("TWR" + twr + "|V" + Velocity.y / s_millisecond + "|A" + _acceleration_ms2.y / s_millisecond + "|T" + ThrottlePercentage);

        // Update position
        if ((rocketPos + Velocity).y >= 1.5f)
            gameObject.transform.position = Vector3.Lerp(rocketPos, rocketPos + Velocity, 1);
        else
            transform.position = Vector3.Lerp(rocketPos, new Vector3(0, 0, 0), 1);

    }

    Vector3 CalculateDrag()
    {
        /* Given trajectory, position camera at trajectory location
        * Calculate surface area and return a resistance value.
        * D = (Cd * rho * V * V * A) / 2 
        */

        // Get Complete Mesh of rocket

        // Project into 2d Plane

        // Calculate area

        Vector3 _drag = new Vector3();

        float _dragCoefficient = .5f; //https://en.wikipedia.org/wiki/Drag_coefficient

        float _gasDensity = .5f; //https://www.omnicalculator.com/physics/air-density
        float _exposedSurfaceArea = 1f;

        // TO FIX: If velocity vector is negative -> output is positive.
        _drag = .5f * _dragCoefficient * _gasDensity * _exposedSurfaceArea * new Vector3(Velocity.x * Velocity.x, Velocity.y * Velocity.y, Velocity.z * Velocity.z);
        Debug.Log("Drag:" + _drag.y);
        return _drag;

    }

    #endregion

    void DrawDebugForces()
    {
        // Gravitational Force
        LineRenderer weight;
        
        if((weight = _weightIndicator.GetComponent<LineRenderer>()) == null)
            weight = _weightIndicator.AddComponent<LineRenderer>();
        
        weight.material = forceIndicatorMaterial;
        weight.material.color = new Color(0, 0, 0, .5f);
        weight.startWidth = .1f;
        weight.endWidth = .01f;
        
        // Draw a line from center of object downwards by 1 unit
        weight.SetPosition(0, transform.position);
        weight.SetPosition(1, transform.position + Vector3.Normalize(Weight));

        // Thrust Force
        LineRenderer resultant;

        if ((resultant = _resultantForceIndicator.GetComponent<LineRenderer>()) == null)
            resultant = _resultantForceIndicator.AddComponent<LineRenderer>();

        resultant.material = forceIndicatorMaterial;
        resultant.material.color = new Color(0, 0, 1, .5f);
        resultant.startWidth = .1f;
        resultant.endWidth = .01f;

        // Draw a line from center of object downwards by 1 unit
        resultant.SetPosition(0, transform.position);
        resultant.SetPosition(1, transform.position + Vector3.Normalize(Velocity));
    }

    /// <summary>
    /// This function is called when collision is detected. Can be used for colliding to ground or another body.
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        _isThrustOn = false;
    }

}
