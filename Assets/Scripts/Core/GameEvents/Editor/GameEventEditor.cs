using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace VHS {
    [CustomEditor(typeof(GameEvent))]
    public class GameEventEditor : Editor {

        private string _newName = "EVT_Example";
        private List<Component> _referencedInComponents = new List<Component>();

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            GUILayout.Space(20);
            _newName = GUILayout.TextField(_newName);

            if (GUILayout.Button("Rename Selected GameEvent"))
                RenameGameEvent(_newName);

            //ShowReferences();

            GUILayout.Space(20);
            if (GUILayout.Button("Raise"))
                (target as GameEvent).Raise(target);
        }

      //  private void OnEnable() => FindReferencesTo();

        private void FindReferencesTo() {
            GameObject[] allObjects = FindObjectsOfType<GameObject>();

            for (int j = 0; j < allObjects.Length; j++) {
                GameObject gameObject = allObjects[j];
                Component[] components = gameObject.GetComponents<Component>();

                for (int i = 0; i < components.Length; i++) {
                    Component component = components[i];
                    if (!component) continue;

                    SerializedObject serializedObject = new SerializedObject(component);
                    SerializedProperty serializedProperty = serializedObject.GetIterator();

                    while (serializedProperty.NextVisible(true))
                        if (serializedProperty.propertyType == SerializedPropertyType.ObjectReference)
                            if (serializedProperty.objectReferenceValue == Selection.activeObject)
                                _referencedInComponents.Add(component);
                }
            }
        }

        private void ShowReferences() {
            GUILayout.Space(20);
            GUILayout.Label("GameEvent references:", EditorStyles.boldLabel);

            foreach (Component component in _referencedInComponents)
                EditorGUILayout.ObjectField(component.GetType().ToString(), component, typeof(GameObject), allowSceneObjects: true);

            if (!_referencedInComponents.Any())
                GUILayout.Label("No references in the scene!");
        }

        private void RenameGameEvent(string name) {
            Object gameEvent = target;
            gameEvent.name = name;
            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(Selection.activeObject), ImportAssetOptions.ForceUpdate);
            EditorUtility.FocusProjectWindow();
            AssetDatabase.SaveAssets();
            (target as GameEvent).GameEventStorage.UpdateGameEventsList();
        }

    }
}


