using UnityEngine;
using System.Collections;

public class randomize_sprite : MonoBehaviour {
    public Sprite[] sprites;

    private SpriteRenderer spriteRenderer;
	// Use this for initialization
	void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        int myVal = Random.Range(0, sprites.Length);
        spriteRenderer.sprite = sprites[myVal];
        myVal = Random.Range(0, 4);
	}
}
