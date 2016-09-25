using UnityEngine;
using System.Collections;

public class player_movement : MonoBehaviour {

    public float playerSpeed;
    public float runFactor = 1.5f;
    private Transform myTransform;
    private Rigidbody2D myBody;
    private Vector3 lastPos;

    // Use this for initialization
    void Start () {
	    myTransform = GetComponent<Transform>();
        myBody = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        bool isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        float playerVelocity = myBody.velocity.magnitude;
        float currentSpeed;
        if (isRunning)
        {
            currentSpeed = playerSpeed * runFactor;
        }
        else
        {
            currentSpeed = playerSpeed;
        }
        

        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");

        Vector3 pushPos = new Vector3(xInput, yInput, myTransform.position.z);
        if(playerVelocity > 0.05f)
        {
            Quaternion targetRotation = Quaternion.Euler(0, 0, (Mathf.Atan2((lastPos.y - myTransform.position.y), (lastPos.x - myTransform.position.x)) * Mathf.Rad2Deg) + 90);
            myTransform.rotation = Quaternion.Lerp(myTransform.rotation, targetRotation, Time.deltaTime * 15);
        }

        myBody.AddForce(pushPos * currentSpeed);

        lastPos = myTransform.position;
    }
}
