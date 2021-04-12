using UnityEngine;
using UnityEditor;
using System.Collections;
 
[CustomEditor(typeof(ShadowCaster2DController))]
public class ShadowCaster2DControllerEditor : Editor {
    override public void  OnInspectorGUI () {
        ShadowCaster2DController shadowCaster2DController = (ShadowCaster2DController)target;
        if(GUILayout.Button("Set shadow shape using collider shape")) {
            shadowCaster2DController.UpdateFromCollider();
         }
        DrawDefaultInspector();
    }
}
