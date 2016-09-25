using UnityEngine;
using System.Collections;

public class world_generator : MonoBehaviour {
    public GameObject worldObjects;

    public GameObject grass;
    public GameObject stone;
    public GameObject flora_rock;
    public int worldx_max;
    public int worldy_max;
    private GameObject newBlock, newFlora;
    private int layerLocation = 0; //prevents overlapping sprites from having artifacts 
	// Use this for initialization
	void Start () {
	    for(int x = -1 * worldx_max/2; x < worldx_max/2; x++)
        {
            for(int y = -1 * worldy_max / 2; y < worldy_max / 2; y++)
            {
                layerLocation -= 1;
                newBlock = Instantiate(stone, new Vector3(x, y, 0.0f), Quaternion.identity) as GameObject;
                newBlock.GetComponent<SpriteRenderer>().sortingOrder = layerLocation;
                placeFlora(newBlock);
                int myRot = Random.Range(1, 4);
                newBlock.transform.rotation = Quaternion.Euler(0, 0, 90 * myRot);
                newBlock.transform.parent = worldObjects.transform;
            }
        }
	}
	
	private void placeFlora(GameObject block)
    {
        //we would check what flora is available based on what type of tile we placed
        int placeChance = 15;
        if(Random.Range(0.0f, 1.0f) * 100 < placeChance)
        {
            float floraSize = Random.Range(0.3f, 0.5f);
            newFlora = Instantiate(flora_rock, new Vector3(block.transform.position.x + Random.Range(0.0f, 1.0f), block.transform.position.y + Random.Range(0.0f, 1.0f), 0.0f), Quaternion.identity) as GameObject;
            newFlora.transform.localScale = new Vector3(floraSize, floraSize, 1);
            newFlora.transform.parent = worldObjects.transform;
        }
    }
}
