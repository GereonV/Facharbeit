using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerController), true)] [CanEditMultipleObjects]
public class PlayerControllerEditor : Editor {

    PlayerController controller;

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        controller = (PlayerController) target;

        if(GUILayout.Button("Save"))
            controller?.Save();
    }

}
