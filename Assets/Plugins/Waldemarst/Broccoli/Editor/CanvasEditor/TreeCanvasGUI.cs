using UnityEngine;
using UnityEditor;

using Broccoli.Utils;

namespace Broccoli.TreeNodeEditor
{
	/// <summary>
	/// Class for nodes GUI style properties.
	/// </summary>
	public static class TreeCanvasGUI {
		public static Color inactiveNodeOutputColor = Color.gray;
		public static Color activeNodeOutputColor = new Color(0.77f, 0.77f, 0.77f);
		public static Color connectDotColor = new Color(1f, 0.4f, 0.4f);
		/// <summary>
		/// Default label style.
		/// </summary>
		public static GUIStyle nodeLabel;
		/// <summary>
		/// Small node label style.
		/// </summary>
		public static GUIStyle smallNodeLabel;
		/// <summary>
		/// Header label style.
		/// </summary>
		public static GUIStyle nodeHeaderLabel;
		/// <summary>
		/// Selected header label style.
		/// </summary>
		public static GUIStyle nodeHeaderSelectedLabel;
		private static GUIStyle _verticalScrollStyle = null;
		public static GUIStyle verticalScrollStyle {
			get {
				if (_verticalScrollStyle == null) {
					_verticalScrollStyle = new GUIStyle (GUI.skin.verticalScrollbar);
				}
				return _verticalScrollStyle;
			}
		}
		private static GUIStyle _catalogItemStyle = null;
		public static GUIStyle catalogItemStyle {
			get {
				if (_catalogItemStyle == null) {
					_catalogItemStyle = new GUIStyle (GUI.skin.button);
					_catalogItemStyle.imagePosition = ImagePosition.ImageAbove;
					_catalogItemStyle.fixedHeight = 100;
				}
				return _catalogItemStyle;
			}
		}
		private static GUIStyle _catalogCategoryStyle = null;
		public static GUIStyle catalogCategoryStyle {
			get {
				if (_catalogCategoryStyle == null) {
					_catalogCategoryStyle = new GUIStyle (EditorStyles.boldLabel);
					_catalogCategoryStyle.normal.textColor = Color.white;
				}
				return _catalogCategoryStyle;
			}
		}
		private static GUIStyle _nodeEditorButtonStyle = null;
		public static GUIStyle nodeEditorButtonStyle {
			get {
				if (_nodeEditorButtonStyle == null) {
					_nodeEditorButtonStyle = new GUIStyle (GUI.skin.button);
					_nodeEditorButtonStyle.alignment = TextAnchor.MiddleLeft;
				}
				return _nodeEditorButtonStyle;
			}
		}
		static TreeCanvasGUI() {
			nodeLabel = new GUIStyle ();
			nodeLabel.fontSize = 10;
			nodeLabel.normal.textColor = BroccoEditorGUI.nodeTextColor;
			smallNodeLabel = new GUIStyle (nodeLabel);
			smallNodeLabel.fontSize = 9;
			smallNodeLabel.normal.textColor = BroccoEditorGUI.headerTextColor;
			nodeHeaderLabel = new GUIStyle (nodeLabel);
			nodeHeaderLabel.fontSize = 11;
			nodeHeaderLabel.normal.textColor = BroccoEditorGUI.headerTextColor;
			nodeHeaderLabel.alignment = TextAnchor.MiddleLeft;
			nodeHeaderSelectedLabel = new GUIStyle (nodeHeaderLabel);
			nodeHeaderSelectedLabel.fontStyle = FontStyle.Bold;
		}
	}
}