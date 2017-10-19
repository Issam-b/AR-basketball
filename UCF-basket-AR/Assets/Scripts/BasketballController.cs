using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasketballController : MonoBehaviour {

    public Text scoreText;
    private int currentScore = 0;
    private Vector3 InitialPosition;
    private TouchController touchsSystem;

    private void Start()
    {
        this.GetComponent<Rigidbody>().useGravity = false;
        touchsSystem = GameObject.FindObjectOfType<TouchController>().GetComponent<TouchController>();
        InitialPosition = this.transform.position;
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
        this.transform.position = InitialPosition + new Vector3(Random.Range(-20f, 20f), 0f, 0f);
        this.GetComponent<Rigidbody>().useGravity = false;
        this.GetComponent<Rigidbody>().isKinematic = true;
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        touchsSystem.canSwipe = true;
    }
}
