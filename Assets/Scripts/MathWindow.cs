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
        double result = ShuntingYardDemo.EvaluateAST(parseTree);
        myString2 = result + "";
        Debug.Log(myString2);
    }

    //need to figure out how to simulate node snapping into branches
    //probably somewhere along the lines of 
    //1. order, parenthesize, then expand
    //2. place root in root position, 
    //3. if root has children, go depth-first
    //4. place left children first, then right
    //5. If left, use a method to instantiate a branch and a node at the proper coordinates
    //6. Then change root to new child
    //7. When there are no more left children
    //8. Move back to the previous root (store it in a list)
    //9. If there are no more left children, then place right child
    //10. Then change root to new child
    //11. If there is a left child, populate with node, and change root
    //12. If there is no left child, populate right with node, and then change root
    //13. If there are no right child either, move to previous root.
    //14. If there are no right child again, move to previous root.
    //15. If we are at the root and there no more children to fill in, then finished


    ////1. order, parenthesize, then expand
    //void TakeStringAndParse() {//later, parenthesized parts should be treated like a completely separate equation (almost)
    //    charArray = myString.ToCharArray();
    //    int[] leftRightArray = new int[charArray.Length];
    //    int[] priorityArray = new int[charArray.Length];
    //    int[] heightArray = new int[charArray.Length];
    //    int orderLeftRight = 0;
    //    int orderPriority = 0;
    //    int height = 0;


    //    int[] orderArray = new int[charArray.Length];
    //    int order = 0;

    //    for (int i = 0; i < charArray.Length; i++)
    //    {
    //        leftRightArray[i] = -1;
    //        priorityArray[i] = -1;
    //    }

    //    for (int i = 0; i < charArray.Length; i++) {
    //        if (charArray[i] == '(')
    //        {
    //            height++;
    //            heightArray[i] = height;
    //        }
    //        else if (charArray[i] == ')')
    //        {
    //            heightArray[i] = height;
    //            height--;
    //        }
    //        else 
    //        {
    //            heightArray[i] = height;
    //        }

    //    }


    //    for (int i = 0; i < leftRightArray.Length; i++)
    //    {
    //        //moving left to right, tag with priority based on Exponents
    //        if (charArray[i] == '^')
    //        {
    //            leftRightArray[i] = orderLeftRight++;
    //            priorityArray[i] = orderPriority;
    //        }
    //    }
    //    orderPriority++;
    //    for (int i = 0; i < leftRightArray.Length; i++)
    //    {
    //        //moving left to right, tag with priority based on Division/Multiplication
    //        if (charArray[i] == '/' || charArray[i] == '*')
    //        {
    //            leftRightArray[i] = orderLeftRight++;
    //            priorityArray[i] = orderPriority;
    //        }
    //    }
    //    orderPriority++;
    //    for (int i = 0; i < leftRightArray.Length; i++)
    //    {
    //        //moving left to right, tag with priority based on Subtraction/Addition
    //        if (charArray[i] == '-' || charArray[i] == '+')
    //        {
    //            leftRightArray[i] = orderLeftRight++;
    //            priorityArray[i] = orderPriority;
    //        }
    //    }

    //    string temp = "";
    //    for (int i = 0; i < charArray.Length; i++) {
    //        temp = temp + charArray[i];
    //    }
    //    Debug.Log(temp.ToString());
    //    temp = "";
    //    for (int i = 0; i < charArray.Length; i++)
    //    {
    //        temp = temp + leftRightArray[i];
    //    }
    //    Debug.Log(temp.ToString());
    //    temp = "";
    //    for (int i = 0; i < charArray.Length; i++)
    //    {
    //        temp = temp + priorityArray[i];
    //    }
    //    Debug.Log(temp.ToString());
    //    temp = "";
    //    for (int i = 0; i < charArray.Length; i++)
    //    {
    //        temp = temp + heightArray[i];
    //    }
    //    Debug.Log(temp.ToString());

    //    //generate rules for inside height and in-between

    //}
    ////2. place root in root position, 
    ////3. if root has children, go depth-first
    ////4. place left children first, then right
    ////5. If left, use a method to instantiate a branch and a node at the proper coordinates
    ////6. Then change root to new child
    ////7. When there are no more left children
    ////8. Move back to the previous root (store it in a list)
    ////9. If there are no more left children, then place right child
    ////10. Then change root to new child
    ////11. If there is a left child, populate with node, and change root
    ////12. If there is no left child, populate right with node, and then change root
    ////13. If there are no right child either, move to previous root.
    ////14. If there are no right child again, move to previous root.
    ////15. If we are at the root and there no more children to fill in, then finished


    //void ShuntingYardToTree(string equation) {
    //    Stack<string> num;
    //    Stack<string> op;

    //    string temp;

    //    for (int i = 0; i < equation.Length; i++) {

    //        //if number, check how long the number is
    //        //then store into temp
    //        switch (equation[i]) { 
    //            case '0';
    //            case '1';
    //            case '2';
    //            case '3';
    //            case '4';
    //            case '5';
    //            case '6';
    //            case '7';
    //            case '8';
    //            case '9';
    //        }
    //    }


    //}
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
            main://used by goto - transferred over from Java labelled breaks - causes infinite looping
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
                            goto main;
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
