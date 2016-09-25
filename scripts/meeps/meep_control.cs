using UnityEngine;
using System.Collections;

public class meep_control : MonoBehaviour {
    public static int meepCount;
    public static int currentMeepCount;
    public world_control worldControl;
    public GameObject assignedStation;

    private meep_stats myStats;
    
	// Use this for initialization
	void Awake () {
        assignedStation = null;
        myStats = GetComponent<meep_stats>();
        worldControl = GameObject.FindGameObjectWithTag("GameController").GetComponent<world_control>();
        meepCount++;
        currentMeepCount++;
        worldControl.meepPopulation = currentMeepCount;
        myStats.meepID = meepCount;
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void OnDestroy()
    {
        currentMeepCount--;
        if(worldControl != null)
            worldControl.GetComponent<world_control>().meepPopulation = currentMeepCount;
        if (assignedStation != null)
            assignedStation.GetComponent<station_control>().assignedMeep = null;
    }
    public void doWork()
    {
        if(assignedStation == null)
        {
            //this should never happen
            Debug.Log("Something went wrong, a meep was asked to do work but has no workstation");
        }
        //first we see if we've reached the location

        //here we would apply work stats from the meep directly to the station controller, applying work to the meep
    }
    public bool isWorkTime()
    {
        //we'd check work hours here
        return true;
    }
}
