using UnityEditor;
using UnityEngine;
using FIMSpace.FEditor;

namespace FIMSpace.FSeamlessGenerator
{
    public class FSeamlessWindow : FTextureProcessWindow
    {
        public enum FESeamlessAxis { XY, X, Y }
        public enum FEStampMode { Stamping, SplatMode, NoStamping }

        private float hardness = 0.6f;
        private float randomize = 0.25f;
        private float stamperRadius = 0.45f;
        private float stampDensity = 0.4f;
        private float stampNoiseMask = 1.0f;
        private int stampRotate = 1;

        private FEStampMode stampMode = FEStampMode.Stamping;
        private FESeamlessAxis toLoop = FESeamlessAxis.XY;


        public static void Init()
        {
            FSeamlessWindow window = (FSeamlessWindow)GetWindow(typeof(FSeamlessWindow));
            window.titleContent = new GUIContent("Seamless Generator", "Stamp sides of the texture or splat stamps");
            window.Show();
            called = true;
        }

        protected override void OnGUICustom()
        {
            EditorGUI.BeginChangeCheck();

            seed = EditorGUILayout.IntSlider(new GUIContent("Seed"), seed, -100, 100);
            stampMode = (FEStampMode)EditorGUILayout.EnumPopup("Stmap Mode", stampMode);

            if (stampMode != FEStampMode.NoStamping)
            {
                if (stampMode == FEStampMode.Stamping)
                {
                    stamperRadius = EditorGUILayout.Slider(new GUIContent("Stamp Radius"), stamperRadius, 0f, 1f);
                    stampDensity = EditorGUILayout.Slider(new GUIContent("Stamp Density"), stampDensity, 0f, 1f);
                    hardness = EditorGUILayout.Slider(new GUIContent("Hardness"), hardness, 0.0f, 1f);
                    stampNoiseMask = EditorGUILayout.Slider(new GUIContent("Stamp Noise"), stampNoiseMask, 0.0f, 2f);
                    randomize = EditorGUILayout.Slider(new GUIContent("Randomize"), randomize, 0.0f, .5f);
                    stampRotate = EditorGUILayout.IntSlider(new GUIContent("Rotate"), stampRotate, 0, 360);
                    toLoop = (FESeamlessAxis)EditorGUILayout.EnumPopup(new GUIContent("Dimensions to loop"), toLoop);
                }
                else
                {
                    stamperRadius = EditorGUILayout.Slider(new GUIContent("Stamp Radius"), stamperRadius, 0f, 1f);
                    stampDensity = EditorGUILayout.Slider(new GUIContent("Stamp Density"), stampDensity, -1f, 1f);
                    hardness = EditorGUILayout.Slider(new GUIContent("Hardness"), hardness, 0.0f, 2f);
                    stampNoiseMask = EditorGUILayout.Slider(new GUIContent("Stamp Noise"), stampNoiseMask, 0.0f, 3f);
                    randomize = EditorGUILayout.Slider(new GUIContent("Randomize"), randomize, 0.0f, 1f);
                    stampRotate = EditorGUILayout.IntSlider(new GUIContent("Rotate"), stampRotate, 0, 360);
                    toLoop = FESeamlessAxis.XY;
                }

                FGUI_Inspector.DrawUILine(Color.white * 0.35f, 2, 4);
            }

            if (EditorGUI.EndChangeCheck())
                somethingChanged = true;
            else
                somethingChanged = false;
        }


        protected override void ProcessTexture(Texture2D source, Texture2D target, bool preview = true)
        {
            if (stampMode == FEStampMode.NoStamping)
            {
                base.ProcessTexture(source, target, preview);
                return;
            }

            if (!preview) EditorUtility.DisplayProgressBar("Generating Seamless Texture...", "Preparing... ", 2f / 5f);

            #region Preparing variables to use down below

            Color32[] sourcePixels = source.GetPixels32();
            Color32[] newPixels = source.GetPixels32();

            if (source.width != target.width || source.height != target.height)
            {
                Debug.LogError("[SEAMLESS GENERATOR] Source texture is different scale or target texture! Can't create seamless texture!");
                return;
            }

            bool doX = toLoop != FESeamlessAxis.X;
            bool doY = toLoop != FESeamlessAxis.Y;

            Vector2 dimensions = GetDimensions(source);

            #endregion

            #region Stamping Texture

            if (!preview)
                EditorUtility.DisplayProgressBar("Generating Seamless Texture...", "Creating stamps... ", 3f / 5f);

            float stampRadiusX = source.width * Mathf.LerpUnclamped(0.05f, .3f, stamperRadius);
            float stampRadiusY = source.height * Mathf.LerpUnclamped(0.05f, .3f, stamperRadius);

            float stampsOffsetX = stampRadiusX * Mathf.LerpUnclamped(1.45f, 0.45f, stampDensity);
            float stampsOffsetY = stampRadiusY * Mathf.LerpUnclamped(1.45f, 0.45f, stampDensity);

            if (doX)
            {
                int stampsCountX = (int)(source.width / (stampsOffsetX));
                for (int x = 0; x <= stampsCountX; x++)
                {
                    Color32[] stamp = GetStamp(source, sourcePixels, (int)stampRadiusX);
                    Vector2 pastePos = new Vector2(0, 0);

                    pastePos.x = (int)(x * stampsOffsetX);
                    // Randomize y
                    float boost = 1f;
                    if (stampMode == FEStampMode.SplatMode) boost = 1f / (0.01f + stamperRadius);
                    pastePos.y = (int)((stampRadiusY * 2) * (-1f + rand.NextDouble() * 2f) * 0.5f * randomize * boost);

                    PasteTo(stamp, newPixels, pastePos, GetSquareDimensions(stamp.Length), dimensions);
                }
            }

            if (!preview)
                EditorUtility.DisplayProgressBar("Generating Seamless Texture...", "Creating stamps... ", 3.5f / 5f);

            if (doY)
            {
                int stampsCountY = (int)(source.width / (stampsOffsetY));
                for (int y = 0; y <= stampsCountY; y++)
                {
                    Color32[] stamp = GetStamp(source, sourcePixels, (int)stampRadiusY);
                    Vector2 pastePos = new Vector2(0, 0);

                    // Randomize x
                    float boost = 1f;
                    if (stampMode == FEStampMode.SplatMode) boost = 1f / (0.01f + stamperRadius);
                    pastePos.x = (int)((stampRadiusX * 2) * (-1f + rand.NextDouble() * 2f) * 0.5f * randomize * boost);
                    pastePos.y = (int)(y * stampsOffsetY);

                    PasteTo(stamp, newPixels, pastePos, GetSquareDimensions(stamp.Length), dimensions);
                }
            }

            #endregion

            // Finalizing changes
            if (!preview) EditorUtility.DisplayProgressBar("Generating Seamless Texture...", "Applying Seamless Texture... ", 3.85f / 5f);

            target.SetPixels32(newPixels);
            target.Apply(false, false);

            if (!preview)
                EditorUtility.ClearProgressBar();
        }


        #region Texture Small Operations

        private Color32[] GetStamp(Texture2D source, Color32[] sourcePixels, int radius)
        {
            double randD = -0.2 + rand.NextDouble() * 1.2;
            if (randomize > 0)
            {
                int tRad = (int)((float)radius * (1f + (randD * (randomize * 1f))));
                if (radius < source.width && radius < source.height) radius = tRad;
            }

            int width = radius * 2;
            Color32[] stampPixels = new Color32[width * width];

            Vector2 origin = new Vector2
            {
                x = RandomRange(radius, source.width - radius),
                y = RandomRange(radius, source.height - radius)
            };

            Vector2 stampDim = new Vector2(width, width);
            Vector2 dimensions = GetDimensions(source);

            float randomOff = (float)rand.NextDouble() * radius * 512f;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < width; y++)
                {
                    int xx = -radius + x;
                    int yy = -radius + y;
                    int i = GetPX(x, y, stampDim);
                    stampPixels[i] = sourcePixels[GetPX((int)origin.x + xx, (int)origin.y + yy, dimensions)];

                    float distance = Vector2.Distance(new Vector2(xx, yy), Vector2.zero);
                    float fadeMul = distance / ((float)radius * 0.95f);
                    stampPixels[i].a = System.Convert.ToByte(Mathf.Min(255, Mathf.Lerp(255 + hardness * 215, 0, fadeMul)));

                    // Applying perlin noise to stamp
                    if (stampNoiseMask > 0f)
                    {
                        if (stampPixels[i].a < 235 + hardness * 15 + stampNoiseMask * 10)
                        {
                            float noise = Mathf.PerlinNoise((float)x / radius * 3f + randomOff, (float)y / radius * 3f + randomOff);

                            float spreadAlpha = Mathf.Lerp(1f, 0f, Mathf.InverseLerp(255, 0, (int)stampPixels[i].a));

                            float noiseMask = stampNoiseMask;
                            if (stampNoiseMask > 1f)
                            {
                                float tA = Mathf.Lerp(255 + Mathf.LerpUnclamped(hardness * 215, 0, noiseMask - 1f), 0, fadeMul);
                                spreadAlpha = Mathf.Lerp(1f, 0f, Mathf.InverseLerp(255, 0, tA));
                                noiseMask -= 1f;
                            }

                            float noiseAlpha = Mathf.LerpUnclamped(1f, noise, noiseMask * 0.95f);
                            noiseAlpha = Mathf.Lerp(noiseAlpha, noiseAlpha * noiseAlpha, noiseMask - 0.5f);
                            noiseAlpha = Mathf.Lerp(1f, noiseAlpha, (1f - (spreadAlpha)));

                            stampPixels[i].a = (byte)(spreadAlpha * (noiseAlpha) * 255);
                        }
                    }
                }
            }

            // Rotating
            if (stampRotate >= 1f)
            {
                RotateImage(stampPixels, width, width, RandomRange(0, stampRotate));
            }


            return stampPixels;
        }


        /// <summary>
        /// Pasting texture on another in certain place
        /// </summary>
        private void PasteTo(Color32[] toPaste, Color32[] target, Vector2 origin, Vector2 toPasteDim, Vector2 targetDim)
        {
            for (int x = 0; x < toPasteDim.x; x++)
            {
                for (int y = 0; y < toPasteDim.y; y++)
                {
                    int index = GetPXLoop((int)(origin.x - toPasteDim.x / 2 + x), (int)(origin.y - toPasteDim.y / 2 + y), targetDim);
                    int toP = GetPX(x, y, toPasteDim);
                    target[index] = BlendPixel(target[index], toPaste[toP]);
                }
            }
        }


        #endregion


    }
}