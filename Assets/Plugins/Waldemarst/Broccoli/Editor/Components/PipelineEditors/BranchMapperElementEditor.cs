using UnityEditor;
using UnityEngine;

using Broccoli.Base;
using Broccoli.Pipe;
using Broccoli.Component;

namespace Broccoli.TreeNodeEditor
{
	/// <summary>
	/// Branch mapper element editor.
	/// </summary>
	[CustomEditor(typeof(BranchMapperElement))]
	public class BranchMapperElementEditor : PipelineElementEditor {
		#region Vars
		/// <summary>
		/// The branch mapper element.
		/// </summary>
		public BranchMapperElement branchMapperElement;
		SerializedProperty propMaterialMode;
		SerializedProperty propCustomMaterial;
		SerializedProperty propMainTexture;
		SerializedProperty propNormalTexture;
		SerializedProperty propMappingXDisplacement;
		SerializedProperty propMappingYDisplacement;
		SerializedProperty propMappingXTiles;
		SerializedProperty propMappingYTiles;
		SerializedProperty propIsGirthSensitive;
		SerializedProperty propColor;
		SerializedProperty propGlossiness;
		SerializedProperty propMetallic;
		SerializedProperty propDiffusionProfileSettings;
		private static int minTreeTilesValue = -5;
		private static int maxTreeTilesValue = 5;
		private static int minShrubTilesValue = -10;
		private static int maxShrubTilesValue = 10;
		private static int minTreeDisplacementValue = -5;
		private static int maxTreeDisplacementValue = 5;
		private static int minShrubDisplacementValue = -10;
		private static int maxShrubDisplacementValue = 10;
		#endregion

		#region GUI Vars
		private static GUIContent materialModeLabel = new GUIContent ("Material Mode", "Select using a material provided by the tree factory or a custom material provided by you.");
		private static GUIContent customMaterialLabel = new GUIContent ("Custom Material", "Custom material to use for branches.");
		private static GUIContent mainTextureLabel = new GUIContent ("Main Texture", "The albedo texture to use on the branch material.");
		private static GUIContent normalTextureLabel = new GUIContent ("Normal Texture", "The normal texture to use on the branch material.");
		private static GUIContent colorTintLabel = new GUIContent ("Tint", "Color tint to apply to the branch material.");
		private static GUIContent glossinessLabel = new GUIContent ("Glossiness", "Glossiness (the quality of the material of being smooth and shiny) value pass to the shader.");
		private static GUIContent metallicLabel = new GUIContent ("Metallic", "Metallic (the quality of the material of being metal-like) value pass to the shader.");
		private static GUIContent mappingXDisplacementLabel = new GUIContent ("X Displacement", "Incremental offset to apply to the U value to the UV mapping along the girth of the branches.");
		private static GUIContent mappingYDisplacementLabel = new GUIContent ("Y Displacement", "Incremental offset to apply to the V value to the UV mapping along the length of the branches.");
		private static GUIContent mappingXTilesLabel = new GUIContent ("X Tiles", "Number of titles to use on the U value on the UV mapping.");
		private static GUIContent mappingYTilesLabel = new GUIContent ("Y Tiles", "Number of titles to use on the V value on the UV mapping.");
		private static GUIContent isGirthSensitiveLabel = new GUIContent ("Girth Sensitive", "The UV mapping displacement values adapts to the branch girth.");
		private static GUIContent diffusionProfileLabel = new GUIContent ("Diffusion Profile", "Diffusion Profile Asset used for HDRP materials.");
		#endregion

		#region Messages
		private static string MSG_MATERIAL_MODE = "Material mode to apply.";
		private static string MSG_CUSTOM_MATERIAL = "Material applied to the branches.";
		private static string MSG_MAIN_TEXTURE = "Main texture for the generated material.";
		private static string MSG_NORMAL_TEXTURE = "Normal map texture for the generated material.";
		private static string MSG_MAPPING_X_DISP = "Girth to be used at the base of the tree trunk.";
		private static string MSG_MAPPING_Y_DISP = "Girth to be used at the tip of a terminal branch.";
		private static string MSG_MAPPING_X_TILES = "Multiplies the number of tiles for the texture on the X axis.";
		private static string MSG_MAPPING_Y_TILES = "Multiplies the number of tiles for the texture on the Y axis.";
		private static string MSG_GIRTH_SENSITIVE = "UV mapping is smaller at lower values of girth on the branches.";
		private static string MSG_COLOR = "Color value to pass to the shader.";
		private static string MSG_GLOSSINESS = "Glossiness value to pass to the shader.";
		private static string MSG_METALLIC = "Metallic value to pass to the shader.";
		private static string MSG_DIFFUSION_PROFILE = "Diffusion Profile Settings asset for HDRP materials. Make sure this profile is listed at the HDRP Project Settings. " +
			"Broccoli can only assign a Diffusion Profile in Edit Mode, so it is not available when creating trees at runtime.";
		#endregion

		#region Events
		/// <summary>
		/// Actions to perform on the concrete class when the enable event is raised.
		/// </summary>
		protected override void OnEnableSpecific () {
			branchMapperElement = target as BranchMapperElement;

			propMaterialMode = GetSerializedProperty ("materialMode");
			propCustomMaterial = GetSerializedProperty ("customMaterial");
			propMainTexture = GetSerializedProperty ("mainTexture");
			propNormalTexture = GetSerializedProperty ("normalTexture");
			propMappingXDisplacement = GetSerializedProperty ("mappingXDisplacement");
			propMappingYDisplacement = GetSerializedProperty ("mappingYDisplacement");
			propMappingXTiles = GetSerializedProperty ("mappingXTiles");
			propMappingYTiles = GetSerializedProperty ("mappingYTiles");
			propIsGirthSensitive = GetSerializedProperty ("isGirthSensitive");
			propColor = GetSerializedProperty ("color");
			propGlossiness = GetSerializedProperty ("glossiness");
			propMetallic = GetSerializedProperty ("metallic");
			propDiffusionProfileSettings = GetSerializedProperty ("diffusionProfileSettings");
		}
		/// <summary>
		/// Raises the inspector GUI event.
		/// </summary>
		protected override void OnInspectorGUISpecific () {
			CheckUndoRequest ();

			UpdateSerialized ();

			// Log box.
			DrawLogBox ();

			int materialModeIndex = propMaterialMode.enumValueIndex;
			EditorGUILayout.PropertyField (propMaterialMode, materialModeLabel);
			ShowHelpBox (MSG_MATERIAL_MODE);
			EditorGUILayout.Space ();

			EditorGUILayout.LabelField ("Material Properties", EditorStyles.boldLabel);
			if (materialModeIndex == (int)BranchMapperElement.MaterialMode.Custom) {
				EditorGUI.BeginChangeCheck ();
				EditorGUILayout.PropertyField (propCustomMaterial, customMaterialLabel);
				ShowHelpBox (MSG_CUSTOM_MATERIAL);
				if (EditorGUI.EndChangeCheck () ||
				    materialModeIndex != propMaterialMode.enumValueIndex) {
					ApplySerialized ();
					UpdateComponent ((int)BranchMapperComponent.ComponentCommand.BuildMaterials, 
						GlobalSettings.processingDelayMedium);
					// TODO: update with pink material when no material is set.
					SetUndoControlCounter ();
				}
			} else if (materialModeIndex == (int)BranchMapperElement.MaterialMode.Texture) {
				bool mainTextureChanged = false;
				EditorGUI.BeginChangeCheck ();
				EditorGUILayout.PropertyField (propMainTexture, mainTextureLabel);
				if (EditorGUI.EndChangeCheck ()) {
					mainTextureChanged = true;
				}
				ShowHelpBox (MSG_MAIN_TEXTURE);

				bool normalTextureChanged = false;	
				EditorGUI.BeginChangeCheck ();
				EditorGUILayout.PropertyField (propNormalTexture, normalTextureLabel);
				if (EditorGUI.EndChangeCheck ()) {
					normalTextureChanged = true;
				}
				ShowHelpBox (MSG_NORMAL_TEXTURE);

				EditorGUI.BeginChangeCheck ();
				EditorGUILayout.PropertyField (propColor, colorTintLabel);
				ShowHelpBox (MSG_COLOR);

				EditorGUILayout.Slider (propGlossiness, 0f, 1f, glossinessLabel);
				ShowHelpBox (MSG_GLOSSINESS);

				EditorGUILayout.Slider (propMetallic, 0f, 1f, metallicLabel);
				ShowHelpBox (MSG_METALLIC);

				if (ExtensionManager.isHDRP) {
					ScriptableObject former = (ScriptableObject)propDiffusionProfileSettings.objectReferenceValue;
					former = 
						(ScriptableObject)EditorGUILayout.ObjectField (
							diffusionProfileLabel, 
							former, 
							System.Type.GetType ("UnityEngine.Rendering.HighDefinition.DiffusionProfileSettings, Unity.RenderPipelines.HighDefinition.Runtime"), 
							false);
					if (former != (ScriptableObject)propDiffusionProfileSettings.objectReferenceValue) {
						propDiffusionProfileSettings.objectReferenceValue = former;
						mainTextureChanged = true;
					}
					ShowHelpBox (MSG_DIFFUSION_PROFILE);
				}
				if (materialModeIndex != propMaterialMode.enumValueIndex ||
				    mainTextureChanged || normalTextureChanged ||
					EditorGUI.EndChangeCheck ()) {
					ApplySerialized ();
					UpdateComponent ((int)BranchMapperComponent.ComponentCommand.BuildMaterials, 
						GlobalSettings.processingDelayLow);
					SetUndoControlCounter ();
				}
			}
			EditorGUILayout.Space ();

			// MAPPING PROPERTIES
			EditorGUILayout.LabelField ("Mapping Properties", EditorStyles.boldLabel);
			float textureXDisplacement = 0;
			float textureYDisplacement = 0;
			if (pipelineElement.preset == PipelineElement.Preset.Tree) {
				textureXDisplacement = propMappingXDisplacement.floatValue;
				EditorGUILayout.Slider (propMappingXDisplacement, minTreeDisplacementValue, maxTreeDisplacementValue, mappingXDisplacementLabel);
				ShowHelpBox (MSG_MAPPING_X_DISP);
				textureYDisplacement = propMappingYDisplacement.floatValue;
				EditorGUILayout.Slider (propMappingYDisplacement, minTreeDisplacementValue, maxTreeDisplacementValue, mappingYDisplacementLabel);
				ShowHelpBox (MSG_MAPPING_Y_DISP);
			} else {
				textureXDisplacement = propMappingXDisplacement.floatValue;
				EditorGUILayout.Slider (propMappingXDisplacement, minShrubDisplacementValue, maxShrubDisplacementValue, mappingXDisplacementLabel);
				ShowHelpBox (MSG_MAPPING_X_DISP);
				textureYDisplacement = propMappingYDisplacement.floatValue;
				EditorGUILayout.Slider (propMappingYDisplacement, minShrubDisplacementValue, maxShrubDisplacementValue, mappingYDisplacementLabel);
				ShowHelpBox (MSG_MAPPING_Y_DISP);
			}

			int textureXTiles = 1;
			int textureYTiles = 1;
			if (pipelineElement.preset == PipelineElement.Preset.Tree) {
				textureXTiles = propMappingXTiles.intValue;
				EditorGUILayout.IntSlider (propMappingXTiles, minTreeTilesValue, maxTreeTilesValue, mappingXTilesLabel);
				ShowHelpBox (MSG_MAPPING_X_TILES);
				textureYTiles = propMappingYTiles.intValue;
				EditorGUILayout.IntSlider (propMappingYTiles, minTreeTilesValue, maxTreeTilesValue, mappingYTilesLabel);
				ShowHelpBox (MSG_MAPPING_Y_TILES);
			} else {
				textureXTiles = propMappingXTiles.intValue;
				EditorGUILayout.IntSlider (propMappingXTiles, minShrubTilesValue, maxShrubTilesValue, mappingXTilesLabel);
				ShowHelpBox (MSG_MAPPING_X_TILES);
				textureYTiles = propMappingYTiles.intValue;
				EditorGUILayout.IntSlider (propMappingYTiles, minShrubTilesValue, maxShrubTilesValue, mappingYTilesLabel);
				ShowHelpBox (MSG_MAPPING_Y_TILES);
			}

			bool isGirthSensitive = propIsGirthSensitive.boolValue;
			EditorGUILayout.PropertyField (propIsGirthSensitive, isGirthSensitiveLabel);
			ShowHelpBox (MSG_GIRTH_SENSITIVE);

			if (textureXDisplacement != propMappingXDisplacement.floatValue ||
				textureYDisplacement != propMappingYDisplacement.floatValue ||
				textureXTiles != propMappingXTiles.intValue ||
				textureYTiles != propMappingYTiles.intValue ||
				isGirthSensitive != propIsGirthSensitive.boolValue) 
			{
				ApplySerialized ();
				UpdateComponent ((int)BranchMapperComponent.ComponentCommand.SetUVs, 
					GlobalSettings.processingDelayLow);
				SetUndoControlCounter ();
			}
			DrawSeparator ();

			// Field descriptors option.
			DrawFieldHelpOptions ();

			// Preset.
			DrawPresetOptions ();
		}
		#endregion
	}
}