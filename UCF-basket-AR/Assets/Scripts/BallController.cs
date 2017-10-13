using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {

    public GameObject ball;
    //public float smoothSpeed = 0.125f;
    public Vector3 offset = new Vector3(0f, -3, +100);

    private void Start()
    {
    //    ball.transform.position = this.transform.position + offset;
    }
    void Update()
    {
        ball.transform.position = transform.position + new Vector3(x: 0f, y: -2.1f, z: 6.13f);
    }
}
