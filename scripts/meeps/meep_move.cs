using UnityEngine;
using System.Collections;
//Note this line, if it is left out, the script won't know that the class 'Path' exists and it will throw compiler errors
//This line should always be present at the top of scripts which use pathfinding
using Pathfinding;
using Pathfinding.Util;

public class meep_move : MonoBehaviour
{
    //The point to move to
    public Vector3 targetPosition;
    public bool newpath;
    public bool pathfinished;
    private Seeker seeker;
    //The calculated path
    public Path path;
    //The AI's speed per second
    public float speed = 100;
    //The max distance from the AI to a waypoint for it to continue to the next waypoint
    public float nextWaypointDistance = 3;
    //The waypoint we are currently moving towards
    private int currentWaypoint = 0;

    private Transform myTransform;
    private Rigidbody2D myBody;
    public meep_ai myAI;
    private Vector3 lastPos;

    public void Start()
    {
        myTransform = GetComponent<Transform>();
        seeker = GetComponent<Seeker>();
        myAI = GetComponent<meep_ai>();
        //Start a new path to the targetPosition, return the result to the OnPathComplete function
        
    }
    public void OnPathComplete(Path p)
    {
        pathfinished = false;
        if (!p.error)
        {
            path = p;
            //Reset the waypoint counter
            currentWaypoint = 0;
        }else
        {
            Debug.Log("Pathfinding Error: " + p.error);
        }
    }
    public void Update()
    {
        if (newpath)
        {
            GraphNode node_A = AstarPath.active.GetNearest(transform.position).node;
            GraphNode node_B = AstarPath.active.GetNearest(targetPosition).node;
            if (!canReach(targetPosition))
            {
                //these two nodes are not walkable, we need to notify the meep that this location is not reachable (and probably the player)
                myAI.failedToReach();
                newpath = false;
                return;
            }
            seeker.StartPath(transform.position, targetPosition, OnPathComplete);
            newpath = false;
            return;
        }
    }
    public void FixedUpdate() { 
        if (path == null)
        {
            //We have no path to move after yet
            return;
        }
        if (currentWaypoint >= path.vectorPath.Count)
        {
            //reached end of path
            if(!pathfinished)
                myAI.startTask(); //only do this the first time the task finishes
            pathfinished = true;
            return;
        }
        //Direction to the next waypoint
        transform.position = Vector3.MoveTowards(transform.position, path.vectorPath[currentWaypoint], speed * Time.deltaTime);
        //Check if we are close enough to the next waypoint
        //If we are, proceed to follow the next waypoint
        if (Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]) < nextWaypointDistance)
        {
            currentWaypoint++;
            return;
        }
        if (Vector3.Distance(lastPos, myTransform.position) > 0)
        {
            Quaternion targetRotation = Quaternion.Euler(0, 0, (Mathf.Atan2((lastPos.y - myTransform.position.y), (lastPos.x - myTransform.position.x)) * Mathf.Rad2Deg) - 180);
            myTransform.rotation = Quaternion.Lerp(myTransform.rotation, targetRotation, Time.deltaTime * 3);
        }
        lastPos = myTransform.position;

    }
    public bool canReach(Vector3 target)
    {
        GraphNode node_A = AstarPath.active.GetNearest(myTransform.position).node;
        GraphNode node_B = AstarPath.active.GetNearest(target).node;
        if (node_A.Area != node_B.Area)
        {
            //these two nodes are not walkable
            return false;
        }
        return true;
    }
}