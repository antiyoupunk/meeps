using UnityEngine;
using System.Collections;

public class camera_control : MonoBehaviour {
    Camera theCam;
    Vector3 targetPosition;

	// Use this for initialization
	void Start () {
        theCam = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
        targetPosition = transform.position;
        targetPosition.z = -10.0f;
        theCam.transform.position = Vector3.Lerp(theCam.transform.position, targetPosition, Time.deltaTime);
        if(Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - 0.5f, 4, 15);
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize + 0.5f, 4, 15);
        }
    }
}
