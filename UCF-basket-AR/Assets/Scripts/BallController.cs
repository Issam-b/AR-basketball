using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {

    public GameObject ball;
    //public float smoothSpeed = 0.125f;
    public Vector3 offset = new Vector3(0, -3, 4.5f);

    private void LateUpdate()
    {
        ball.transform.position = transform.position + offset;
    }
}
