using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace Quibli {
public class QuibliEditor : BaseShaderGUI {
    private Material _target;
    private MaterialProperty[] _properties;

    private static readonly Dictionary<string, bool> FoldoutStates = new Dictionary<string, bool> {{"Rendering options", false}};

    void DrawStandard(MaterialEditor editor, MaterialProperty property) {
        string displayName = property.displayName;

        // Remove everything in brackets.
        displayName = Regex.Replace(displayName, @" ?\[.*?\]", string.Empty);
        displayName = Regex.Replace(displayName, @" ?\{.*?\}", string.Empty);

        var tooltip = Tooltips.Get(editor, displayName);
        var guiContent = new GUIContent(displayName, tooltip);

        if (property.type == MaterialProperty.PropType.Texture && !property.displayName.Contains("Gradient")
            && !property.name.Contains("Ramp")) {
            if (!property.name.Contains("_BaseMap") && !property.name.Contains("_EmissionMap")) {
                EditorGUILayout.Space(15);
            }

            materialEditor.TexturePropertySingleLine(guiContent, property);
        } else {
            materialEditor.ShaderProperty(property, guiContent);
        }
    }

    MaterialProperty FindProperty(string name) {
        return FindProperty(name, _properties);
    }

    bool HasProperty(string name) {
        return _target != null && _target.HasProperty(name);
    }


#if UNITY_2021_2_OR_NEWER
    public override void ValidateMaterial(Material material) {
#else
    public override void MaterialChanged(Material material) {
#endif
        if (material == null) throw new ArgumentNullException(nameof(material));
        SetMaterialKeywords(material);
    }

    public override void OnGUI(MaterialEditor editor, MaterialProperty[] properties) {
        materialEditor = editor;
        _properties = properties;
        _target = editor.target as Material;
        Debug.Assert(_target != null);
        
        FindProperties(properties);

        if (_target.IsKeywordEnabled("DR_OUTLINE_ON") && _target.IsKeywordEnabled("_ALPHATEST_ON")) {
            EditorGUILayout.HelpBox("The 'Outline' and 'Alpha Clip' features are usually " +
                                    "incompatible. The outline shader pass will not be using alpha " +
                                    "clipping.", MessageType.Warning);
        }

        int originalIntentLevel = EditorGUI.indentLevel;
        int foldoutRemainingItems = 0;
        bool latestFoldoutState = false;

        foreach (MaterialProperty property in properties) {
            string displayName = property.displayName;

            if (displayName.Contains("[") && !displayName.Contains("FOLDOUT")) {
                EditorGUI.indentLevel += 1;
            }

            var skipProperty = false;
            foreach (Match match in Regex.Matches(displayName, @" ?\[DR_.*?\]")) {
                var keyword = match.Value.Replace("[", string.Empty).Replace("]", string.Empty);
                skipProperty |= !_target.IsKeywordEnabled(keyword);
            }

            if (_target.IsKeywordEnabled("DR_ENABLE_LIGHTMAP_DIR") &&
                displayName.ToLower().Contains("override light direction")) {
                var dirPitch = _target.GetFloat("_LightmapDirectionPitch");
                var dirYaw = _target.GetFloat("_LightmapDirectionYaw");

                var dirPitchRad = dirPitch * Mathf.Deg2Rad;
                var dirYawRad = dirYaw * Mathf.Deg2Rad;

                var direction = new Vector4(Mathf.Sin(dirPitchRad) * Mathf.Sin(dirYawRad), Mathf.Cos(dirPitchRad),
                                            Mathf.Sin(dirPitchRad) * Mathf.Cos(dirYawRad), 0.0f);
                _target.SetVector("_LightmapDirection", direction);
            }

            // TODO: Disable texture impact via keyword.
            if (_target.HasProperty("_TextureImpact") && _target.HasProperty("_BaseMap") &&
                _target.GetTexture("_BaseMap") == null) {
                _target.SetFloat("_TextureImpact", 0f);
            }

            if (displayName.Contains("FOLDOUT")) {
                string foldoutName = displayName.Split('(', ')')[1];
                string foldoutItemCount = displayName.Split('{', '}')[1];
                foldoutRemainingItems = Convert.ToInt32(foldoutItemCount);
                if (!FoldoutStates.ContainsKey(property.name)) {
                    FoldoutStates.Add(property.name, false);
                }

                EditorGUILayout.Space();
                FoldoutStates[property.name] = EditorGUILayout.Foldout(FoldoutStates[property.name], foldoutName);
                latestFoldoutState = FoldoutStates[property.name];
            }

            if (foldoutRemainingItems > 0) {
                skipProperty = skipProperty || !latestFoldoutState;
                EditorGUI.indentLevel += 1;
                --foldoutRemainingItems;
            }

            bool hideInInspector = (property.flags & MaterialProperty.PropFlags.HideInInspector) != 0;
            if (!hideInInspector && !skipProperty) {
                EditorGUI.BeginChangeCheck();
                DrawStandard(editor, property);
                if (EditorGUI.EndChangeCheck()) {
#if UNITY_2021_2_OR_NEWER
                    ValidateMaterial(_target);
#else
                    MaterialChanged(_target);
#endif
                }
            }

            if (!skipProperty && property.name.Contains("_EmissionMap")) {
                EditorGUILayout.Space(15);
                DrawEmissionProperties(_target, true);

                EditorGUILayout.Space(15);
                DrawTileOffset(editor, FindProperty("_BaseMap"));
            }

            EditorGUI.indentLevel = originalIntentLevel;
        }

        EditorGUILayout.Space();
        FoldoutStates["Rendering options"] =
            EditorGUILayout.Foldout(FoldoutStates["Rendering options"], "Rendering options");

        if (FoldoutStates["Rendering options"]) {
            EditorGUI.indentLevel += 1;

            HandleUrpSettings(_target, materialEditor);

            EditorGUILayout.Space();
            materialEditor.EnableInstancingField();
        }

        // Toggle the outline pass.
        _target.SetShaderPassEnabled("SRPDefaultUnlit", _target.IsKeywordEnabled("DR_OUTLINE_ON"));
    }

    // Adapted from BaseShaderGUI.cs.
    private void HandleUrpSettings(Material material, MaterialEditor materialEditor) {
        bool alphaClip = false;
        if (material.HasProperty("_AlphaClip")) {
            alphaClip = material.GetFloat("_AlphaClip") >= 0.5;
        }

        if (alphaClip) {
            material.EnableKeyword("_ALPHATEST_ON");
        } else {
            material.DisableKeyword("_ALPHATEST_ON");
        }

        if (HasProperty("_Surface")) {
            EditorGUI.BeginChangeCheck();
            var surfaceProp = FindProperty("_Surface");
            EditorGUI.showMixedValue = surfaceProp.hasMixedValue;
            var surfaceType = (SurfaceType) surfaceProp.floatValue;
            EditorGUILayout.Separator();
            surfaceType = (SurfaceType) EditorGUILayout.EnumPopup("Surface Type", surfaceType);
            if (EditorGUI.EndChangeCheck()) {
                materialEditor.RegisterPropertyChangeUndo("Surface Type");
                surfaceProp.floatValue = (float) surfaceType;
            }

            if (surfaceType == SurfaceType.Opaque) {
                if (alphaClip) {
                    material.renderQueue = (int) UnityEngine.Rendering.RenderQueue.AlphaTest;
                    material.SetOverrideTag("RenderType", "TransparentCutout");
                } else {
                    material.renderQueue = (int) UnityEngine.Rendering.RenderQueue.Geometry;
                    material.SetOverrideTag("RenderType", "Opaque");
                }

                material.renderQueue +=
                    material.HasProperty("_QueueOffset") ? (int) material.GetFloat("_QueueOffset") : 0;
                material.SetInt("_SrcBlend", (int) UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int) UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.SetShaderPassEnabled("ShadowCaster", true);
            } else // Transparent
            {
                BlendMode blendMode = (BlendMode) material.GetFloat("_Blend");

                // Specific Transparent Mode Settings
                switch (blendMode) {
                    case BlendMode.Alpha:
                        material.SetInt("_SrcBlend", (int) UnityEngine.Rendering.BlendMode.SrcAlpha);
                        material.SetInt("_DstBlend", (int) UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                        break;
                    case BlendMode.Premultiply:
                        material.SetInt("_SrcBlend", (int) UnityEngine.Rendering.BlendMode.One);
                        material.SetInt("_DstBlend", (int) UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                        material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                        break;
                    case BlendMode.Additive:
                        material.SetInt("_SrcBlend", (int) UnityEngine.Rendering.BlendMode.SrcAlpha);
                        material.SetInt("_DstBlend", (int) UnityEngine.Rendering.BlendMode.One);
                        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                        break;
                    case BlendMode.Multiply:
                        material.SetInt("_SrcBlend", (int) UnityEngine.Rendering.BlendMode.DstColor);
                        material.SetInt("_DstBlend", (int) UnityEngine.Rendering.BlendMode.Zero);
                        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                        material.EnableKeyword("_ALPHAMODULATE_ON");
                        break;
                }

                // General Transparent Material Settings
                material.SetOverrideTag("RenderType", "Transparent");
                material.SetInt("_ZWrite", 0);
                material.renderQueue = (int) UnityEngine.Rendering.RenderQueue.Transparent;
                material.renderQueue +=
                    material.HasProperty("_QueueOffset") ? (int) material.GetFloat("_QueueOffset") : 0;
                material.SetShaderPassEnabled("ShadowCaster", false);
            }

            // DR: draw popup.
            if (surfaceType == SurfaceType.Transparent && HasProperty("_Blend")) {
                EditorGUI.BeginChangeCheck();
                var blendModeProp = FindProperty("_Blend");
                EditorGUI.showMixedValue = blendModeProp.hasMixedValue;
                var blendMode = (BlendMode) blendModeProp.floatValue;
                blendMode = (BlendMode) EditorGUILayout.EnumPopup("Blend Mode", blendMode);
                if (EditorGUI.EndChangeCheck()) {
                    materialEditor.RegisterPropertyChangeUndo("Blend Mode");
                    blendModeProp.floatValue = (float) blendMode;
                }
            }
        }

        // DR: draw popup.
        if (HasProperty("_Cull")) {
            EditorGUILayout.Separator();
            EditorGUI.BeginChangeCheck();
            var cullingProp = FindProperty("_Cull");
            EditorGUI.showMixedValue = cullingProp.hasMixedValue;
            var culling = (RenderFace) cullingProp.floatValue;
            culling = (RenderFace) EditorGUILayout.EnumPopup("Render Faces", culling);
            if (EditorGUI.EndChangeCheck()) {
                materialEditor.RegisterPropertyChangeUndo("Render Faces");
                cullingProp.floatValue = (float) culling;
                material.doubleSidedGI = (RenderFace) cullingProp.floatValue != RenderFace.Front;
            }
        }

        if (HasProperty("_AlphaClip")) {
            EditorGUILayout.Separator();
            EditorGUI.BeginChangeCheck();
            var alphaClipProp = FindProperty("_AlphaClip");
            EditorGUI.showMixedValue = alphaClipProp.hasMixedValue;
            var alphaClipEnabled = EditorGUILayout.Toggle("Alpha Clipping", alphaClipProp.floatValue == 1);
            if (EditorGUI.EndChangeCheck()) alphaClipProp.floatValue = alphaClipEnabled ? 1 : 0;
            EditorGUI.showMixedValue = false;

            if (alphaClipProp.floatValue == 1 && HasProperty("_Cutoff")) {
                var alphaCutoffProp = FindProperty("_Cutoff");
                materialEditor.ShaderProperty(alphaCutoffProp, "Threshold", 1);
            }
        }
    }

    // Adapted from BaseShaderGUI.cs.
    private new static void SetMaterialKeywords(Material material, Action<Material> shadingModelFunc = null,
                                                Action<Material> shaderFunc = null) {
        // Setup blending - consistent across all Universal RP shaders
        SetupMaterialBlendMode(material);

        // Receive Shadows
        if (material.HasProperty("_ReceiveShadows"))
            CoreUtils.SetKeyword(material, "_RECEIVE_SHADOWS_OFF", material.GetFloat("_ReceiveShadows") == 0.0f);

        // Emission
        if (material.HasProperty("_EmissionColor")) MaterialEditor.FixupEmissiveFlag(material);
        bool shouldEmissionBeEnabled =
            (material.globalIlluminationFlags & MaterialGlobalIlluminationFlags.EmissiveIsBlack) == 0;
        if (material.HasProperty("_EmissionEnabled") && !shouldEmissionBeEnabled)
            shouldEmissionBeEnabled = material.GetFloat("_EmissionEnabled") >= 0.5f;
        CoreUtils.SetKeyword(material, "_EMISSION", shouldEmissionBeEnabled);

        // Normal Map
        if (material.HasProperty("_BumpMap"))
            CoreUtils.SetKeyword(material, "_NORMALMAP", material.GetTexture("_BumpMap"));

        // Shader specific keyword functions
        shadingModelFunc?.Invoke(material);
        shaderFunc?.Invoke(material);
    }
}
}
