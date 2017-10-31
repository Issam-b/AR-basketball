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
    Text userName;
    Toggle[] RateToggles = new Toggle[5];
    int result;
    bool toggleOn = false;
    public ToggleGroup RateToggleGroup;
    public Button nextButton;
    int curQuest = 1;

    public void Start()
    {
        //login = GameObject.FindGameObjectWithTag("Login").GetComponent<GameObject>();
        //survey1 = GameObject.FindGameObjectWithTag("Survey1").GetComponent<GameObject>();
        //startGame = GameObject.FindGameObjectWithTag("StartGame").GetComponent<GameObject>();

        RateToggles = survey1.GetComponentsInChildren<Toggle>();

        login.SetActive(true);
        survey1.SetActive(false);
        startGame.SetActive(false);

        // Setting Firebase database
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://ucfarbasketball.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        player = new Player("test", reference);
    }

    public void StartSurvey ()
    {
        
        userName = GameObject.FindGameObjectWithTag("UserField").GetComponent<Text>();
        if (userName.text != "")
        {
            //player = new Player(userName.text, reference);
            login.SetActive(false);
            survey1.SetActive(true);
            

            for (int i = 0; i < 5; i++)
                RateToggles[i].onValueChanged.AddListener((value) =>
                {
                    toggleOn = true;
                    result = i + 1;
                });

            // get first question
            string temp = GameObject.FindGameObjectWithTag("QuestionText").GetComponent<Text>().text = player.GetQuestion(1);

            curQuest++;
            
            nextButton.onClick.AddListener(() =>
            {
                FetchNextQuestion(curQuest);
            });
            

            


            
        }
    }

    public void FetchNextQuestion (int qNumber)
    {
        string temp = GameObject.FindGameObjectWithTag("QuestionText").GetComponent<Text>().text;

        //for (int qNumber = 0; qNumber < player.QuestsNumber; qNumber++)
        //{
           

            if (toggleOn)
            {

            if (temp != player.GetQuestion(qNumber))
            {
                temp = player.GetQuestion(qNumber);
                // load question
                GameObject.FindGameObjectWithTag("QuestionText").GetComponent<Text>().text = player.GetQuestion(qNumber);
            }

            player.Answer1(qNumber, result);

                //toggleOn = false;

                //foreach (Toggle tog in RateToggles)
                //{
                //    tog.isOn = false;
               // }
            if (curQuest < player.QuestsNumber)
                curQuest++;

            }
        //}
    }

}
