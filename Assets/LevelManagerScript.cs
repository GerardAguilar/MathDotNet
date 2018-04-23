using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManagerScript : MonoBehaviour {
    EnemyManagerScript enemyManagerScript;
    float start;
    //bool firstLevel;//Automatic
    bool secondLevel;
    bool thirdLevel;

	// Use this for initialization
	void Awake () {
        enemyManagerScript = GameObject.Find("EnemyManager").GetComponent<EnemyManagerScript>();
        start = Time.time;
        secondLevel = false;
        thirdLevel = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (!secondLevel && Time.time >= start + 10)
        {
            secondLevel = true;
            enemyManagerScript.RepopulateEnemies(2);
        }
        else if (!thirdLevel && Time.time >= start + 20) 
        {
            thirdLevel = true;
            enemyManagerScript.RepopulateEnemies(4);
        }
    }
}
