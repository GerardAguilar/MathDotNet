using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpScript : MonoBehaviour {
    GameObject manager;
    Parser parserScript;
    public string equation;

	// Use this for initialization
	void Awake () {
        manager = GameObject.Find("Manager");
        parserScript = manager.GetComponent<Parser>();
        //SetupEquation("1 + 2 * 3 - 4 / 5");
        SetupRandomEquation();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetupEquation(string eqn) {
        this.equation = eqn;
    }

    public void SetupRandomEquation() {
        //number of terms
        //number of ops
        //parenthesis?
        int termMaxCount = 5;
        int termMinCount = 2;
        int termCount = Random.Range(termMaxCount, termMinCount);
        int term;
        int opsCount = termCount - 1;
        string eqn = "";

        for (int i = 0; i < termCount+opsCount; i++){
            //if even
            if (i % 2 == 0)
            {
                term = Random.Range(1, 99);
                eqn = eqn + term + " ";
            }
            //if odd
            else {
                term = Random.Range(0, 4);
                switch (term) {
                    case 0:
                        eqn = eqn + "+ ";
                        break;
                    case 1:
                        eqn = eqn + "- ";
                        break;
                    case 2:
                        eqn = eqn + "* ";
                        break;
                    case 3:
                        eqn = eqn + "/ ";
                        break;
                    case 4:
                        eqn = eqn + "^ ";
                        break;
                    default:
                        eqn = eqn + "+ ";
                        break;
                }

            }
        }
        SetupEquation(eqn);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) {
            Debug.Log("Player entered trigger");
            parserScript.Initialize(equation);
            gameObject.SetActive(false);
        }
    }
}
