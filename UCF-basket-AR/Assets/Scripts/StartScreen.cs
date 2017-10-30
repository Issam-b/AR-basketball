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
    bool toggleOn = false;
    public ToggleGroup RateToggleGroup;

    public void Start()
    {
        //login = GameObject.FindGameObjectWithTag("Login").GetComponent<GameObject>();
        //survey1 = GameObject.FindGameObjectWithTag("Survey1").GetComponent<GameObject>();
        //startGame = GameObject.FindGameObjectWithTag("StartGame").GetComponent<GameObject>();

        login.SetActive(true);
        survey1.SetActive(false);
        startGame.SetActive(false);

        // Setting Firebase database
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://ucfarbasketball.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        
    }

    public void StartSurvey ()
    {
        userName = GameObject.FindGameObjectWithTag("UserField").GetComponent<Text>();
        if (userName.text != "")
        {
            player = new Player(userName.text, reference);
            login.SetActive(false);
            survey1.SetActive(true);

            // load first question
            GameObject.FindGameObjectWithTag("QuestionText").GetComponent<Text>().text = player.GetQuestion(1);

            //RateToggles = survey1.GetComponentsInChildren<Toggle>();
            //foreach (Toggle tog in RateToggles)
            ////    tog.onValueChanged.AddListener((value) =>
             //   {
             //       toggleOn = true;
              //  });
            
        }
    }

    public void FetchNextQuestion (int aNumber)
    {
        //int toggleNumber = RateToggleGroup.ActiveToggles().FirstOrDefault();
        if (toggleOn)
        {
            //player.Answer1(qNumber, toggleNumber)

        }
    }

}
