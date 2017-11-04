using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class EndScreen : MonoBehaviour {

    private Player player;
    private Toggle[] RateToggles = new Toggle[5];
    private int answer;
    private int qNumber = 1;
    public GameObject survey1, newGame;
    public ToggleGroup RateToggleGroup;
    public Button nextButton; 
    public Text questionText;


    public void Start()
    {
        // Get all toggles instances
        RateToggles = survey1.GetComponentsInChildren<Toggle>();

        // Activate username view
        newGame.SetActive(false);

        player = StartScreen.player;
        Debug.Log("Got reference to player object: " + player);

        // get first question
        FirstQuestion();
    }

    void FirstQuestion ()
    {
        Debug.Log("Ftech question: 1");
        FetchNextQuestion(1);   
    }

    // For next question button
    public void NextButton ()
    {
        if (RateToggleGroup.AnyTogglesOn() && qNumber < Player.QuestsNumber)
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
            player.Answer2(qNumber, answer);

            // Clear toggles
            RateToggleGroup.SetAllTogglesOff();

            // Get next question
            qNumber++;
            Debug.Log("qNumber " + qNumber);
            if (qNumber != 1 & qNumber <= Player.QuestsNumber - 1)
            {
                Debug.Log("Ftech question: " + qNumber);
                FetchNextQuestion(qNumber);
            }

            else if (qNumber == Player.QuestsNumber)
            {
                Debug.Log("Questionnaire done!");
                survey1.SetActive(false);
                newGame.SetActive(true);
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

    // For start game button
    public void NewGame ()
    {
        SceneManager.LoadScene(0);
    }
}
