using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class MaterialGradientDrawer : MaterialPropertyDrawer {
    private readonly int _resolution = 256;

    public MaterialGradientDrawer() { }

    public MaterialGradientDrawer(float res) {
        _resolution = (int)res;
    }

    private string TextureName(MaterialProperty prop) => $"z_{prop.name}Tex";

    public override void OnGUI(Rect position, MaterialProperty prop, string label, MaterialEditor editor) {
        var guiContent = new GUIContent(label);
        OnGUI(position, prop, guiContent, editor);
    }

    public void OnGUI(Rect position, MaterialProperty prop, string label, MaterialEditor editor, string tooltip) {
        var guiContent = new GUIContent(label, tooltip);
        OnGUI(position, prop, guiContent, editor);
    }

    public override void OnGUI(Rect position, MaterialProperty prop, GUIContent label, MaterialEditor editor) {
        if (prop.type != MaterialProperty.PropType.Texture) {
            EditorGUI.HelpBox(position, $"[Gradient] used on non-texture property \"{prop.name}\"", MessageType.Error);
            return;
        }

        if (!AssetDatabase.Contains(prop.targets.FirstOrDefault())) {
            EditorGUI.HelpBox(position, $"Material \"{prop.targets.FirstOrDefault()?.name}\" is not an Asset.",
                              MessageType.Error);
            return;
        }

        var textureName = TextureName(prop);

        Gradient currentGradient = null;
        if (prop.targets.Length == 1) {
            var target = (Material)prop.targets[0];
            var path = AssetDatabase.GetAssetPath(target);
            var textureAsset = LoadTexture(path, textureName);
            if (textureAsset != null) {
                currentGradient = Decode(prop, textureAsset.name);
            }

            bool materialReset = target.GetTexture(prop.name) == null;
            if (currentGradient == null || materialReset) {
                // Create the default gradient.
                var colorKeys = new GradientColorKey[2];
                var alphaKeys = new GradientAlphaKey[2];
                colorKeys[0] = new GradientColorKey(Color.white, 0f);
                alphaKeys[0] = new GradientAlphaKey(1, 0f);
                colorKeys[1] = new GradientColorKey(Color.white, 1f);
                alphaKeys[1] = new GradientAlphaKey(1, 1f);
                currentGradient = new Gradient { colorKeys = colorKeys, alphaKeys = alphaKeys };
            }

            EditorGUI.showMixedValue = false;
        } else {
            EditorGUI.showMixedValue = true;
        }

        using (var changeScope = new EditorGUI.ChangeCheckScope()) {
            EditorGUILayout.Space(-18);
            currentGradient = EditorGUILayout.GradientField(label, currentGradient);

            if (changeScope.changed) {
                string encodedGradient = Encode(currentGradient);
                string fullAssetName = textureName + encodedGradient;
                foreach (Object target in prop.targets) {
                    if (!AssetDatabase.Contains(target)) {
                        continue;
                    }

                    var path = AssetDatabase.GetAssetPath(target);
                    var filterMode = currentGradient.mode == GradientMode.Blend
                        ? FilterMode.Bilinear
                        : FilterMode.Point;
                    var textureAsset = GetTexture(path, textureName, filterMode);
                    Undo.RecordObject(textureAsset, "Change Material Gradient");
                    textureAsset.name = fullAssetName;
                    BakeGradient(currentGradient, textureAsset);

                    var material = (Material)target;
                    material.SetTexture(prop.name, textureAsset);
                    EditorUtility.SetDirty(material);
                }
            }
        }

        EditorGUI.showMixedValue = false;
    }

    private Texture2D GetTexture(string path, string name, FilterMode filterMode) {
        var textureAsset = LoadTexture(path, name);

        if (textureAsset == null) {
            textureAsset = CreateTexture(path, name, filterMode);
        }

        // Force set filter mode for legacy materials.
        textureAsset.filterMode = filterMode;

        if (textureAsset.width != _resolution) {
#if UNITY_2021_2_OR_NEWER
            textureAsset.Reinitialize(_resolution, 1);
#else
            textureAsset.Resize(_resolution, 1);
#endif
        }

        return textureAsset;
    }

    private Texture2D CreateTexture(string path, string name, FilterMode filterMode) {
        var textureAsset = new Texture2D(_resolution, 1, TextureFormat.ARGB32, false) {
            name = name, wrapMode = TextureWrapMode.Clamp, filterMode = filterMode
        };
        AssetDatabase.AddObjectToAsset(textureAsset, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.ImportAsset(path);
        return textureAsset;
    }

    private string Encode(Gradient gradient) {
        return gradient == null ? null : JsonUtility.ToJson(new GradientRepresentation(gradient));
    }

    private Gradient Decode(MaterialProperty prop, string name) {
        if (prop == null) {
            return null;
        }

        string json = name.Substring(TextureName(prop).Length);
        try {
            var gradientRepresentation = JsonUtility.FromJson<GradientRepresentation>(json);
            return gradientRepresentation?.ToGradient();
        }
        catch (Exception e) {
            Debug.Log($"[Quibli] Bypass decoding a gradient. Debug info: {json} - {e}");
            return null;
        }
    }

    private Texture2D LoadTexture(string path, string name) {
        return AssetDatabase.LoadAllAssetsAtPath(path)
            .FirstOrDefault(asset => asset.name.StartsWith(name)) as Texture2D;
    }

    private void BakeGradient(Gradient gradient, Texture2D texture) {
        if (gradient == null) {
            return;
        }

        for (int x = 0; x < texture.width; x++) {
            var color = gradient.Evaluate((float)x / (texture.width - 1));
            for (int y = 0; y < texture.height; y++) {
                texture.SetPixel(x, y, color);
            }
        }

        texture.Apply();
    }

    [Serializable]
    class GradientRepresentation {
        public GradientMode mode;
        public ColorKey[] colorKeys;
        public AlphaKey[] alphaKeys;

        public GradientRepresentation() { }

        public GradientRepresentation(Gradient source) {
            FromGradient(source);
        }

        public void FromGradient(Gradient source) {
            mode = source.mode;
            colorKeys = source.colorKeys.Select(key => new ColorKey(key)).ToArray();
            alphaKeys = source.alphaKeys.Select(key => new AlphaKey(key)).ToArray();
        }

        public void ToGradient(Gradient gradient) {
            gradient.mode = mode;
            gradient.colorKeys = colorKeys.Select(key => key.ToGradientKey()).ToArray();
            gradient.alphaKeys = alphaKeys.Select(key => key.ToGradientKey()).ToArray();
        }

        public Gradient ToGradient() {
            var gradient = new Gradient();
            ToGradient(gradient);
            return gradient;
        }

        [Serializable]
        public struct ColorKey {
            public Color color;
            public float time;

            public ColorKey(GradientColorKey source) {
                color = default;
                time = default;
                FromGradientKey(source);
            }

            public void FromGradientKey(GradientColorKey source) {
                color = source.color;
                time = source.time;
            }

            public GradientColorKey ToGradientKey() {
                GradientColorKey key;
                key.color = color;
                key.time = time;
                return key;
            }
        }

        [Serializable]
        public struct AlphaKey {
            public float alpha;
            public float time;

            public AlphaKey(GradientAlphaKey source) {
                alpha = default;
                time = default;
                FromGradientKey(source);
            }

            public void FromGradientKey(GradientAlphaKey source) {
                alpha = source.alpha;
                time = source.time;
            }

            public GradientAlphaKey ToGradientKey() {
                GradientAlphaKey key;
                key.alpha = alpha;
                key.time = time;
                return key;
            }
        }
    }
}