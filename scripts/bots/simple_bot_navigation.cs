using UnityEngine;
using System.Collections;
using Pathfinding;

public class simple_bot_navigation : MonoBehaviour {
    private AILerp myAI;
    public float followDistance = 1;


	// Use this for initialization
	void Start () {
        myAI = GetComponent<AILerp>();
        myAI.target = GameObject.FindWithTag("Player").transform;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        float targetDistance = Vector3.Distance(transform.position, myAI.target.position);
        if(targetDistance > followDistance)
        {
            myAI.canMove = true;
            myAI.canSearch = true;
        } else
        {
            myAI.canMove = false;
            myAI.canSearch = false;
        }
    }
}
