using System.Text.RegularExpressions;
using Quibli;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class CustomDrawersShaderEditor : ShaderGUI {
    private readonly MaterialGradientDrawer _gradientDrawer = new MaterialGradientDrawer();
    private readonly MaterialVector2Drawer _vectorDrawer = new MaterialVector2Drawer();

    public override void OnGUI(MaterialEditor editor, MaterialProperty[] properties) {
        foreach (var property in properties) {
            bool hideInInspector = (property.flags & MaterialProperty.PropFlags.HideInInspector) != 0;
            if (hideInInspector) {
                continue;
            }

            var displayName = property.displayName;
            var tooltip = Tooltips.Get(editor, displayName);

            if (displayName.Contains("[Header]")) {
                DrawHeader(property, tooltip);
                continue;
            }

            if (displayName.Contains("[Space]")) {
                EditorGUILayout.Space();
                continue;
            }

            if (displayName.ToLower().Contains("hide")) {
                continue;
            }

            if (displayName.Contains("[s]")) {
                EditorGUILayout.Space();
            }

            displayName = HandleTabs(displayName);
            displayName = RemoveEverythingInBrackets(displayName);

            if (property.type == MaterialProperty.PropType.Texture && property.name.Contains("GradientTexture")) {
                EditorGUILayout.Space(18);
                _gradientDrawer.OnGUI(Rect.zero, property, property.displayName, editor, tooltip);
            } else if (property.type == MaterialProperty.PropType.Vector &&
                       property.displayName.Contains("[Vector2]")) {
                EditorGUILayout.Space(18);
                _vectorDrawer.OnGUI(Rect.zero, property, displayName, editor, tooltip);
            } else {
                var guiContent = new GUIContent(displayName, tooltip);
                editor.ShaderProperty(property, guiContent);
            }
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        if (SupportedRenderingFeatures.active.editableMaterialRenderQueue) editor.RenderQueueField();
        editor.EnableInstancingField();
        editor.DoubleSidedGIField();

        // Backwards compatibility from Quibli 1.4.0.
        {
            var material = (Material)editor.target;

            int wind = Shader.PropertyToID("Wind");
            if (material.HasProperty(wind) && material.GetFloat(wind) > 0.5f) {
                Debug.Log("[Quibli] Material upgrade - wind enabled.");
                material.EnableKeyword("_WIND");
                material.SetFloat("_WIND", 1);
                material.SetFloat(wind, 0f);
            }

            int fresnelPower = Shader.PropertyToID("Fresnel_Power");
            int persistence = Shader.PropertyToID("_FresnelToggleUpgrade");
            if (material.HasProperty(fresnelPower) && material.HasProperty(persistence) &&
                material.GetFloat(fresnelPower) > 0f && material.GetFloat(persistence) < 0.5f) {
                Debug.Log("[Quibli] Material upgrade - fresnel enabled.");
                material.SetFloat("_FRESNEL", 1);
                material.EnableKeyword("_FRESNEL");
                material.SetFloat(persistence, 1f);
            }
        }
    }

    private string HandleTabs(string displayName) {
        while (displayName.Contains("[t]")) {
            displayName = displayName.Replace("[t]", "    ");
        }

        return displayName;
    }

    void DrawHeader(MaterialProperty property, string tooltip) {
        EditorGUILayout.Space();
        string displayName = RemoveEverythingInBrackets(property.displayName);
        var guiContent = new GUIContent(displayName, tooltip);
        EditorGUILayout.LabelField(guiContent);
    }

    private string RemoveEverythingInBrackets(string s) {
        s = Regex.Replace(s, @" ?\[.*?\]", string.Empty);
        s = Regex.Replace(s, @" ?\{.*?\}", string.Empty);
        return s;
    }
}