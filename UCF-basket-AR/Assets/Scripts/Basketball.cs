using UnityEngine;


public class Basketball : MonoBehaviour {

    //private Vector3 InitialPosition;
    private GameController gameController;
    public float threshHold = 4;
    Transform cameraTransform;
    Vector3 offset;
    private Player player;
    public AudioSource swishSound, bounceSound, bellSound, DoneSound;

    private void Start()
    {
        this.GetComponent<Rigidbody>().useGravity = false;
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        //InitialPosition = this.transform.position;
        ResetPositionCamera();
        player = StartScreen.player;
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
        //gameController.initialTarget = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().net.transform.position;

        offset = new Vector3(0.5f + Random.Range(-threshHold, threshHold), -9f, 17.3f);
        cameraTransform = Camera.main.transform;

        transform.position = cameraTransform.position + cameraTransform.forward * 5f;
        transform.position = cameraTransform.position + offset;
        transform.SetParent(Camera.main.transform, true);
        transform.rotation = cameraTransform.rotation;

        this.GetComponent<Rigidbody>().useGravity = false;
        this.GetComponent<Rigidbody>().isKinematic = true;
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        gameController.canSwipe = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ring")
        {
            gameController.UpdateScore();
            swishSound.Play();
        }

        else if (other.gameObject.tag == "Border")
        {
            if (!player.GetGameDone())
            {
                ResetPositionCamera();
                bellSound.Play();
            }
            else if (player.GetGameDone())
            {
                //GameController.resultsPanel.SetActive(true);
                gameController.GameResults(player.GetPlayerId(), player.GetScore());
                DoneSound.Play();
            }
        }
        else
        {
            //gameController.ball.velocity = Vector3.zero;
            //gameController.ball.isKinematic = true;
            bounceSound.Play();
        }
    }
}
