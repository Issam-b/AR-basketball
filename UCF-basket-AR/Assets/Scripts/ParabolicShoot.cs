using System.Collections;
using UnityEngine;

public class ParabolicShoot : MonoBehaviour
{


    [SerializeField]
    GameObject _target;                                     /* The target object to hit */

    [SerializeField]
    public float _time = 3f;                                /* The time of the travel */

    [SerializeField]
    public bool _repeat = true;                             /* If true, the program will keep on shot */

    [SerializeField]
    public Vector3 _startVelocity = new Vector3(0, 0, 0);   /* The start velocity to be applied to the shot object before the shoot */

    [SerializeField]
    public Vector3 _speed = Vector3.zero;                   /* Shows the speed during the shoot */

    [SerializeField]
    public float _elapsed = 0f;                             /* Time elapsed from the starting of the shoot */

    public delegate void OnHitActionHandler();              /* Delegate function to be called when target hit */
    public static event OnHitActionHandler OnHit;

    Rigidbody _rigidBody;                               /* Rigidbody of the shot object */

    Vector3 _startPosition;                         /* Start position of the shot object */

    Transform _parent;                              /* Parent object of the shot object (in case you want to to shoot the object from another moving one) */

    float _timeStartThrust = 0f;                    /* Time the thrust force has been applied */

    bool _shootCompleted = false;               /* Register if the shoot has taken place */



    void Start()
    {

        _rigidBody = gameObject.GetComponent<Rigidbody>();      // Rigidbody caching
        _startPosition = transform.localPosition;               // Save shot start position
        _parent = transform.parent;

        if (_repeat)
        {
            InvokeRepeating("Reset", 1f, _time + 2.5f);
        }
        else
        {
            Invoke("Reset", 1.5f);
        }


    }


    void Update()
    {

        if (_shootCompleted) _elapsed = Time.time - _timeStartThrust;
        _speed = _rigidBody.velocity;

    }

    public void Reset()
    {

        transform.parent = _parent;
        _shootCompleted = false;
        _rigidBody.isKinematic = true;                  // Avoid bounces on the ground before the shooting begin
        transform.localPosition = _startPosition;

        ApplyStartVelocity();
        Invoke("ApplyThrust", 1.5f);

    }

    public void ApplyStartVelocity()
    {

        _rigidBody.isKinematic = false;

        _rigidBody.AddForce(Vector3.right * _startVelocity.x, ForceMode.VelocityChange);
        _rigidBody.AddForce(Vector3.up * _startVelocity.y, ForceMode.VelocityChange);
        _rigidBody.AddForce(Vector3.forward * _startVelocity.z, ForceMode.VelocityChange);

    }

    public void ApplyThrust()
    {

        float X;
        float Y;
        float Z;
        float X0;
        float Y0;
        float Z0;
        float V0x;
        float V0y;
        float V0z;
        float t;


        _rigidBody.isKinematic = false;

        Vector3 forceDirection = _target.transform.position - transform.position;

        X = forceDirection.x;       // Distance to travel along X : Space traveled @ time t
        Y = forceDirection.y;       // Distance to travel along Y : Space traveled @ time t
        Z = forceDirection.z;       // Distance to travel along Z : Space traveled @ time t

        // As we calculate in this very moment the distance between the shot object and the target, the intial space coordinates X0, Y0, Z0 will be always 0.
        X0 = 0;
        Y0 = 0;
        Z0 = 0;

        transform.parent = null;        // Detach the shot object from parent in order to get its own velocity

        t = _time;

        // Calculation of the required velocity along each axis to hit the target from the current starting position as if the shot object were stopped 
        V0x = (X - X0) / t;
        V0z = (Z - Z0) / t;
        V0y = (Y - Y0 + (0.5f * Mathf.Abs(Physics.gravity.magnitude) * Mathf.Pow(t, 2))) / t;

        /* Subtraction of the current velocity of the shot object */
        V0x -= _rigidBody.velocity.x;
        V0y -= _rigidBody.velocity.y;
        V0z -= _rigidBody.velocity.z;

        _rigidBody.AddForce(Vector3.right * V0x, ForceMode.VelocityChange); // VelocityChange Add an instant velocity change to the rigidbody, applying an impulsive force, ignoring its mass.
        _rigidBody.AddForce(Vector3.up * V0y, ForceMode.VelocityChange);
        _rigidBody.AddForce(Vector3.forward * V0z, ForceMode.VelocityChange);

        _timeStartThrust = Time.time;
        _shootCompleted = true;

    }


    void OnCollisionEnter(Collision c)
    {

        // Logic for Target Hit

        if (!_shootCompleted)
            return;

        if (c.gameObject.name == _target.name)
        {

            if (OnHit != null)
                OnHit();

        }


    }




}

