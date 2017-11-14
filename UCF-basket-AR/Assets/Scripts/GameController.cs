using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class GameController : MonoBehaviour {

    public const int gameTime = 60; 
    private Vector3 InitialTouchPosition, FinalTouchPosition;
    private float XaxisForce, YaxisForce;
    private int ballCount = 0;
    private float startTime = 0f, temp;
    string minutes, seconds;
    private bool redTime = false, isRed = false;


    public Rigidbody ball;
    public Transform imageTarget;
    public float speed = 0.5f;
    public bool canSwipe = true;
    public Text scoreText, timeText, ballCountText;
    public Text resultsText;
    public GameObject resultsPanel;

    private float distanceX, distanceY, distanceZ;
    public AudioSource DoneSound;

    private Player player;

    public GameObject net;

    [Range(10f, 80f)]
    public float angle = 45f;


    

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

    //Rigidbody ball;                               /* Rigidbody of the shot object */

    Vector3 _startPosition;                         /* Start position of the shot object */

    Transform _parent;                              /* Parent object of the shot object (in case you want to to shoot the object from another moving one) */

    float _timeStartThrust = 0f;                    /* Time the thrust force has been applied */

    bool _shootCompleted = false;               /* Register if the shoot has taken place */
    public Vector3 initialTarget;

    public float angleDiff; 

    private void Start()
    {

        resultsPanel.SetActive(false);
        player = StartScreen.player;
        timeText.text = "Time: 5:00.0";

        //
        _startPosition = ball.transform.localPosition;               // Save shot start position
        //_parent = transform.parent;

        //if (_repeat)
        //{
        //    InvokeRepeating("Reset", 1f, _time + 2.5f);
        //}
        //else
       // {
          //  Invoke("Reset", 1.5f);
        //}
    }

    private void Update()
    {
        NormalMode();
        if (!player.GetGameDone())
        {
            UpdateTime();
        }

        //ball.transform.LookAt(net.transform);
        //angleDiff = - Vector3.SignedAngle( ball.transform.right, - net.transform.right, gameObject.transform.up);
        //Debug.Log(angleDiff);
        //
        //if (_shootCompleted) _elapsed = Time.time - _timeStartThrust;
        //_speed = ball.velocity;
    }

    public void Reset()
    {

        //transform.parent = _parent;
        //_shootCompleted = false;
        ball.useGravity = false;
        ball.isKinematic = true;                  // Avoid bounces on the ground before the shooting begin
        ball.transform.position = _startPosition;

        //ApplyStartVelocity();
        Invoke("ApplyThrust", 1.5f);

    }

    public void ApplyStartVelocity()
    {
        //ball.useGravity = true;
        ball.isKinematic = false;

        ball.AddForce(Vector3.right * _startVelocity.x, ForceMode.VelocityChange);
        ball.AddForce(Vector3.up * _startVelocity.y, ForceMode.VelocityChange);
        ball.AddForce(Vector3.forward * _startVelocity.z, ForceMode.VelocityChange);

    }

   


    void OnCollisionEnter(Collision c)
    {

        // Logic for Target Hit

        if (!_shootCompleted)
            return;

        if (c.gameObject.name == net.name)
        {

            if (OnHit != null)
                OnHit();

        }


    }

    public void OnTouchDown ()
    {
        if (canSwipe)
        {
            InitialTouchPosition = Input.mousePosition;
        }
    }

    public void OnTouchUp ()
    {
        if (canSwipe)
        {
            FinalTouchPosition = Input.mousePosition;
            if (ballCount == 0)
                startTime = Time.time;
            BallThrow();
            ballCount++;
            UpdateThrows();
        }
    }

    private void BallThrow()
    {
        //
        distanceX = Mathf.Abs(imageTarget.GetChild(3).GetChild(2).transform.position.x - ball.transform.position.x);
        distanceY = Mathf.Abs(imageTarget.GetChild(3).GetChild(2).transform.position.y - ball.transform.position.y);
        distanceZ = Mathf.Abs(imageTarget.GetChild(3).GetChild(2).transform.position.z - ball.transform.position.z);
        //
        XaxisForce = FinalTouchPosition.x - InitialTouchPosition.x;
        YaxisForce = FinalTouchPosition.y - InitialTouchPosition.y;
        ball.useGravity = true;
        ball.isKinematic = false;

        //GameModes();

        GameModes();

        // Lose mode
        //if (!player.GetWinOn())
        //{
        //    LoseMode();
        //}

        // Win mode
        //if (player.GetWinOn())
        //{
        //    WinMode();
        //}

        //ball.AddForce(new Vector3(0.1f, 0.1f, 0.1f));
        //FireCannonAtPoint(net.transform.position);

        //Normal Game
        //NormalMode();

        canSwipe = false;
        ball.transform.SetParent(imageTarget, true);
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


        ball.isKinematic = false;
        ball.useGravity = true;

        // get angle
        angleDiff = - Vector3.SignedAngle(ball.transform.right, -net.transform.right, gameObject.transform.up);
        Debug.Log(angleDiff);

        //net.transform.position = new Vector3(initialTarget.x, net.transform.position.y, net.transform.position.z);

        net.transform.position = new Vector3(initialTarget.x + (angleDiff * 10.4f / 45), net.transform.position.y, net.transform.position.z);

        //if (angleDiff < 45f & angleDiff > 0)
           // net.transform.position = new Vector3(net.transform.position.x - (angleDiff * 10.4f / 45), net.transform.position.y, net.transform.position.z);
        //else if (angleDiff < 0 & angleDiff > -45f)
            //net.transform.position = new Vector3(net.transform.position.x - (angleDiff * 10.4f / 45), net.transform.position.y, net.transform.position.z);


        Vector3 forceDirection = net.transform.position - ball.transform.position;


        X = forceDirection.x;          // Distance to travel along X : Space traveled @ time t
        Y = forceDirection.y;       // Distance to travel along Y : Space traveled @ time t
        Z = forceDirection.z;       // Distance to travel along Z : Space traveled @ time t

        // As we calculate in this very moment the distance between the shot object and the target, the intial space coordinates X0, Y0, Z0 will be always 0.
        X0 = 0;
        Y0 = 0;
        Z0 = 0;

        //transform.parent = null;        // Detach the shot object from parent in order to get its own velocity

        t = _time;

        // Calculation of the required velocity along each axis to hit the target from the current starting position as if the shot object were stopped 
        V0x = (X - X0) / t;
        V0z = (Z - Z0) / t;
        V0y = (Y - Y0 + (0.5f * Mathf.Abs(Physics.gravity.magnitude) * Mathf.Pow(t, 2))) * 1.06f / t;

        /* Subtraction of the current velocity of the shot object */
        V0x -= ball.velocity.x;
        V0y -= ball.velocity.y;
        V0z -= ball.velocity.z;

        ball.AddForce(Vector3.right * V0x, ForceMode.VelocityChange); // VelocityChange Add an instant velocity change to the rigidbody, applying an impulsive force, ignoring its mass.
        ball.AddForce(Vector3.up * V0y, ForceMode.VelocityChange);
        ball.AddForce(Vector3.forward * V0z, ForceMode.VelocityChange);

        _timeStartThrust = Time.time;
        _shootCompleted = true;

    }


    // all modes
    public void GameModes()
    {
        //Normal Game
        int ran = (int)(Random.value * 10);
        if (ran == 5)
        {
            ball.AddForce(new Vector3(XaxisForce * 0.2f, YaxisForce * 0.1f, YaxisForce * 0.2f) * speed);
            ball.AddTorque(new Vector3(XaxisForce * 7.1f, YaxisForce * 7f, YaxisForce * 4.3f) * speed);
        }

        else
        {
            // Lose mode
            if (!player.GetWinOn())
            {
                LoseModeJ();
            }

            // Win mode
            if (player.GetWinOn())
            {
                ApplyThrust();
            }
        }
    }

    // all modes julien
    public void GameModesJ()
    {
        //Normal Game
        int ran = (int)(Random.value * 10);
        if (ran == 5)
        {
            ball.AddForce(new Vector3(XaxisForce * 0.2f, YaxisForce * 0.1f, YaxisForce * 0.2f) * speed);
            ball.AddTorque(new Vector3(XaxisForce * 7.1f, YaxisForce * 7f, YaxisForce * 4.3f) * speed);
        }

        else
        {
            // Lose mode
            if (!player.GetWinOn())
            {
                LoseModeJ();
            }

            // Win mode
            if (player.GetWinOn())
            {
                WinModeJ();
            }
        }
    }
    // lose mode julien
    public void LoseModeJ ()
    {
        int ran2 = (int)(Random.value * 3);
        float alpha = distanceZ / distanceY;
        if (alpha > 3f && alpha < 3.5f)
        {
            if (ran2 == 0)
            {
                ball.AddForce(new Vector3(distanceX * (2) * 0.2f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 4) * 0.8f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 4) * 0.65f)) * speed);
                ball.AddTorque(new Vector3(distanceX * (2) * 7.1f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 4) * 0.8f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 4) * 0.65f)) * speed);
            }
            if (ran2 == 1)
            {
                ball.AddForce(new Vector3(distanceX * (12) * 0.2f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 4) * 0.7f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 4) * 0.65f)) * speed);
                ball.AddTorque(new Vector3(distanceX * (12) * 7.1f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 4) * 0.7f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 4) * 0.65f)) * speed);
            }
            if (ran2 == 2)
            {
                ball.AddForce(new Vector3(distanceX * (2) * 0.2f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 4) * 0.7f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 4) * 0.85f)) * speed);
                ball.AddTorque(new Vector3(distanceX * (2) * 7.1f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 4) * 0.7f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 4) * 0.85f)) * speed);
            }
        }
        if (alpha > 2.5f && alpha < 3f)
        {
            if (ran2 == 0)
            {
                ball.AddForce(new Vector3(distanceX * (2) * 0.2f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 4) * 0.93f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 4) * 0.75f)) * speed);
                ball.AddTorque(new Vector3(distanceX * (2) * 7.1f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 4) * 0.93f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 4) * 0.75f)) * speed);
            }
            if (ran2 == 1)
            {
                ball.AddForce(new Vector3(distanceX * (12) * 0.2f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 4) * 0.73f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 4) * 0.75f)) * speed);
                ball.AddTorque(new Vector3(distanceX * (12) * 7.1f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 4) * 0.73f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 4) * 0.75f)) * speed);
            }
            if (ran2 == 2)
            {
                ball.AddForce(new Vector3(distanceX * (2) * 0.2f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 4) * 0.73f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 4) * 0.95f)) * speed);
                ball.AddTorque(new Vector3(distanceX * (2) * 7.1f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 4) * 0.73f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 4) * 0.95f)) * speed);
            }
        }
        if (alpha > 3.5f)
        {
            if (ran2 == 0)
            {
                ball.AddForce(new Vector3(distanceX * (2) * 0.2f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 2.5f) * 1.5f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 2.5f) * 1.2f)) * speed);
                ball.AddTorque(new Vector3(distanceX * (2) * 7.1f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 2.5f) * 1.5f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 2.5f) * 1.2f)) * speed);
            }
            if (ran2 == 1)
            {
                ball.AddForce(new Vector3(distanceX * (12) * 0.2f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 2.5f) * 1.5f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 2.5f) * 0.7f)) * speed);
                ball.AddTorque(new Vector3(distanceX * (12) * 7.1f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 2.5f) * 1.5f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 2.5f) * 0.7f)) * speed);
            }
            if (ran2 == 2)
            {
                ball.AddForce(new Vector3(distanceX * (2) * 0.2f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 2.5f) * 2.5f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 2.5f) * 0.7f)) * speed);
                ball.AddTorque(new Vector3(distanceX * (2) * 7.1f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 2.5f) * 2.5f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 2.5f) * 0.7f)) * speed);
            }
        }
        if (alpha < 2.5f && alpha > 2f)
        {
            if (ran2 == 0)
            {
                ball.AddForce(new Vector3(distanceX * (2) * 0.2f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 6) * 0.92f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 6) * 0.95f)) * speed);
                ball.AddTorque(new Vector3(distanceX * (2) * 7.1f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 6) * 0.92f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 6) * 0.95f)) * speed);
            }
            if (ran2 == 1)
            {
                ball.AddForce(new Vector3(distanceX * (12) * 0.2f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 6) * 0.62f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 6) * 0.95f)) * speed);
                ball.AddTorque(new Vector3(distanceX * (12) * 7.1f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 6) * 0.62f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 6) * 0.95f)) * speed);
            }
            if (ran2 == 2)
            {
                ball.AddForce(new Vector3(distanceX * (2) * 0.2f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 6) * 0.62f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 6) * 1.15f)) * speed);
                ball.AddTorque(new Vector3(distanceX * (2) * 7.1f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 6) * 0.62f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 6) * 1.15f)) * speed);
            }
        }
        if (alpha < 2f && alpha > 1.5f)
        {
            if (ran2 == 0)
            {
                ball.AddForce(new Vector3(distanceX * (2) * 0.2f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 6) * 0.87f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 6) * 1.0f)) * speed);
                ball.AddTorque(new Vector3(distanceX * (2) * 7.1f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 6) * 0.87f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 6) * 1.0f)) * speed);
            }
            if (ran2 == 1)
            {
                ball.AddForce(new Vector3(distanceX * (12) * 0.2f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 6) * 0.67f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 6) * 1.0f)) * speed);
                ball.AddTorque(new Vector3(distanceX * (12) * 7.1f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 6) * 0.67f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 6) * 1.0f)) * speed);
            }
            if (ran2 == 2)
            {
                ball.AddForce(new Vector3(distanceX * (2) * 0.2f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 6) * 0.67f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 6) * 1.3f)) * speed);
                ball.AddTorque(new Vector3(distanceX * (2) * 7.1f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 6) * 0.67f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 6) * 1.3f)) * speed);
            }
        }
        if (alpha < 1.5f)
        {
            if (ran2 == 0)
            {
                ball.AddForce(new Vector3(distanceX * (2) * 0.2f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 6) * 0.98f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 6) * 0.8f)) * speed);
                ball.AddTorque(new Vector3(distanceX * (2) * 7.1f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 6) * 0.98f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 6) * 0.8f)) * speed);
            }
            if (ran2 == 1)
            {
                ball.AddForce(new Vector3(distanceX * (12) * 0.2f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 6) * 0.78f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 6) * 0.8f)) * speed);
                ball.AddTorque(new Vector3(distanceX * (12) * 7.1f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 6) * 0.78f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 6) * 0.8f)) * speed);
            }
            if (ran2 == 2)
            {
                ball.AddForce(new Vector3(distanceX * (2) * 0.2f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 6) * 0.78f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 6) * 1.1f)) * speed);
                ball.AddTorque(new Vector3(distanceX * (2) * 7.1f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 6) * 0.78f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 6) * 1.1f)) * speed);
            }
        }
    }

    // win mode julien
    public void WinModeJ ()
    {
        //ball.transform.LookAt(net.transform);
        float alpha = distanceZ / distanceY;
        if (alpha > 3f && alpha < 3.5f)
        {
            ball.AddForce(new Vector3(distanceX * (2) * 0.2f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 4) * 0.7f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 4) * 0.65f)) * speed);
            ball.AddTorque(new Vector3(distanceX * (2) * 7.1f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 4) * 0.7f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 4) * 0.65f)) * speed);
        }
        else if (alpha > 2.5f && alpha < 3f)
        {
            ball.AddForce(new Vector3(distanceX * (2) * 0.2f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 4) * 0.68f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 4) * 0.75f)) * speed);
            ball.AddTorque(new Vector3(distanceX * (2) * 7.1f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 4) * 0.68f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 4) * 0.75f)) * speed);
        }
        else if (alpha > 3.5f)
        {
            ball.AddForce(new Vector3(distanceX * (2) * 0.2f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 2.5f) * 1.5f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 2.5f) * 0.7f)) * speed);
            ball.AddTorque(new Vector3(distanceX * (2) * 7.1f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 2.5f) * 1.5f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 2.5f) * 0.7f)) * speed);
        }
        else if (alpha < 2.5f && alpha > 2f)
        {
            ball.AddForce(new Vector3(distanceX * (2) * 0.2f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 6) * 0.52f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 6) * 0.82f)) * speed);
            ball.AddTorque(new Vector3(distanceX * (2) * 7.1f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 6) * 0.52f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 6) * 0.82f)) * speed);
        }
        else if (alpha < 2f && alpha > 1.5f)
        {
            ball.AddForce(new Vector3(distanceX * (2) * 0.2f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 6) * 0.73f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 6) * 0.88f)) * speed);
            ball.AddTorque(new Vector3(distanceX * (2) * 7.1f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 6) * 0.73f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 6) * 0.88f)) * speed);
        }
        else if (alpha < 1.5f)
        {
            ball.AddForce(new Vector3(distanceX * (2) * 0.2f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 6) * 0.78f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 6) * 0.8f)) * speed);
            ball.AddTorque(new Vector3(distanceX * (2) * 7.1f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 6) * 0.78f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 6) * 0.8f)) * speed);
        }
    }

    // Lose mode
    public void LoseMode ()
    {
        ball.AddForce(new Vector3(distanceX * (4) * 0.2f, distanceY * (15.5f) * 0.1f, distanceZ * (2.8f) * 0.2f) * speed);
        ball.AddTorque(new Vector3(distanceX * (4) * 7.1f, distanceY * (15.5f) * 7f, distanceZ * (2.8f) * 4.3f) * speed);
    }

    // Win mode
    public void WinMode()
    {
        ball.AddForce(new Vector3(distanceX * (6) * 0.2f, distanceY * (25.5f) * 0.1f, distanceZ * (5) * 0.2f) * speed);
        ball.AddTorque(new Vector3(distanceX * (5) * 7.1f, distanceY * (20.5f) * 7f, distanceZ * (2) * 4.3f) * speed);
        //ball.AddForce(new Vector3(distanceX * (5) * 0.2f, distanceY * (20.5f) * 0.1f, distanceZ * (2) * 0.2f) * speed);
        //ball.AddTorque(new Vector3(distanceX * (5) * 7.1f, distanceY * (20.5f) * 7f, distanceZ * (2) * 4.3f) * speed);
    }

    public void NormalMode ()
    {

        //// source and target positions
        //Vector3 pos = ball.position;
        //Vector3 target = net.transform.position;

        //// distance between target and source
        //float dist = Vector3.Distance(pos, target);

        //// rotate the object to face the target
        //ball.transform.LookAt(target);

        //// calculate initival velocity required to land the cube on target using the formula (9)
        //float Vi = Mathf.Sqrt(dist * -Physics.gravity.y / (Mathf.Sin(Mathf.Deg2Rad * _angle * 2)));
        //float Vy, Vz;   // y,z components of the initial velocity

        //Vy = Vi * Mathf.Sin(Mathf.Deg2Rad * _angle);
        //Vz = Vi * Mathf.Cos(Mathf.Deg2Rad * _angle);

        //// create the velocity vector in local space
        //Vector3 localVelocity = new Vector3(0f, Vy, Vz);

        //// transform it to global vector
        //Vector3 globalVelocity = ball.transform.TransformVector(localVelocity);

        //// launch the cube by setting its initial velocity
        //ball.velocity = globalVelocity;

        //2
        //Vector3 vector = CalculateTrajectoryVelocity(ball.transform.position, net.transform.position, 1);
        //ball.velocity = vector;


        //ball.AddForce(ball.transform.forward * 200, ForceMode.Impulse);

        //ball.AddForce(new Vector3(XaxisForce * 0.2f, YaxisForce * 0.1f, YaxisForce * 0.2f) * speed);
        //ball.AddTorque(new Vector3(XaxisForce * 7.1f, YaxisForce * 7f, YaxisForce * 4.3f) * speed);
    }

    
    private void FireCannonAtPoint(Vector3 point)
    {

        var velocity = BallisticVelocity(point, angle);
        Debug.Log("Firing at " + point + " velocity " + velocity);

        //ball.transform.position = transform.position;
        ball.velocity = velocity;
    }

    private Vector3 BallisticVelocity(Vector3 destination, float angle)
    {
        Vector3 dir = destination - transform.position; // get Target Direction
        float height = dir.y; // get height difference
        dir.y = 0; // retain only the horizontal difference
        float dist = dir.magnitude; // get horizontal direction
        float a = angle * Mathf.Deg2Rad; // Convert angle to radians
        dir.y = dist * Mathf.Tan(a); // set dir to the elevation angle.
        dist += height / Mathf.Tan(a); // Correction for small height differences

        // Calculate the velocity magnitude
        float velocity = Mathf.Sqrt(dist * Physics.gravity.magnitude / Mathf.Sin(2 * a));
        return velocity * dir.normalized; // Return a normalized vector.
    }


    Vector3 CalculateTrajectoryVelocity(Vector3 origin, Vector3 target, float t)
    {
        float vx = (target.x - origin.x) / t;
        float vz = (target.z - origin.z) / t;
        float vy = ((target.y - origin.y) - 0.5f * Physics.gravity.y * t * t) / t;
        return new Vector3(vx, vy, vz);
    }


    public void UpdateScore()
    {
        int currentScore = player.GetScore();
        player.SetScore(currentScore + 1);
        scoreText.text = "Score: " + (currentScore + 1).ToString();
    }

    public void UpdateThrows()
    {
        ballCountText.text = "Throws: " + ballCount.ToString();
        player.SetThrows(ballCount);
    }

    public void UpdateTime()
    {
        if (ballCount != 0)
        {
            temp = gameTime - (Time.time - startTime);
            //Debug.Log(temp);
            if (temp <= 10 && !redTime)
            {
                InvokeRepeating("RedTimer", 0, 0.25f);
                redTime = true;
            }
            if (temp <= 0)
            {
                canSwipe = false;
                player.SetGameDone(true);
                GameResults(player.GetPlayerId(), player.GetScore());
                DoneSound.Play();
            }
            minutes = ((int)temp / 60).ToString();
            seconds = (temp % 60).ToString("f1");
            timeText.text = "Time: " + minutes + ":" + seconds;

            player.SetTime(float.Parse(minutes) * 60 + float.Parse(seconds));
        }
    }

    public void GameResults(string userName, int score)
    {
        resultsPanel.SetActive(true);
        resultsText.text = "Your time is UP ! \n\nUsername: " + Regex.Replace(userName, @"[^a-zA-Z]", "") + "\nScore: " + score;
                            //resultsText.text = "Your time is UP ! \n\nUsername: " + player.GetPlayerId() + "\nScore: " +
                            //player.GetScore() + "\nTime: " + player.GetTime() +
                            //"\nNumber of throws: " + player.GetThrows();
    }

    // blink red timer
    public void RedTimer ()
    {
        if (!isRed)
        {
            timeText.color = Color.red;
            isRed = !isRed;
        }
        else
        {
            timeText.color = Color.white;
            isRed = !isRed;
        }
        

    }
    public void EndSurvey ()
    {
        SceneManager.LoadScene(2);
    }

    public void WinToggle (bool state)
    {
            player.SetWinOn(state);
    }
}
