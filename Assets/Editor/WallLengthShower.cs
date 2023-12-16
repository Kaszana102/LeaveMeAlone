using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(Wall))]
[CanEditMultipleObjects]
public class WallLengthShower : Editor
{
    private SerializedProperty wallLength;


    protected void OnEnable()
    {
        wallLength = serializedObject.FindProperty("wallLength");
    }

    private void OnDisable()
    {
    }

    void OnSceneGUI()
    {
        // get the chosen game object
        Wall t = target as Wall;

        if (t == null || t.gameObject == null)
            return;



        
        Handles.color = Color.yellow;

        float rectWidth = 15f;

        Handles.DrawLine(t.transform.position,
                t.transform.position + t.transform.up * wallLength.floatValue, rectWidth);
    }
}



