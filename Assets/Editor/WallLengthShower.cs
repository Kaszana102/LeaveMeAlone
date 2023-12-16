using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Wall))]
public class WallLengthShower : Editor
{
    private SerializedProperty wallLength;


    protected void OnEnable()
    {
        wallLength = serializedObject.FindProperty("wallLength");        
    }

    void OnSceneGUI()
    {
        // get the chosen game object
        Wall t = target as Wall;

        if (t == null || t.gameObject == null)
            return;



        
        Handles.color = Color.yellow;

        float rectWidth = 0.25f;

        Rect rect = new Rect(t.transform.position.x - rectWidth, t.transform.position.y,
            2 * rectWidth, wallLength.floatValue);

        Handles.DrawSolidRectangleWithOutline(rect, Color.white, Color.black);

          
    }
}
