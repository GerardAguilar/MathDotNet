using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;


//code is inaccurate, but functional
//[InitializeOnLoad]
public class MathWindow : MonoBehaviour {
    public string input = "Hello World";
    string myString2 = "Yo";
    bool groupEnabled;
    bool myBool = true;
    bool button;
    float myFloat = 1.23f;
    char[] charArray;
    int treeHeight = 0;

    //[MenuItem("Window/MathWindow")]//This is the location of the window
    //public static void ShowWindow()
    //{
    //    EditorWindow.GetWindow(typeof(MathWindow));
    //}
    //static MathWindow() {
    //    EditorApplication.update += Update;
    //}

    //static void Update() {
    //    Debug.Log("Updating");
    //}

    //there may be a better way of doing this by using two stacks
    //or by figuring out the 'last' operation, setting that as a root, splitting to left and right, then continuing the same thing with the branches

    //void OnGUI() {
    //    GUILayout.Label("Base Settings", EditorStyles.boldLabel);
    //    myString = EditorGUILayout.TextField("Text Field", myString);
    //    myString2 = EditorGUILayout.TextField("Text Field", myString2);

    //    groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
    //    myBool = EditorGUILayout.Toggle("Toggle", myBool);
    //    myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);
    //    EditorGUILayout.EndToggleGroup();

    //    String input = "3 + 4 * 2 / (1 - 5) ^ 2 ^ 3";
    //    myString = "3 + 4 * 2 / (1 - 5) ^ 2 ^ 3";

    //    button = GUI.Button(new Rect(4, 100, 30, 30), "X");
    //    if (button) {
    //        ICollection<IMyOperator> operators = new List<IMyOperator>();
    //        operators.Add(new MyBaseOperator('^', true, 4));
    //        operators.Add(new MyBaseOperator('*', false, 3));
    //        operators.Add(new MyBaseOperator('/', false, 3));
    //        operators.Add(new MyBaseOperator('+', false, 2));
    //        operators.Add(new MyBaseOperator('-', false, 2));

    //        ShuntingYardParser parser = new ShuntingYardParser(operators);
            
    //        ASTNode parseTree = parser.ConvertInfixNotationToAST(input);
    //        //double result = ShuntingYardDemo.EvaluateAST(parseTree);
    //        //myString2 = result + "";
    //    }
    //}

    void Awake()
    {
        //String input = "3 + 4 * 2 / (1 - 5) ^ 2 ^ 3";
        //String input = "(1 - 5) ^ 2 ^ 3";
        //String input = "3 + 4 * 2 / (1 - 5) ^ 2";
        //String input = "1 + 2";
        //String input = "1 + 2";
        //Console.WriteLine("input = " + input);
        Debug.Log("input = " + input);
        ICollection<IMyOperator> operators = new List<IMyOperator>();
        operators.Add(new MyBaseOperator('^', true, 4));
        operators.Add(new MyBaseOperator('*', false, 3));
        operators.Add(new MyBaseOperator('/', false, 3));
        operators.Add(new MyBaseOperator('+', false, 2));
        operators.Add(new MyBaseOperator('-', false, 2));

        ShuntingYardParser parser = new ShuntingYardParser(operators);

        ASTNode parseTree = parser.ConvertInfixNotationToAST(input);
        Debug.Log(parseTree.getValue());
        GameObject root = GameObject.Find("Root");

        parser.DrawAST(parseTree, root, false, false);
        
        //double result = ShuntingYardDemo.EvaluateAST(parseTree);
        //myString2 = result + "";
        //Debug.Log(myString2);
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

    public MyBaseOperator(char symbol, bool rightAssociative, int precedence) {
        this.symbol = symbol;
        this.rightAssociative = rightAssociative;
        this.precedence = precedence;
    }

    public bool IsRightAssociative() {
        return rightAssociative;
    }

    
    public int ComparePrecedence(IMyOperator o) {
        bool result = (o is MyBaseOperator);
        int tempPrecedence;
        if (result){
            MyBaseOperator other = (MyBaseOperator)o;
            if (precedence > other.precedence)
            {
                tempPrecedence = 1;
            }
            else if (other.precedence == precedence)
            {
                tempPrecedence = 0;
            }
            else {
                tempPrecedence = -1;
            }
            return tempPrecedence;
            //since I'm not used to the bottom structure, I unfolded it above.
            //return precedence > other.precedence ? 1 : 
            //    other.precedence == precedence ? 0 : -1;
        }
        else{
            return -o.ComparePrecedence(this);
        }
    }

    public char GetSymbol() {
        return symbol;
    }

    public override string ToString()
    {
        return char.ToString(symbol);
    }
}

public class ASTNode
{
    private char value;
    private ASTNode leftASTNode;
    private ASTNode rightASTNode;


    public ASTNode(char value, ASTNode leftASTNode, ASTNode rightASTNode) {
        this.value = value;
        this.leftASTNode = leftASTNode;
        this.rightASTNode = rightASTNode;

    }

    public char getValue() {
        return value;
    }

    public ASTNode getLeftASTNode() {
        return leftASTNode;
    }

    public ASTNode getRightASTNode() {
        return rightASTNode;
    }
}

public class ShuntingYardParser {

    int treeHeight = 0;
    private Dictionary<char, IMyOperator> operators;

    private static void AddNode(Stack<ASTNode> stack, char myOperator) {
        ASTNode rightASTNode = stack.Pop();
        ASTNode leftASTNode = stack.Pop();
        stack.Push(new ASTNode(myOperator, leftASTNode, rightASTNode));
    }

    public ShuntingYardParser(ICollection<IMyOperator> operators){
        this.operators = new Dictionary<char, IMyOperator>();
        foreach (IMyOperator o in operators) {
            this.operators.Add(o.GetSymbol(), o);
        }
    }

    public ASTNode ConvertInfixNotationToAST(string input) {
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

    internal void DrawAST(ASTNode parseTree, GameObject parent, bool left, bool right)
    {
        GameObject nodePrefab = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Node"));
        nodePrefab.transform.SetParent(parent.transform);

        float x = 0;
        if (left)
        {
            x = -1f;
        }
        else if (right)
        {
            x = 1f;
        }
        else 
        {
            x = 0f;
        }


        nodePrefab.transform.localPosition = new Vector3(x, 1f, 0f);
        treeHeight++;
        Console.WriteLine(treeHeight);
        NodeScript nodeScript = nodePrefab.GetComponent<NodeScript>();
        nodeScript.val = parseTree.getValue()+"";
        nodeScript.left = left;
        nodeScript.right = right;
        nodeScript.parent = parent;

        ASTNode leftNode = parseTree.getLeftASTNode();
        ASTNode rightNode = parseTree.getRightASTNode();

        if (leftNode != null) {
            DrawAST(leftNode, nodePrefab, true, false);
        }

        if (rightNode != null){
            DrawAST(rightNode, nodePrefab, false, true);
        }

        return;

        //instantiate physical node
        //set value
        //set location
    }
}

public static class ShuntingYardDemo {
    public static double EvaluateAST(ASTNode tree) {
        double tempVal;
        switch (tree.getValue()) { 
            case '^':
                tempVal = Math.Pow(EvaluateAST(tree.getLeftASTNode()), EvaluateAST(tree.getRightASTNode()));
                break;
            case '*':
                tempVal = EvaluateAST(tree.getLeftASTNode()) * EvaluateAST(tree.
                        getRightASTNode());
                break;
            case '/':
                tempVal = EvaluateAST(tree.getLeftASTNode()) / EvaluateAST(tree.
                        getRightASTNode());
                break;
            case '+':
                tempVal = EvaluateAST(tree.getLeftASTNode()) + EvaluateAST(tree.
                        getRightASTNode());
                break;
            case '-':
                tempVal = EvaluateAST(tree.getLeftASTNode()) - EvaluateAST(tree.
                        getRightASTNode());
                break;
            default:
                //return Double.valueOf(char.toString(
                //        tree.getValue()));
                tempVal = Double.Parse(tree.getValue().ToString());
                break;
        }
        return tempVal;
    }

    //public static void main(String[] args) {
    //    ICollection<IMyOperator> operators = new List<IMyOperator>();
    //    operators.Add(new MyBaseOperator('^', true, 4));
    //    operators.Add(new MyBaseOperator('*', false, 3));
    //    operators.Add(new MyBaseOperator('/', false, 3));
    //    operators.Add(new MyBaseOperator('+', false, 2));
    //    operators.Add(new MyBaseOperator('-', false, 2));

    //    ShuntingYardParser parser = new ShuntingYardParser(operators);
    //    String input = "3 + 4 * 2 / (1 - 5) ^ 2 ^ 3";

    //    ASTNode parseTree = parser.ConvertInfixNotationToAST(input);
    //    double result = EvaluateAST(parseTree);
        
    //}
}

//public class MathNode{
//    //attributes
//    string operation;
//    char[] left;
//    char[] right;

//    //constructor
//    public MathNode(string op, char[] leftside, char[] rightside) {
//        operation = op;
//        left = new char[leftside.Length];
//        for (int i = 0; i < leftside.Length; i++) {
//            left[i] = leftside[i];
//        }
//        right = new char[rightside.Length];
//        for (int i = 0; i < rightside.Length; i++) {
//            right[i] = rightside[i];
//        }
//    }
    
//    //method
//}
