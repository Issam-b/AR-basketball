using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public const int gameTime = 300; 
    private Vector3 InitialTouchPosition, FinalTouchPosition;
    private float XaxisForce, YaxisForce;
    private int ballCount = 0;
    private float startTime = 0f, temp;
    string minutes, seconds;
    private bool gameDone = false;

    public Rigidbody ball;
    public Transform imageTarget;
    public float speed = 0.5f;
    public bool canSwipe = true;
    public Text scoreText, timeText, ballCountText, resultsText;
    public GameObject resultsPanel;
    

    private Player player;

    private void Start()
    {
        resultsPanel.SetActive(false);
        player = StartScreen.player;
        timeText.text = "Time: 5:00.0";
    }

    private void Update()
    {
        if (!gameDone)
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
            XaxisForce = FinalTouchPosition.x - InitialTouchPosition.x;
            YaxisForce = FinalTouchPosition.y - InitialTouchPosition.y;
            ball.useGravity = true;
            ball.isKinematic = false;
            ball.AddForce(new Vector3(XaxisForce * 0.2f, YaxisForce * 0.1f, YaxisForce * 0.2f) * speed);
            ball.AddTorque(new Vector3(XaxisForce * 7.1f, YaxisForce * 7f, YaxisForce * 4.3f) * speed);
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
            if (temp <= 0)
            {
                canSwipe = false;
                temp = 0;
                resultsPanel.SetActive(true);
                gameDone = true;
                GameResults();
            }
            minutes = ((int)temp / 60).ToString();
            seconds = (temp % 60).ToString("f1");
            timeText.text = "Time: " + minutes + ":" + seconds;

            player.SetTime(float.Parse(minutes) * 60 + float.Parse(seconds));
        }
    }

    public void GameResults()
    {
        resultsText.text = "Your time is UP ! \n\nUsername: " + player.GetPlayerId() + "\nScore: " +
                            player.GetScore() + "\nTime: " + player.GetTime() +
                            "\nNumber of throws: " + player.GetThrows();
    }

    public void EndSurvey ()
    {
        SceneManager.LoadScene(2);
    }
}
