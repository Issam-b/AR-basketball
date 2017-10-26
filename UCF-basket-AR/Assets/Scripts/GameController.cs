﻿using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class GameController : MonoBehaviour {

    //private float InitialTouchTime, FinalTouchTime;
    private Vector3 InitialTouchPosition, FinalTouchPosition;
    private float XaxisForce, YaxisForce;

    public Rigidbody ball;
    public Transform imageTarget;

    private int ballCount = 0;
    public float speed = 0.5f;
    public bool canSwipe = true;
    public Text scoreText, timeText, ballCountText;
    private float startTime = 0f, temp;
    string minutes, seconds;    
   
    DatabaseReference reference;
    Player player;

    private void Start()
    {
        //ball.useGravity = false;

        // Setting Firebase database
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://ucfarbasketball.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        
        player = new Player("test", reference);
    }

    private void Update()
    {
        UpdateTime();
    }

    public void OnTouchDown ()
    {
        if (canSwipe)
        {
            //InitialTouchTime = Time.time;
            InitialTouchPosition = Input.mousePosition;
        }
    }

    public void OnTouchUp ()
    {
        if (canSwipe)
        {
            //FinalTouchTime = Time.time;
            FinalTouchPosition = Input.mousePosition;
            if (ballCount == 0)
                startTime = Time.time;
            BallThrow();
            ballCount++;
            UpdateThrows();
        }
    }

    public void BallThrow ()
    {
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
    }

    public void UpdateScore ()
    {
        int currentScore = player.GetScore();
        player.SetScore(currentScore + 1);
        scoreText.text = "Score: " + currentScore.ToString();
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
            temp = Time.time - startTime;
            minutes = ((int)temp / 60).ToString();
            seconds = (temp % 60).ToString("f2");
            timeText.text = "Time: " + minutes + "." + seconds;

            player.SetTime(float.Parse(minutes) * 60 + float.Parse(seconds));
        }
    }
}