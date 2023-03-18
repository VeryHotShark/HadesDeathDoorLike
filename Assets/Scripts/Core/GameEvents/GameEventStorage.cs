using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace VHS {
    [CreateAssetMenu(menuName = "Hyperstrange/GameEventStorage")]
    public class GameEventStorage : ScriptableObject {

        public List<Object> GameEvents = new List<Object>();

#if UNITY_EDITOR
        public void UpdateGameEventsList() {
            GameEvents.Clear();
            Object[] children = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(Selection.activeObject));

            foreach (Object child in children) {
                if (child == Selection.activeObject) continue;
                GameEvents.Add(child);
            }
        }
#endif

    }
}