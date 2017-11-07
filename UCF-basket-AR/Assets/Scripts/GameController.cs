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

    private void Start()
    {
        resultsPanel.SetActive(false);
        player = StartScreen.player;
        timeText.text = "Time: 5:00.0";
    }

    private void Update()
    {
        NormalMode();
        if (!player.GetGameDone())
        {
            UpdateTime();
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

            // Win mode
            if (player.GetWinOn())
            {
                //ball.transform.LookAt(net.transform);
                float alpha = distanceZ / distanceY;
                if (alpha > 3f && alpha < 3.5f)
                {
                    ball.AddForce(new Vector3(distanceX * (2) * 0.2f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 4) * 0.7f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 4) * 0.65f)) * speed);
                    ball.AddTorque(new Vector3(distanceX * (2) * 7.1f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 4) * 0.7f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 4) * 0.65f)) * speed);
                }
                if (alpha > 2.5f && alpha < 3f)
                {
                    ball.AddForce(new Vector3(distanceX * (2) * 0.2f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 4) * 0.68f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 4) * 0.75f)) * speed);
                    ball.AddTorque(new Vector3(distanceX * (2) * 7.1f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 4) * 0.68f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 4) * 0.75f)) * speed);
                }
                if (alpha > 3.5f)
                {
                    ball.AddForce(new Vector3(distanceX * (2) * 0.2f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 2.5f) * 1.5f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 2.5f) * 0.7f)) * speed);
                    ball.AddTorque(new Vector3(distanceX * (2) * 7.1f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 2.5f) * 1.5f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 2.5f) * 0.7f)) * speed);
                }
                if (alpha < 2.5f && alpha > 2f)
                {
                    ball.AddForce(new Vector3(distanceX * (2) * 0.2f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 6) * 0.52f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 6) * 0.82f)) * speed);
                    ball.AddTorque(new Vector3(distanceX * (2) * 7.1f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 6) * 0.52f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 6) * 0.82f)) * speed);
                }
                if (alpha < 2f && alpha > 1.5f)
                {
                    ball.AddForce(new Vector3(distanceX * (2) * 0.2f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 6) * 0.73f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 6) * 0.88f)) * speed);
                    ball.AddTorque(new Vector3(distanceX * (2) * 7.1f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 6) * 0.73f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 6) * 0.88f)) * speed);
                }
                if (alpha < 1.5f)
                {
                    ball.AddForce(new Vector3(distanceX * (2) * 0.2f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 6) * 0.78f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 6) * 0.8f)) * speed);
                    ball.AddTorque(new Vector3(distanceX * (2) * 7.1f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 6) * 0.78f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 6) * 0.8f)) * speed);
                }

            }


            //}



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
