using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace VHS {
    public static class VHSLogger
    {
        private static string ColorToHex(Color color) {
            return "#" + ColorUtility.ToHtmlStringRGB(color);
        }
        public static string Color(this string myStr, string color)
        {
            return $"<color={color}>{myStr}</color>";
        }
        
        public static void DoLog(Action<string, Object> LogFunction, string prefix, Object myObj, Color color, params object[] msg)
        {
#if UNITY_EDITOR
            var name = (myObj ? myObj.name : "NullObject").Color("lightblue");
            var joinedMsg = String.Join("; ", msg);

            string hexColor = ColorToHex(color); 
            
            LogFunction($"[{prefix.Color("brown")}][{name}]: {joinedMsg.Color(hexColor)}\n ", myObj);
#endif
        }
        
        public static void Log(this Object myObj, params object[] msg)
        {
            DoLog(Debug.Log, "", myObj, UnityEngine.Color.white, msg);
        }

        public static void LogError(this Object myObj, params object[] msg)
        {
            DoLog(Debug.LogError, "<!>".Color("red"), myObj,UnityEngine.Color.white, msg);
        }

        public static void LogWarning(this Object myObj, params object[] msg)
        {
            DoLog(Debug.LogWarning, "⚠️".Color("yellow"),  myObj,UnityEngine.Color.white, msg);
        }

        public static void LogSuccess(this Object myObj, params object[] msg)
        {
            DoLog(Debug.Log, "☻".Color("green"), myObj,UnityEngine.Color.white, msg);
        }
    }
}
