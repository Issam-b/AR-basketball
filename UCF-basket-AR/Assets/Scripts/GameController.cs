using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class GameController : MonoBehaviour {

    public const int gameTime = 15; 
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
        timeText.text = "Time: 5:00.0";
    }

    private void Update()
    {
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

    private void BallThrow ()
    {
        //
        distanceX = imageTarget.GetChild(3).GetChild(2).transform.position.x - ball.transform.position.x;
        distanceY = imageTarget.GetChild(3).GetChild(2).transform.position.y - ball.transform.position.y;
        distanceZ = imageTarget.GetChild(3).GetChild(2).transform.position.z - ball.transform.position.z;
        //
        XaxisForce = FinalTouchPosition.x - InitialTouchPosition.x;
        YaxisForce = FinalTouchPosition.y - InitialTouchPosition.y;
        ball.useGravity = true;
        ball.isKinematic = false;

        // Lose mode
        if (!player.GetWinOn())
        {
            ball.AddForce(new Vector3(distanceX * (4) * 0.2f, distanceY * (15.5f) * 0.1f, distanceZ * (2.8f) * 0.2f) * speed);
            ball.AddTorque(new Vector3(distanceX * (4) * 7.1f, distanceY * (15.5f) * 7f, distanceZ * (2.8f) * 4.3f) * speed);
        }
        // Win mode
        if (player.GetWinOn())
        {
            ball.AddForce(new Vector3(distanceX * (5) * 0.2f, distanceY * (20.5f) * 0.1f, distanceZ * (2) * 0.2f) * speed);
            ball.AddTorque(new Vector3(distanceX * (5) * 7.1f, distanceY * (20.5f) * 7f, distanceZ * (2) * 4.3f) * speed);
        }
        //

        //Normal Game
        //ball.AddForce(new Vector3(XaxisForce * 0.2f, YaxisForce * 0.1f, YaxisForce * 0.2f) * speed);
        //ball.AddTorque(new Vector3(XaxisForce * 7.1f, YaxisForce * 7f, YaxisForce * 4.3f) * speed);
        canSwipe = false;
        ball.transform.SetParent(imageTarget, true);
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
            Debug.Log(temp);
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
