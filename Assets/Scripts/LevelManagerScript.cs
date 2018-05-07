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
    float waveInterval;

	// Use this for initialization
	void Awake () {
        enemyManagerScript = GameObject.Find("EnemyManager").GetComponent<EnemyManagerScript>();
        start = Time.time;
        secondLevel = false;
        thirdLevel = false;
        waveInterval = 6f;
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time > (start + waveInterval))
        {
            start = Time.time;
            enemyManagerScript.RepopulateEnemies(2);
        }
    }
}
