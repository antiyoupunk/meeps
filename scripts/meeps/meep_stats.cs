using UnityEngine;
using System.Collections;

public class meep_stats : MonoBehaviour {
    //MeepIDs
    public string meepName;
    public int meepID;

    //Vital Stats
    public int health;      //if it reaches 0, you're fucked
    public int power;       //like mana, only less gay

    //Basic Needs
    public float hunger;    //decreases over time and as work is performed, refilled by eating
    public float fun;       //decreases through work, refilled through activities and socializing
    public float energy;    //decreases over time and as work is performed, refilled by resting/sleeping
    public float social;    //decreases over time, improves through social activities, reduces less when working with others (multi-meep workstations)
    public float safety;    //based on surroundings, will creep towards area safety at a rate based on bravery and loyalty/attitude - can drop by chunks when attacked
    public float esteem;    //Completing tasks improves esteem, all tasks require esteem to start - also improved by participating in group events (protests, etc...);

    //Basic Stats
    public int vitality;
    public int intelligence;
    public int charisma;
    public int agility;
    public int focus;
    public int loyalty;
    public int bravery;
    public int perception;

    //Derived Invisible Stats
    public float metabolism;        //0.01 to 0.1 - determines how often food is required
    public float effectiveness;     //0 to 1 - determines how much work is achieved
    public float productivity;      //0 to 1 - factors into effectiveness
    public float happy;             //0 to 100 - general happiness of the meep
    public int attitude;            //0 to 100 - determines how much the meep respects the player and how much esteem is used up to perform duties
    public int morale;              //0 to 100 - determines how likely the meep is to stand and fight (if that's what it's supposed to do)
    public int psycho;               //0 to 100 - determines how crazy the meep is, 0 would be monk, 100 would be feminist

    public bool loadme = false;
    public bool isWorking = false;
    public bool isSleeping = false;
    private int step = 0;

    private Object myController;
    private Object myAI;

    private string[] names_first = { "Bob", "Fred", "Frank", "Jim", "Hunter", "Mick", "John", "Brody", "Dylan", "Postulous", "'Wimpy'", "Vito" };
    private string[] names_last = { "O'Doyle", "Fredrickson", "McKalister", "Grandy", "Corleon", "Speer", "Smith", "Franklin"};

	// Use this for initialization
	void Start () {
        meepName = names_first[Random.Range(0, names_first.Length)] + " " + names_last[Random.Range(0, names_last.Length)];
        myController = transform.GetComponent<meep_control>();
        myAI = transform.GetComponent<meep_ai>();
        if (loadme)
        {
            //do the loading of the meep stuff, world control spawns me
        }else
        {
            //newly spawned meep - set me up
            health = 100;
            power = 0;

            hunger = 100;
            fun = 100;
            energy = 100;
            social = 100;
            safety = 100;
            esteem = 100;

            vitality = Random.Range(5, 11);
            intelligence = Random.Range(5, 11);
            charisma = Random.Range(5, 11);
            agility = Random.Range(5, 11);
            focus = Random.Range(5, 11);
            loyalty = Random.Range(5, 11);
            bravery = Random.Range(5, 11);
            perception = Random.Range(5, 11);
        }
	}
	
	// Update is called once per frame
	void Update () {
        step++;
        if(step > 20)
        {
            step = 0;
            //set base stats
            metabolism = vitality / 100.0f; //more vitality == higher metabolism
            effectiveness = ((intelligence + agility + focus) / 30.0f) * (esteem / 100.0f);
            productivity = 1;      
            attitude = 100;            
            morale = 100;              
            psycho = 0;               

            if (isWorking)
            {
                metabolism = metabolism * 1.5f; //more food is required while working
            }
            if (isSleeping)
            {
                metabolism = .1f; //metabolism greatly slowed while sleeping;
                energy = energy + vitality / 10.0f;
            }
            

            //update needs
            hunger -= metabolism;
            energy -= metabolism/3.0f;
        }
    }
    public bool canWork()
    {
        //check starvation

        //check exhaustion

        //check attitude and psycho

        //check if we're in a panic for some reason

        return true;
    }
    public bool requireFood()
    {
        //check if we just woke up, always eat first thing
        return hunger < 10; //buffer for requiring food to prevent meeps far away from food from starving... hopefully
    }
    public bool isAble()
    {
        if (energy < 0)
            return false; //this meep collapsed from exhaustion, sad
        //here we would check vital stats like health, or check for conditions
        return true;
    }
}
