using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {

    public GameObject ball;
    public Vector3 offset;

    private void Start()
    {

    }
    void Update()
    {
        ball.transform.position = transform.position + offset;
    }
}
