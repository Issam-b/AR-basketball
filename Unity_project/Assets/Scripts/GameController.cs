using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class GameController : MonoBehaviour {

    public const int gameTime = 180; 
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

    private void Start()
    {
        resultsPanel.SetActive(false);
        player = StartScreen.player;
        timeText.text = "Time: " + gameTime/60 + ":" + (gameTime - ((gameTime/60) * 60)) + ".0";
    }

    private void Update()
    {
        NormalMode();
        if (!player.GetGameDone())
        {
            UpdateTime();
        }
    }

    // on touch release to throw the ball
    public void OnTouchDown()
    {
        if (canSwipe)
        {
            InitialTouchPosition = Input.mousePosition;
        }
    }

    // On first touch of screen to throw the ball
    public void OnTouchUp()
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
    // decison on throwing the ball
    private void BallThrow()
    {
        // get distance of swiping the ball
        distanceX = Mathf.Abs(imageTarget.GetChild(3).GetChild(2).transform.position.x - ball.transform.position.x);
        distanceY = Mathf.Abs(imageTarget.GetChild(3).GetChild(2).transform.position.y - ball.transform.position.y);
        distanceZ = Mathf.Abs(imageTarget.GetChild(3).GetChild(2).transform.position.z - ball.transform.position.z);
       
        // calculate the difference to be used for force of throwing ball
        XaxisForce = FinalTouchPosition.x - InitialTouchPosition.x;
        YaxisForce = FinalTouchPosition.y - InitialTouchPosition.y;
        ball.useGravity = true;
        ball.isKinematic = false;

        // ball throwing calculation according to the mode
        GameModes();

        canSwipe = false;
        ball.transform.SetParent(imageTarget, true);
    }

    // all modes
    public void GameModes()
    {
        //Normal Game
        int ran = (int)(Random.value * 10);
        if (ran == 5)
        {
            NormalMode();
        }

        else
        {
            // Lose mode
            if (player.GetGameMode() == 2)
            {
                LoseModeJ();
            }

            // Win mode
            if (player.GetGameMode() == 1)
            {
                WinModeJ();
            }
        }
    }

    // lose mode calculations
    public void LoseModeJ()
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
                ball.AddForce(new Vector3(distanceX * (15) * 0.2f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 4) * 0.7f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 4) * 0.65f)) * speed);
                ball.AddTorque(new Vector3(distanceX * (15) * 7.1f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 4) * 0.7f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 4) * 0.65f)) * speed);
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
                ball.AddForce(new Vector3(distanceX * (15) * 0.2f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 4) * 0.73f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 4) * 0.75f)) * speed);
                ball.AddTorque(new Vector3(distanceX * (15) * 7.1f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 4) * 0.73f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 4) * 0.75f)) * speed);
            }
            if (ran2 == 2)
            {
                ball.AddForce(new Vector3(distanceX * (2) * 0.2f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 4) * 0.58f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 4) * 0.85f)) * speed);
                ball.AddTorque(new Vector3(distanceX * (2) * 7.1f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 4) * 0.58f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 4) * 0.85f)) * speed);
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
                ball.AddForce(new Vector3(distanceX * (15) * 0.2f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 2.5f) * 1.5f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 2.5f) * 0.7f)) * speed);
                ball.AddTorque(new Vector3(distanceX * (15) * 7.1f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 2.5f) * 1.5f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 2.5f) * 0.7f)) * speed);
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
                ball.AddForce(new Vector3(distanceX * (15) * 0.2f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 6) * 0.6f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 6) * 0.8f)) * speed);
                ball.AddTorque(new Vector3(distanceX * (15) * 7.1f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 6) * 0.6f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 6) * 0.8f)) * speed);
            }
            if (ran2 == 2)
            {
                ball.AddForce(new Vector3(distanceX * (2) * 0.2f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 6) * 0.48f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 6) * 0.95f)) * speed);
                ball.AddTorque(new Vector3(distanceX * (2) * 7.1f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 6) * 0.48f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 6) * 0.95f)) * speed);
            }
        }
        if (alpha < 2f && alpha > 1.5f)
        {
            if (ran2 == 0)
            {
                ball.AddForce(new Vector3(distanceX * (2) * 0.2f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 6) * 0.57f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 6) * 1.0f)) * speed);
                ball.AddTorque(new Vector3(distanceX * (2) * 7.1f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 6) * 0.57f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 6) * 1.0f)) * speed);
            }
            if (ran2 == 1)
            {
                ball.AddForce(new Vector3(distanceX * (15) * 0.2f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 6) * 0.61f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 6) * 0.8f)) * speed);
                ball.AddTorque(new Vector3(distanceX * (15) * 7.1f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 6) * 0.61f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 6) * 0.8f)) * speed);
            }
            if (ran2 == 2)
            {
                ball.AddForce(new Vector3(distanceX * (2) * 0.2f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 6) * 0.57f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 6) * 0.8f)) * speed);
                ball.AddTorque(new Vector3(distanceX * (2) * 7.1f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 6) * 0.57f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 6) * 0.8f)) * speed);
            }
        }
        if (alpha < 1.5f)
        {
            if (ran2 == 0)
            {
                ball.AddForce(new Vector3(distanceX * (2) * 0.2f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 6) * 1.18f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 6) * 0.7f)) * speed);
                ball.AddTorque(new Vector3(distanceX * (2) * 7.1f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 6) * 1.18f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 6) * 0.7f)) * speed);
            }
            if (ran2 == 1)
            {
                ball.AddForce(new Vector3(distanceX * (15) * 0.2f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 6) * 0.78f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 6) * 0.8f)) * speed);
                ball.AddTorque(new Vector3(distanceX * (15) * 7.1f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 6) * 0.78f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 6) * 0.8f)) * speed);
            }
            if (ran2 == 2)
            {
                ball.AddForce(new Vector3(distanceX * (2) * 0.2f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 6) * 0.58f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 6) * 1.1f)) * speed);
                ball.AddTorque(new Vector3(distanceX * (2) * 7.1f, (distanceY + distanceZ) * Mathf.Cos(Mathf.PI / 6) * 0.58f, ((distanceY + distanceZ) * Mathf.Sin(Mathf.PI / 6) * 1.1f)) * speed);
            }
        }
    }

    // win mode calculations
    public void WinModeJ ()
    {
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

    // Normal mode calculations
    public void NormalMode ()
    {
        ball.AddForce(new Vector3(XaxisForce * 0.2f, YaxisForce * 0.1f, YaxisForce * 0.2f) * speed);
        ball.AddTorque(new Vector3(XaxisForce * 7.1f, YaxisForce * 7f, YaxisForce * 4.3f) * speed);
    }

    // Update UI and DB with current score
    public void UpdateScore()
    {
        int currentScore = player.GetScore();
        player.SetScore(currentScore + 1);
        scoreText.text = "Score: " + (currentScore + 1).ToString();
    }

    // update UI & DB with number of throws
    public void UpdateThrows()
    {
        ballCountText.text = "Throws: " + ballCount.ToString();
        player.SetThrows(ballCount);
    }

    // update UI & DB with the time counter
    public void UpdateTime()
    {
        if (ballCount != 0)
        {
            temp = gameTime - (Time.time - startTime);
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

    // Show summary of game and results at end of game
    public void GameResults(string userName, int score)
    {
        resultsPanel.SetActive(true);
        resultsText.text = "Your time is UP ! \n\nUsername: " + Regex.Replace(userName, @"[^a-zA-Z]", "") + "\nScore: " + score;
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
}
