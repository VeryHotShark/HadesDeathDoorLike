using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

using Broccoli.Base;
using Broccoli.Pipe;
using Broccoli.Model;
using Broccoli.Utils;

namespace Broccoli.TreeNodeEditor
{
	/// <summary>
	/// Baker element editor.
	/// </summary>
	[CustomEditor(typeof(BakerElement))]
	public class BakerElementEditor : PipelineElementEditor {
		#region Vars
		/// <summary>
		/// The positioner element.
		/// </summary>
		public BakerElement bakerElement;
		/// <summary>
		/// Options to show on the toolbar.
		/// </summary>
		static GUIContent[] toolbarOptions = new GUIContent[] {
			new GUIContent ("LOD", "Edit the Level of Detail Group Settings."), 
			new GUIContent ("Collider", "Options to create colliders based on the tree geometry."), 
			new GUIContent ("AO", "Options to bake Ambient Occlusion in the tree geometry.")};
		static int OPTION_LOD = 0;
		static int OPTION_COLLIDER = 1;
		static int OPTION_AO = 2;
        int selectedToolbarOption = 0;
		SerializedProperty propEnableAO;
		SerializedProperty propEnableAOInPreview;
		SerializedProperty propEnableAOAtRuntime;
		SerializedProperty propSamplesAO;
		SerializedProperty propStrengthAO;
		SerializedProperty propLodFade;
		SerializedProperty propLodFadeAnimate;
		SerializedProperty propLodTransitionWidth;
		SerializedProperty propAddCollider;
		SerializedProperty propColliderType;
		SerializedProperty propColliderScale;
		SerializedProperty propColliderMeshResolution;
		SerializedProperty propColliderMinLevel;
		SerializedProperty propColliderMaxLevel;
		GUIContent lodFadingGUIContent = new GUIContent ("LOD Fading Mode");
		GUIContent lodFadingAnimateGUIContent = new GUIContent ("LOD Fading Animation");
		#endregion

		#region GUI Content and Labels
		private static string labelLODPanelTitle = "LOD Group Settings";
		private static string labelColliderPanelTitle = "Collider Settings";
		private static string labelAOPanelTitle = "Ambient Occlusion Settings";
		#endregion

		#region Messages
		private static string MSG_ENABLE_AO = "Enables ambient occlusion baked on the final prefab mesh.";
		private static string MSG_ENABLE_AO_IN_PREVIEW = "Enable ambient occlusion when previewing the tree in the editor.";
		private static string MSG_ENABLE_AO_AT_RUNTIME = "Enable ambient occlusion when creating trees at runtime. Baking ambient occlusion to the mesh at runtime is processing intensive.";
		private static string MSG_SAMPLES_AO = "Enables this position to be a possible point of origin for a tree.";
		private static string MSG_STRENGTH_AO = "Amount of ambient occlusion to bake into the mesh.";
		private static string MSG_LOD_FADE = "LOD transition mode on the final prefab.";
		private static string MSG_LOD_FADE_ANIMATE = "LOD transition mode animation enabled or disabled.";
		private static string MSG_LOD_TRANSITION_WIDTH = "Transition value to cross-fade between elements within the LOD group.";
		private static string MSG_ADD_COLLIDER = "Enables creating a collider for this pipeline.";
		/*
		private static string MSG_COLLIDER_TYPE = "Type of collider to use on the tree:\nCapsule: a capsule collider from the trunk of the tree." +
			"\nConvex: a convex mesh collider including the branch/root levels specified." +
			"\nNon Convex: a non-convex mesh collider including the branch/root levels specified.";
			*/
		private static string MSG_COLLIDER_SCALE = "Scale for the capsule collider from the girth at the base of the trunk.";
		/*
		private static string MSG_COLLIDER_MESH_RESOLUTION = "Resolution for the mesh collider based on the resolution of the tree mesh. When the value = 1 the mesh will be identical to that of the tree.";
		private static string MSG_COLLIDER_MIN_MAX_LEVEL = "Structure levels to include in the collider.\nLevel 0: trunk.\nLevels > 0: branches.\nLevels < 0: roots.";
		*/
		#endregion

		#region Events
		/// <summary>
		/// Actions to perform on the concrete class when the enable event is raised.
		/// </summary>
		protected override void OnEnableSpecific () {
			bakerElement = target as BakerElement;

			propEnableAO = GetSerializedProperty ("enableAO");
			propEnableAOInPreview = GetSerializedProperty ("enableAOInPreview");
			propEnableAOAtRuntime = GetSerializedProperty ("enableAOAtRuntime");
			propSamplesAO = GetSerializedProperty ("samplesAO");
			propStrengthAO = GetSerializedProperty ("strengthAO");
			propLodFade = GetSerializedProperty ("lodFade");
			propLodFadeAnimate = GetSerializedProperty ("lodFadeAnimate");
			propLodTransitionWidth = GetSerializedProperty ("lodTransitionWidth");
			propAddCollider = GetSerializedProperty ("addCollider");
			propColliderType = GetSerializedProperty ("colliderType");
			propColliderScale = GetSerializedProperty ("colliderScale");
			propColliderMeshResolution = GetSerializedProperty ("colliderMeshResolution");
			propColliderMinLevel = GetSerializedProperty ("colliderMinLevel");
			propColliderMaxLevel = GetSerializedProperty ("colliderMaxLevel");
		}
		/// <summary>
		/// Raises the inspector GUI event.
		/// </summary>
		protected override void OnInspectorGUISpecific () {
			UpdateSerialized ();

			// Log box.
			DrawLogBox ();

			selectedToolbarOption = GUILayout.Toolbar (selectedToolbarOption, toolbarOptions);
			EditorGUILayout.Space ();

			if (selectedToolbarOption == OPTION_LOD) {
				EditorGUILayout.LabelField (labelLODPanelTitle, EditorStyles.boldLabel);
				EditorGUI.BeginChangeCheck ();
				EditorGUILayout.PropertyField (propLodFade, lodFadingGUIContent);
				ShowHelpBox (MSG_LOD_FADE);
				EditorGUILayout.PropertyField (propLodFadeAnimate, lodFadingAnimateGUIContent);
				ShowHelpBox (MSG_LOD_FADE_ANIMATE);
				EditorGUILayout.Slider (propLodTransitionWidth, 0f, 1f, "Transition Width");
				ShowHelpBox (MSG_LOD_TRANSITION_WIDTH);
				if (EditorGUI.EndChangeCheck ()) {
					ApplySerialized ();
					bakerElement.Validate ();
				}
			} else if (selectedToolbarOption == OPTION_COLLIDER) {
				EditorGUILayout.LabelField (labelColliderPanelTitle, EditorStyles.boldLabel);
				EditorGUI.BeginChangeCheck ();
				// Enables colliders on this pipeline.
				EditorGUILayout.PropertyField (propAddCollider);
				ShowHelpBox (MSG_ADD_COLLIDER);
				if (propAddCollider.boolValue) {
					// Type of collider.
					/*
					EditorGUILayout.PropertyField (propColliderType);
					ShowHelpBox (MSG_COLLIDER_TYPE);
					if (propColliderType.enumValueIndex == (int)BakerElement.ColliderType.Capsule) {
						*/
						EditorGUILayout.Slider (propColliderScale, 0.5f, 3f);
						ShowHelpBox (MSG_COLLIDER_SCALE);
					/*
					} else {
						EditorGUILayout.Slider (propColliderMeshResolution, 0.01f, 1f);
						ShowHelpBox (MSG_COLLIDER_MESH_RESOLUTION);
						IntRangePropertyField (propColliderMinLevel, propColliderMaxLevel, -3, 3, "Structure Levels");
						ShowHelpBox (MSG_COLLIDER_MIN_MAX_LEVEL);
					}
					*/
				}
				if (EditorGUI.EndChangeCheck ()) {
					UpdatePipeline (GlobalSettings.processingDelayLow);
					ApplySerialized ();
					bakerElement.Validate ();
				}
			} else if (selectedToolbarOption == OPTION_AO) {
				EditorGUILayout.LabelField (labelAOPanelTitle, EditorStyles.boldLabel);
				EditorGUI.BeginChangeCheck ();
				// Enables AO baking on the final prefab mesh.
				EditorGUILayout.PropertyField (propEnableAO);
				ShowHelpBox (MSG_ENABLE_AO);
				if (propEnableAO.boolValue) {
					// AO Samples.
					EditorGUILayout.IntSlider (propSamplesAO, 1, 8);
					ShowHelpBox (MSG_SAMPLES_AO);
					// AO Strength.
					EditorGUILayout.Slider (propStrengthAO, 0f, 1f);
					ShowHelpBox (MSG_STRENGTH_AO);
					// Enables AO in the preview tree of the editor.
					EditorGUILayout.PropertyField (propEnableAOInPreview);
					ShowHelpBox (MSG_ENABLE_AO_IN_PREVIEW);
					// Enables AO at runtime.
					EditorGUILayout.PropertyField (propEnableAOAtRuntime);
					ShowHelpBox (MSG_ENABLE_AO_AT_RUNTIME);
				}
				if (EditorGUI.EndChangeCheck ()) {
					UpdatePipeline (GlobalSettings.processingDelayLow);
					ApplySerialized ();
					bakerElement.Validate ();
				}
			}
			EditorGUILayout.Space ();

			// Seed options.
			//DrawSeedOptions ();
			// Field descriptors option.
			DrawFieldHelpOptions ();
		}
		/// <summary>
		/// Raises the scene GUI event.
		/// </summary>
		/// <param name="sceneView">Scene view.</param>
		protected override void OnSceneGUI (SceneView sceneView) {
			if (bakerElement.addCollider) {
				BroccoTree tree = TreeFactoryEditorWindow.editorWindow.treeFactory.previewTree;
				if (tree == null) return;
				float scale = TreeFactoryEditorWindow.editorWindow.treeFactory.treeFactoryPreferences.factoryScale;
				List<BroccoTree.Branch> rootBranches = tree.branches;
				Vector3 trunkBase;
				Vector3 trunkTip;
				for (int i = 0; i < rootBranches.Count; i++) {
					trunkBase = rootBranches [i].GetPointAtPosition (0f);
					trunkTip = rootBranches [i].GetPointAtPosition (1f);
					Vector3 treePos = TreeFactoryEditorWindow.editorWindow.treeFactory.GetPreviewTreeWorldOffset ();
					EditorDrawUtils.DrawWireCapsule (
						treePos + (trunkTip + trunkBase) / 2f * scale, 
						Quaternion.identity, 
						rootBranches [i].maxGirth * scale * bakerElement.colliderScale, 
						Vector3.Distance (trunkTip, trunkBase) * scale,
						Color.yellow);
				}
			}
		}
		#endregion
	}
}