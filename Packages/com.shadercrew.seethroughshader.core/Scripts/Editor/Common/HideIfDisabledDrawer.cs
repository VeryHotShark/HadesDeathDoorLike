
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;

namespace ShaderCrew.SeeThroughShader
{
    public class HideIfDisabledDrawer : MaterialPropertyDrawer
    {
        protected string[] argValue;
        bool bElementHidden;

        //constructor permutations -- params doesn't seem to work for property drawer inputs :( -----------
        public HideIfDisabledDrawer(string name1)
        {
            argValue = new string[] { name1 };
        }

        public HideIfDisabledDrawer(string name1, string name2)
        {
            argValue = new string[] { name1, name2 };
        }

        public HideIfDisabledDrawer(string name1, string name2, string name3)
        {
            argValue = new string[] { name1, name2, name3 };
        }

        public HideIfDisabledDrawer(string name1, string name2, string name3, string name4)
        {
            argValue = new string[] { name1, name2, name3, name4 };
        }

        public HideIfDisabledDrawer(string name1, string name2, string name3, string name4, string name5)
        {
            argValue = new string[] { name1, name2, name3, name4, name5 };
        }

        public HideIfDisabledDrawer(string name1, string name2, string name3, string name4, string name5, string name6)
        {
            argValue = new string[] { name1, name2, name3, name4, name5, name6 };
        }
        //-------------------------------------------------------------------------------------------------

        public override void OnGUI(Rect position, MaterialProperty prop, string label, MaterialEditor editor)
        {

            bElementHidden = false;
            for (int i = 0; i < editor.targets.Length; i++)
            {
                //material object that we're targetting...
                Material mat = editor.targets[i] as Material;
                if (mat != null)
                {
                    //check for the dependencies:
                    bool anyEnabled = false;
                    for (int j = 0; j < argValue.Length; j++)
                    {
                        if (mat.IsKeywordEnabled(argValue[j]))
                        {
                            anyEnabled = true;
                            //Debug.Log("argValue[j] enabled: " + argValue[j]);
                        }
                        bElementHidden = !anyEnabled;
                        //bElementHidden |= !mat.IsKeywordEnabled(argValue[j]);
                    }

                    //bElementHidden |= anyEnabled;

                }
            }

            if (!bElementHidden)
            {
                //Debug.Log("label:" + label);
                //Debug.Log("prop:" + prop);
                //Debug.Log("prop.type:" + prop.GetType());

                //if (label == "Use Cone Obstruction Destroyer Super Saiyan" || label == "Use only cone obstruction destroyer")
                //{
                //    bool value = prop.floatValue != 0.0f;
                //    value = EditorGUILayout.Toggle(label, value);
                //    prop.floatValue = value ? 1.0f : 0.0f;
                //} else
                //{
                //    editor.DefaultShaderProperty(prop, label);
                //}
                editor.DefaultShaderProperty(prop, label);
                // 
            }
            //editor.ShaderProperty(prop, label);


        }

        //We need to override the height so it's not adding any extra (unfortunately texture drawers will still add an extra bit of padding regardless):
        public override float GetPropertyHeight(MaterialProperty prop, string label, MaterialEditor editor)
        {
            //@TODO: manually standardise element compaction
            //     float height = base.GetPropertyHeight (prop, label, editor);
            //     return bElementHidden ? 0.0f : height-16;

            return 0;
        }

    }
#endif
}