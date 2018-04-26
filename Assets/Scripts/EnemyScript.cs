using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour {
    NavMeshAgent agent;
    GameObject player;
    EnemyManagerScript enemyManagerScript;
    GameObject powerUp;

    // Use this for initialization
    void Awake () {
        powerUp = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/PowerUp"));
        powerUp.SetActive(false);
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");
        agent.destination = player.transform.position;
        enemyManagerScript = GameObject.Find("EnemyManager").GetComponent<EnemyManagerScript>();
	}
	
	// Update is called once per frame
	void Update () {
        //move randomly or in a pattern
        //hunt down the user when in range
        agent.destination = player.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Projectile")) {
            AskForLoot();
            gameObject.SetActive(false);
            other.gameObject.SetActive(false);
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
}
