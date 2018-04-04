using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// NodeScript should store all the interfaces needed for Unity to interact with our ASTNode
/// </summary>
public class NodeScript : MonoBehaviour {
    public ASTNode node;//stores ASTNode version of parent, partner, leftChild, and rightChild
    public GameObject gameObjectParent;
    public GameObject gameObjectPartner;
    public GameObject leftGameObjectChild;
    public GameObject rightGameObjectChild;

    public void Start()
    {
        GetComponent<Text>().text = node.getValue() + "";
    }
}

