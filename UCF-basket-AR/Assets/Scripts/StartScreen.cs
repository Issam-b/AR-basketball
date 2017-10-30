using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class StartScreen : MonoBehaviour {

    public GameObject login, survey1, startGame;
    DatabaseReference reference;
    Player player;

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

        player = new Player("test", reference);
    }

    public void StartSurvey ()
    {
        if (GameObject.FindGameObjectWithTag("UserField").GetComponent<Text>().text != "")
        {
            login.SetActive(false);
            survey1.SetActive(true);

            // load first question
            GameObject.FindGameObjectWithTag("QuestionText").GetComponent<Text>().text = player.GetQuestion(1);
        }
    }


}
