using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(FloorGenerator))]
public class FloorGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        FloorGenerator script = (FloorGenerator)target;

        if (GUILayout.Button("Clear", GUILayout.Height(35)))
        {
            script.Clear();
        }
        if (GUILayout.Button("Generate", GUILayout.Height(35)))
        {
            script.Create();
        }
    }
}
#endif