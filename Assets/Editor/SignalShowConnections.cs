using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SignalSource))]
[CanEditMultipleObjects]
public class SignalShowConnections : Editor
{
    void OnSceneGUI()
    {
        // get the chosen game object
        SignalSource t = target as SignalSource;

        if (t == null || t.gameObject == null)
            return;



        foreach (Signalable signalable in t.signalables)
        {
            if (signalable != null)
            {
                Handles.color = t.color;
                Handles.DrawLine(t.transform.position, signalable.transform.position);
            }
        }          
    }
}

