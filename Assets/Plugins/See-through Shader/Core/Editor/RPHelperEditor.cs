using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static ShaderCrew.SeeThroughShader.GeneralUtils;

namespace ShaderCrew.SeeThroughShader
{
    [CustomEditor(typeof(RPHelper))]
    public class RPHelperEditor : Editor
    {

        bool m_FirstTimeApply = true;

        bool LinkLabel(GUIContent label)
        {
            GUIStyle m_LinkStyle = new GUIStyle(EditorStyles.label);
            m_LinkStyle.wordWrap = false;
            m_LinkStyle.normal.textColor = new Color(0x00 / 255f, 0x78 / 255f, 0xDA / 255f, 1f);
            m_LinkStyle.stretchWidth = false;
            m_LinkStyle.contentOffset = new Vector2(15, 0);
            m_LinkStyle.alignment = TextAnchor.MiddleCenter;
            return GUILayout.Button(label, m_LinkStyle);
        }

        Material[] mats;
        UnityVersionRenderPipelineShaderInfo unityVersionRenderPipelineShader;

        Dictionary<string, string> UnityToSTSShaderNameMapping;

        Dictionary<string, Shader> UnityToSTSShaderMapping;

        GUIStyle stepHeaderStyle;
        GUIStyle centerLabelStyle;
        GUIStyle textAreaStyle;
        GUIStyle m_LinkStyle;
        GUIStyle headerStyle;
        GUIStyle grayHeaderStyle;
        GUIStyle bigTextStyle;
        GUIStyle bigErrorStyle;
        GUIStyle textAreaBoldStyle;
        GUIStyle step2TextStyle;

        string richColor;

        Color buttonColor;


        public override void OnInspectorGUI()
        {


            if (m_FirstTimeApply)
            {
                m_FirstTimeApply = false;
                DoSetup();
                m_FirstTimeApply = false;
            }

            if (UnityToSTSShaderNameMapping == null) {
                UnityToSTSShaderNameMapping = GeneralUtils.getUnityToSTSShaderMapping();
            }

            if (UnityToSTSShaderMapping == null)
            {
                UnityToSTSShaderMapping = new Dictionary<string, Shader>();
            }
            foreach (string key in UnityToSTSShaderNameMapping.Keys.ToList())
            {
                Shader shader = Shader.Find(UnityToSTSShaderNameMapping[key]);
                UnityToSTSShaderMapping[key] = shader ?? Shader.Find(UnityToSTSShaderNameMapping[SeeThroughShaderConstants.STS_SHADER_DEFAULT_KEY]);
            }

            serializedObject.Update();
            EditorUtils.usualStart("Render Pipeline Setup Helper");

            EditorGUILayout.Space(5);


            GUILayout.Label("Welcome to the 'See-through Shader' Demo!", headerStyle);
            EditorUtils.DrawUILineGray(2, 0);

            EditorGUILayout.Space(10);
            GUILayout.TextArea("When you import the package for the first time, " +
                "it is set up for the 'Built-in' Render Pipeline. To ensure that it works with URP or HDRP, depending on which Render Pipeline you chose," +
                " we have to do some minor changes to the materials, i.e. assign the correct shader! " +
                "\nHere we will walk you through the needed steps. So let's get started! :)  ", textAreaStyle);
            EditorGUILayout.Space(10);
            EditorUtils.DrawUILineGray(2, 0);
            unityVersionRenderPipelineShader = getUnityVersionAndRenderPipelineCorrectedShaderString();
            EditorGUILayout.Space(10);


            //// hdrp test
            //unityVersionRenderPipelineShader.renderPipeline = "HDRP";
            //unityVersionRenderPipelineShader.versionAndRPCorrectedShader = "Custom/SeeThroughShaderHDRP2020";

            // urp test
            //unityVersionRenderPipelineShader.renderPipeline = "URP";
            //unityVersionRenderPipelineShader.versionAndRPCorrectedShader = "Custom/SeeThroughShaderURP2020";

            GUILayout.Label("RenderPipeline currently in use ", grayHeaderStyle);
            GUILayout.Label(unityVersionRenderPipelineShader.renderPipeline, bigTextStyle);


            EditorGUILayout.Space(5);


            bool isCorrectShaderUsed = true;
            mats = Resources.LoadAll("STSReferenceMaterials", typeof(Material)).Cast<Material>().ToArray();
            //Material[] mats2 = Resources.LoadAll("STSTreeMaterials", typeof(Material)).Cast<Material>().ToArray();
            //var concat = new Material[mats.Length + mats2.Length];
            //mats.CopyTo(concat, 0);
            //mats2.CopyTo(concat, mats.Length);
            //mats = concat;

            List<Material> matListNotCorrectShader = new List<Material>();
            if (mats != null)
            {
                foreach (Material mat in mats)
                {
                    //if (mat.shader.name != unityVersionRenderPipelineShader.versionAndRPCorrectedShader)
                    if (!UnityToSTSShaderNameMapping.Values.Contains(mat.shader.name))
                    {
                        isCorrectShaderUsed = false;
                        matListNotCorrectShader.Add(mat);
                    }
                }
            }
            else
            {
                Debug.LogError("Reference mats are empty. Please check if all your materials are inside resources/STSReferenceMaterials");
                isCorrectShaderUsed = false;
            }

            GUILayout.Label("Reference Materials Shader", grayHeaderStyle);
            //bigTextStyle.fontSize = 16;
            if (isCorrectShaderUsed)
            {
                GUILayout.Label(unityVersionRenderPipelineShader.shaderFolder, bigTextStyle);
                //foreach (string item in UnityToSTSShaderNameMapping.Keys)
                //{
                //    if(item != SeeThroughShaderConstants.STS_SHADER_DEFAULT_KEY)
                //    {
                //        GUILayout.Label(UnityToSTSShaderNameMapping[item], bigTextStyle);
                //    }
                //}
            }
            else
            {
                GUILayout.Label("One or more of your reference materials have the wrong shader assigned! \n Action is required!", bigErrorStyle);
                GUILayout.Label("Materials with incorrect shaders: ", bigErrorStyle);
                foreach (Material item in matListNotCorrectShader)
                {
                    GUILayout.Label(item.name + " with shader: \n" + item.shader.name);
                }
            }




            EditorGUILayout.Space(10);

            if (!isCorrectShaderUsed)
            {
                if ((unityVersionRenderPipelineShader.renderPipeline == "Built-in RP"))
                {
                    autoAssignShader(mats, unityVersionRenderPipelineShader);
                }
                else if ((unityVersionRenderPipelineShader.renderPipeline == "URP"))
                {
                    displayStepsForURPandHDRP(true);

                }
                else if ((unityVersionRenderPipelineShader.renderPipeline == "HDRP"))
                {
                    displayStepsForURPandHDRP(false);
                }
            }
            else
            {
                GUILayout.Label("Everything should work fine! No changes needed!", stepHeaderStyle);
            }

            EditorGUILayout.Space(10);

            //base.DrawDefaultInspector();
            EditorUtils.usualEnd();
            //EditorStyles.label.normal.textColor = oriCol;
            serializedObject.ApplyModifiedProperties();
        }
        void DoSetup()
        {
            m_LinkStyle = new GUIStyle(EditorStyles.label);
            m_LinkStyle.wordWrap = false;
            m_LinkStyle.stretchWidth = false;

            headerStyle = new GUIStyle();
            headerStyle.alignment = TextAnchor.MiddleCenter;
            headerStyle.fontStyle = FontStyle.Bold;
            headerStyle.fontSize = 15;

            grayHeaderStyle = new GUIStyle();
            grayHeaderStyle.fontSize = 14;
            grayHeaderStyle.alignment = TextAnchor.MiddleCenter;
            grayHeaderStyle.wordWrap = true;


            bigTextStyle = new GUIStyle();

            bigTextStyle.fontStyle = FontStyle.Bold;
            bigTextStyle.fontSize = 15;
            bigTextStyle.alignment = TextAnchor.MiddleCenter;

            bigErrorStyle = new GUIStyle();
            bigErrorStyle.fontStyle = FontStyle.Bold;
            bigErrorStyle.fontSize = 13;
            bigErrorStyle.alignment = TextAnchor.MiddleCenter;
            bigErrorStyle.wordWrap = true;

            textAreaStyle = new GUIStyle();

            textAreaStyle.padding.left = 20;
            textAreaStyle.padding.right = 20;
            //textAreaStyle.fontStyle = FontStyle.Bold;
            textAreaStyle.wordWrap = true;
            textAreaStyle.fontSize = 15;
            textAreaStyle.richText = true;

            textAreaBoldStyle = new GUIStyle();
            textAreaBoldStyle.normal.textColor = Color.white;
            textAreaBoldStyle.padding.left = 20;
            textAreaBoldStyle.padding.right = 20;
            textAreaBoldStyle.fontStyle = FontStyle.Bold;
            textAreaBoldStyle.wordWrap = true;
            textAreaBoldStyle.fontSize = 15;

            stepHeaderStyle = new GUIStyle();
            stepHeaderStyle.fontStyle = FontStyle.Bold;
            stepHeaderStyle.alignment = TextAnchor.MiddleCenter;
            //bigTextStyle2.fontSize = 16;
            centerLabelStyle = new GUIStyle();
            centerLabelStyle.normal.textColor = Color.gray;
            centerLabelStyle.alignment = TextAnchor.MiddleCenter;

            //textColor = Color.white;
            //oriCol = EditorStyles.label.normal.textColor;

            step2TextStyle = new GUIStyle();

            step2TextStyle.fontSize = 15;
            step2TextStyle.padding.left = 20;
            step2TextStyle.padding.right = 20;
            step2TextStyle.alignment = TextAnchor.MiddleCenter;
            step2TextStyle.wordWrap = true;


            if (EditorGUIUtility.isProSkin)
            {

                m_LinkStyle.normal.textColor = new Color(1f, 0.3f, 0.3f, 1);
                headerStyle.normal.textColor = Color.white;
                grayHeaderStyle.normal.textColor = new Color(0.7f, 0.7f, 0.7f, 1);
                textAreaStyle.normal.textColor = Color.white;
                bigErrorStyle.normal.textColor = new Color(1f, 0.3f, 0.3f, 1);
                richColor = "silver";
                stepHeaderStyle.normal.textColor = Color.white;
                step2TextStyle.normal.textColor = new Color(0.7f, 0.7f, 0.7f, 1);
                bigTextStyle.normal.textColor = Color.white;
                buttonColor = new Color(0.6f, 0.6f, 0.6f, 1);
            }
            else
            {
                m_LinkStyle.normal.textColor = new Color(1f, 0.3f, 0.3f, 1);
                headerStyle.normal.textColor = Color.black;
                grayHeaderStyle.normal.textColor = new Color(0.2f, 0.2f, 0.2f, 1);
                textAreaStyle.normal.textColor = Color.black;
                bigErrorStyle.normal.textColor = new Color(0.6f, 0.0f, 0.0f, 1);
                richColor = "#161616";
                stepHeaderStyle.normal.textColor = Color.black;
                step2TextStyle.normal.textColor = Color.black;
                bigTextStyle.normal.textColor = Color.black;
                buttonColor = new Color(0.8f, 0.8f, 0.8f, 1);
            }
        }
        private void autoAssignShader(Material[] mats, UnityVersionRenderPipelineShaderInfo unityVersionRenderPipelineShader)
        {
            GUILayout.Label("Automatically assign the correct shader to your Reference Materials", step2TextStyle);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            Color originalBackgroundColor = GUI.backgroundColor;
            GUI.backgroundColor = buttonColor;
            if (GUILayout.Button("Assign Shader", GUILayout.Width(100), GUILayout.Height(30)))
            {
                if (mats != null)
                {
                    foreach (Material mat in mats)
                    {
                        //if (mat.shader.name != unityVersionRenderPipelineShader.versionAndRPCorrectedShader)
                        if (!UnityToSTSShaderMapping.Values.Contains(mat.shader))
                        {
                            mat.shader = UnityToSTSShaderMapping.TryGetValue(mat.shader.name, out Shader value) ? value : UnityToSTSShaderMapping[SeeThroughShaderConstants.STS_SHADER_DEFAULT_KEY];
                        }
                    }
                }
                else
                {
                    Debug.LogError("Reference mats are empty. Can NOT do auto assign!");
                }
            }
            GUI.backgroundColor = originalBackgroundColor;
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }


        private void displayStepsForURPandHDRP(bool isURP)
        {

            string step1Text;
            string link;
            string commonNote = "\n\nIMPORTANT: <color=" + richColor + "> In case you added this asset to an existing project, please select all the materials inside </color>" +
                //"<size=12> \n\nSee-through Shader/ Core/ Resources/ STSReferenceMaterials<color=" + richColor + "><size=15>, </size></color> " +

                "\n\nSee-through Shader/ Sample Materials" +
                //"\n\nSee-through Shader/ Sample Models/ MedievalHouse/ medievalHouse.fbm" +
                "\n\n<color=" + richColor + "> and use </color> ";
            if (isURP)
            {
                step1Text = "<color=" + richColor + ">First you need to convert all standard shaders to Lit shaders using the tool provided by unity. For this go to </color> " +
                "<b>Edit > Render Pipeline > Universal Render Pipeline</b> <color=" + richColor + ">and then select</color> <b>Upgrade Project Materials to URP Materials</b>." +
                commonNote +
                "<b>Upgrade Selected Materials to URP Materials</b> <color=" + richColor + ">instead!</color>";
                link = "https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@10.7/manual/upgrading-your-shaders.html";
            }
            else
            {
                step1Text = "<color=" + richColor + ">First you need to convert all standard shaders to Lit shaders using the tool provided by unity. For this go to </color> " +
                "<b>Edit > Render Pipeline > High Definition RP</b> <color=" + richColor + ">and then select</color> <b>Upgrade Project Materials to High Definition Materials</b>" +
                 commonNote + "<b>Upgrade Selected Materials to High Definition Materials</b> <color=" + richColor + ">instead!</color>";
                link = "https://docs.unity3d.com/Packages/com.unity.render-pipelines.high-definition@10.7/manual/Upgrading-To-HDRP.html";
            }
            Rect rect = EditorGUILayout.BeginVertical();
            rect.width -= 20;
            rect.x += 10;
            //GUI.Box(rect, GUIContent.none);

            if (EditorGUIUtility.isProSkin)
            {
                GUI.Box(rect, GUIContent.none);
            }
            else
            {
                EditorUtils.DrawBox(rect, new Color(0.8f, 0.8f, 0.8f, 1));
            }
            GUILayout.Label("STEP 1:", stepHeaderStyle);
            EditorGUILayout.Space(5);
            GUILayout.TextArea(step1Text, textAreaStyle);
            if (LinkLabel(new GUIContent("Unity Documentation")))
            {
                Application.OpenURL(link);
            }
            //GUILayout.FlexibleSpace();

            EditorGUILayout.Space(5);
            GUILayout.Label("Please finish step 1 before you do step 2!", centerLabelStyle);
            EditorGUILayout.Space(15);
            EditorUtils.DrawUILineCenter(new Color(0.2f, 0.2f, 0.2f, 1));

            EditorGUILayout.Space(15);
            GUILayout.Label("STEP 2:", stepHeaderStyle);
            EditorGUILayout.Space(5);
            autoAssignShader(mats, unityVersionRenderPipelineShader);
            EditorGUILayout.Space(15);
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(100);
        }

    }
}