using UnityEngine;
using System.Collections;
using Pathfinding;

public class building_erect : MonoBehaviour {
    public int buildingID = -1;
    public GameObject genericWall, genericFloor, genericCorner;

    private Vector3 CurrentPos;
    private GameObject newFloor, newWall, newCorner;
    private bool wallSpawned = false;
    // Use this for initialization
    void Start () {
        if(buildingID != -1)
            addComponents();
	}
	
    private void addComponents()
    {
        //place floors and walls
        float xStep = 1 / transform.localScale.x;
        float yStep = 1 / transform.localScale.y;
        float xOffset = xStep / 2;
        float yOffset = yStep / 2;
        for (int x = 0; x < transform.localScale.x; x++)
        {
            for (int y = 0; y < transform.localScale.y; y++)
            {
                Vector3 CurrentPos = new Vector3(0.5f - (x * xStep) - xOffset, 0.5f - (y * yStep) - yOffset);
                //spawn floor
                newFloor = Instantiate(genericFloor, new Vector3(0, 0, 0), transform.rotation) as GameObject;
                newFloor.transform.parent = transform;
                newFloor.transform.localPosition = CurrentPos;
                //spawn walls
                wallSpawned = false;
                if (x == 0 && y == 0)
                {
                    //TOP RIGHT
                    newCorner = Instantiate(genericCorner, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                    newCorner.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 270));
                    newCorner.transform.SetParent(transform);
                    newCorner.transform.localPosition = CurrentPos;
                    updatePathing(newCorner);
                    wallSpawned = true;
                }
                if (x == transform.localScale.x - 1 && y == 0 && !wallSpawned)
                {
                    //TOP LEFT
                    newCorner = Instantiate(genericCorner, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                    newCorner.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    newCorner.transform.parent = transform;
                    newCorner.transform.localPosition = CurrentPos;
                    updatePathing(newCorner);
                    wallSpawned = true;
                }
                if (x == transform.localScale.x - 1 && y == transform.localScale.y - 1 && !wallSpawned)
                {
                    //BOTTOM LEFT
                    newCorner = Instantiate(genericCorner, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                    newCorner.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                    newCorner.transform.parent = transform;
                    newCorner.transform.localPosition = CurrentPos;
                    updatePathing(newCorner);
                    wallSpawned = true;
                }
                if (x == 0 && y == transform.localScale.y - 1 && !wallSpawned)
                {
                    //BOTTOM RIGHT
                    newCorner = Instantiate(genericCorner, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                    newCorner.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
                    newCorner.transform.parent = transform;
                    newCorner.transform.localPosition = CurrentPos;
                    updatePathing(newCorner);
                    wallSpawned = true;
                }

                if ((y == 0 && !wallSpawned) || (x == 0 && !wallSpawned) || (x == transform.localScale.x - 1 && !wallSpawned) || (y == transform.localScale.y - 1 && !wallSpawned))
                {
                    //we still need a wall for all edges, but this is not a corner, let's determine rotation
                    float zRot = 0.0f; //rotation for top walls (y==0)
                    if (x == 0)
                        zRot = -90f;
                    if (x == transform.localScale.x - 1)
                        zRot = 90f;
                    if (y == transform.localScale.y - 1)
                        zRot = 180; 
                    newWall = Instantiate(genericWall, new Vector3(0, 0, 1), Quaternion.identity) as GameObject;
                    newWall.transform.rotation = Quaternion.Euler(new Vector3(0, 0, zRot));
                    newWall.transform.parent = transform;
                    newWall.transform.localPosition = CurrentPos;
                    updatePathing(newWall);
                    wallSpawned = true;
                }

            }
        }
    }
    private void updatePathing(GameObject newObj)
    {
        var PathObj = new GraphUpdateObject(newObj.GetComponent<Collider2D>().bounds);
        AstarPath.active.UpdateGraphs(PathObj);
        AstarPath.active.FloodFill();
    }
}
