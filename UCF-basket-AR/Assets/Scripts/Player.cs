using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class Player {

    private int Score = 0;
    private int Throws = 0;
    private float Time = 0f;
    private string PlayerId;
    private bool CheatOn = false;
    private List<string> questions = new List<string>();
    private int[,] answers = new int[2,10];
    private DatabaseReference reference, player, playerStats, playerAns1, playerAns2, playerScore;
    private DatabaseReference playerTime, playerThrows, playerCheatOn;
    private DatabaseReference questionsRef, answersRef;
    private int numQuests;
    DataSnapshot snapshot;
    public int QuestsNumber { get; set; }

    public Player ()
    {

    }
    public Player(string PlayerId, DatabaseReference reference)
    {
        this.PlayerId = PlayerId;
        this.reference = reference;
        player = reference.Child(this.PlayerId);
        playerStats = reference.Child(this.PlayerId).Child("GameStats");
        playerScore = reference.Child(this.PlayerId).Child("GameStats").Child("Score");
        playerTime = reference.Child(this.PlayerId).Child("GameStats").Child("Time");
        playerThrows = reference.Child(this.PlayerId).Child("GameStats").Child("Throws");
        playerCheatOn = reference.Child(this.PlayerId).Child("GameStats").Child("CheatOn");
        questionsRef = reference.Child("Questions");
        playerAns1 = reference.Child(this.PlayerId).Child("Answers1");
        playerAns2 = reference.Child(this.PlayerId).Child("Answers2");


        //playerAns1.Child(2.ToString()).SetValueAsync(4);

        InitStats();
        FetchQuests();

        
        Answer1(2, 3);
    }

    public void InitStats()
    {
        this.Score = 0;
        SetScore(0);
        this.Time = 0f;
        SetTime(0f);
        this.Throws = 0;
        SetThrows(0);
        this.CheatOn = false;
        SetCheatOn(false);
    }

    public void FetchQuests()
    {
        FirebaseDatabase.DefaultInstance
        .GetReference("Questions")
        .GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {
                Debug.LogError("Questions fetch not completed !");
            }
            else if (task.IsCompleted)
            {
                snapshot = task.Result;
                if (snapshot.Child(1.ToString()).Value.ToString() != null)
                {
                    int counter = 1;
                    while (snapshot.Child(counter.ToString()).Value.ToString() != null)
                    {
                        questions.Add(snapshot.Child(counter.ToString()).Value.ToString());
                        //Debug.Log(snapshot.Child(counter.ToString()).Value.ToString());
                        //Debug.Log(questions[counter - 1]);
                        counter++;
                    }
                    counter--;

                    QuestsNumber = counter;

                    //Debug.Log(counter);
                    //InitArrays(counter);
                }
            }
        });
    }

    //private void InitArrays(int counter)
    //{
    //    answers = new int[2, counter];
    //}

    public void Answer1(int qNumber, int answer)
    {
        answers[1, qNumber - 1] = answer;
        playerAns1.Child(qNumber.ToString()).SetValueAsync(answer);
    }

    public void Answer2(int qNumber, int answer)
    {
        answers[1, qNumber] = answer;
        playerAns2.Child(qNumber.ToString()).SetValueAsync(answer);
    }

    public void SetScore(int value)
    {
        this.Score = value;
        playerScore.SetValueAsync(value);
    }
    public int GetScore()
    {
        return this.Score;
    }
    public void SetThrows(int value)
    {
        this.Throws = value;
        playerThrows.SetValueAsync(value);
    }
    public int GetThrows()
    {
        return this.Throws;
    }
    public void SetTime(float value)
    {
        this.Time = value;
        playerTime.SetValueAsync(value);
    }
    public float GetTime()
    {
        return this.Time;
    }
    public void SetCheatOn(bool value)
    {
        this.CheatOn = value;
        playerCheatOn.SetValueAsync(value);
    }
    public bool GetCheatOn()
    {
        return this.CheatOn;
    }

    public string GetQuestion (int qNumber)
    {
        return questions[qNumber - 1];
    }
}