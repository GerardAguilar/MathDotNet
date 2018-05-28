using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeShardScript : MonoBehaviour {
    GameObject player;
    Rigidbody rb;
    float timeSinceStarted;
    float lerpStart;
    float lerpDuration;
    float percentageComplete;
    int random;
    public bool r;
    public bool g;
    public bool b;
    public bool w;
    SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Awake () {
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody>();
        lerpStart = Time.time;
        lerpDuration = 100f;

        r = false;
        g = false;
        b = false;
        w = false;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        
        random = Random.Range(0, 4);
        if (random == 0)
        {
            r = true;
            spriteRenderer.color = Color.red;
        }
        else if (random == 1)
        {
            g = true;
            spriteRenderer.color = Color.green;
        }
        else if (random == 2)
        {
            b = true;
            spriteRenderer.color = Color.blue;
        }
        else if (random == 3) {
            w = true;
            spriteRenderer.color = Color.white;
        }


    }
	
	// Update is called once per frame
	void FixedUpdate () {
        //rb.MovePosition(player.transform.position*Time.deltaTime);
        timeSinceStarted = Time.time - lerpStart;//Gives a baseline with lerpDuration being 100%
        percentageComplete = timeSinceStarted / lerpDuration;

        //start moving
        transform.position = Vector3.Lerp(transform.position, player.transform.position, percentageComplete);
    }

    
}
