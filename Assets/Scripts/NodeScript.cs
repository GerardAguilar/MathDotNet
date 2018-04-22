using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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
    Text myText;

    //public void Awake()
    //{
    //    lineRenderer = GetComponentInChildren<LineRenderer>();
    //}

    public void Awake()
    {
        //GetComponentInChildren<Text>().text = node.getValue() + "";
        myText = GetComponent<Text>();
    }

    public void Update()
    {
        if (!gameObject.name.Equals("Tree1") && !gameObject.name.Equals("Tree2")) 
        {
            myText.text = node.getValue() + "";
        }
    }

    public void DisableLeaves() {
        this.node.astNodeLeftChild = null;
        this.node.astNodeRightChild = null;
        this.leftGameObjectChild.SetActive(false);
        this.rightGameObjectChild.SetActive(false);
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

    public bool CheckIfOperation() 
    {
        
        string character = node.getValue();
        bool itsAnOp = false;
        switch (character) {
            case "+":
                itsAnOp = true;
                break;
            case "-":
                itsAnOp = true;
                break;
            case "*":
                itsAnOp = true;
                break;
            case "/":
                itsAnOp = true;
                break;            
            case "^":
                itsAnOp = true;
                break;
            default:
                itsAnOp = false;
                break;
        }

        return itsAnOp;
    }

    public string GetOperation() {
        if (CheckIfOperation())
        {
            return node.getValue();
        }
        else {
            return null;
        }
        
    }

    public bool CheckIfBothLeaves() 
    {
        ASTNode leftChild = node.getLeftASTNode();
        ASTNode rightChild = node.getRightASTNode();

        ASTNode leftsLeftChild = leftChild.getLeftASTNode();
        ASTNode leftsRightChild = leftChild.getRightASTNode();
        ASTNode rightsLeftChild = rightChild.getLeftASTNode();
        ASTNode rightsRightChild = rightChild.getRightASTNode();

        bool bothAreLeaves = false;

        if (leftsLeftChild == null
            && leftsRightChild == null
            && rightsLeftChild == null
            && rightsRightChild == null) 
        {
            bothAreLeaves = true;
        }

        return bothAreLeaves;
        
    }

    public void Solve()
    {
        Console.WriteLine("nodeScript.Solve()");
        if (this.CheckIfOperation())
        {
            if (this.CheckIfBothLeaves())
            {
                ASTNode leftChild = node.getLeftASTNode();
                ASTNode rightChild = node.getRightASTNode();
                string temp = this.RunOperation();
                Debug.Log("nodeScript.Solve() = " + temp);
                this.node.setValue(temp);
                this.DisableLeaves();
                //this.leftGameObjectChild.SetActive(false);
                //this.rightGameObjectChild.SetActive(false);
            }
        }
        EventSystem.current.SetSelectedGameObject(null);
    }

    //TODO: need to account for long formatted floats and values like 1/3
    //TODO: Need to change nodeScript/ASTNode value as string, not char
    public string RunOperation() {
        string solution = "<empty>";
        string leftVal = node.getLeftASTNode().getValue() +"";
        string rightVal = node.getRightASTNode().getValue() +"";
        string op = node.getValue();

        switch (op) {
            case "+":
                solution = (float.Parse(leftVal) + float.Parse(rightVal)) + "";
                break;
            case "-":
                solution = (float.Parse(leftVal) - float.Parse(rightVal)) + "";
                break;
            case "*":
                solution = (float.Parse(leftVal) * float.Parse(rightVal)) + "";
                break;
            case "/":
                solution = (float.Parse(leftVal) / float.Parse(rightVal)) + "";
                break;
            case "^":
                solution = Mathf.Pow(float.Parse(leftVal), float.Parse(rightVal)) + "";
                break;
        }

        return solution;
    }
}

