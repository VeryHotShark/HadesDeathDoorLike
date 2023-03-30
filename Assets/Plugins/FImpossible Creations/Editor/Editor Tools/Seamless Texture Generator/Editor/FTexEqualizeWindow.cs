using UnityEditor;
using UnityEngine;

namespace FIMSpace.FSeamlessGenerator
{
    public class FTexEqualizeWindow : FTextureProcessWindow
    {
        private float Equalize = 0f;
        private float EqualizeWhites = 1f;
        private float EqualizeBlacks = 1f;
        private float EqualizeNoise = 1f;


        public static void Init()
        {
            FTexEqualizeWindow window = (FTexEqualizeWindow)GetWindow(typeof(FTexEqualizeWindow));
            window.titleContent = new GUIContent("Texure Equalize", "Tweak too bright or too dark parts of your texture");
            window.Show();
            window.previewScale = FEPreview.m_1x1;
            window.drawPreviewScale = true;
            called = true;
        }


        protected override void OnGUICustom()
        {
            EditorGUI.BeginChangeCheck();
            seed = EditorGUILayout.IntSlider(new GUIContent("Seed"), seed, -100, 100);
            Equalize = EditorGUILayout.Slider(new GUIContent("Equalize Amount"), Equalize, 0.0f, 1f);

            EditorGUI.indentLevel++;
            EqualizeWhites = EditorGUILayout.Slider(new GUIContent("Whites"), EqualizeWhites, 0f, 2f);
            EqualizeBlacks = EditorGUILayout.Slider(new GUIContent("Blacks"), EqualizeBlacks, 0f, 2f);
            EqualizeNoise = EditorGUILayout.Slider(new GUIContent("Texturize"), EqualizeNoise, 0f, 1f);
            if (EqualizeNoise > 0f) EditorGUILayout.HelpBox("Texture must be seamless for texturize!", MessageType.None);
            EditorGUI.indentLevel--;

            if (EditorGUI.EndChangeCheck())
                somethingChanged = true;
            else
                somethingChanged = false;
        }


        protected override void ProcessTexture(Texture2D source, Texture2D target, bool preview = true)
        {
            if (!preview) EditorUtility.DisplayProgressBar("Equalizing Texture...", "Preparing... ", 2f / 5f);


            #region Preparing variables to use down below

            Color32[] sourcePixels = source.GetPixels32();
            Color32[] newPixels = source.GetPixels32();

            if (source.width != target.width || source.height != target.height)
            {
                Debug.LogError("[SEAMLESS GENERATOR] Source texture is different scale or target texture! Can't create seamless texture!");
                return;
            }

            Vector2 dimensions = GetDimensions(source);

            #endregion


            #region Equalizing Texture

            if (!preview)
                EditorUtility.DisplayProgressBar("Equalizing...", "Equalizing... ", 3f / 5f);


            Vector3 rgbAverages = new Vector3(0.0f, 0.0f, 0.0f);
            Vector3 hsvAverages = new Vector3(0.0f, 0.0f, 0.0f);
            Vector2 dim = new Vector2(source.width, source.height);

            for (int x = 0; x < source.width; x++)
            {
                for (int y = 0; y < source.height; y++)
                {
                    int px = GetPX(x, y, dim);
                    Color c = sourcePixels[px];
                    rgbAverages.x += c.r;
                    rgbAverages.y += c.g;
                    rgbAverages.z += c.b;

                    float h, s, v;
                    Color.RGBToHSV(sourcePixels[px], out h, out s, out v);
                    hsvAverages.x += h;
                    hsvAverages.y += s;
                    hsvAverages.z += v;
                }
            }

            float norm = source.width * source.height;
            Vector3 rgbAveragesN = rgbAverages / norm;
            Vector3 hsvAveragesN = hsvAverages / norm;

            float highestVDiff = 0f;
            for (int x = 0; x < source.width; x++)
            {
                for (int y = 0; y < source.height; y++)
                {
                    int px = GetPX(x, y, dim);

                    float h, s, v;
                    Color.RGBToHSV(sourcePixels[px], out h, out s, out v);
                    hsvAverages.x += h;
                    hsvAverages.y += s;
                    hsvAverages.z += v;

                    float diff = Mathf.Abs(v - hsvAveragesN.z);
                    if (diff > highestVDiff) highestVDiff = diff;
                }
            }


            Color avgHsvCol = Color.HSVToRGB(hsvAveragesN.x, hsvAveragesN.y, hsvAveragesN.z);
            int randX = RandomRange(source.width / 2, source.width / 2 + (UnityEngine.Random.Range(0f, 1f) < 0.5f ? source.width / 3 : -source.width / 3));
            int randY = RandomRange(source.height / 2, source.height / 2 + (UnityEngine.Random.Range(0f, 1f) < 0.5f ? source.height / 3 : -source.height / 3));

            for (int x = 0; x < source.width; x++)
            {
                for (int y = 0; y < source.height; y++)
                {
                    int px = GetPX(x, y, dim);
                    float h, s, v;
                    Color.RGBToHSV(sourcePixels[px], out h, out s, out v);
                    float vDiff = v - hsvAveragesN.z;
                    float diffTexBase = Mathf.Abs(v - hsvAveragesN.z);

                    if (vDiff > 0)
                    {
                        // Whites lower then less changed
                        vDiff *= EqualizeWhites;
                    }
                    else
                    if (vDiff < 0)
                    {
                        // Blacks lower then less changed
                        vDiff *= EqualizeBlacks;
                    }

                    float lerpV = Mathf.Abs(vDiff) / highestVDiff;
                    float tgtV = Mathf.LerpUnclamped(v, hsvAveragesN.z, lerpV);
                    Color tgtColor = Color.HSVToRGB(h, s, tgtV);

                    if (EqualizeNoise > 0f)
                    {
                        Color offsetRefColor = sourcePixels[GetPXLoop(randX + x, randY + y, new Vector2(source.width, source.height))];
                        tgtColor = Color.Lerp(tgtColor, offsetRefColor, lerpV * 2f * EqualizeNoise);
                    }

                    newPixels[px] = Color32.LerpUnclamped(sourcePixels[px], tgtColor, Equalize);
                }
            }

            #endregion


            // Finalizing changes
            if (!preview) EditorUtility.DisplayProgressBar("Equalizing Texture...", "Applying Equalization to Texture... ", 3.85f / 5f);

            target.SetPixels32(newPixels);
            target.Apply(false, false);

            if (!preview)
                EditorUtility.ClearProgressBar();

        }


    }
}