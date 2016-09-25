using UnityEngine;
using System.Collections;
using Pathfinding;

public class meep_ai : MonoBehaviour {
    private meep_move moveme;
    private meep_stats myStats;
    private meep_control myController;
    private Transform myTransform;

    public float hunger;
    public float energy;

    public float hungerThreshold = 20.0f;
    public float energyThreshold = 1.0f;

    public float metabolism;

    public string task = "";

    private bool onTask = false;
    private int step;

	// Use this for initialization
	void Start () {
        //grab references
        myTransform = GetComponent<Transform>();
        moveme = GetComponent<meep_move>();
        myStats = GetComponent<meep_stats>();
        myController = GetComponent<meep_control>();
	}
	
	void Update () {
        step++;
        if (step < 20)
            return; //no need to make decisions EVERY frame
        step = 0;
        if (!myStats.isAble())
            return; //this meep is incapacitated and either needs to rest/heal or be rescued

        if (myStats.requireFood())
        {
            //this meep needs to eat - starving can kill meeps so this should be a priority if the meep is at a critical hunger level

        }
        if(myController.assignedStation != null && myController.isWorkTime())
        {
            //try to work
            if (myStats.canWork())
            {
                myController.doWork();
            }
        }

        //check if we need a task, if so, set one - we can roll this out to a function later
        if (task == "")
        {
            //doing nothing, figure out the best thing to do
            if(hunger > hungerThreshold - 5.0f)
            {
                //pretty hungry and nothing to do, go eat
                task = "FoodStation";
            }
            else if(energy < energyThreshold * 3)
            {
                //will need energy soo, rest
                task = "EntertainmentStation";
            }
            else
            {
                //feeling good, go to work
                task = "WorkStation";
            }
            findNearbyTask(task);
            onTask = false;
        }

        if (onTask)
        {
            doTask();
        }
        


    }
    
    public void startTask()
    {
        if(task == "work")
        {
            task = "working";
        }
        if (task == "rest")
        {
            task = "resting";
        }
        if (task == "eat")
        {
            task = "eating";
        }
        onTask = true;
    }

    public void doTask()
    {
        switch (task)
        {
            case "working":
                energy -= 0.05f;
                if(energy < energyThreshold)
                {
                    task = "";
                    onTask = false;
                }
                if(hunger > hungerThreshold)
                {
                    task = "";
                    onTask = false;
                }
                break;
            case "resting":
                energy += 0.1f;
                if (energy > 100)
                {
                    task = "";
                    onTask = false;
                }
                if (hunger > hungerThreshold)
                {
                    task = "";
                    onTask = false;
                }
                break;
            case "eating":
                hunger -= 0.5f;
                if (hunger <= 0)
                {
                    task = "";
                    onTask = false;
                }
                break;
            default:
                task = "";
                onTask = false;
                break;
        }
    }
    private void findNearbyTask(string taskTag)
    {
        Vector3 taskLocation;
        GameObject[] locations = GameObject.FindGameObjectsWithTag(taskTag);
        bool foundTask = false;
        if (locations.Length == 0)
        {
            //for some reason, no location was found
            failedToReach();
            return;
        }
        taskLocation = new Vector3(10000, 10000, 10000);
        foreach (GameObject taskStation in locations)
        {
            if(Vector3.Distance(myTransform.position, taskStation.transform.position) < Vector3.Distance(myTransform.position, taskLocation))
            {
                if (moveme.canReach(taskStation.transform.position))
                {
                    taskLocation = taskStation.transform.position;
                    foundTask = true;
                }      
            }
        }
        if (foundTask)
        {
            //looks like we dound a reasonable task
            moveme.targetPosition = taskLocation;
            moveme.newpath = true;
        }else
        {
            //no task was found
            taskNotFound(taskTag);
        }
        
    }
    public void failedToReach()
    {
        //this function is called if the meep was unable to reach their destination

        //if they were headed to work, we should clear their work station and notify the player

        //if it was another type of task, we should mark that location as unreachable, and try to find another
    }
    public void taskNotFound(string taskTag)
    {
        //this function is called if they tried to do a task, but didn't find a station

        //we should notify the player that the meep wants to do something and can't

        //we should update the meep's stats with whatever effect this would cause (lower attitude, happiness, etc...)

        //we should mark that location as unreachable, and try to find another
    }
}
