using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector.Editor.Drawers;
using UnityEditor;
using UnityEngine;
using VHS;

public class DraggablePointAttributeDrawer : OdinAttributeDrawer<DraggablePointAttribute, Vector3>, IDisposable {

    protected override void DrawPropertyLayout(GUIContent label) {
        CallNextDrawer(null);
    }

    protected override void Initialize() {
        UnityEditor.SceneView.duringSceneGui += DrawHandle;
    }

    private void DrawHandle(SceneView sceneview) {
        this.ValueEntry.SmartValue = Handles.DoPositionHandle(this.ValueEntry.SmartValue,Quaternion.identity);
    }

    public void Dispose() {
        UnityEditor.SceneView.duringSceneGui -= DrawHandle;
    }
}
