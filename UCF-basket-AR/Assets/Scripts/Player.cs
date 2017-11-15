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
    private bool WinOn;
    private bool gameDone = false;
    private List<string> questions = new List<string>();
    private int[,] answers = new int[2,10];
    private DatabaseReference reference, player, playerStats, playerAns1, playerAns2, playerScore;
    private DatabaseReference playerTime, playerThrows, playerCheatOn, playerGameDone;
    private DatabaseReference questionsRef, answersRef;
    private int numQuests;
    DataSnapshot snapshot;
    public static int QuestsNumber { get; set; }

    public Player(string PlayerId)
    {
        // Setting Firebase database
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://ucfarbasketball.firebaseio.com/");
        this.reference = FirebaseDatabase.DefaultInstance.RootReference;
        this.PlayerId = PlayerId;
        player = reference.Child(this.PlayerId);
        //playerStats = reference.Child(this.PlayerId).Child("GameStats");
        //playerScore = reference.Child(this.PlayerId).Child("GameStats").Child("Score");
        //playerTime = reference.Child(this.PlayerId).Child("GameStats").Child("Time");
        //playerThrows = reference.Child(this.PlayerId).Child("GameStats").Child("Throws");
        //playerCheatOn = reference.Child(this.PlayerId).Child("GameStats").Child("WinOn");
        //playerGameDone = reference.Child(this.PlayerId).Child("GameStats").Child("GameDone");
        playerStats = reference.Child(this.PlayerId);
        playerScore = reference.Child(this.PlayerId).Child("Score");
        playerTime = reference.Child(this.PlayerId).Child("Time");
        playerThrows = reference.Child(this.PlayerId).Child("Throws");
        playerCheatOn = reference.Child(this.PlayerId).Child("WinOn");
        playerGameDone = reference.Child(this.PlayerId).Child("GameDone");

        questionsRef = reference.Child("Questions");
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
        this.WinOn = true;
        SetWinOn(true);
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
                if (snapshot.Child("q" + 1.ToString()).Value.ToString() != null)
                {
                    QuestsNumber = 1;
                    while (snapshot.Child("q" + QuestsNumber.ToString()).Value.ToString() != null)
                    {
                        Debug.Log(snapshot.Child("q" + QuestsNumber.ToString()).Value.ToString());
                        questions.Add(snapshot.Child("q" + QuestsNumber.ToString()).Value.ToString());
                        QuestsNumber++;
                    }
                    Debug.Log("Questions fetched");
                }
            }
        });
    }

    public void Answer1(int qNumber, int answer)
    {
        answers[1, qNumber - 1] = answer;
        playerAns1.Child("a1-" + qNumber.ToString()).SetValueAsync(answer);
    }

    public void Answer2(int qNumber, int answer)
    {
        answers[1, qNumber - 1] = answer;
        playerAns2.Child("a2-" + qNumber.ToString()).SetValueAsync(answer);
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

    public void SetGameDone(bool value)
    {
        this.gameDone = value;
        playerGameDone.SetValueAsync(value);
    }

    public bool GetGameDone()
    {
        return this.gameDone;
    }

    public void SetTime(float value)
    {
        this.Time = value;
        //playerTime.SetValueAsync(value);
    }

    public float GetTime()
    {
        return this.Time;
    }

    public void SetWinOn(bool value)
    {
        this.WinOn = value;
        playerCheatOn.SetValueAsync(value);
    }

    public bool GetWinOn()
    {
        return this.WinOn;
    }

    public string GetPlayerId ()
    {
        return this.PlayerId;
    }

    public string GetQuestion (int qNumber)
    {
        return questions[qNumber - 1];
    }
}