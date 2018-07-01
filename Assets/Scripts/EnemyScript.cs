using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour {
    NavMeshAgent agent;
    GameObject player;
    EnemyManagerScript enemyManagerScript;
    GameObject powerUp;
    List<GameObject> attributeShardList;
    GameObject attributeShard;
    GameObject environment;
    Rigidbody rb;
    Player playerScript;
    float hitstun;
    float time;
    public int health;
    public Renderer rend;
    Color healthColor;
    SoundManager soundManager;

    // Use this for initialization
    void Awake () {
        powerUp = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/PowerUp"));
        powerUp.SetActive(false);
        environment = GameObject.Find("Environment");
        attributeShardList = new List<GameObject>();
        for (int i = 0; i < 10; i++) {
            attributeShard = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/AttributeShard"));
            attributeShard.transform.SetParent(this.transform);
            attributeShardList.Add(attributeShard);
            attributeShard.SetActive(false);
        }
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
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();

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
            AskForAttributeShards(other.ClosestPoint(player.transform.position));
            Hit();
            other.gameObject.SetActive(false);
            soundManager.PlaySound(5);
        }
        else if (other.gameObject.tag.Equals("Sword")) {
            //GetHit(other.transform.position, 2.5f);
            //AskForAttributeShards(other.ClosestPoint(player.transform.position));
            ////GetHit(playerScript.faceDirection, 1f);
            ////GetHit(playerScript.gameObject.transform.position, playerScript.faceDirection, 2.5f);
            //time = Time.time;
            ////gameObject.SetActive(false);
            //Hit();
            //soundManager.PlaySound(1);
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

    private void AskForAttributeShards(Vector3 impactSource ) {
        int random = Random.Range(0, 10);
        for (int i = 0; i < random; i++) {
            GetInactiveAttributeShardFromList(impactSource);
        }
    }

    private GameObject GetInactiveAttributeShardFromList(Vector3 impactSource) {
        GameObject attributeShard = null; ;
        for (int i = 0; i < attributeShardList.Count; i++) {
            if (!attributeShardList[i].activeInHierarchy) {
                attributeShardList[i].transform.position = transform.position;
                attributeShardList[i].SetActive(true);
                attributeShard = attributeShardList[i];                
            }
        }
        if (attributeShard != null) {
            attributeShard.transform.position = this.transform.position;
            attributeShard.transform.SetParent(environment.transform);
            //attributeShard.GetComponent<Rigidbody>().AddForce(impactSource, ForceMode.Impulse);
        }
        return attributeShard;
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
