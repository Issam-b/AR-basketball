using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasketballController : MonoBehaviour {

    public Text scoreText, DistanceY, DistanceZ;
    private int currentScore = 0;
    private Vector3 InitialPosition;
    private TouchController touchsSystem;
    public float threshHold = 4;
    Transform cameraTransform;
    public Transform imageTarget;
    Vector3 offset;
    float distanceZ, distanceY;
    public Transform canvas;

    private void Start()
    {
        this.GetComponent<Rigidbody>().useGravity = false;
        touchsSystem = GameObject.FindObjectOfType<TouchController>().GetComponent<TouchController>();
        InitialPosition = this.transform.position;
        ResetPositionCamera();
    }

    private void Update()
    {
        //distanceZ = Mathf.Abs(imageTarget.Find("Basketball_hoop").Find("NET").position.z - Camera.main.transform.position.z) / 100;
        //distanceY = Mathf.Abs(imageTarget.Find("Basketball_hoop").Find("NET").position.y - Camera.main.transform.position.y) / 25;
        //DistanceY.text = "Distance Y: " + distanceY.ToString("f2") + "m";
        //DistanceZ.text = "Distance Z: " + distanceZ.ToString("f0") + "m";
    }

    public void ResetPositionCamera()
    {
        offset = new Vector3(0.5f + Random.Range(-threshHold, threshHold), -9.1f, 17.3f);
        cameraTransform = Camera.main.transform;

        transform.position = cameraTransform.position + cameraTransform.forward * 5f;
        transform.position = cameraTransform.position + offset;
        transform.SetParent(Camera.main.transform, true);
        transform.rotation = cameraTransform.rotation;

        //transform.localScale = Vector3.one;
        //this.transform.position = InitialPosition;
        this.GetComponent<Rigidbody>().useGravity = false;
        this.GetComponent<Rigidbody>().isKinematic = true;
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        touchsSystem.canSwipe = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ring")
        {
            ScoreUpdate();
        }
    }

    private void ScoreUpdate()
    {
        currentScore++;
        scoreText.text = "Score: " + currentScore.ToString();
    }

    public void ResetPosition()
    {
            this.transform.position = InitialPosition + new Vector3(Random.Range(-threshHold, threshHold), 0f, 0f);
            this.GetComponent<Rigidbody>().useGravity = false;
            this.GetComponent<Rigidbody>().isKinematic = true;
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}
