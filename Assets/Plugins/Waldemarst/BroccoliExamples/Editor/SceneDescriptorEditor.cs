using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

using Broccoli.Utils;

namespace Broccoli.Examples 
{
    [CustomEditor(typeof(SceneDescriptor))]
    public class SceneDescriptorEditor : Editor {
        #region Vars
        SceneDescriptor sceneDescriptor;
        SerializedProperty titleProp;
        SerializedProperty develShowFieldsProp;
        SerializedProperty scenePipelineTypeProp;
        SerializedProperty detectedPipelineTypeProp;
        SerializedProperty autoselectOnSceneLoadProp;
        GUIStyle titleStyle;
        GUIStyle textAreaStyle;
        #endregion

        #region Messages
        private static string MSG_PIPELINE_MISMATCH = 
            "This scene was built using the {0}. This project is using the {1}, reprocessing the scene materials to fit this setup is required.";
        private static string MSG_REQUIRES_REBUILD = "\nThe Broccoli Tree Factory GameObjects also need to be rebuild, you can do so by clicking on the 'Rebuild Broccoli Trees' button.";
        #endregion

        #region Monobehaviour Events
        public void OnEnable ()
        {
		    sceneDescriptor = (SceneDescriptor)target;
            titleProp = serializedObject.FindProperty ("title");
            develShowFieldsProp = serializedObject.FindProperty ("develShowFields");
            scenePipelineTypeProp = serializedObject.FindProperty ("scenePipelineType");
            detectedPipelineTypeProp = serializedObject.FindProperty ("detectedPipelineType");
            autoselectOnSceneLoadProp = serializedObject.FindProperty ("autoselectOnSceneLoad");

            titleStyle = new GUIStyle (EditorStyles.boldLabel);
            titleStyle.alignment = TextAnchor.MiddleCenter;

            textAreaStyle = new GUIStyle (EditorStyles.textArea);
            textAreaStyle.wordWrap = true;
            textAreaStyle.richText = true;
        }
        public override void OnInspectorGUI () {
            // Title.
            BroccoEditorGUI.DrawHeader (sceneDescriptor.title, BroccoEditorGUI.structureGeneratorHeaderolor);
            //EditorGUILayout.LabelField (sceneDescriptor.title, titleStyle);
            EditorGUILayout.Space ();

            // Description.
            EditorGUILayout.LabelField (sceneDescriptor.description.Replace ("\\n", "\n"), textAreaStyle);

            //Mismatch Pipeline Types.
            if (sceneDescriptor.scenePipelineType != sceneDescriptor.detectedPipelineType) {
                EditorGUILayout.Space ();
                EditorGUILayout.HelpBox (string.Format (MSG_PIPELINE_MISMATCH,
                    sceneDescriptor.PipelineTypeToString (sceneDescriptor.scenePipelineType),
                    sceneDescriptor.PipelineTypeToString (sceneDescriptor.detectedPipelineType)) + 
                    (sceneDescriptor.requiresRebuild?MSG_REQUIRES_REBUILD:""), MessageType.Warning);
                if (sceneDescriptor.requiresRebuild) {
                    if (GUILayout.Button (string.Format ("Rebuild Broccoli Trees to {0}", sceneDescriptor.detectedPipelineType))) {
                        sceneDescriptor.RebuildTreeFactories ();
                    }
                }
            }

            // Modify scene description message if on development environment.
            #if BROCCOLI_DEVEL
            EditorGUILayout.Space ();
            EditorGUILayout.Space ();

            serializedObject.Update ();
            EditorGUI.BeginChangeCheck ();

            // Show fields.
            EditorGUILayout.PropertyField (develShowFieldsProp);

            if (sceneDescriptor.develShowFields) {
                // Title.
                EditorGUILayout.PropertyField (titleProp);

                // Description.
                string desc = EditorGUILayout.TextArea (sceneDescriptor.description, textAreaStyle);
                if (desc.CompareTo (sceneDescriptor.description) != 0) {
                    sceneDescriptor.description = desc; 
                }

                EditorGUILayout.PropertyField (scenePipelineTypeProp);
                EditorGUILayout.PropertyField (detectedPipelineTypeProp);
                EditorGUILayout.PropertyField (autoselectOnSceneLoadProp);
            }

            if (EditorGUI.EndChangeCheck ()) {
                sceneDescriptor.CheckRenderPipelineMismatch ();
            }

            serializedObject.ApplyModifiedProperties ();
            #endif
        }
        #endregion
    }
}