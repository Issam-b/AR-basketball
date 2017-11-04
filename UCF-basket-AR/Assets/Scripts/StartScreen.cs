using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Linq;

public class StartScreen : MonoBehaviour {

    public GameObject login, survey1, startGame;
    DatabaseReference reference;
    Player player;
    public Text userNameText;
    string userName;
    Toggle[] RateToggles = new Toggle[5];
    int answer;
    bool toggleOn = false;
    public ToggleGroup RateToggleGroup;
    public Button nextButton;
    int qNumber = 1;
    public Button startGameButton;
    public Text questionText;
    int waited = 0;

    public void Start()
    {
        RateToggles = survey1.GetComponentsInChildren<Toggle>();

        login.SetActive(true);
        survey1.SetActive(false);
        startGame.SetActive(false);
        
        //player = new Player("test", reference);

    }

    public void StartSurvey ()
    {
        if (userName != "")
        {
                userName = userNameText.text;
                //Debug.Log(userName);
                player = new Player(userName);
            //Debug.Log(player);




            for (int i = 0; i < 5; i++)
                RateToggles[i].onValueChanged.AddListener((value) =>
                {
                    toggleOn = true;
                    //if (RateToggles[i].isOn)
                    //{
                       
                    //} 
                });

            // get first question

            InvokeRepeating("DelayInvoke", 1, 1);
        }
    }

    void DelayInvoke ()
    {
        if (waited < 2)
            waited++;
        else
        {
            login.SetActive(false);
            survey1.SetActive(true);
            FetchNextQuestion(1);
            string[] test = player.questions.ToArray<string>();
            Debug.Log(test.Length);
            CancelInvoke("DelayInvoke");

        }
    }

    public void NextButton ()
    {
        //if (qNumber == Player.QuestsNumber)
        //{
        //    survey1.SetActive(false);
        //    startGame.SetActive(true);
        //}
        if (toggleOn && qNumber < Player.QuestsNumber)
        {
            // Get active toggle value
            foreach (Toggle tog in RateToggles)
            {
                if (tog.isOn)
                {
                    answer = int.Parse(tog.transform.Find("Label").GetComponent<Text>().text);
                    Debug.Log(answer);
                }
            }

            // Save and send answer
            Debug.Log("send");
            player.Answer1(qNumber, answer);

            // Clear toggles
            RateToggleGroup.SetAllTogglesOff();

            // Get next question
            qNumber++;
            Debug.Log("qNumber " + qNumber);
            if (qNumber != 1 & qNumber <= Player.QuestsNumber - 1)
            {
                Debug.Log("here");
                FetchNextQuestion(qNumber);
            }

            else if (qNumber == Player.QuestsNumber)
            {
                survey1.SetActive(false);
                startGame.SetActive(true);
            }

        }
    }

    public void FetchNextQuestion (int qNumber)
    {
        string temp = questionText.text;
        // Check if current questions is the same
        if (temp != player.GetQuestion(qNumber))
        {
            temp = player.GetQuestion(qNumber);
            // load question
            questionText.text = player.GetQuestion(qNumber);
        }
    }

    // TODO: remove this later
    public void UntoggleAll (Toggle[] RateToggles)
    {
        foreach (Toggle tog in RateToggles)
            tog.isOn = false;

        toggleOn = false;
    }

    public Player GetPlayer ()
    {
        return this.player;
    }
}
