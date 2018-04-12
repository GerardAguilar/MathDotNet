using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// NodeScript should store all the interfaces needed for Unity to interact with our ASTNode
/// </summary>
public class NodeScript : MonoBehaviour {
    public ASTNode node;//stores ASTNode version of parent, partner, leftChild, and rightChild
    public int height;
    public string path;
    public GameObject gameObjectParent;
    public GameObject gameObjectPartner;
    public GameObject leftGameObjectChild;
    public GameObject rightGameObjectChild;
    public LineRenderer lineRenderer;

    //public void Awake()
    //{
    //    lineRenderer = GetComponentInChildren<LineRenderer>();
    //}

    public void Start()
    {
        if (!gameObject.name.Equals("Tree"))
        {
            GetComponent<Text>().text = node.getValue() + "";
        }
    }

    public void Shift(int steps)
    {
        //steps = steps * 200;
        Vector3 tempPosition = transform.position;
        transform.position = new Vector3(tempPosition.x + steps, tempPosition.y, tempPosition.z);
        Debug.Log("New position: " + transform.position.ToString());
    }

    public void Shift(int steps, GameObject tempObject) 
    {
        //steps = steps * 200;
        //Vector3 tempPosition = transform.position;
        //transform.position = new Vector3(tempPosition.x + steps, tempPosition.y, tempPosition.z);
        //if (gameObjectParent != null) 
        //{
        //    Shift(steps, tempObject.transform.parent.gameObject);
        //}        
        Shift(steps);
    }

    public void GenerateLine(Vector3 nodeParentLocation)
    {
        lineRenderer = GetComponentInChildren<LineRenderer>();

        //we're dealing with two parents, the node parent and the transform parent
        RectTransform tempRect = gameObject.GetComponent<RectTransform>();
        float transformParentHorizontalDistance = tempRect.anchoredPosition.x;
        float nodeParentHorizontalDistance = nodeParentLocation.x;
        float horizontalDistance = nodeParentHorizontalDistance - transformParentHorizontalDistance;

        float transformParentVerticalDistance = tempRect.anchoredPosition.y;    
        float nodeParentVerticalDistance = nodeParentLocation.y;
        float verticalDistance = nodeParentVerticalDistance - transformParentVerticalDistance;

        lineRenderer.SetPosition(0, new Vector3 (0f,0f,0f));
        lineRenderer.SetPosition(1, new Vector3 (horizontalDistance,verticalDistance,0f));
    }
}

