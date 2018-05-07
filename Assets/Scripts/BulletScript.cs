using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {
    Vector3 faceDirection;
    Rigidbody rb;
    public int speed;
    GameObject nozzle;

	// Use this for initialization
	void Awake () {
        rb = GetComponent<Rigidbody>();
        speed = 100;
        nozzle = GameObject.Find("Nozzle");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void FixedUpdate()
    {
        rb.velocity = faceDirection * speed;
        
    }

    internal void SetDirection(Vector3 fd) {
        this.faceDirection = fd;
        
    }

    //below doesn't always get called
    private void OnEnable()
    {
        //transform.position = nozzle.transform.position;
        //transform.rotation = nozzle.transform.rotation;
    }

    private void OnDisable()
    {
        //reset position and velocity
        rb.velocity = Vector3.zero;
    }

    internal void SetInitialPosition(Vector3 position)
    {
        transform.position = position;
    }

    internal void SetRotation(Quaternion rotation)
    {
        transform.rotation = rotation;
    }
}
