using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;


//code is inaccurate, but functional
//[InitializeOnLoad]
public class Parser : MonoBehaviour
{
    public string input = "Hello World";
    string myString2 = "Yo";
    //bool groupEnabled;
    //bool myBool = true;
    //bool button;
    //float myFloat = 1.23f;
    char[] charArray;
    //int treeHeight = 0;

    void Awake()
    {
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
        Debug.Log(parseTree.getValue());
        GameObject treeTop = GameObject.Find("Tree");

        //parser.DrawAST(parseTree, treeTop, false, false);
        parser.designateHierarchy(parseTree, false, false);
        parser.DrawAST2(parseTree, treeTop);


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

/// <summary>
/// ASTNode should store all of the node's actual information, it should also be able to link to it's own gameobject
/// value, leftASTNode, rightASTNode
/// </summary>
public class ASTNode
{
    private char value;
    public ASTNode astNodeParent;
    public ASTNode astNodePartner;
    public ASTNode astNodeLeftChild;
    public ASTNode astNodeRightChild;
    public bool isLeft;
    public bool isRight;

    public ASTNode(char value, ASTNode leftASTNode, ASTNode rightASTNode)
    {
        this.value = value;
        this.astNodeLeftChild = leftASTNode;
        this.astNodeRightChild = rightASTNode;
    }

    public char getValue()
    {
        return this.value;
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

    int treeHeight = 0;
    private Dictionary<char, IMyOperator> operators;

    private static void AddNode(Stack<ASTNode> stack, char myOperator)
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

    public ASTNode ConvertInfixNotationToAST(string input)
    {
        Stack<char> operatorStack = new Stack<char>();
        Stack<ASTNode> operandStack = new Stack<ASTNode>();
        char[] chars = input.ToCharArray();

        foreach (char c in chars)
        {
            char popped;
            //main://used by goto - transferred over from Java labelled breaks - causes infinite loopingf
            switch (c)
            {
                case ' ':
                    break;
                case '(':
                    operatorStack.Push('(');
                    break;
                case ')':
                    while (!(operatorStack.Count == 0))
                    {//stack is not empty
                        popped = operatorStack.Pop();
                        if ('(' == popped)
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
                    if (operators.ContainsKey(c))
                    {
                        IMyOperator o1;
                        operators.TryGetValue(c, out o1);
                        IMyOperator o2;
                        //while operatorStack is not empty and that the next operator in the operatorstack is a valid operator
                        while (!(operatorStack.Count == 0) && operators.TryGetValue(operatorStack.Peek(), out o2))
                        {
                            //TODO: clarify bottom conditions
                            if (!o1.IsRightAssociative() && 0 == o1.ComparePrecedence(o2) || o1.ComparePrecedence(o2) < 0)//this condition needs to be modified, because it assumes that the 3rd and up operations have no say in precedence. Example 1 + 2 * 3 ^ 2
                            {
                                operatorStack.Pop();
                                AddNode(operandStack, o2.GetSymbol());
                            }
                            else
                            {
                                break;
                            }
                        }
                        operatorStack.Push(c);
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
            AddNode(operandStack, operatorStack.Pop());
        }
        return operandStack.Pop();
    }

    //was completing but inaccurate left/right marking. Fixing caused NullReference for MathWindow cs:324
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

    internal void DrawAST2(ASTNode node, GameObject parentToBe)
    {
        GameObject nodePrefab = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Node"));
        nodePrefab.transform.SetParent(parentToBe.transform);

        //assign ASTNode values to GameObject<NodeScript> values
        NodeScript nodeScript = nodePrefab.GetComponent<NodeScript>();
        nodeScript.node = node;
        nodeScript.gameObjectParent = parentToBe;

        //change the object's transform
        //store parent and parent's partner in grandparent
        NodeScript parentNodeScript = parentToBe.GetComponent<NodeScript>();
        if (node.isLeft)
        {
            parentNodeScript.leftGameObjectChild = nodePrefab;
        }
        else if (node.isRight) {
            parentNodeScript.rightGameObjectChild = nodePrefab;
        }

        nodeScript.leftGameObjectChild = null;
        nodeScript.rightGameObjectChild = null;

        if (parentNodeScript.node != null)
        {
            if (parentNodeScript.node.isLeft)
            {
                nodeScript.gameObjectPartner = null;
                nodeScript.leftGameObjectChild = null;
                nodeScript.rightGameObjectChild = null;

                if (nodeScript.node.isRight)
                {
                    parentToBe.transform.position = new Vector3(parentToBe.transform.position.x - 1, parentToBe.transform.position.y, parentToBe.transform.position.z);
                    nodePrefab.transform.position = new Vector3(parentToBe.transform.position.x + 1, parentToBe.transform.position.y + 1, parentToBe.transform.position.z);
                }
                else if (nodeScript.node.isLeft)
                {
                    nodePrefab.transform.position = new Vector3(parentToBe.transform.position.x - 1, parentToBe.transform.position.y + 1, parentToBe.transform.position.z);
                }
            }
            else if (parentNodeScript.node.isRight)
            {
                nodeScript.gameObjectPartner = null;
                nodeScript.leftGameObjectChild = null;
                nodeScript.rightGameObjectChild = null;

                if (nodeScript.node.isLeft)
                {
                    parentToBe.transform.position = new Vector3(parentToBe.transform.position.x + 1, parentToBe.transform.position.y, parentToBe.transform.position.z);
                    nodePrefab.transform.position = new Vector3(parentToBe.transform.position.x - 1, parentToBe.transform.position.y + 1, parentToBe.transform.position.z);
                }
                else if (nodeScript.node.isRight)
                {
                    nodePrefab.transform.position = new Vector3(parentToBe.transform.position.x + 1, parentToBe.transform.position.y + 1, parentToBe.transform.position.z);
                }
            }
            else //if parent is the root 
            {
                nodeScript.gameObjectPartner = null;
                nodeScript.leftGameObjectChild = null;
                nodeScript.rightGameObjectChild = null;

                if (nodeScript.node.isLeft)
                {
                    nodePrefab.transform.position = new Vector3(parentToBe.transform.position.x - 1, parentToBe.transform.position.y + 1, parentToBe.transform.position.z);
                }
                else if (nodeScript.node.isRight)
                {
                    nodePrefab.transform.position = new Vector3(parentToBe.transform.position.x + 1, parentToBe.transform.position.y + 1, parentToBe.transform.position.z);
                }
            }
        }
        else if(parentNodeScript.node == null){

            nodeScript.gameObjectPartner = null;
            nodeScript.leftGameObjectChild = null;
            nodeScript.rightGameObjectChild = null;

            if (nodeScript.node.isLeft)
            {
                nodePrefab.transform.position = new Vector3(parentToBe.transform.position.x - 1, parentToBe.transform.position.y + 1, parentToBe.transform.position.z);
            }
            else if (nodeScript.node.isRight)
            {
                nodePrefab.transform.position = new Vector3(parentToBe.transform.position.x + 1, parentToBe.transform.position.y + 1, parentToBe.transform.position.z);
            }
        }
        

        //Recurse through the left and right children of the newly instantiated node
        if (node.getLeftASTNode() != null)
        {
            DrawAST2(node.getLeftASTNode(), nodePrefab);
        }

        if (node.getRightASTNode() != null)
        {
            DrawAST2(node.getRightASTNode(), nodePrefab);
        }

        return;
    }

    
}




