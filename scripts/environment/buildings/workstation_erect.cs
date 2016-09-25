using UnityEngine;
using System.Collections;

public class workstation_erect : MonoBehaviour
{
    public int workstationID = -1;
    public GameObject genericStation;
    // Use this for initialization
    void Start()
    {
        if (workstationID != -1)
            addComponents();
    }
    private void addComponents()
    {
        //we would build a for each based on building attributes here to create each station in the workstation
        GameObject theStation = Instantiate(genericStation, Vector3.zero, Quaternion.identity) as GameObject;
        theStation.transform.parent = transform;
        theStation.transform.localPosition = new Vector3(-1.05f, 0.2f, 0.0f);
        theStation.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        //second spot
        theStation = Instantiate(genericStation, Vector3.zero, Quaternion.identity) as GameObject;
        theStation.transform.parent = transform;
        theStation.transform.localPosition = new Vector3(-0.1f, -0.6f, 0.0f);
        theStation.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
    }
}
