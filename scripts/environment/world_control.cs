using UnityEngine;
using System.Collections;

public class world_control : MonoBehaviour {
    public int meepPopulation = 0;

    private int step = 0;

	// Use this for initialization
	void Start () {
	    //here we would check if we're laoding, and then call that script
        //this should also call the actual world generator to load a world from file
	}
	
	// Update is called once per frame
	void Update () {
        step++;
        if(step > 100)
        {
            step = 0;
            //world stuff
        }
	}
}
