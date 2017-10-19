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
    //private Basketball ball;

    //TODO: fix the ball not reseting by creating a bigger second boundary
    void Start () {
        //ball = ball = GameObject.FindGameObjectWithTag("Ball").GetComponent<Basketball>();
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
    }

    public void OnTrackableStateChanged(
                                    TrackableBehaviour.Status previousStatus,
                                    TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED)
        {
            //ball.Show();
            GameObject.FindGameObjectWithTag("Ball").GetComponent<Basketball>().ResetPositionCamera();
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().canSwipe = true;
            InfoText.text = "";
        }
        else
        {
            //ball.Hide();
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().canSwipe = false;
            InfoText.text = LostText;
        }
    }
}
