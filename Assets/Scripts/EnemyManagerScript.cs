using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManagerScript : MonoBehaviour {
    List<GameObject> enemies;

	// Use this for initialization
	void Start () {
		
	}

    void Awake()
    {
        RepopulateEnemies(1);
    }

    public void RepopulateEnemies(int enemyCount) {
        GameObject temp;
        enemies = new List<GameObject>();
        for (int i = 0; i < enemyCount; i++) 
        {
            temp = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Enemy"));
            temp.transform.position = gameObject.transform.position + new Vector3(i, 0, 0);
            enemies.Add(temp);
        }
    }

    public void RemoveEnemyFromList(GameObject temp) {
        enemies.Remove(temp);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
