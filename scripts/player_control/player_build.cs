using UnityEngine;
using System.Collections;

public class player_build : MonoBehaviour {
    public Transform meep;
    public GameObject widget;
    public GameObject building;
    public GameObject factory;
    public GameObject door;
    public GameObject workstation;
    private BoxCollider2D footprint;

    private Color goColor;
    private Color noColor;
    private int bWidth, bHeight;
    private bool bIsFixed, isSelecting;
    private Vector3 buildingStart, widgetScale, footprintInsideMin, footprintInsideMax;

    public int selectedBuildingID;

    public static bool isBuilding;

    private void Start()
    {
        footprint = widget.GetComponent<BoxCollider2D>();
        goColor = widget.GetComponent<SpriteRenderer>().color;
        noColor = goColor;
        noColor.r = 1;
        noColor.g = 0.1f;
        noColor.b = 0.1f;
        widget.SetActive(false);
        isSelecting = false;
    }

	public void makemeep()
    {
        Instantiate(meep, new Vector3(0,0,0), Quaternion.identity);
    }
    public void startPlacement(int buildID)
    {
        //we should pull the building info from a DB

        widget.SetActive(true);
        selectedBuildingID = buildID;

        //first, get the building size, properties - we'll fake it for now - 1x1 would be the default for variable-sized objects
        switch (buildID)
        {
            case 0:
                //generic factory
                bIsFixed = false;
                bWidth = 1;
                bHeight = 1;
                break;
            case 1:
                //generic door
                bIsFixed = true;
                bWidth = 1;
                bHeight = 1;
                break;
            case 2:
                //generic work station
                bIsFixed = true;
                bWidth = 3;
                bHeight = 2;
                break;
            default:
                isBuilding = false;
                break;
        }
        
        widget.transform.localScale = new Vector3(bWidth, bHeight, 0);
        widget.transform.rotation = Quaternion.Euler(0, 0, 0);

        isBuilding = true;
    }
    public void stopPlacement()
    {
        widget.SetActive(false);
        isSelecting = false;
        isBuilding = false;
    }
    private void Update()
    {

        if (!isBuilding)
        {
            stopPlacement();
            return;
        }
        //right click to cancel building mode
        if (Input.GetMouseButtonDown(1))
        {
            stopPlacement();
            return;
        }

        Vector3 widgetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        widgetPos.z = 0;
        widgetPos.x = Mathf.Round(widgetPos.x);
        widgetPos.y = Mathf.Round(widgetPos.y);
        if(bIsFixed && bWidth%2 == 0)
        {
            //placement on tiles requires width to be odd, otherwise we must adjust placement to match tiles - rotation swaps what factor needs to be adjusted
            if(widget.transform.rotation.z == 0 || widget.transform.rotation.z == 180)
            {
                widgetPos.x = widgetPos.x + 0.5f;
            }else
            {
                widgetPos.y = widgetPos.y + 0.5f;
            }
        }
        if (bIsFixed && bHeight % 2 == 0)
        {
            //same as above
            if (widget.transform.rotation.z == 0 || widget.transform.rotation.z == 180)
            {
                widgetPos.y = widgetPos.y + 0.5f;
            }
            else
            {
                widgetPos.x = widgetPos.x + 0.5f;
            }
        }

        Vector3 mousePos = widgetPos;

        //update the footprint location
        footprintInsideMin = new Vector3(footprint.bounds.min.x + 0.1f, footprint.bounds.min.y + 0.1f, 0.0f);
        footprintInsideMax = new Vector3(footprint.bounds.max.x - 0.1f, footprint.bounds.max.y - 0.1f, 0.0f);

        if (Input.GetKeyDown(KeyCode.R))
        {
            widget.transform.Rotate(0, 0, 90);
        }
        bool meetsMinSize = false;
        if (isSelecting)
        {
            float xDiff = (buildingStart.x - mousePos.x);
            float yDiff = (buildingStart.y - mousePos.y);
            float xOffset = -0.5f;
            float yOffset = -0.5f;
            if(buildingStart.x > mousePos.x)
            {
                xOffset = 0.5f;
            }
            if (buildingStart.y > mousePos.y)
            {
                yOffset = 0.5f;
            }
            widgetPos = new Vector3(buildingStart.x - xDiff/2 + xOffset, buildingStart.y - (yDiff / 2) + yOffset, 0.0f);
            
            

            widgetScale = new Vector3(Mathf.Abs(xDiff), Mathf.Abs(yDiff), 1);
            meetsMinSize = widgetScale.x > 2 && widgetScale.y > 2;
        }
        else
        {
            widgetScale = new Vector3(bWidth, bHeight, 1);
            meetsMinSize = true;
        }
        widget.transform.position = widgetPos;
        widget.transform.localScale = widgetScale;

        widget.GetComponent<SpriteRenderer>().color = goColor;
        
        bool canPlace = checkPlacement(selectedBuildingID) && meetsMinSize;
        if (!canPlace)
        {
            widget.GetComponent<SpriteRenderer>().color = noColor;
            return;
        }

        //let's place an object
        if (Input.GetMouseButtonDown(0))
        {
            if(!canPlace && bIsFixed)
            {
                //tried to place in a bad area
                //need feedback here to indicate failed attempt
                return;
            }

            if (bIsFixed)
            {
                //fixed size structure, place and be done - allow them to place multiple
                buildByID(widget.transform.position, widget.transform.localScale, selectedBuildingID);
                //isBuilding = false; - player will likely want to place multiple objects without selecting them over and over
                return;
            }else
            {
                //area select
                if (!isSelecting)
                {
                    buildingStart = widgetPos;
                }
                isSelecting = true;
            }
                

        }
        if(Input.GetMouseButtonUp(0))
        {
            //drag released, if we can build let's place
            if (!canPlace)
            {
                isBuilding = false;
                return;
            }
            if (isSelecting)
            {
                buildByID(widget.transform.position, widget.transform.localScale, selectedBuildingID);
                isBuilding = false;
            }
        }
        
    }
    
    void buildByID(Vector3 buildLocation, Vector3 buildScale, int buildingID)
    {
        //we'll take the buildingID here, and get a type
        if(buildingID == 0)
        {
            //generic factory type
            GameObject theBuilding = Instantiate(factory, new Vector3(widget.transform.position.x, widget.transform.position.y, 0.0f), widget.transform.rotation) as GameObject;
            theBuilding.transform.localScale = buildScale;
            theBuilding.GetComponent<building_erect>().buildingID = buildingID;
        }
        if(buildingID == 1)
        {
            //generic door type;
            GameObject theDoor = Instantiate(door, new Vector3(widget.transform.position.x, widget.transform.position.y, 0.0f), widget.transform.rotation) as GameObject;
            //doors are children of their building, and must rotate to match the wall they replace (and remove that wall)
            GameObject myWall = Physics2D.OverlapArea(footprintInsideMax, footprintInsideMin, LayerMask.GetMask("Obstacles")).gameObject;
            GameObject myBuilding = Physics2D.OverlapArea(footprint.bounds.max, footprint.bounds.min, LayerMask.GetMask("Buildings")).gameObject;
            theDoor.transform.rotation = myWall.transform.rotation;
            var wallBounds = myWall.GetComponent<Collider2D>().bounds;
            Destroy(myWall);
            AstarPath.active.UpdateGraphs(wallBounds, 1); // Second parameter is the delay
            theDoor.transform.parent = myBuilding.transform;
        }
        if (buildingID == 2)
        {
            //generic workstation type;
            GameObject theWS = Instantiate(workstation, new Vector3(widget.transform.position.x, widget.transform.position.y, 0.0f), widget.transform.rotation) as GameObject;
            //workstations must be placed on building floors, and are children of the building, they can be rotated to match the widget
            GameObject myBuilding = Physics2D.OverlapArea(footprint.bounds.max, footprint.bounds.min, LayerMask.GetMask("Buildings")).gameObject;//get building
            theWS.transform.rotation = widget.transform.rotation; //rotate to match widget
            //var WSBounds = myFloors.GetComponent<Collider2D>().bounds;
            //AstarPath.active.UpdateGraphs(WSBounds, 1); // Second parameter is the delay
            theWS.transform.parent = myBuilding.transform;
            theWS.GetComponent<workstation_erect>().workstationID = buildingID;
        }

    }
    private bool checkPlacement(int toPlaceID)
    {
        bool placeOK = false;
        //here we would get the "placement_type" property of the building by its ID
        if(toPlaceID == 0)
        {
            //we'll call this a "clean" placement type in the building properties when we create them later
            Collider2D hits = Physics2D.OverlapArea(footprint.bounds.max, footprint.bounds.min, LayerMask.GetMask("Obstacles"));
            if (hits == null)
            {
                placeOK = true;
            }
        }
        if(toPlaceID == 1)
        {
            //we'll call this "onWall" placement later, and get the desired rotation from the object properties
            Collider2D hits = Physics2D.OverlapArea(footprintInsideMax, footprintInsideMin, LayerMask.GetMask("Obstacles"));
            if (hits != null)
            {
                if(hits.gameObject.tag == "Walls")
                {
                    placeOK = true;
                }
            }
        }
        if (toPlaceID == 2)
        {
            //we'll call this "building" placement later
            Collider2D hits = Physics2D.OverlapArea(footprintInsideMax, footprintInsideMin, LayerMask.GetMask("Buildings"));
            if (hits != null)
            {
                if (hits.gameObject.tag == "Factory") //valid tag types will be pulled from the object
                {
                    //we need every tile to land on a floor
                    Collider2D[] FloorHits = Physics2D.OverlapAreaAll(footprintInsideMax, footprintInsideMin, LayerMask.GetMask("Floors"));
                    if (FloorHits.Length == 6)
                    {
                        placeOK = true;
                    }
                }
            }
        }


        return placeOK;
    }
}
