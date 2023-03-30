using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

using Broccoli.Base;
using Broccoli.Pipe;

namespace Broccoli.TreeNodeEditor
{
	/// <summary>
	/// Girth transform element editor.
	/// </summary>
	[CustomEditor(typeof(GirthTransformElement))]
	public class GirthTransformElementEditor : PipelineElementEditor {
		#region Vars
		/// <summary>
		/// The girth transform pipeline element.
		/// </summary>
		public GirthTransformElement girthTransformElement;
		/// <summary>
		/// The girth curve range.
		/// </summary>
		private static Rect girthCurveRange = new Rect (0f, 0f, 1f, 1f);

		SerializedProperty propMinGirthAtBase;
		SerializedProperty propMaxGirthAtBase;
		SerializedProperty propMinGirthAtTop;
		SerializedProperty propMaxGirthAtTop;
		SerializedProperty propGirthCurve;
		SerializedProperty propHierarchyScalingEnabled;
		SerializedProperty propMinHierarchyScaling;
		SerializedProperty propMaxHierarchyScaling;
		SerializedProperty propGirthAtRootBase;
		SerializedProperty propGirthAtRootBottom;
		SerializedProperty propGirthRootCurve;
		private static float minTreeGirthValue = 0.01f;
		private static float maxTreeGirthValue = 0.8f;
		private static float minShrubGirthValue = 0.001f;
		private static float maxShrubGirthValue = 0.1f;
		private static float minRootTreeGirthValue = 0.01f;
		private static float maxRootTreeGirthValue = 0.5f;
		private static float minRootShrubGirthValue = 0.001f;
		private static float maxRootShrubGirthValue = 0.1f;
		#endregion

		#region GUI Vars
		private static GUIContent girthAtBaseLabel = new GUIContent ("Girth at Base", "");
		private static GUIContent girthAtTopLabel = new GUIContent ("Girth at Top", "");
		private static GUIContent girthCurveLabel = new GUIContent ("Curve", "");
		private static GUIContent hierarchyScalingLabel = new GUIContent ("Hierarchy Scaling", "");
		private static GUIContent girthAtRootBaseLabel = new GUIContent ("Girth at Root Base", "");
		private static GUIContent girthAtRootBottomLabel = new GUIContent ("Girth at Root Bottom", "");
		private static GUIContent girthRootCurveLabel = new GUIContent ("Root Curve", "");
		#endregion

		#region Messages
		private static string MSG_GIRTH_AT_BASE = "Girth to be used at the base of the tree trunk.";
		private static string MSG_GIRTH_AT_TOP = "Girth to be used at the tip of a terminal branch.";
		private static string MSG_CURVE = "Curve of girth values from tree trunk (base) " +
			"to the tip of a terminal branch (top).";
		private static string MSG_HIERARCHY_SCALING_ENABLED = "Adds girth scaling to terminal branches that come directly from the tree trunk.";
		//private static string MSG_MIN_HIERARCHY_SCALING = "";
		private static string MSG_MAX_HIERARCHY_SCALING = "Scaling for the girth on terminal branches coming out of the tree trunk.";
		private static string MSG_GIRTH_AT_ROOT_BASE = "Girth to be used at the base of the tree trunk.";
		private static string MSG_GIRTH_AT_ROOT_BOTTOM = "Girth to be used at the tip of a terminal branch.";
		private static string MSG_ROOT_CURVE = "Curve of girth values from tree trunk (base) " +
			"to the tip of a terminal branch (top).";
		#endregion

		#region Events
		/// <summary>
		/// Actions to perform on the concrete class when the enable event is raised.
		/// </summary>
		protected override void OnEnableSpecific () {
			girthTransformElement = target as GirthTransformElement;

			propMinGirthAtBase = GetSerializedProperty ("minGirthAtBase");
			propMaxGirthAtBase = GetSerializedProperty ("maxGirthAtBase");
			propMinGirthAtTop = GetSerializedProperty ("minGirthAtTop");
			propMaxGirthAtTop = GetSerializedProperty ("maxGirthAtTop");
			propGirthCurve = GetSerializedProperty ("curve");
			propHierarchyScalingEnabled = GetSerializedProperty ("hierarchyScalingEnabled");
			propMinHierarchyScaling = GetSerializedProperty ("minHierarchyScaling");
			propMaxHierarchyScaling = GetSerializedProperty ("maxHierarchyScaling");
			propGirthAtRootBase = GetSerializedProperty ("girthAtRootBase");
			propGirthAtRootBottom = GetSerializedProperty ("girthAtRootBottom");
			propGirthRootCurve = GetSerializedProperty ("rootCurve");
		}
		/// <summary>
		/// Raises the inspector GU event.
		/// </summary>
		protected override void OnInspectorGUISpecific () {
			CheckUndoRequest ();

			UpdateSerialized ();
			EditorGUILayout.LabelField ("Branches", EditorStyles.boldLabel);

			bool girthChanged = false;
			EditorGUI.BeginChangeCheck ();
			if (pipelineElement.preset == PipelineElement.Preset.Tree) {
				FloatRangePropertyField (propMinGirthAtBase, propMaxGirthAtBase, 
					minTreeGirthValue, maxTreeGirthValue, girthAtBaseLabel);
				ShowHelpBox (MSG_GIRTH_AT_BASE);
				FloatRangePropertyField (propMinGirthAtTop, propMaxGirthAtTop, 
					minTreeGirthValue, maxTreeGirthValue, girthAtTopLabel);
				ShowHelpBox (MSG_GIRTH_AT_TOP);
			} else {
				FloatRangePropertyField (propMinGirthAtBase, propMaxGirthAtBase, 
					minShrubGirthValue, maxShrubGirthValue, girthAtBaseLabel, 3);
				ShowHelpBox (MSG_GIRTH_AT_BASE);
				FloatRangePropertyField (propMinGirthAtTop, propMaxGirthAtTop, 
					minShrubGirthValue, maxShrubGirthValue, girthAtTopLabel, 3);
				ShowHelpBox (MSG_GIRTH_AT_TOP);
			}
			if (EditorGUI.EndChangeCheck ()) {
				girthChanged = true;
			}

			bool curveChanged = false;
			EditorGUI.BeginChangeCheck ();
			EditorGUILayout.CurveField (propGirthCurve, Color.green, girthCurveRange, girthCurveLabel);
			ShowHelpBox (MSG_CURVE);
			if (EditorGUI.EndChangeCheck ()) {
				curveChanged = true;
			}

			bool hierarchyScaleChanged = false;
			EditorGUI.BeginChangeCheck ();
			EditorGUILayout.PropertyField (propHierarchyScalingEnabled);
			ShowHelpBox (MSG_HIERARCHY_SCALING_ENABLED);

			if (propHierarchyScalingEnabled.boolValue) {
				EditorGUILayout.Slider (propMaxHierarchyScaling, 0.01f, 1f, hierarchyScalingLabel);
				ShowHelpBox (MSG_MAX_HIERARCHY_SCALING);
				EditorGUILayout.Space ();
			}

			if (EditorGUI.EndChangeCheck () && 
				propMaxHierarchyScaling.floatValue >= propMinHierarchyScaling.floatValue) {
				hierarchyScaleChanged = true;
			}
			EditorGUILayout.Space ();

			EditorGUILayout.LabelField ("Roots", EditorStyles.boldLabel);
			EditorGUI.BeginChangeCheck ();
			if (pipelineElement.preset == PipelineElement.Preset.Tree) {
				float girthAtRootBase = propGirthAtRootBase.floatValue;
				EditorGUILayout.Slider (propGirthAtRootBase, minRootTreeGirthValue, maxRootTreeGirthValue, girthAtRootBaseLabel);
				ShowHelpBox (MSG_GIRTH_AT_ROOT_BASE);
				float girthAtRootBottom = propGirthAtRootBottom.floatValue;
				EditorGUILayout.Slider (propGirthAtRootBottom, minRootTreeGirthValue, maxRootTreeGirthValue, girthAtRootBottomLabel);
				ShowHelpBox (MSG_GIRTH_AT_ROOT_BOTTOM);
			} else {
				float girthAtRootBase = propGirthAtRootBase.floatValue;
				EditorGUILayout.Slider (propGirthAtRootBase, minRootShrubGirthValue, maxRootShrubGirthValue, girthAtRootBaseLabel);
				ShowHelpBox (MSG_GIRTH_AT_ROOT_BASE);
				float girthAtRootBottom = propGirthAtRootBottom.floatValue;
				EditorGUILayout.Slider (propGirthAtRootBottom, minRootShrubGirthValue, maxRootShrubGirthValue, girthAtRootBottomLabel);
				ShowHelpBox (MSG_GIRTH_AT_ROOT_BOTTOM);
			}
			if (EditorGUI.EndChangeCheck ()) {
				girthChanged = true;
			}

			bool rootCurveChanged = false;
			EditorGUI.BeginChangeCheck ();
			EditorGUILayout.CurveField (propGirthRootCurve, Color.green, girthCurveRange, girthRootCurveLabel);
			ShowHelpBox (MSG_ROOT_CURVE);
			if (EditorGUI.EndChangeCheck ()) {
				curveChanged = true;
			}
			DrawSeparator ();

			// Seed options.
			DrawSeedOptions ();

			if (girthChanged ||
				curveChanged ||
				rootCurveChanged || 
				hierarchyScaleChanged)
			{
				ApplySerialized ();
				UpdatePipeline (GlobalSettings.processingDelayHigh);
				girthTransformElement.Validate ();
				SetUndoControlCounter ();

			}

			// Field descriptors option.
			DrawFieldHelpOptions ();

			// Draw preset.
			DrawPresetOptions ();
		}
		#endregion
	}
}