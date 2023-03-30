using System.Collections;
using System.Collections.Generic;

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

using Broccoli.Base;
using Broccoli.Pipe;
using Broccoli.NodeEditorFramework;
using Broccoli.NodeEditorFramework.Utilities;

namespace Broccoli.Utils
{
	public static partial class BroccoEditorGUI {
		#region Vars
		public static bool isInit = false;
		#endregion

        #region Style Vars
		public static GUIStyle label;
		public static GUIStyle labelBold;
		public static GUIStyle labelCentered;
		public static GUIStyle labelBoldCentered;
		public static GUIStyle labelRichText;
		public static GUIStyle foldoutBold;
		private static GUIContent iconDocGUI = new GUIContent (GUITextureManager.iconDoc, "Go to the documentation web page for this element.");
		private static GUIContent iconInfoGUI = new GUIContent (GUITextureManager.iconInfo, "Display the description for this element.");
		private static GUIStyle iconHeaderStyle = new GUIStyle { alignment = TextAnchor.MiddleCenter, stretchHeight = true, stretchWidth = true };
        #endregion

		#region Color Vars
		public static Color nodeTextColor = new Color (0.6f, 0.6f, 0.6f);
		public static Color headerTextColor = new Color (0.8f, 0.8f, 0.8f);
		public static Color neutralHeaderColor = new Color (0.2f, 0.2f, 0.2f);
		public static Color structureGeneratorHeaderolor = new Color (0f, 0.32f, 0.44f); // Blue.
		public static Color structureTransformerHeaderColor = new Color (0.2f, 0.50f, 0.50f); // Light blue.
		public static Color meshGeneratorHeaderColor = new Color (0.53f, 0.03f, 0.19f); // Pomegranade.
		public static Color meshTransformerHeaderColor = new Color (0.63f, 0.18f, 0.29f); // Light pomegranade.
		public static Color mapperHeaderColor = new Color (0.8f, 0.51f, 0.05f); // Mustard.
		public static Color functionHeaderColor = new Color (0.27f, 0.28f, 0.5f); // Purple.
		/// <summary>
		/// Gets the color of the element based on its category.
		/// </summary>
		/// <value>The color.</value>
		public static Color GetElementColor (PipelineElement pipelineElement) {
			switch (pipelineElement.classType) {
				case PipelineElement.ClassType.Base:
				case PipelineElement.ClassType.LSystem:
				case PipelineElement.ClassType.SproutGenerator:
				case PipelineElement.ClassType.StructureGenerator:
					return BroccoEditorGUI.structureGeneratorHeaderolor;
				case PipelineElement.ClassType.GirthTransform:
				case PipelineElement.ClassType.SparsingTransform:
				case PipelineElement.ClassType.LengthTransform:
				case PipelineElement.ClassType.BranchBender:
					return BroccoEditorGUI.structureTransformerHeaderColor;
				case PipelineElement.ClassType.BranchMeshGenerator:
				case PipelineElement.ClassType.SproutMeshGenerator:
				case PipelineElement.ClassType.TrunkMeshGenerator:
					return BroccoEditorGUI.meshGeneratorHeaderColor;
				case PipelineElement.ClassType.BranchMapper:
				case PipelineElement.ClassType.SproutMapper:
					return BroccoEditorGUI.mapperHeaderColor;
				case PipelineElement.ClassType.Positioner:
				case PipelineElement.ClassType.Baker:
				case PipelineElement.ClassType.WindEffect:
					return BroccoEditorGUI.functionHeaderColor;
				default:
					return BroccoEditorGUI.neutralHeaderColor;
			}
		}
		#endregion

        #region Methods
		public static bool Init () {
			if (isInit) return true;
			
			// Sometimes EditorStyles is not initialized.
			if (EditorStyles.label == null) return false;
			
			RectOffset labelPadding = new RectOffset (0, 0, 3, 3);
			// Label
			label = new GUIStyle (EditorStyles.label);
			label.padding = labelPadding;
			label.wordWrap = true;
			// Label bold
			labelBold = new GUIStyle (EditorStyles.boldLabel);
			labelBold.padding = labelPadding;
			labelBold.wordWrap = true;
			// Label centered.
			labelCentered = new GUIStyle (EditorStyles.label);
			labelCentered.alignment = TextAnchor.MiddleCenter;
			labelCentered.padding = labelPadding;
			labelCentered.wordWrap = true;
			// Label bold and centered.
			labelBoldCentered = new GUIStyle (EditorStyles.boldLabel);
			labelBoldCentered.alignment = TextAnchor.MiddleCenter;
			labelBoldCentered.padding = labelPadding;
			labelBoldCentered.wordWrap = true;
			// Label Rich Text.
			labelRichText = new GUIStyle {richText = true, alignment = TextAnchor.MiddleCenter};
			// Foldout bold.
			foldoutBold = new GUIStyle (EditorStyles.foldout);
			foldoutBold.font = labelBold.font;

			isInit = true;

			return true;
		}
		#endregion

		#region Property Fields
		/// <summary>
		/// Range slider for float min and max value properties.
		/// </summary>
		/// <param name="propMinValue">Property with the minumum value.</param>
		/// <param name="propMaxValue">Property with the maximum value.</param>
		/// <param name="minRangeValue">Minimum possible value in the range.</param>
		/// <param name="maxRangeValue">Maximum possible value in the range.</param>
		/// <param name="label">Label to display on the field.</param>
		/// <param name="labelContent">Label content to display on the field if label is an empty string.</param>
		/// <returns>True if the range was changed.</returns>
		public static bool FloatRangePropertyField (SerializedProperty propMinValue, SerializedProperty propMaxValue, float minRangeValue, float maxRangeValue, string label, GUIContent labelContent = null) {
			float minValue = propMinValue.floatValue;
			float maxValue = propMaxValue.floatValue;
			EditorGUILayout.BeginHorizontal ();
			if (string.IsNullOrEmpty (label)) {
				EditorGUILayout.MinMaxSlider (labelContent, ref minValue, ref maxValue, minRangeValue, maxRangeValue);
			} else {
				EditorGUILayout.MinMaxSlider (label, ref minValue, ref maxValue, minRangeValue, maxRangeValue);
			}
			EditorGUILayout.LabelField (minValue.ToString("F2") + "/" + maxValue.ToString("F2"), GUILayout.Width (72));
			EditorGUILayout.EndHorizontal ();
			if (minValue != propMinValue.floatValue || maxValue != propMaxValue.floatValue) {
				propMinValue.floatValue = minValue;
				propMaxValue.floatValue = maxValue;
				return true;
			}
			return false;
		}
		/// <summary>
		/// Range slider for float min and max value properties.
		/// </summary>
		/// <param name="minValue">Property with the minumum value.</param>
		/// <param name="maxValue">Property with the maximum value.</param>
		/// <param name="minRangeValue">Minimum possible value in the range.</param>
		/// <param name="maxRangeValue">Maximum possible value in the range.</param>
		/// <param name="label">Label to display on the field.</param>
		/// <param name="labelContent">Label content to display on the field if label is an empty string.</param>
		/// <returns>True if the range was changed.</returns>
		public static bool FloatRangePropertyField (ref float minValue, ref float maxValue, 
			float minRangeValue, float maxRangeValue, string label, GUIContent labelContent = null, int labelWidth = 72) {
			float _minValue = minValue;
			float _maxValue = maxValue;
			EditorGUILayout.BeginHorizontal ();
			if (string.IsNullOrEmpty (label)) {
				EditorGUILayout.MinMaxSlider (labelContent, ref _minValue, ref _maxValue, minRangeValue, maxRangeValue);
			} else {
				EditorGUILayout.MinMaxSlider (label, ref _minValue, ref _maxValue, minRangeValue, maxRangeValue);
			}
			EditorGUILayout.LabelField (_minValue.ToString("F2") + "/" + _maxValue.ToString("F2"), GUILayout.Width (labelWidth));
			EditorGUILayout.EndHorizontal ();
			if (_minValue != minValue || _maxValue != maxValue) {
				minValue = _minValue;
				maxValue = _maxValue;
				return true;
			}
			return false;
		}
		/// <summary>
		/// Range slider for float min and max value properties.
		/// </summary>
		/// <param name="propMinValue">Property with the minumum value.</param>
		/// <param name="propMaxValue">Property with the maximum value.</param>
		/// <param name="minRangeValue">Minimum possible value in the range.</param>
		/// <param name="maxRangeValue">Maximum possible value in the range.</param>
		/// <param name="label">Label to display on the field.</param>
		/// <param name="labelContent">Label content to display on the field if label is an empty string.</param>
		/// <returns>True if the range was changed.</returns>
		public static bool IntRangePropertyField (SerializedProperty propMinValue, SerializedProperty propMaxValue, int minRangeValue, int maxRangeValue, string label, GUIContent labelContent = null) {
			float minValue = propMinValue.intValue;
			float maxValue = propMaxValue.intValue;
			EditorGUILayout.BeginHorizontal ();
			if (string.IsNullOrEmpty (label)) {
				EditorGUILayout.MinMaxSlider (labelContent, ref minValue, ref maxValue, minRangeValue, maxRangeValue);
			} else {
				EditorGUILayout.MinMaxSlider (label, ref minValue, ref maxValue, minRangeValue, maxRangeValue);
			}
			EditorGUILayout.LabelField (Mathf.RoundToInt (minValue) + "-" + Mathf.RoundToInt (maxValue), GUILayout.Width (60));
			EditorGUILayout.EndHorizontal ();
			if (Mathf.RoundToInt (minValue) != propMinValue.intValue || Mathf.RoundToInt (maxValue) != propMaxValue.intValue) {
				propMinValue.intValue = Mathf.RoundToInt (minValue);
				propMaxValue.intValue = Mathf.RoundToInt (maxValue);
				return true;
			}
			return false;
		}
		/// <summary>
		/// Range slider for int min and max value properties.
		/// </summary>
		/// <param name="minValue">Property with the minumum value.</param>
		/// <param name="maxValue">Property with the maximum value.</param>
		/// <param name="minRangeValue">Minimum possible value in the range.</param>
		/// <param name="maxRangeValue">Maximum possible value in the range.</param>
		/// <param name="label">Label to display on the field.</param>
		/// <param name="labelContent">Label content to display on the field if label is an empty string.</param>
		/// <returns>True if the range was changed.</returns>
		public static bool IntRangePropertyField (ref int minValue, ref int maxValue, 
			int minRangeValue, int maxRangeValue, string label, GUIContent labelContent = null, int labelWidth = 72) {
			float _minValue = minValue;
			float _maxValue = maxValue;
			EditorGUILayout.BeginHorizontal ();
			if (string.IsNullOrEmpty (label)) {
				EditorGUILayout.MinMaxSlider (labelContent, ref _minValue, ref _maxValue, minRangeValue, maxRangeValue);
			} else {
				EditorGUILayout.MinMaxSlider (label, ref _minValue, ref _maxValue, minRangeValue, maxRangeValue);
			}
			EditorGUILayout.LabelField (Mathf.RoundToInt (_minValue) + "/" + Mathf.RoundToInt (_maxValue) , GUILayout.Width (labelWidth));
			EditorGUILayout.EndHorizontal ();
			if (Mathf.RoundToInt (_minValue) != minValue || Mathf.RoundToInt (_maxValue) != maxValue) {
				minValue = Mathf.RoundToInt (_minValue);
				maxValue = Mathf.RoundToInt (_maxValue);
				return true;
			}
			return false;
		}
		#endregion

		#region Draw Functions
		/// <summary>
		/// Shows a header on an inspector editor or a window editor.
		/// </summary>
		/// <param name="title">Header title.</param>
		/// <param name="description">Description for the header.</param>
		/// <param name="refURL">URL to a reference document.</param>
		/// <param name="bgColor">Header background color.</param>
		/// <param name="showDescriptionState">State of the shown description.</param>
		/// <param name="showDescriptionBtn">Flag to display a description button for the node.</param>
		/// <param name="showRefURLBtn">Flag to display a button with a URL to the reference for the node.</param>
		/// <returns>The changed value for the show description property.</returns>
		public static bool DrawHeader (string title, string description, string refURL, Color bgColor, bool showDescriptionState, bool showDescriptionBtn = true, bool showRefURLBtn = true) {
			Init ();
			
            GUILayout.Space (3);
            
			var bannerFullRect = GUILayoutUtility.GetRect(0, 0, 26, 0);
            var bannerBeginRect = new Rect(bannerFullRect.position.x, bannerFullRect.position.y, 15, 26);
            var bannerMiddleRect = new Rect(bannerFullRect.position.x + 15, bannerFullRect.position.y, bannerFullRect.xMax - 45, 26);
            var bannerEndRect = new Rect(bannerFullRect.xMax - 15, bannerFullRect.position.y, 15, 26);
			var iconInfoRect = new Rect(bannerFullRect.xMax - 56, bannerFullRect.position.y + 5, 30, 16);
            var iconDocRect = new Rect(bannerFullRect.xMax - 34, bannerFullRect.position.y + 5, 30, 16);

            Color guiColor;

           guiColor = BroccoEditorGUI.headerTextColor;

            GUI.color = bgColor;
			
			if (GUITextureManager.iconGradient == null) GUITextureManager.Init (true);

            GUI.DrawTexture(bannerBeginRect, GUITextureManager.iconGradient, ScaleMode.StretchToFill, true);
            GUI.DrawTexture(bannerMiddleRect, GUITextureManager.iconGradient, ScaleMode.StretchToFill, true);
            GUI.DrawTexture(bannerEndRect, GUITextureManager.iconGradient, ScaleMode.StretchToFill, true);
			

            GUI.color = guiColor;

			GUI.Label(bannerFullRect, "<size=14><color=#" + ColorUtility.ToHtmlStringRGB(guiColor) + ">" + title + "</color></size>", BroccoEditorGUI.labelRichText);

			// Draw buttons.
			if (showDescriptionBtn && GUI.Button (iconInfoRect, iconInfoGUI, iconHeaderStyle)) {
                showDescriptionState = !showDescriptionState;
            }
			if (showRefURLBtn && GUI.Button (iconDocRect, iconDocGUI, iconHeaderStyle)) {
                Application.OpenURL (refURL);
            }

            GUI.color = Color.white;

			if (showDescriptionState) {
				EditorGUILayout.HelpBox (description, MessageType.Info);
			}
			GUILayout.Space (6);

			return showDescriptionState;
        }
		/// <summary>
		/// Shows a header on an inspector editor or a window editor.
		/// </summary>
		/// <param name="title">Header title.</param>
		/// <param name="bgColor">Header background color.</param>
		public static void DrawHeader (string title, Color bgColor) {
            DrawHeader (title, string.Empty, string.Empty, bgColor, false, false, false);
        }
		#endregion
	}
}