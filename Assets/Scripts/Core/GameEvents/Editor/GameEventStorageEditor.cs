using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace VHS {
    [CustomEditor(typeof(GameEventStorage))]
    public class GameEventStorageEditor : Editor {

        private SerializedProperty shortcutData;
        private ReorderableList reorderableList;

        private void OnEnable() {
            shortcutData = serializedObject.FindProperty("GameEvents");

            reorderableList = new ReorderableList(serializedObject, shortcutData, true, true, true, true);

            reorderableList.drawHeaderCallback += DrawHeaderCallback;
            reorderableList.drawElementCallback += DrawElementCallback;
            reorderableList.elementHeightCallback += ElementHeightCallback;
            reorderableList.onAddCallback += OnAddCallback;
            reorderableList.onRemoveCallback += OnRemoveCallback;
        }

        private void OnDisable() {
            reorderableList.drawHeaderCallback -= DrawHeaderCallback;
            reorderableList.drawElementCallback -= DrawElementCallback;
            reorderableList.elementHeightCallback -= ElementHeightCallback;
            reorderableList.onAddCallback -= OnAddCallback;
            reorderableList.onRemoveCallback -= OnRemoveCallback;
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();
            reorderableList.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawHeaderCallback(Rect rect) {
            EditorGUI.LabelField(rect, "Game Events");
        }

        private void DrawElementCallback(Rect rect, int index, bool isactive, bool isfocused) {
            SerializedProperty element = reorderableList.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;

            EditorGUI.PropertyField(new Rect(rect.x += 10, rect.y, Screen.width * .8f, height: EditorGUIUtility.singleLineHeight),
                element, new GUIContent("GameEvent"), true);
        }

        private float ElementHeightCallback(int index) {
            float propertyHeight =
                EditorGUI.GetPropertyHeight(reorderableList.serializedProperty.GetArrayElementAtIndex(index), true);

            float spacing = EditorGUIUtility.singleLineHeight / 2;

            return propertyHeight + spacing;
        }

        private void OnAddCallback(ReorderableList list) {
            GameEvent gameEvent = new GameEvent();
            gameEvent = CreateInstance<GameEvent>();
            gameEvent.name = "EVT_Event_" + list.count;
            gameEvent.SetGameEventStorage(target as GameEventStorage);
            AssetDatabase.AddObjectToAsset(gameEvent, Selection.activeObject);
            AssetDatabase.SaveAssets();
            (target as GameEventStorage).UpdateGameEventsList();
        }

        private void OnRemoveCallback(ReorderableList list) {
            Object gameEvent = (target as GameEventStorage).GameEvents[list.index];
            DestroyImmediate(gameEvent, true);
            AssetDatabase.SaveAssets();
            (target as GameEventStorage).UpdateGameEventsList();
        }

    }
}