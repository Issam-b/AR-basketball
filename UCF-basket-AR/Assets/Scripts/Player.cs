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
    private List<string> quests = new List<string>();
    private int[][] ans;
    //private ArrayList answers2 = new ArrayList();
    private DatabaseReference reference, player, playerStats, playerAns1, playerAns2, playerScore;
    private DatabaseReference playerTime, playerThrows, playerCheatOn;
    private DatabaseReference questions, answers;
    private int numQuests;
    DataSnapshot snapshot;

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
        questions = reference.Child("Questions");
        playerAns1 = reference.Child(this.PlayerId).Child("Answers1");
        playerAns2 = reference.Child(this.PlayerId).Child("Answers2");

        InitStats();
        FetchQuests();

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
                
                for (int i = 1; i < 4; i++)
                {
                    quests.Add(snapshot.Child(i.ToString()).Value.ToString());
                    //Debug.Log(snapshot.Child(i.ToString()).Value.ToString());
                }
                //ans = new int[2][quests.];
            }
        });
    }

    public void Answer1(int answer, int qNumber)
    {
        //ans[0][]
        playerAns1.Child(qNumber.ToString()).SetValueAsync(answer);
    }

    public void Answer2(int answer, int qNumber)
    {

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
}