using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            other.gameObject.SetActive(false);
        }
        else if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Enemy")) 
        {
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            rb.velocity = new Vector3(0f, 0f, 0f);
        }
    }
}
