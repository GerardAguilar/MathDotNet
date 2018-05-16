using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmScript : MonoBehaviour {
    

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy")) {
            //Debug.Log("OnTriggerEnter");
            Rigidbody targetRigidbody = other.gameObject.GetComponent<Rigidbody>();
            targetRigidbody.AddExplosionForce(300f, transform.position, 10f);
        }
    }
}
