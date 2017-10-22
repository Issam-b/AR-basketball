using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TouchController : MonoBehaviour {

    private float InitialTouchTime;
    private float FinalTouchTime;
    private Vector3 InitialTouchPosition;
    private Vector3 FinalTouchPosition;

    private float XaxisForce;
    private float YaxisForce;
    private float ZaxisForce;

    private Vector3 RequireForce;
    public float speed = 0.5f;

    public Rigidbody ball;

    public bool canSwipe = true;

    public Text timeText, ballCountText;
    private float startTime = 0f, temp;
    string minutes, seconds;
    public Transform imageTarget;

    private int ballCount = 0;

    private void Start()
    {
        //ball.useGravity = false;
    }

    private void Update()
    {
        if (ballCount != 0)
        {
            temp = Time.time - startTime;
            minutes = ((int)temp / 60).ToString();
            seconds = (temp % 60).ToString("f2");

            timeText.text = "Time: " + minutes + "." + seconds;
            ballCountText.text = "Throws: " + ballCount.ToString();
        }
    }

    public void OnTouchDown ()
    {
        if (canSwipe)
        {
            InitialTouchTime = Time.time;
            InitialTouchPosition = Input.mousePosition;
        }
    }

    public void OnTouchUp ()
    {
        if (canSwipe)
        {
            FinalTouchTime = Time.time;
            FinalTouchPosition = Input.mousePosition;
            if (ballCount == 0)
                startTime = Time.time;
            BallThrow();
            ballCount++;
        }
    }

    private void BallThrow ()
    {        {
            XaxisForce = FinalTouchPosition.x - InitialTouchPosition.x;
            YaxisForce = FinalTouchPosition.y - InitialTouchPosition.y;
            ZaxisForce = (FinalTouchTime - InitialTouchTime);

            //RequireForce = new Vector3(XaxisForce / 7, YaxisForce / 7, ZaxisForce * 110f);
            ball.useGravity = true;
            ball.isKinematic = false;
            ball.AddForce(new Vector3(XaxisForce * 0.2f, YaxisForce * 0.1f, YaxisForce * 0.2f) * speed);
            ball.AddTorque(new Vector3(XaxisForce * 7.1f, YaxisForce * 7f, ZaxisForce * 4.3f) * speed);
            canSwipe = false;
            ball.transform.SetParent(imageTarget, true);
        }
    }
}
