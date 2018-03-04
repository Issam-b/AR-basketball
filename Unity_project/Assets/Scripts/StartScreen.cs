using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase.Database;

public class StartScreen : MonoBehaviour {

    public const int waitForQuests = 3;
    private DatabaseReference reference;
    public static Player player;
    private string userName;
    private Toggle[] RateToggles = new Toggle[5];
    private int answer;
    private int qNumber = 1;
    private int waited = 0;
    public GameObject login, survey1, startGame, GameMode;
    public ToggleGroup RateToggleGroup;
    public Button nextButton;
    public Button startGameButton;
    public Text questionText;
    public Text userNameText;
    private int gameMode = 1;
    private string[] modes = new string[] { "Win", "Lose", "Normal" };


    public void Start()
    {
        // Get all toggles instances
        RateToggles = survey1.GetComponentsInChildren<Toggle>();

        // Activate username view
        GameMode.SetActive(true);
        login.SetActive(false);
        survey1.SetActive(false);
        startGame.SetActive(false);
    }

    // Set game mode
    public void SetGameMode(int choice)
    {
        gameMode = choice + 1;
        Debug.Log("Game mode: " + modes[gameMode]);
    }

    // Go to the game
    public void GoGame()
    {
        // Activate username view
        GameMode.SetActive(false);
        login.SetActive(true);
        survey1.SetActive(false);
        startGame.SetActive(false);
    }

    // For start survey button
    public void StartSurvey ()
    {
        userName = userNameText.text;
        if (userName != "")
        {
            if (userName != "test")
            {
                userName = System.DateTime.Now.ToString("dd-MM_HH-mm") + "-" + userNameText.text;
            }
            Debug.Log("Username: " + userName);
            player = new Player(userName);
            Debug.Log("Created player object: " + player);

            // set game mode
            player.SetGameMode(gameMode);
            // get first question
            InvokeRepeating("FirstQuestion", 1, 1);

        }
    }

    void FirstQuestion ()
    {
        if (waited < waitForQuests)
            waited++;
        else
        {
            Debug.Log("Ftech question: 1");
            FetchNextQuestion(1);
            login.SetActive(false);
            survey1.SetActive(true);
            CancelInvoke("FirstQuestion");
        }
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
                    answer = int.Parse(tog.transform.Find("Label").GetComponent<Text>().text.Substring(0, 1));
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
                Debug.Log("Ftech question: " + qNumber);
                FetchNextQuestion(qNumber);
            }

            else if (qNumber == Player.QuestsNumber)
            {
                Debug.Log("Questionnaire done!");
                survey1.SetActive(false);
                startGame.SetActive(true);
            }
        }
    }

    public void FetchNextQuestion (int qNumber)
    {
        string previousQuest = questionText.text;
        // Check if current questions is the same
        if (previousQuest != player.GetQuestion(qNumber))
        {
            previousQuest = player.GetQuestion(qNumber);
            // load question
            questionText.text = player.GetQuestion(qNumber);
        }
    }

    // For start game button
    public void StartGame ()
    {
        SceneManager.LoadScene(1);
    }
}
