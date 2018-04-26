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

//String input = "3 + 4 * 2 / (1 - 5) ^ 2 ^ 3";
//String input = "(1 - 5) ^ 2 ^ 3";
//String input = "3 + 4 * 2 / (1 - 5) ^ 2";
//String input = "1 + 2";
//String input = "1 + 2";
//Console.WriteLine("input = " + input);

//double result = ShuntingYardDemo.EvaluateAST(parseTree);
//myString2 = result + "";
//Debug.Log(myString2);

//internal void DrawAST(ASTNode parseTree, GameObject parent, bool left, bool right)
//{
//    GameObject nodePrefab = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Node"));
//    nodePrefab.transform.SetParent(parent.transform);

//    float x = 0;
//    if (left)
//    {
//        x = -1f;
//    }
//    else if (right)
//    {
//        x = 1f;
//    }
//    else
//    {
//        x = 0f;
//    }


//    nodePrefab.transform.localPosition = new Vector3(x, 1f, 0f);
//    treeHeight++;
//    Console.WriteLine(treeHeight);
//    NodeScript nodeScript = nodePrefab.GetComponent<NodeScript>();
//    nodeScript.val = parseTree.getValue() + "";
//    nodeScript.left = left;
//    nodeScript.right = right;
//    nodeScript.parent = parent;

//    ASTNode leftNode = parseTree.getLeftASTNode();
//    ASTNode rightNode = parseTree.getRightASTNode();

//    if (leftNode != null)
//    {
//        DrawAST(leftNode, nodePrefab, true, false);
//    }

//    if (rightNode != null)
//    {
//        DrawAST(rightNode, nodePrefab, false, true);
//    }

//    return;

//    //instantiate physical node
//    //set value
//    //set location
//}


/// <summary>
/// ASTNode should store all of the node's actual information, it should also be able to link to it's own gameobject
/// value, leftASTNode, rightASTNode
/// </summary>
//public class ASTNode
//{
//    private char value;
//    public ASTNode astNodeParent;
//    public ASTNode astNodePartner;
//    public ASTNode astNodeLeftChild;
//    public ASTNode astNodeRightChild;
//    public NodeScript astNodeScript;
//    public bool isLeft;
//    public bool isRight;

//    public ASTNode(char value, ASTNode leftASTNode, ASTNode rightASTNode)
//    {
//        this.value = value;
//        this.astNodeLeftChild = leftASTNode;
//        this.astNodeRightChild = rightASTNode;
//    }

//    public char getValue()
//    {
//        return this.value;
//    }

//    public void setValue(char val){
//        this.value = val;
//    }

//    public ASTNode getLeftASTNode()
//    {
//        return this.astNodeLeftChild;
//    }

//    public ASTNode getRightASTNode()
//    {
//        return this.astNodeRightChild;
//    }

//}

//public class MixedNumber 
//{
//    int wholeNumber;
//    int numerator;
//    int denominator;

//    public MixedNumber(int whole, int num, int den) {
//        this.wholeNumber = whole;
//        this.numerator = num;
//        this.denominator = den;
//    }

//    //some part from https://stackoverflow.com/questions/13569810/least-common-multiple
//    public static MixedNumber Add(MixedNumber m1, MixedNumber m2) {
//        int newWhole = m1.wholeNumber+m2.wholeNumber;
//        int newNum;
//        int newNum1;
//        int newNum2;
//        int newDen;
//        int tempFactor1;
//        int tempFactor2;

//        int num1, num2;
//        if (m1.denominator > m2.denominator)
//        {
//            num1 = m1.denominator;
//            num2 = m2.denominator;
//            tempFactor1 = num2;
//            //for loop checks if num1 is a factor of num2
//            for (int i = 1; i < num2; i++)
//            {
//                if ((num1 * i) % num2 == 0)
//                {
//                    tempFactor1 = i;
//                    break;
//                    //return i * num1;
//                }
//            }
//            //Getting confused here
//            newDen = num1 * tempFactor1;//should be the same as num2*tempFactor2; Should now apply to both? fractions? No.....
//        }
//        else
//        {
//            num1 = m2.denominator; 
//            num2 = m1.denominator;
//            newDen = num1 * num2;
//            for (int i = 1; i < num2; i++)
//            {
//                if ((num1 * i) % num2 == 0)
//                {
//                    newDen = i * num1;
//                    break;
//                    //return i * num1;
//                }
//            }
//        }




//        MixedNumber m3 = new MixedNumber(newWhole, newNum, newDen);

//        //MixedNumber m3 = new MixedNumber(m1.wholeNumber+m2.wholeNumber)
//    }


//}

//internal void DrawAST2(ASTNode node, GameObject parentToBe, int shiftNodeLocation)
//{
//    int localShiftNodeLocation = shiftNodeLocation;

//    GameObject nodePrefab = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/NodeText"));
//    nodePrefab.transform.SetParent(parentToBe.transform);

//    //assign ASTNode values to GameObject<NodeScript> values
//    NodeScript nodeScript = nodePrefab.GetComponent<NodeScript>();

//    nodeScript.gameObjectParent = parentToBe;
//    //this should allow us to assign to its nodescript.parent at a later time
//    node.astNodeScript = nodeScript;
//    //this should allow us to identify nodes and gameobjects together
//    nodeScript.node = node;


//    //change the object's transform
//    //store parent and parent's partner in grandparent
//    NodeScript parentNodeScript = parentToBe.GetComponent<NodeScript>();
//    if (node.isLeft)
//    {
//        parentNodeScript.leftGameObjectChild = nodePrefab;
//    }
//    else if (node.isRight) 
//    {
//        parentNodeScript.rightGameObjectChild = nodePrefab;
//    }

//    nodeScript.leftGameObjectChild = null;
//    nodeScript.rightGameObjectChild = null;

//    if (parentNodeScript.node != null)
//    {
//        if (parentNodeScript.node.isLeft)
//        {
//            nodeScript.gameObjectPartner = null;
//            nodeScript.leftGameObjectChild = null;
//            nodeScript.rightGameObjectChild = null;

//            if (nodeScript.node.isRight)
//            {

//                nodeScript.gameObjectPartner = parentNodeScript.leftGameObjectChild;
//                parentNodeScript.leftGameObjectChild.GetComponent<NodeScript>().gameObjectPartner = nodeScript.gameObject;
//                parentNodeScript.Shift(-1, parentNodeScript.gameObject);//which way would you shift the parent?
//                nodePrefab.transform.position = new Vector3(parentToBe.transform.position.x + 1, parentToBe.transform.position.y + 1, parentToBe.transform.position.z);
//                nodeScript.gameObjectPartner.GetComponent<NodeScript>().Shift(1);

//            }
//            else if (nodeScript.node.isLeft)
//            {
//                nodePrefab.transform.position = new Vector3(parentToBe.transform.position.x - 1, parentToBe.transform.position.y + 1, parentToBe.transform.position.z);
//                parentNodeScript.Shift(-1, parentNodeScript.gameObject);
//            }
//        }
//        else if (parentNodeScript.node.isRight)
//        {
//            nodeScript.gameObjectPartner = null;
//            nodeScript.leftGameObjectChild = null;
//            nodeScript.rightGameObjectChild = null;

//            if (nodeScript.node.isLeft)
//            {
//                parentToBe.transform.position = new Vector3(parentToBe.transform.position.x + 1, parentToBe.transform.position.y, parentToBe.transform.position.z);
//                nodePrefab.transform.position = new Vector3(parentToBe.transform.position.x - 1, parentToBe.transform.position.y + 1, parentToBe.transform.position.z);
//                parentNodeScript.Shift(-1, parentNodeScript.gameObject);
//            }
//            else if (nodeScript.node.isRight)
//            {
//                nodePrefab.transform.position = new Vector3(parentToBe.transform.position.x + 1, parentToBe.transform.position.y + 1, parentToBe.transform.position.z);
//                nodeScript.gameObjectPartner = parentNodeScript.leftGameObjectChild;
//                parentNodeScript.leftGameObjectChild.GetComponent<NodeScript>().gameObjectPartner = nodeScript.gameObject;
//                parentNodeScript.Shift(1, parentNodeScript.gameObject);
//                nodeScript.gameObjectPartner.GetComponent<NodeScript>().Shift(1);

//            }
//        }
//        else //if parent is the root 
//        {
//            nodeScript.gameObjectPartner = null;
//            nodeScript.leftGameObjectChild = null;
//            nodeScript.rightGameObjectChild = null;

//            if (nodeScript.node.isLeft)
//            {
//                nodePrefab.transform.position = new Vector3(parentToBe.transform.position.x - 1, parentToBe.transform.position.y + 1, parentToBe.transform.position.z);
//                parentNodeScript.Shift(-1, parentNodeScript.gameObject);
//            }
//            else if (nodeScript.node.isRight)
//            {
//                nodePrefab.transform.position = new Vector3(parentToBe.transform.position.x + 1, parentToBe.transform.position.y + 1, parentToBe.transform.position.z);
//                nodeScript.gameObjectPartner = parentNodeScript.leftGameObjectChild;
//                parentNodeScript.leftGameObjectChild.GetComponent<NodeScript>().gameObjectPartner = nodeScript.gameObject;
//                parentNodeScript.Shift(1, parentNodeScript.gameObject);
//                nodeScript.gameObjectPartner.GetComponent<NodeScript>().Shift(1);

//            }
//        }
//    }
//    else if(parentNodeScript.node == null){//if node is the root
//        nodeScript.gameObjectPartner = null;
//        nodeScript.leftGameObjectChild = null;
//        nodeScript.rightGameObjectChild = null;
//    }


//    //Recurse through the left and right children of the newly instantiated node
//    if (node.getLeftASTNode() != null)
//    {
//        DrawAST2(node.getLeftASTNode(), nodePrefab, localShiftNodeLocation);
//    }

//    if (node.getRightASTNode() != null)
//    {
//        DrawAST2(node.getRightASTNode(), nodePrefab, localShiftNodeLocation);
//    }

//    if (nodePrefab.transform.parent.gameObject.name.Equals("Node(Clone)")) { 

//    }

//    return;
//}

//if (nodeScript.leftGameObjectChild != null)
//{
//    GameObject leftObject = nodeScript.leftGameObjectChild;
//    GameObject rightObject = nodeScript.rightGameObjectChild;
//    Vector3 pos = myObject.GetComponent<RectTransform>().anchoredPosition;
//    Vector3 leftPos = leftObject.GetComponent<RectTransform>().anchoredPosition;
//    Vector3 rightPos = rightObject.GetComponent<RectTransform>().anchoredPosition;
//    float midX = (rightPos.x - leftPos.x) / 2.0f + leftPos.x;
//    myObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(midX, pos.y, pos.z);
//}
//else { 

//}

//this will draw the node with the longest paths first
//for (int j = pathLength; j >= 0; j--) {            
//    for (int i = 0; i < nodeObjects.Count; i++)
//    {
//        NodeScript nodeScript = nodeObjects[i].GetComponent<NodeScript>();
//        if (nodeScript.path.Length == j)
//        {
//            //Debug.Log(j + ":" + i);
//            GameObject myObject = nodeScript.gameObject;
//            Vector3 pos = myObject.GetComponent<RectTransform>().anchoredPosition;
//            myObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(pos.x + x, pos.y + y, pos.z);
//            x = x+200;
//        }                
//    }
//    y=y+200;
//    x = 0;
//}

////change the bottom leaves here where j=pathLength;
//for (int i = 0; i < nodeObjects.Count; i++)
//{ 
//    NodeScript nodeScript = nodeObjects[i].GetComponent<NodeScript>();
//    if (nodeScript.path.Length == pathLength) {
//        int temp = Convert.ToInt32(nodeScript.path, 2);
//        GameObject myObject = nodeScript.gameObject;
//        Vector3 pos = myObject.GetComponent<RectTransform>().anchoredPosition;
//        myObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(temp*200, pos.y, pos.z);
//    }
//}

//2nd Tree
//ASTNode parseTree2 = parser.ConvertInfixNotationToAST(input2);
//GameObject treeTop2 = GameObject.Find("Tree2");

////parser.DrawAST(parseTree, treeTop, false, false);
//parser.designateHierarchy(parseTree2, false, false);
////parser.DrawAST2(parseTree, treeTop, 0);
//parser.DrawAST3(parseTree2, treeTop2, "Tree2", nodeObjects2, 0, 0, "0");
////parser.DrawASTTemplate(5, treeTop, nodeObjects, true, true);
//gameObjectTreeHeight = parser.GetHeight(nodeObjects2);
//parser.DrawASTTemplate2(nodeObjects2, gameObjectTreeHeight2);

//public List<GameObject> nodeObjects2 = new List<GameObject>();
//string myString2 = "Yo";
//bool groupEnabled;
//bool myBool = true;
//bool button;
//float myFloat = 1.23f;
//char[] charArray;

//void MoveWithAddForce() {
//    if (Input.GetKeyDown(KeyCode.W)) {
//        rb.AddForce(Vector3.forward * sensitivity);
//    }
//    if (Input.GetKeyDown(KeyCode.S)){
//        rb.AddForce(Vector3.back * sensitivity);
//    }
//    if (Input.GetKeyDown(KeyCode.A)) {
//        rb.AddForce(Vector3.left * sensitivity);
//    }
//    if (Input.GetKeyDown(KeyCode.D)) {
//        rb.AddForce(Vector3.right * sensitivity);
//    }
//}

//void MoveWithTranslate() {
//    if (Input.GetKeyDown(KeyCode.W))
//    {
//        transform.Translate(Vector3.forward * sensitivity);            
//    }
//    if (Input.GetKeyDown(KeyCode.S))
//    {
//        transform.Translate(Vector3.back * sensitivity);

//    }
//    if (Input.GetKeyDown(KeyCode.A))
//    {
//        transform.Translate(Vector3.left * sensitivity);

//    }
//    if (Input.GetKeyDown(KeyCode.D))
//    {
//        transform.Translate(Vector3.right * sensitivity);

//    }
//}