using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;


//code is inaccurate, but functional
//[InitializeOnLoad]
public class Parser : MonoBehaviour
{
    public string initialInput = "0";
    //public string input2 = "Hello too";
    public List<GameObject> nodeObjects = new List<GameObject>();
    public int gameObjectTreeHeight = 0;
    public int gameObjectTreeHeight2 = 0;
    GameObject player;


    void Awake()
    {
        player = GameObject.Find("Player");
        Initialize(initialInput);        
    }

    public void Initialize(string input) {
        player.GetComponent<Player>().UpdateParserScript();
        ClearTree();
        Debug.Log("input = " + input);//3 + 4 * 2 / (1 - 5) ^ 2 ^ 3
        /*
         3 + 4 * 2 / (1 - 5) ^ 2 ^ 3
           +       
         3          
                   /         
               *             ^
             4   2   (  -  )     ^  
                      1   5    2   3
         */
        ICollection<IMyOperator> operators = new List<IMyOperator>();
        operators.Add(new MyBaseOperator('^', true, 4));
        operators.Add(new MyBaseOperator('*', false, 3));
        operators.Add(new MyBaseOperator('/', false, 3));
        operators.Add(new MyBaseOperator('+', false, 2));
        operators.Add(new MyBaseOperator('-', false, 2));

        ShuntingYardParser parser = new ShuntingYardParser(operators);

        ASTNode parseTree = parser.ConvertInfixNotationToAST(input);
        //Debug.Log(parseTree.getValue());
        GameObject treeTop = GameObject.Find("Tree1");

        //parser.DrawAST(parseTree, treeTop, false, false);
        parser.designateHierarchy(parseTree, false, false);
        //parser.DrawAST2(parseTree, treeTop, 0);
        parser.DrawAST3(parseTree, treeTop, "Tree1", nodeObjects, 0, 0, "0");
        //parser.DrawASTTemplate(5, treeTop, nodeObjects, true, true);
        gameObjectTreeHeight = parser.GetHeight(nodeObjects);
        parser.DrawASTTemplate2(nodeObjects, gameObjectTreeHeight);        
    }

    public NodeScript FindFirstNodeWithOnlyLeaves(string op)
    {
        NodeScript ns = null;
        for (int i = 0; i < nodeObjects.Count; i++)
        {
            ns = nodeObjects[i].GetComponent<NodeScript>();
            if (ns.CheckIfOperation() && ns.CheckIfBothLeaves())
            {
                if (ns.GetOperation().Equals(op))
                {
                    //should return ns above
                    break;
                }
            }
            //nullifies ns on each pass
            ns = null;
        }
        return ns;
    }

    public void UpdateLeaves(bool r, bool g, bool b, bool w) {
        List<NodeScript> list = new List<NodeScript>();
        list = FindLeaves();
        for (int i = 0; i < list.Count; i++) {
            list[i].DecreaseColorValues(r, g, b, w);
            list[i].ColorSprites();
        }
    }

    public List<NodeScript> FindLeaves() {
        List<NodeScript> list = new List<NodeScript>();
        for (int i = 0; i < nodeObjects.Count; i++) {
            NodeScript ns = nodeObjects[i].GetComponent<NodeScript>();
            if (ns.CheckIfOperation() && ns.CheckIfBothLeaves()) {
                NodeScript left = ns.leftGameObjectChild.GetComponent<NodeScript>();
                NodeScript right = ns.rightGameObjectChild.GetComponent<NodeScript>();
                list.Add(left);
                list.Add(right);
            }
        }

        return list;
    }


    public void ClearTree()
    {
        if (nodeObjects.Count > 0) {
            for (int i = nodeObjects.Count - 1; i >= 0; i--)
            {
                //Destroy(nodeObjects[i]);
                Debug.Log("Disabling: nodeObjects[" + i + "]");
                nodeObjects[i].SetActive(false);
            }
        }
    }
}

//from https://www.klittlepage.com/2013/12/22/twelve-days-2013-shunting-yard-algorithm/
//modified for C#
public interface IMyOperator
{
    bool IsRightAssociative();
    int ComparePrecedence(IMyOperator o);
    char GetSymbol();
}
public class MyBaseOperator : IMyOperator
{
    public char symbol;
    public bool rightAssociative;
    public int precedence;

    public MyBaseOperator(char symbol, bool rightAssociative, int precedence)
    {
        this.symbol = symbol;
        this.rightAssociative = rightAssociative;
        this.precedence = precedence;
    }

    public bool IsRightAssociative()
    {
        return rightAssociative;
    }


    public int ComparePrecedence(IMyOperator o)
    {
        bool result = (o is MyBaseOperator);
        int tempPrecedence;
        if (result)
        {
            MyBaseOperator other = (MyBaseOperator)o;
            if (precedence > other.precedence)
            {
                tempPrecedence = 1;
            }
            else if (other.precedence == precedence)
            {
                tempPrecedence = 0;
            }
            else
            {
                tempPrecedence = -1;
            }
            return tempPrecedence;
            //since I'm not used to the bottom structure, I unfolded it above.
            //return precedence > other.precedence ? 1 : 
            //    other.precedence == precedence ? 0 : -1;
        }
        else
        {
            return -o.ComparePrecedence(this);
        }
    }

    public char GetSymbol()
    {
        return symbol;
    }

    public override string ToString()
    {
        return char.ToString(symbol);
    }
}

public class ASTNode
{
    private string value;
    public bool positive;
    public ASTNode astNodeParent;
    public ASTNode astNodePartner;
    public ASTNode astNodeLeftChild;
    public ASTNode astNodeRightChild;
    public NodeScript astNodeScript;
    public bool isLeft;
    public bool isRight;

    public ASTNode(string value, ASTNode leftASTNode, ASTNode rightASTNode)
    {
        this.value = value;
        this.astNodeLeftChild = leftASTNode;
        this.astNodeRightChild = rightASTNode;
        if (value.Length > 1)
        {
            if (value.Contains("-"))
            {
                positive = false;
            }
        }
        else 
        {
            positive = true;
        }
    }

    public string getValue()
    {
        return this.value;
    }

    public void setValue(string val)
    {
        this.value = val;
        if (val.Length > 1)
        {
            if (val.Contains("-"))
            {
                positive = false;
            }
        }
        else
        {
            positive = true;
        }
    }

    public ASTNode getLeftASTNode()
    {
        return this.astNodeLeftChild;
    }

    public ASTNode getRightASTNode()
    {
        return this.astNodeRightChild;
    }

}

public class ShuntingYardParser
{
    private Dictionary<char, IMyOperator> operators;
    List<string> equationStringArray;
    float nodeInBetweenWidth = 150f;
    float nodeInBetweenHeight = 250f;

    private static void AddNode(Stack<ASTNode> stack, string myOperator)
    {
        ASTNode rightASTNode = stack.Pop();
        ASTNode leftASTNode = stack.Pop();
        stack.Push(new ASTNode(myOperator, leftASTNode, rightASTNode));//does the stack know what is left or right?
    }

    public ShuntingYardParser(ICollection<IMyOperator> operators)
    {
        this.operators = new Dictionary<char, IMyOperator>();
        foreach (IMyOperator o in operators)
        {
            this.operators.Add(o.GetSymbol(), o);
        }
    }

    public List<string> parseInputToEquationStringArray(string input) {
        List<string> tempArray = new List<string>();
        string tempString = "";
        char[] chars = input.ToCharArray();
        //foreach (char c in chars) {
        for(int i=0; i<chars.Length; i++)
        {
            char c = chars[i];
            switch (c)
            {
                case '(':
                    //add the string of characters accumulated from default
                    //tempArray.Add(tempString);
                    //add the "("
                    if (tempString.Length > 0)
                    {
                        tempArray.Add(tempString);
                        tempString = "";
                    }
                    tempString = "(";
                    tempArray.Add(tempString);
                    //reset tempString
                    tempString = "";
                    break;
                case ')':
                    if (tempString.Length > 0)
                    {
                        tempArray.Add(tempString);
                    }
                    tempString = ")";
                    tempArray.Add(tempString);
                    tempString = "";
                    break;
                //' ' case should be after a string of characters accumulated from default
                case ' ':
                    if (tempString.Length > 0) 
                    {
                        tempArray.Add(tempString);
                        tempString = "";
                    }                    
                    break;
                case '\0':
                    if (tempString.Length > 0)
                    {
                        tempArray.Add(tempString);
                        tempString = "";
                    }
                    break;
                default:
                    tempString = tempString + c;
                    if (i == chars.Length - 1) {
                        tempArray.Add(tempString);
                        tempString = "";
                    }
                    break;
            }
        }

        return tempArray;
    }

    public ASTNode ConvertInfixNotationToAST(string input)
    {
        Stack<char> operatorStack = new Stack<char>();
        Stack<ASTNode> operandStack = new Stack<ASTNode>();
        //need to change input to an array of strings instead, maybe use a custom split
        equationStringArray = parseInputToEquationStringArray(input);
        char[] chars = input.ToCharArray();

        //foreach (char c in chars)
        foreach(string c in equationStringArray)
        {
            string popped;
            //main://used by goto - transferred over from Java labelled breaks - causes infinite loopingf
            switch (c)
            {
                case " ":
                    break;
                case "(":
                    operatorStack.Push('(');
                    break;
                case ")":
                    while (!(operatorStack.Count == 0))
                    {//stack is not empty
                        popped = operatorStack.Pop() +"";
                        if ("(" == popped)
                        {
                            //goto main;
                            break;
                        }
                        else
                        {
                            AddNode(operandStack, popped);
                        }
                        //throw new System.Exception("Unbalanced right parenthesis");                        
                    }
                    break;
                default:
                    //if (operators.ContainsKey(c))
                    //Debug.Log(">_" + c + "_<");
                    char possibleOp = c.Substring(0, 1)[0];
                    if (operators.ContainsKey(possibleOp))
                    {                        
                        //this is the part where we make it so that -1 isn't the same as - 1
                        //
                        //

                        IMyOperator o1;
                        operators.TryGetValue(possibleOp, out o1);
                        IMyOperator o2;
                        //while operatorStack is not empty and that the next operator in the operatorstack is a valid operator
                        while (!(operatorStack.Count == 0) && operators.TryGetValue(operatorStack.Peek(), out o2))
                        {
                            //TODO: clarify bottom conditions
                            if (!o1.IsRightAssociative() && 0 == o1.ComparePrecedence(o2) || o1.ComparePrecedence(o2) < 0)//this condition needs to be modified, because it assumes that the 3rd and up operations have no say in precedence. Example 1 + 2 * 3 ^ 2
                            {
                                operatorStack.Pop();
                                AddNode(operandStack, o2.GetSymbol() + "");
                            }
                            else
                            {
                                break;
                            }
                        }
                        operatorStack.Push(possibleOp);
                    }
                    
                    else
                    {
                        operandStack.Push(new ASTNode(c, null, null));
                    }
                    
                    break;
            }
        }
        while (!(operatorStack.Count == 0))
        {
            AddNode(operandStack, operatorStack.Pop()+"");
        }
        return operandStack.Pop();
    }

    internal void designateHierarchy(ASTNode node, bool currentNodeIsLeft, bool currentNodeIsRight)
    {
        //set root left/right to false when first calling designateHierarchy
        //subsequent calls would use this to designate the left/right positions of the node it's going into
        node.isLeft = currentNodeIsLeft;
        node.isRight = currentNodeIsRight;

        if (node.getLeftASTNode() != null && node.getRightASTNode() != null)
        {
            //label left nodes as left, and right nodes as right

            node.getLeftASTNode().isLeft = true;
            node.getLeftASTNode().isRight = false;

            node.getRightASTNode().isLeft = false;
            node.getRightASTNode().isRight = true;

            //node.getLeftASTNode().parentPartner = node.getRightASTNode();
            //node.getRightASTNode().parentPartner = node.getLeftASTNode();


            if (node.getLeftASTNode().getLeftASTNode() != null)
            {
                designateHierarchy(node.getLeftASTNode().getLeftASTNode(), true, false);
            }

            if (node.getLeftASTNode().getRightASTNode() != null)
            {
                designateHierarchy(node.getLeftASTNode().getRightASTNode(), false, true);

            }

            if (node.getRightASTNode().getLeftASTNode() != null)
            {
                designateHierarchy(node.getRightASTNode().getLeftASTNode(), true, false);
            }

            if (node.getRightASTNode().getRightASTNode() != null)
            {
                designateHierarchy(node.getRightASTNode().getRightASTNode(), false, true);
            }
        }
        return;
    }
    
    internal int GetHeight(List<GameObject> nodeObjects) {
        int height = 0;
        for (int i = 0; i < nodeObjects.Count; i++) {
            NodeScript nodeScript = nodeObjects[i].GetComponent<NodeScript>();
            if (nodeScript.path.Length > height) {
                height = nodeScript.path.Length;
            }
        }
        return height; 
    }

    internal void DrawAST3(ASTNode node, GameObject parentToBe, String root, List<GameObject> nodeObjects, int shiftNodeLocation, int height, string path)
    {
        int localShiftNodeLocation = shiftNodeLocation;

        GameObject nodePrefab = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/NodeText"));
        //nodePrefab.transform.SetParent(parentToBe.transform);
        nodePrefab.transform.SetParent(GameObject.Find(root).transform);
        nodeObjects.Add(nodePrefab);
        //nodePrefab.SetActive(false);

        //assign ASTNode values to GameObject<NodeScript> values
        NodeScript nodeScript = nodePrefab.GetComponent<NodeScript>();
        nodeScript.gameObjectParent = parentToBe;
        nodeScript.height = height;
        nodeScript.path = path;
        
        //this should allow us to assign to its nodescript.parent at a later time
        node.astNodeScript = nodeScript;
        //this should allow us to identify nodes and gameobjects together
        nodeScript.node = node;

        //change the object's transform
        //store parent and parent's partner in grandparent
        NodeScript parentNodeScript = parentToBe.GetComponent<NodeScript>();
        if (node.isLeft)
        {
            parentNodeScript.leftGameObjectChild = nodePrefab;
        }
        else if (node.isRight)
        {
            parentNodeScript.rightGameObjectChild = nodePrefab;
        }

        nodeScript.leftGameObjectChild = null;
        nodeScript.rightGameObjectChild = null;

        if (nodeScript.node.isRight)
        {
            //set right's partner as left
            nodeScript.gameObjectPartner = parentNodeScript.leftGameObjectChild;
            //set left's partner as right
            parentNodeScript.leftGameObjectChild.GetComponent<NodeScript>().gameObjectPartner = nodeScript.gameObject;
        }
        else if (nodeScript.node.isLeft)
        {
            //right wouldn't be instantiated yet, so we'll set partners on right nodes only (in which case the left is already instantiated)
        }

        //Recurse through the left and right children of the newly instantiated node
        height++;
        if (node.getLeftASTNode() != null)
        {
            //0 for left
            DrawAST3(node.getLeftASTNode(), nodePrefab, root, nodeObjects, localShiftNodeLocation, height, nodeScript.path + "0");
        }

        if (node.getRightASTNode() != null)
        {
            //1 for right
            DrawAST3(node.getRightASTNode(), nodePrefab, root, nodeObjects, localShiftNodeLocation, height, nodeScript.path + "1");
        }

        //if (nodePrefab.transform.parent.gameObject.name.Equals("Node(Clone)"))
        //{

        //}

        return;
    }

    /*This method will take in the nodeObject list, iterate through it, and output it in levels
     * 
     */
    internal void DrawASTTemplate2(List<GameObject> nodeObjects, int depth) {
        int pathLength = depth;
        int y = 0;
        int x = 0;

        //J > 0 too since we really start with length of 1 at the root
        //change the rest of the branches
        for (int j = pathLength; j > 0; j--) {
            for (int i = 0; i < nodeObjects.Count; i++) {
                NodeScript nodeScript = nodeObjects[i].GetComponent<NodeScript>();
                if (nodeScript.path.Length == j) {
                    GameObject myObject = nodeScript.gameObject;
                    int temp = Convert.ToInt32(nodeScript.path, 2);

                    Vector3 pos = myObject.GetComponent<RectTransform>().anchoredPosition;
                    float leftShiftedX = (temp*nodeInBetweenWidth) - ((Mathf.Pow(2,j)/4)* nodeInBetweenWidth);
                    myObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(leftShiftedX, -j * nodeInBetweenHeight, pos.z);
                    //myObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(leftShiftedX, pos.y, -j * 300);
                }
            }
        }

        //shifts parents to middle of children
        for (int j = pathLength; j > 0; j--)
        {
            for (int i = 0; i < nodeObjects.Count; i++)
            {
                NodeScript nodeScript = nodeObjects[i].GetComponent<NodeScript>();
                if (nodeScript.path.Length == j)
                {
                    GameObject myObject = nodeScript.gameObject;
                    if (nodeScript.leftGameObjectChild != null)
                    {
                        GameObject leftObject = nodeScript.leftGameObjectChild;
                        GameObject rightObject = nodeScript.rightGameObjectChild;
                        Vector3 pos = myObject.GetComponent<RectTransform>().anchoredPosition;
                        Vector3 leftPos = leftObject.GetComponent<RectTransform>().anchoredPosition;
                        Vector3 rightPos = rightObject.GetComponent<RectTransform>().anchoredPosition;
                        float midX = (rightPos.x - leftPos.x) / 2.0f + leftPos.x;
                        myObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(midX, pos.y, pos.z);
                    }
                }
            }
        }

        //attach lines to parents
        for (int i = 1; i < nodeObjects.Count; i++)
        {
            NodeScript nodeScript = nodeObjects[i].GetComponent<NodeScript>();
            Vector3 anchoredPos = nodeScript.gameObjectParent.GetComponent<RectTransform>().anchoredPosition;
            nodeScript.GenerateLine(anchoredPos);
        }
    }


}




