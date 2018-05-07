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
    public bool r;
    public bool g;
    public bool b;
    public bool w;
    public int rValue;
    public int gValue;
    public int bValue;
    public int wValue;


    public SpriteRenderer redSprite;
    public SpriteRenderer greenSprite;
    public SpriteRenderer blueSprite;
    public SpriteRenderer whiteSprite;

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

        if (!gameObject.name.Equals("Tree1")) {
            //Debug.Log(transform.GetChild(0).transform.childCount);
            //Debug.Log(transform.GetChild(0).GetChild(0).name);
            //Debug.Log(transform.GetChild(0).GetChild(1).name);
            //Debug.Log(transform.GetChild(0).GetChild(2).name);
            //Debug.Log(transform.GetChild(0).GetChild(3).name);

            redSprite = transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
            greenSprite = transform.GetChild(0).GetChild(1).GetComponent<SpriteRenderer>();
            blueSprite = transform.GetChild(0).GetChild(2).GetComponent<SpriteRenderer>();
            whiteSprite = transform.GetChild(0).GetChild(3).GetComponent<SpriteRenderer>();

            redSprite.color = new Color(redSprite.color.r, redSprite.color.g, redSprite.color.b, 0f);
            greenSprite.color = new Color(greenSprite.color.r, greenSprite.color.g, greenSprite.color.b, 0f);
            blueSprite.color = new Color(blueSprite.color.r, blueSprite.color.g, blueSprite.color.b, 0f);
            whiteSprite.color = new Color(whiteSprite.color.r, whiteSprite.color.g, whiteSprite.color.b, 0f);

            rValue = 0;
            gValue = 0;
            bValue = 0;
            wValue = 0;
            

            RandomizeColors();
            ColorSprites();


        }
    }

    public void Update()
    {
        if (!gameObject.name.Equals("Tree1") && !gameObject.name.Equals("Tree2")) 
        {
            myText.text = node.getValue() + "";
        }
    }

    void RandomizeColors() {
        //designate attribute shards, with a always on
        int rand;
        rand = UnityEngine.Random.Range(0, 3);
        if (rand == 0)
        {
            r = true;
            rValue = 2;
        }
        rand = UnityEngine.Random.Range(0, 3);
        if (rand == 1)
        {
            g = true;
            gValue = 2;
        }
        rand = UnityEngine.Random.Range(0, 3);
        if (rand == 2)
        {
            b = true;
            bValue = 2;
        }
        //rand = UnityEngine.Random.Range(0, 3);
        //if (rand == 3)
        //{
        //    a = true;
        //}
        w = true;
        wValue = 2;
    }

    public bool AreAllColorValuesZeroOrLessOfMyChildren() {
        bool allZeroOrLess = false;
        NodeScript left = leftGameObjectChild.GetComponent<NodeScript>();
        NodeScript right = rightGameObjectChild.GetComponent<NodeScript>();

        if (left.AreAllColorValuesZeroOrLess() && right.AreAllColorValuesZeroOrLess()) {
            allZeroOrLess = true;
        }
        return allZeroOrLess;
    }

    public bool AreAllColorValuesZeroOrLess() {
        bool allZeroOrLess = false;
        if ((rValue <= 0) && (gValue <= 0) && (bValue <= 0) && (wValue <= 0))
        {
            allZeroOrLess = true;
        }
        return allZeroOrLess;
    }

    public void DecreaseColorValues(bool rBool, bool gBool, bool bBool, bool wBool) {
        if (rBool) {
            rValue = rValue - 1;
        }
        if (gBool) {
            gValue = gValue - 1;
        }
        if (bBool) {
            bValue = bValue - 1;
        }
        if (wBool) {
            wValue = wValue - 1;
        }        
    }

    public void ColorSprites() {

        if (r)
        {
            redSprite.color = new Color(redSprite.color.r, redSprite.color.g, redSprite.color.b, rValue * 50f);
        }

        if (g)
        {
            greenSprite.color = new Color(greenSprite.color.r, greenSprite.color.g, greenSprite.color.b, gValue * 50f);
        }

        if (b)
        {
            blueSprite.color = new Color(blueSprite.color.r, blueSprite.color.g, blueSprite.color.b, bValue * 50f);
        }

        if (w)
        {
            whiteSprite.color = new Color(whiteSprite.color.r, whiteSprite.color.g, whiteSprite.color.b, wValue * 50f);
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

    public bool CheckIfNoChildren() {
        bool noChildren = false;
        ASTNode leftChild = node.getLeftASTNode();
        ASTNode rightChild = node.getRightASTNode();

        if (leftChild == null && rightChild == null) {
            noChildren = true;
        }
        return noChildren;

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
                //call up the operation storer in Player.cs
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

