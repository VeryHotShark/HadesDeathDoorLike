#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using System.Linq;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using ParadoxNotion;

namespace NodeCanvas.Editor
{

    public class WelcomeWindow : EditorWindow
    {

        private static Texture2D header;
        private static System.Type assetType;
        private static Texture2D docsIcon;
        private static Texture2D resourcesIcon;
        private static Texture2D supportIcon;
        private static Texture2D communityIcon;

        private GraphInfoAttribute att;
        private string packageName;
        private string docsURL;
        private string resourcesURL;
        private string forumsURL;
        private string discordUrl = "https://discord.gg/97q2Rjh";

        //...
        public static void ShowWindow(System.Type t) {
            assetType = t;
            var window = CreateInstance<WelcomeWindow>();
            window.ShowUtility();
        }

        //...
        void OnEnable() {
            titleContent = new GUIContent("Welcome");

            att = assetType != null ? (GraphInfoAttribute)assetType.GetCustomAttributes(typeof(GraphInfoAttribute), true).FirstOrDefault() : null;
            packageName = att != null ? att.packageName : "Empty";
            docsURL = att != null ? att.docsURL : "https://paradoxnotion.com/";
            resourcesURL = att != null ? att.resourcesURL : "https://paradoxnotion.com/";
            forumsURL = att != null ? att.forumsURL : "https://paradoxnotion.com/";

            header = Resources.Load(string.Format("{0}Header", packageName)) as Texture2D;
            docsIcon = Resources.Load("Manual") as Texture2D;
            resourcesIcon = Resources.Load("Resources") as Texture2D;
            supportIcon = Resources.Load("Support") as Texture2D;
            communityIcon = Resources.Load("Community") as Texture2D;
            var size = new Vector2(header != null ? header.width : 20, 480);
            minSize = size;
            maxSize = size;
        }

        //...
        void OnGUI() {

            if ( header == null ) { return; }

            var headerRect = new Rect(0, 0, header.width, header.height);
            EditorGUIUtility.AddCursorRect(headerRect, MouseCursor.Link);
            if ( GUI.Button(headerRect, header, GUIStyle.none) ) {
                UnityEditor.Help.BrowseURL("https://paradoxnotion.com");
            }
            GUILayout.Space(header.height);

            GUI.skin.label.richText = true;
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            GUILayout.BeginVertical();

            GUILayout.Space(10);

            var titleRect = headerRect;
            titleRect.x += 30;

            GUILayout.Label(string.Format("Welcome and thank you for purchasing {0}! Following are a few important links to get you started:", packageName));
            GUILayout.Space(10);

            ///----------------------------------------------------------------------------------------------

            ShowEntry(docsIcon, "<size=16><b>Documentation</b></size>\nRead thorough documentation and API reference online.", docsURL);
            ShowEntry(resourcesIcon, "<size=16><b>Resources</b></size>\nDownload samples, extensions and other resources.", resourcesURL);
            ShowEntry(supportIcon, "<size=16><b>Support</b></size>\nJoin the online forums, get support and give feedback.", forumsURL);
            ShowEntry(communityIcon, "<size=16><b>Community</b></size>\nJoin the online Discord community.", discordUrl);


            ///----------------------------------------------------------------------------------------------

            GUILayout.FlexibleSpace();

            GUILayout.Label("Please consider leaving a review to support the product!");

            GUILayout.Space(5);

            Prefs.hideWelcomeWindow = EditorGUILayout.ToggleLeft("Don't show again.", Prefs.hideWelcomeWindow);

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            GUILayout.Space(20);
        }

        //...
        void ShowEntry(Texture2D icon, string text, string url) {
            GUILayout.BeginHorizontal(Styles.roundedBox);
            GUI.backgroundColor = Color.clear;
            GUI.contentColor = EditorGUIUtility.isProSkin ? ColorUtils.Grey(0.8f) : Color.black;
            if ( GUILayout.Button(icon, GUILayout.Width(50), GUILayout.Height(50)) ) {
                UnityEditor.Help.BrowseURL(url);
            }
            GUI.contentColor = Color.white;
            EditorGUIUtility.AddCursorRect(GUILayoutUtility.GetLastRect(), MouseCursor.Link);
            GUILayout.BeginVertical();
            GUILayout.Space(6);
            GUILayout.Label(text);
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
            GUI.backgroundColor = Color.white;
            GUI.contentColor = Color.white;
        }
    }
}

#endif