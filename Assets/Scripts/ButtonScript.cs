using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour {
    NodeScript nodeScript;

    void Awake() {
        
    }
    
    //TODO: Need to dynamically set up the button click
    //public void Solve() {
    //    nodeScript = gameObject.GetComponent<NodeScript>();
    //    Debug.Log("nodeScript.Solve()");
    //    if (nodeScript.CheckIfOperation()) {
    //        if (nodeScript.CheckIfBothLeaves()) {
    //            Debug.Log("nodeScript.Solve() = " + nodeScript.Solve());
    //            nodeScript.node.setValue(nodeScript.Solve()[0]);
    //        }
    //    }
    //}
}
