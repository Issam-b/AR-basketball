using UnityEngine;


public class Basketball : MonoBehaviour {

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
        ResetPositionCamera();
        player = StartScreen.player;
    }

    // reset the ball position relative to the AR camera
    public void ResetPositionCamera()
    {
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
        // collision handling and sound triggering of the ball with other objects
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
                gameController.GameResults(player.GetPlayerId(), player.GetScore());
                DoneSound.Play();
            }
        }
        else
        {
            bounceSound.Play();
        }
    }
}
