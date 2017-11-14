using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using UnityEngine.UI;

public class DetectedScript : MonoBehaviour, ITrackableEventHandler
{
    public string LostText = "Please Find the Astronaut marker !";
    public Text InfoText;
    private TrackableBehaviour mTrackableBehaviour;
    private Player player;
    //private Basketball ball;

    //TODO: fix the ball not reseting by creating a bigger second boundary
    void Start () {
        //ball = ball = GameObject.FindGameObjectWithTag("Ball").GetComponent<Basketball>();
        player = StartScreen.player;
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
    }

    public void Update()
    {
        if (player.GetGameDone())
        {
            InfoText.text = "";
        }
    }

    public void OnTrackableStateChanged(
                                    TrackableBehaviour.Status previousStatus,
                                    TrackableBehaviour.Status newStatus)
    {
        if (!player.GetGameDone())
        {
            if (newStatus == TrackableBehaviour.Status.DETECTED ||
                newStatus == TrackableBehaviour.Status.TRACKED)
            {
                GameObject.FindGameObjectWithTag("Ball").GetComponent<Basketball>().ResetPositionCamera();
                GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().canSwipe = true;
                InfoText.text = "";

                GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().initialTarget = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().net.transform.position;
            }
            else
            {
                GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().canSwipe = false;
                InfoText.text = LostText;
            }
        }
    }
}
