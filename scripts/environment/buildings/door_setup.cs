using UnityEngine;
using System.Collections;

public class door_setup : MonoBehaviour {
    BoxCollider2D myTrigger;
    Animator myAnimation;
	// Use this for initialization
	void Start () {
        myTrigger = transform.GetComponent<BoxCollider2D>();
        myAnimation = transform.GetComponent<Animator>();
        //update trigger size
        myTrigger.size = new Vector2(1.5f, 2.0f);
        myTrigger.offset = new Vector2(0, .25f);
	}
	
	// Update is called once per frame
	void Update () {
        Collider2D MeepHit = Physics2D.OverlapArea(myTrigger.bounds.max, myTrigger.bounds.min, LayerMask.GetMask("Meeps"));
        Collider2D PlayerHit = Physics2D.OverlapArea(myTrigger.bounds.max, myTrigger.bounds.min, LayerMask.GetMask("Player"));

        if(MeepHit == null && PlayerHit == null)
        {
            //no meeps or players
            myAnimation.SetBool("closed", true);
        }else
        {
            //someone standing nearby
            myAnimation.SetBool("closed", false);
        }
    }
}
