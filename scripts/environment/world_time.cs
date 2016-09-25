using UnityEngine;
using System.Collections;

public class world_time : MonoBehaviour {
    public int tickInterval;
    public int dayLength;

    public static float currentTime, sunStep;

    public GameObject worldLight;

    private int tick = 0;
    private Quaternion sunTarget;

    // Times:
    /*w
     * 0 - midnight
     * 0.5 noon
     */
    void Start () {
        currentTime = 0.5f; //noon
        sunStep = 360.0f / dayLength;
    }
	
	// Update is called once per frame
	void Update () {
        tick++;
        if(tick > tickInterval)
        {
            tick = 0;
            doTime();
        }
        worldLight.transform.rotation = Quaternion.Lerp(worldLight.transform.rotation, sunTarget, Time.deltaTime * sunStep);
    }
    private void doTime()
    {
        sunTarget = Quaternion.Euler(28, worldLight.transform.rotation.eulerAngles.y + sunStep, 0);
    }
}
