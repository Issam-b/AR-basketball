using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class DetectedScript : MonoBehaviour, ITrackableEventHandler
{

    private TrackableBehaviour mTrackableBehaviour;

    //TODO: fix the ball not reseting by creating a bigger second boundary
    void Start () {
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
            GameObject.FindGameObjectWithTag("Ball").GetComponent<GameObject>().SetActive(true);
            GameObject.FindGameObjectWithTag("Ball").GetComponent<Basketball>().ResetPositionCamera();
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().canSwipe = true;
            GameObject.FindGameObjectWithTag("Ball").GetComponent<GameObject>().transform.SetParent(Camera.main.transform, true);
        }
        else
        {
            GameObject.FindGameObjectWithTag("Ball").GetComponent<GameObject>().transform.SetParent(Camera.main.transform, true);
            GameObject.FindGameObjectWithTag("Ball").GetComponent<GameObject>().SetActive(false);
        }
    }
}
