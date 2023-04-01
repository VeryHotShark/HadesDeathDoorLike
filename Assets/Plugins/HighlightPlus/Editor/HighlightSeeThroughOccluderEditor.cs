using UnityEditor;
using UnityEngine;

namespace HighlightPlus {

    [CustomEditor(typeof(HighlightSeeThroughOccluder))]
    public class HighlightSeeThroughOccluderEditor : Editor {

        SerializedProperty mode, detectionMethod;

        void OnEnable() {
            mode = serializedObject.FindProperty("mode");
            detectionMethod = serializedObject.FindProperty("detectionMethod");
        }

        public override void OnInspectorGUI() {

            serializedObject.Update();

            EditorGUILayout.PropertyField(mode);
            if (mode.intValue == (int)OccluderMode.BlocksSeeThrough) {
                EditorGUILayout.HelpBox("This object will occlude any see-through effect.", MessageType.Info);
                EditorGUILayout.PropertyField(detectionMethod);
            } else {
                EditorGUILayout.HelpBox("This object will trigger see-through effect. Use only on objects that do not write to depth buffer normally, like sprites or transparent objects.", MessageType.Info);
            }

            serializedObject.ApplyModifiedProperties();

        }
    }

}
