using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class MathInput : EditorWindow{
    string myString;


    //[MenuItem("Window/MathWindow")]//This is the location of the window
    //public static void ShowWindow()
    //{
    //    EditorWindow.GetWindow(typeof(MathWindow));
    //}

    //[MenuItem("Window/MathInput")]
    //public static void ShowWindow() 
    //{
    //    EditorWindow.GetWindow(typeof(MathInput));
    //}

    //static MathWindow() {
    //    EditorApplication.update += Update;
    //}

    //static MathInput() {
    //    EditorApplication.update += Update;
    //}

    //static void Update() { 
        
    //}

    void OnGUI()
    {
        //myString = GameObject.Find("Manager").GetComponent<MathWindow>().input;
        //GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        //myString = EditorGUILayout.TextField("Text Field", myString);
        ////groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
        ////myBool = EditorGUILayout.Toggle("Toggle", myBool);
        ////myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);

        //EditorGUILayout.EndToggleGroup();

    }
}
