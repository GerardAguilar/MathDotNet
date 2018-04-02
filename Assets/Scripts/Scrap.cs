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
