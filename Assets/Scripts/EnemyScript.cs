using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour {
    NavMeshAgent agent;
    GameObject player;
    EnemyManagerScript enemyManagerScript;
    GameObject powerUp;
    Rigidbody rb;
    Player playerScript;
    float hitstun;
    float time;
    public int health;
    public Renderer rend;
    Color healthColor;

    // Use this for initialization
    void Awake () {
        powerUp = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/PowerUp"));
        powerUp.SetActive(false);
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");
        playerScript = player.GetComponent<Player>();
        agent.destination = player.transform.position;
        enemyManagerScript = GameObject.Find("EnemyManager").GetComponent<EnemyManagerScript>();
        rb = GetComponent<Rigidbody>();
        hitstun = 1f;
        health = 3;
        rend = GetComponent<Renderer>();
        rend.material.shader = Shader.Find("_Color");
        healthColor = new Color(.25f * health, .25f * health, .25f * health);
        rend.material.SetColor("_Color", healthColor);


    }
	
	// Update is called once per frame
	void Update () {
        //move randomly or in a pattern
        //hunt down the user when in range
        //agent.destination = player.transform.position;
        agent.destination = player.transform.position;
        if (Time.time > time + hitstun)
        {
            //start moving again
            
            agent.isStopped = false;
        }
        else
        {
            //don't move;
            agent.isStopped = true;
        }

        if (health <= 0) {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Projectile"))
        {
            AskForLoot();
            Hit();
            other.gameObject.SetActive(false);
        }
        else if (other.gameObject.tag.Equals("Sword")) {
            GetHit(other.transform.position, 2.5f);
            //GetHit(playerScript.faceDirection, 1f);
            //GetHit(playerScript.gameObject.transform.position, playerScript.faceDirection, 2.5f);
            time = Time.time;
            //gameObject.SetActive(false);
            Hit();
        }
    }

    private void AskForLoot()
    {
        if (Random.Range(0, 10) > 7)
        {
            powerUp.SetActive(true);
            powerUp.transform.position = gameObject.transform.position;
            enemyManagerScript.RemoveEnemyFromList(this.gameObject);
        }
    }

    private void GetHit(Vector3 source, float force) {
        //rb.AddForceAtPosition(force * gameObject.transform.forward, gameObject.transform.forward, ForceMode.Impulse);
        //Debug.Log("GetHit(Vector3 source, Vector3 direction, float force):\nsource:" + source.ToString() + "\ndirection:" + direction.ToString() + "\nforce:" + force.ToString());
        //rb.AddExplosionForce(10f, source, 10f);
    }

    private void Hit() {
        health--;
        healthColor = new Color(50f * health, 50f * health, 50f * health);
        rend.material.SetColor("_Color", healthColor);//does not work
        //rend.material.color = healthColor;
    }
}
