using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// NodeScript should store all the interfaces needed for Unity to interact with our ASTNode
/// </summary>
public class NodeScript : MonoBehaviour {
    public ASTNode node;//stores ASTNode version of parent, partner, leftChild, and rightChild
    public GameObject gameObjectParent;
    public GameObject gameObjectPartner;
    public GameObject leftGameObjectChild;
    public GameObject rightGameObjectChild;

}
