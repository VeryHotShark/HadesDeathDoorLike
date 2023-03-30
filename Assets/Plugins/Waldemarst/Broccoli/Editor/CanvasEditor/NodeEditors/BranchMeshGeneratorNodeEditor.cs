using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

using Broccoli.Base;
using Broccoli.Pipe;
using Broccoli.Model;
using Broccoli.Factory;
using Broccoli.Catalog;

namespace Broccoli.TreeNodeEditor
{
	/// <summary>
	/// Branch mesh generator node editor.
	/// </summary>
	[CustomEditor(typeof(BranchMeshGeneratorNode))]
	public class BranchMeshGeneratorNodeEditor : BaseNodeEditor {
		#region Vars
		/// <summary>
		/// The branch mesh generator node.
		/// </summary>
		public BranchMeshGeneratorNode branchMeshGeneratorNode;
		/// <summary>
		/// The branch mesh generator element.
		/// </summary>
		private BranchMeshGeneratorElement branchMeshGeneratorElement;
		/// <summary>
		/// Options to show on the toolbar.
		/// </summary>
		static string[] toolbarOptions = new string[] {"LODs", "Welding", "Shape"};
		static int OPTION_LOD = 0;
		static int OPTION_WELDING = 1;
		static int OPTION_SHAPE = 2;
		/// <summary>
		/// Shape catalog.
		/// </summary>
		//ShapeCatalog shapeCatalog;
		/// <summary>
		/// Selected shape index.
		/// </summary>
		int selectedShapeIndex = 0;
		/// <summary>
		/// The welding curve range.
		/// </summary>
		private static Rect weldingCurveRange = new Rect (0f, 0f, 1f, 1f);


		SerializedProperty propMeshMode;
		SerializedProperty propRangeContext;
		SerializedProperty propNodesMode;
		SerializedProperty propMinNodes;
		SerializedProperty propMaxNodes;
		SerializedProperty propMinNodeLength;
		SerializedProperty propMaxNodeLength;
		SerializedProperty propLengthVariance;


		SerializedProperty propUseBranchWelding;
		SerializedProperty propUseBranchWeldingMeshCap;
		SerializedProperty propMinBranchWeldingHierarchyRange;
		SerializedProperty propMaxBranchWeldingHierarchyRange;
		SerializedProperty propBranchWeldingHierarchyRangeCurve;
		SerializedProperty propBranchWeldingCurve;
		SerializedProperty propMinBranchWeldingDistance;
		SerializedProperty propMaxBranchWeldingDistance;
		SerializedProperty propMinAdditionalBranchWeldingSegments;
		SerializedProperty propMaxAdditionalBranchWeldingSegments;
		SerializedProperty propMinBranchWeldingUpperSpread;
		SerializedProperty propMaxBranchWeldingUpperSpread;
		SerializedProperty propMinBranchWeldingLowerSpread;
		SerializedProperty propMaxBranchWeldingLowerSpread;
		SerializedProperty propUseRootWelding;
		SerializedProperty propUseRootWeldingMeshCap;
		SerializedProperty propMinRootWeldingHierarchyRange;
		SerializedProperty propMaxRootWeldingHierarchyRange;
		SerializedProperty propRootWeldingHierarchyRangeCurve;
		SerializedProperty propRootWeldingCurve;
		SerializedProperty propMinRootWeldingDistance;
		SerializedProperty propMaxRootWeldingDistance;
		SerializedProperty propMinAdditionalRootWeldingSegments;
		SerializedProperty propMaxAdditionalRootWeldingSegments;
		SerializedProperty propMinRootWeldingUpperSpread;
		SerializedProperty propMaxRootWeldingUpperSpread;
		SerializedProperty propMinRootWeldingLowerSpread;
		SerializedProperty propMaxRootWeldingLowerSpread;

		SerializedProperty propShapeTopScale;
		SerializedProperty propShapeTopCapScale;
		SerializedProperty propShapeBottomScale;
		SerializedProperty propShapeBottomCapScale;
		SerializedProperty propShapeCapPositioning;
		SerializedProperty propShapeTopCapPos;
		SerializedProperty propShapeBottomCapPos;
		SerializedProperty propShapeTopCapFn;
		SerializedProperty propShapeBottomCapFn;
		SerializedProperty propShapeTopParam1;
		SerializedProperty propShapeTopCapParam1;
		SerializedProperty propShapeBottomParam1;
		SerializedProperty propShapeBottomCapParam1;
		SerializedProperty propShapeTopParam2;
		SerializedProperty propShapeTopCapParam2;
		SerializedProperty propShapeBottomParam2;
		SerializedProperty propShapeBottomCapParam2;
		#endregion

		#region GUI Vars
		LODListComponent lodList = new LODListComponent ();
		#endregion

		#region Messages
		private static string MSG_ALPHA = "Shape meshing is a feature currently in alpha release. Although functional, improvements and testing is being performed to identify bugs on this feature.";
			/*
		private static string MSG_USE_HARD_NORMALS = "Hard normals increases the number vertices per face while " +
			"keeping the same number of triangles. This option is useful to give a lowpoly flat shaded effect on the mesh.";
			*/
		private string MSG_SHAPE = "Selects a shape to use to stylize the branches mesh.";
		private string MSG_MESH_MODE = "Option to select how each branch mesh should be stylized.";
		private string MSG_RANGE_CONTEXT = "";
		private string MSG_NODES_MINMAX = "Range of the number of nodes to generate.";
		private string MSG_NODES_MINMAX_LENGTH = "";
		private string MSG_NODES_LENGTH_VARIANCE = "Variance in length size of nodes. Variance with value 0 gives nodes with the same length within a mesh context.";
		private string MSG_USE_BRANCH_WELDING = "Enables mesh welding between a branch and its parent branch.";
		private string MSG_BRANCH_WELDING_HIERARCHY_RANGE = "Hierarchy limit to apply welding to branches across the tree hierarchy. The base of the trunk is 0, the last tip of a terminal branch is 1.";
		private string MSG_BRANCH_WELDING_HIERARCHY_RANGE_CURVE = "Curve to control the amount of welding applied across the hierarchy limit selected for branches.";
		private string MSG_BRANCH_WELDING_CURVE = "Curve to control the shape of the welding range used on a branch.";
		private string MSG_BRANCH_WELDING_DISTANCE = "How long from the base of a branch welding should expand. This value multiplies the girth at the parent branch to get the distance.";
		private string MSG_ADDITIONAL_BRANCH_WELDING_SEGMENTS = "Adds additional points to the welding range.";
		private string MSG_BRANCH_WELDING_UPPER_SPREAD = "How much length welding should take along the parent branch on the growth (upper) direction.";
		private string MSG_BRANCH_WELDING_LOWER_SPREAD = "How much length welding should take along the parent branch against the growth (lower) direction.";
		private string MSG_USE_ROOT_WELDING = "Enables mesh welding between a root and its parent branch or root.";
		private string MSG_ROOT_WELDING_HIERARCHY_RANGE = "Hierarchy limit to apply welding to roots across the tree hierarchy. The base of the trunk is 0, the last tip of a terminal root is 1.";
		private string MSG_ROOT_WELDING_HIERARCHY_RANGE_CURVE = "Curve to control the amount of welding applied across the hierarchy limit selected for roots.";
		private string MSG_ROOT_WELDING_CURVE = "Curve to control the shape of the welding range used on a root.";
		private string MSG_ROOT_WELDING_DISTANCE = "How long from the base of a root welding should expand. This value multiplies the girth at the parent branch or root to get the distance.";
		private string MSG_ADDITIONAL_ROOT_WELDING_SEGMENTS = "Adds additional points to the welding range.";
		private string MSG_ROOT_WELDING_UPPER_SPREAD = "How much length welding should take along the parent branch on the growth (upper) direction.";
		private string MSG_ROOT_WELDING_LOWER_SPREAD = "How much length welding should take along the parent branch against the growth (lower) direction.";
		private string MSG_BRANCH_WELDING_NOT_ALLOWED = "The preview LOD definition does not implement branch welding.";
		private string MSG_ROOT_WELDING_NOT_ALLOWED = "The preview LOD definition does not implement root welding.";
		#endregion

		#region Events
		/// <summary>
		/// Actions to perform on the concrete class when the enable event is raised.
		/// </summary>
		protected override void OnEnableSpecific () {
			branchMeshGeneratorNode = target as BranchMeshGeneratorNode;
			SetPipelineElementProperty ("branchMeshGeneratorElement");
			branchMeshGeneratorElement = branchMeshGeneratorNode.branchMeshGeneratorElement;

			propMeshMode = GetSerializedProperty ("meshMode");
			propRangeContext = GetSerializedProperty ("rangeContext");
			propNodesMode = GetSerializedProperty ("nodesMode");
			propMinNodes = GetSerializedProperty ("minNodes");
			propMaxNodes = GetSerializedProperty ("maxNodes");
			propMinNodeLength = GetSerializedProperty ("minNodeLength");
			propMaxNodeLength = GetSerializedProperty ("maxNodeLength");
			propLengthVariance = GetSerializedProperty ("nodeLengthVariance");

			propUseBranchWelding = GetSerializedProperty ("useBranchWelding");
			propUseBranchWeldingMeshCap = GetSerializedProperty ("useBranchWeldingMeshCap");
			propMinBranchWeldingHierarchyRange = GetSerializedProperty ("minBranchWeldingHierarchyRange");
			propMaxBranchWeldingHierarchyRange = GetSerializedProperty ("maxBranchWeldingHierarchyRange");
			propBranchWeldingHierarchyRangeCurve = GetSerializedProperty ("branchWeldingHierarchyRangeCurve");
			propBranchWeldingCurve = GetSerializedProperty ("branchWeldingCurve");
			propMinBranchWeldingDistance = GetSerializedProperty ("minBranchWeldingDistance");
			propMaxBranchWeldingDistance = GetSerializedProperty ("maxBranchWeldingDistance");
			propMinAdditionalBranchWeldingSegments = GetSerializedProperty ("minAdditionalBranchWeldingSegments");
			propMaxAdditionalBranchWeldingSegments = GetSerializedProperty ("maxAdditionalBranchWeldingSegments");
			propMinBranchWeldingUpperSpread = GetSerializedProperty ("minBranchWeldingUpperSpread");
			propMaxBranchWeldingUpperSpread = GetSerializedProperty ("maxBranchWeldingUpperSpread");
			propMinBranchWeldingLowerSpread = GetSerializedProperty ("minBranchWeldingLowerSpread");
			propMaxBranchWeldingLowerSpread = GetSerializedProperty ("maxBranchWeldingLowerSpread");
			propUseRootWelding = GetSerializedProperty ("useRootWelding");
			propUseRootWeldingMeshCap = GetSerializedProperty ("useRootWeldingMeshCap");
			propMinRootWeldingHierarchyRange = GetSerializedProperty ("minRootWeldingHierarchyRange");
			propMaxRootWeldingHierarchyRange = GetSerializedProperty ("maxRootWeldingHierarchyRange");
			propRootWeldingHierarchyRangeCurve = GetSerializedProperty ("rootWeldingHierarchyRangeCurve");
			propRootWeldingCurve = GetSerializedProperty ("rootWeldingCurve");
			propMinRootWeldingDistance = GetSerializedProperty ("minRootWeldingDistance");
			propMaxRootWeldingDistance = GetSerializedProperty ("maxRootWeldingDistance");
			propMinAdditionalRootWeldingSegments = GetSerializedProperty ("minAdditionalRootWeldingSegments");
			propMaxAdditionalRootWeldingSegments = GetSerializedProperty ("maxAdditionalRootWeldingSegments");
			propMinRootWeldingUpperSpread = GetSerializedProperty ("minRootWeldingUpperSpread");
			propMaxRootWeldingUpperSpread = GetSerializedProperty ("maxRootWeldingUpperSpread");
			propMinRootWeldingLowerSpread = GetSerializedProperty ("minRootWeldingLowerSpread");
			propMaxRootWeldingLowerSpread = GetSerializedProperty ("maxRootWeldingLowerSpread");

			propShapeTopScale = GetSerializedProperty ("shapeTopScale");
			propShapeTopCapScale = GetSerializedProperty ("shapeTopCapScale");
			propShapeBottomScale = GetSerializedProperty ("shapeBottomScale");
			propShapeBottomCapScale = GetSerializedProperty ("shapeBottomCapScale");
			propShapeCapPositioning = GetSerializedProperty ("shapeCapPositioning");
			propShapeTopCapPos = GetSerializedProperty ("shapeTopCapPos");
			propShapeBottomCapPos = GetSerializedProperty ("shapeBottomCapPos");
			propShapeTopCapFn = GetSerializedProperty ("shapeTopCapFn");
			propShapeBottomCapFn = GetSerializedProperty ("shapeBottomCapFn");
			propShapeTopParam1 = GetSerializedProperty ("shapeTopParam1");
			propShapeTopCapParam1 = GetSerializedProperty ("shapeTopCapParam1");
			propShapeBottomParam1 = GetSerializedProperty ("shapeBottomParam1");
			propShapeBottomCapParam1 = GetSerializedProperty ("shapeBottomCapParam1");
			propShapeTopParam2 = GetSerializedProperty ("shapeTopParam2");
			propShapeTopCapParam2 = GetSerializedProperty ("shapeTopCapParam2");
			propShapeBottomParam2 = GetSerializedProperty ("shapeBottomParam2");
			propShapeBottomCapParam2 = GetSerializedProperty ("shapeBottomCapParam2");

			lodList.LoadLODs (TreeFactory.GetActiveInstance ().treeFactoryPreferences.lods, 
				TreeFactory.GetActiveInstance ().treeFactoryPreferences.previewLODIndex,
				TreeFactory.GetActiveInstance ().treeFactoryPreferences.prefabIncludeBillboard,
				TreeFactory.GetActiveInstance ().treeFactoryPreferences.prefabBillboardPercentage);
			lodList.showFieldHelp = showFieldHelp;
			lodList.onBeforeAddLOD += OnBeforeAddLOD;
			lodList.onAddLOD += OnAddLOD;
			lodList.onBeforeEditLOD += OnBeforeEditLOD;
			lodList.onEditLOD += OnEditLOD;
			lodList.onBeforeRemoveLOD += OnBeforeRemoveLOD;
			lodList.onRemoveLOD += OnRemoveLOD;
			lodList.onPreviewLODSet += OnPreviewLODSet;
			lodList.onReorderLODs += OnReorderLODs;
			lodList.onRequiresRebuild += OnRequiresRebuild;
			lodList.onSelectLOD += OnSelectLOD;
			lodList.onEditBillboard += OnEditBillboard;

			//shapeCatalog = ShapeCatalog.GetInstance ();
			//selectedShapeIndex = shapeCatalog.GetShapeIndex (branchMeshGeneratorNode.branchMeshGeneratorElement.selectedShapeId);
			selectedShapeIndex = BranchShaper.GetShaperIndex (branchMeshGeneratorNode.branchMeshGeneratorElement.shaperId);
		}
		/// <summary>
		/// Raises the disable specific event.
		/// </summary>
		protected override void OnDisableSpecific () {
			lodList.Clear ();
		}
		/// <summary>
		/// Raises the inspector GUI event.
		/// </summary>
		protected override void OnInspectorGUISpecific () {
			CheckUndoRequest ();

			UpdateSerialized ();

			bool changeCheck = false;

			branchMeshGeneratorNode.selectedToolbarOption = GUILayout.Toolbar (branchMeshGeneratorNode.selectedToolbarOption, toolbarOptions);
			EditorGUILayout.Space ();

			if (branchMeshGeneratorNode.selectedToolbarOption == OPTION_LOD) {
				lodList.DoLayout ();
				EditorGUILayout.Space ();
				lodList.DrawLODsBar ("");
			} else if (branchMeshGeneratorNode.selectedToolbarOption == OPTION_WELDING) {
				branchMeshGeneratorElement.showSectionBranchWelding = 
					EditorGUILayout.BeginFoldoutHeaderGroup (branchMeshGeneratorElement.showSectionBranchWelding, "Branch Welding");
				if (branchMeshGeneratorElement.showSectionBranchWelding) {
					EditorGUI.indentLevel++;
					EditorGUI.BeginChangeCheck ();

					bool useBranchWelding = propUseBranchWelding.boolValue;
					EditorGUILayout.PropertyField (propUseBranchWelding);
					ShowHelpBox (MSG_USE_BRANCH_WELDING);
					if (useBranchWelding) {
						LODDef previewLOD = TreeFactory.GetActiveInstance ().treeFactoryPreferences.GetPreviewLOD ();
						if (previewLOD != null && !previewLOD.allowBranchWelding) {
							EditorGUILayout.HelpBox (MSG_BRANCH_WELDING_NOT_ALLOWED, MessageType.Warning);
						}

						FloatRangePropertyField (propMinBranchWeldingHierarchyRange, propMaxBranchWeldingHierarchyRange, 0f, 1f, "Hierarchy Range");
						ShowHelpBox (MSG_BRANCH_WELDING_HIERARCHY_RANGE);

						EditorGUILayout.CurveField (propBranchWeldingHierarchyRangeCurve, Color.green, weldingCurveRange);
						ShowHelpBox (MSG_BRANCH_WELDING_HIERARCHY_RANGE_CURVE);
						EditorGUILayout.Space ();

						EditorGUILayout.CurveField (propBranchWeldingCurve, Color.green, weldingCurveRange);
						ShowHelpBox (MSG_BRANCH_WELDING_CURVE);

						FloatRangePropertyField (propMinBranchWeldingDistance, propMaxBranchWeldingDistance, 1.5f, 5f, "Welding Distance");
						ShowHelpBox (MSG_BRANCH_WELDING_DISTANCE);

						IntRangePropertyField (propMinAdditionalBranchWeldingSegments, propMaxAdditionalBranchWeldingSegments, 0, 7, "Additional Segments");
						ShowHelpBox (MSG_ADDITIONAL_BRANCH_WELDING_SEGMENTS);

						FloatRangePropertyField (propMinBranchWeldingUpperSpread, propMaxBranchWeldingUpperSpread, 1f, 4f, "Welding Upper Spread");
						ShowHelpBox (MSG_BRANCH_WELDING_UPPER_SPREAD);

						FloatRangePropertyField (propMinBranchWeldingLowerSpread, propMaxBranchWeldingLowerSpread, 1f, 4f, "Welding Lower Spread");
						ShowHelpBox (MSG_BRANCH_WELDING_LOWER_SPREAD);
					}

					if (EditorGUI.EndChangeCheck ()) changeCheck = true;
					EditorGUI.indentLevel--;
				}
				EditorGUILayout.EndFoldoutHeaderGroup ();
				EditorGUILayout.Space ();

				branchMeshGeneratorElement.showSectionRootWelding = 
					EditorGUILayout.BeginFoldoutHeaderGroup (branchMeshGeneratorElement.showSectionRootWelding, "Root Welding");
				if (branchMeshGeneratorElement.showSectionRootWelding) {
					EditorGUI.indentLevel++;
					EditorGUI.BeginChangeCheck ();

					bool useRootWelding = propUseRootWelding.boolValue;
					EditorGUILayout.PropertyField (propUseRootWelding);
					ShowHelpBox (MSG_USE_ROOT_WELDING);
					if (useRootWelding) {
						LODDef previewLOD = TreeFactory.GetActiveInstance ().treeFactoryPreferences.GetPreviewLOD ();
						if (previewLOD != null && !previewLOD.allowRootWelding) {
							EditorGUILayout.HelpBox (MSG_ROOT_WELDING_NOT_ALLOWED, MessageType.Warning);
						}

						FloatRangePropertyField (propMinRootWeldingHierarchyRange, propMaxRootWeldingHierarchyRange, 0f, 1f, "Hierarchy Range");
						ShowHelpBox (MSG_ROOT_WELDING_HIERARCHY_RANGE);

						EditorGUILayout.CurveField (propRootWeldingHierarchyRangeCurve, Color.green, weldingCurveRange);
						ShowHelpBox (MSG_ROOT_WELDING_HIERARCHY_RANGE_CURVE);
						EditorGUILayout.Space ();

						EditorGUILayout.CurveField (propRootWeldingCurve, Color.green, weldingCurveRange);
						ShowHelpBox (MSG_ROOT_WELDING_CURVE);

						FloatRangePropertyField (propMinRootWeldingDistance, propMaxRootWeldingDistance, 1.5f, 5f, "Welding Distance");
						ShowHelpBox (MSG_ROOT_WELDING_DISTANCE);

						IntRangePropertyField (propMinAdditionalRootWeldingSegments, propMaxAdditionalRootWeldingSegments, 0, 7, "Additional Segments");
						ShowHelpBox (MSG_ADDITIONAL_ROOT_WELDING_SEGMENTS);

						FloatRangePropertyField (propMinRootWeldingUpperSpread, propMaxRootWeldingUpperSpread, 1f, 4f, "Welding Upper Spread");
						ShowHelpBox (MSG_ROOT_WELDING_UPPER_SPREAD);

						FloatRangePropertyField (propMinRootWeldingLowerSpread, propMaxRootWeldingLowerSpread, 1f, 4f, "Welding Lower Spread");
						ShowHelpBox (MSG_ROOT_WELDING_LOWER_SPREAD);
					}

					if (EditorGUI.EndChangeCheck ()) changeCheck = true;
					EditorGUI.indentLevel--;
				}

				if (changeCheck) {
					ApplySerialized ();
					UpdatePipeline (GlobalSettings.processingDelayHigh);
					NodeEditorFramework.NodeEditor.RepaintClients ();
					branchMeshGeneratorNode.branchMeshGeneratorElement.Validate ();
					SetUndoControlCounter ();
				}
			} else if (branchMeshGeneratorNode.selectedToolbarOption == OPTION_SHAPE) {
				EditorGUI.BeginChangeCheck ();

				// MESHING MODES
				EditorGUILayout.PropertyField (propMeshMode);
				ShowHelpBox (MSG_MESH_MODE);
				EditorGUILayout.Space ();

				// IF SHAPE MODE SELECTED
				if (propMeshMode.enumValueIndex == (int)BranchMeshGeneratorElement.MeshMode.Shape) {
					// ALPHA MESSAGE.
					EditorGUILayout.HelpBox (MSG_ALPHA, MessageType.Warning);
					EditorGUILayout.Space ();

					// SELECT SHAPE.
					selectedShapeIndex = EditorGUILayout.Popup ("Shape", selectedShapeIndex, BranchShaper.shaperNames);
					ShowHelpBox (MSG_SHAPE);
					EditorGUILayout.Space ();

					// RANGE CONTEXT.
					EditorGUILayout.PropertyField (propRangeContext);
					ShowHelpBox (MSG_RANGE_CONTEXT);
					EditorGUILayout.Space ();

					// IF LENGTH NODE MODE SELECTED.
					if (propNodesMode.enumValueIndex == (int)BranchMeshGeneratorElement.NodesMode.Length) {
						FloatRangePropertyField (propMinNodeLength, propMaxNodeLength, 0.1f, 2f, "Length");
						ShowHelpBox (MSG_NODES_MINMAX_LENGTH);
					} else {
						// NUMBER MODE SELECTED.
						IntRangePropertyField (propMinNodes, propMaxNodes, 1, 8, "Nodes");
						ShowHelpBox (MSG_NODES_MINMAX);
						
						EditorGUILayout.Slider (propLengthVariance, 0f, 1f, "Node Size Variance");
						ShowHelpBox (MSG_NODES_LENGTH_VARIANCE);
					}

					BranchShaper shaper = BranchShaper.GetSingleton (branchMeshGeneratorNode.branchMeshGeneratorElement.shaperId);
					if (shaper != null) {
						if (shaper.bottomScaleExposed) {
							branchMeshGeneratorNode.branchMeshGeneratorElement.shapeBottomScale = 
								EditorGUILayout.Slider (
									shaper.bottomScaleName,
									branchMeshGeneratorNode.branchMeshGeneratorElement.shapeBottomScale,
									shaper.minScale, shaper.maxScale);
						}
						if (shaper.bottomCapScaleExposed) {
							branchMeshGeneratorNode.branchMeshGeneratorElement.shapeBottomCapScale = 
								EditorGUILayout.Slider (
									shaper.bottomCapScaleName,
									branchMeshGeneratorNode.branchMeshGeneratorElement.shapeBottomCapScale,
									shaper.minScale, shaper.maxScale);
						}
						if (shaper.topScaleExposed) {
							branchMeshGeneratorNode.branchMeshGeneratorElement.shapeTopScale = 
								EditorGUILayout.Slider (
									shaper.topScaleName,
									branchMeshGeneratorNode.branchMeshGeneratorElement.shapeTopScale,
									shaper.minScale, shaper.maxScale);
						}
						if (shaper.topCapScaleExposed) {
							branchMeshGeneratorNode.branchMeshGeneratorElement.shapeTopCapScale = 
								EditorGUILayout.Slider (
									shaper.topCapScaleName,
									branchMeshGeneratorNode.branchMeshGeneratorElement.shapeTopCapScale,
									shaper.minScale, shaper.maxScale);
						}
						if (shaper.isCapGirthPos) {
							if (shaper.bottomCapGirthPosExposed) {
								branchMeshGeneratorNode.branchMeshGeneratorElement.shapeBottomCapGirthPos = 
									EditorGUILayout.Slider (
										shaper.bottomCapGirthPosName,
										branchMeshGeneratorNode.branchMeshGeneratorElement.shapeBottomCapGirthPos,
										shaper.minCapGirthPos, shaper.maxCapGirthPos);
							}
							if (shaper.topCapGirthPosExposed) {
								branchMeshGeneratorNode.branchMeshGeneratorElement.shapeTopCapGirthPos = 
									EditorGUILayout.Slider (
										shaper.topCapGirthPosName,
										branchMeshGeneratorNode.branchMeshGeneratorElement.shapeTopCapGirthPos,
										shaper.minCapGirthPos, shaper.maxCapGirthPos);
							}
						}
						if (shaper.bottomParam1Exposed) {
							branchMeshGeneratorNode.branchMeshGeneratorElement.shapeBottomParam1 = 
								EditorGUILayout.Slider (
									shaper.bottomParam1Name,
									branchMeshGeneratorNode.branchMeshGeneratorElement.shapeBottomParam1,
									shaper.minParam1, shaper.maxParam1);
						}
						if (shaper.bottomCapParam1Exposed) {
							branchMeshGeneratorNode.branchMeshGeneratorElement.shapeBottomCapParam1 = 
								EditorGUILayout.Slider (
									shaper.bottomCapParam1Name,
									branchMeshGeneratorNode.branchMeshGeneratorElement.shapeBottomCapParam1,
									shaper.minParam1, shaper.maxParam1);
						}
						if (shaper.topParam1Exposed) {
							branchMeshGeneratorNode.branchMeshGeneratorElement.shapeTopParam1 = 
								EditorGUILayout.Slider (
									shaper.topParam1Name,
									branchMeshGeneratorNode.branchMeshGeneratorElement.shapeTopParam1,
									shaper.minParam1, shaper.maxParam1);
						}
						if (shaper.topCapParam1Exposed) {
							branchMeshGeneratorNode.branchMeshGeneratorElement.shapeTopCapParam1 = 
								EditorGUILayout.Slider (
									shaper.topCapParam1Name,
									branchMeshGeneratorNode.branchMeshGeneratorElement.shapeTopCapParam1,
									shaper.minParam1, shaper.maxParam1);
						}
						if (shaper.bottomParam2Exposed) {
							branchMeshGeneratorNode.branchMeshGeneratorElement.shapeBottomParam2 = 
								EditorGUILayout.Slider (
									shaper.bottomParam2Name,
									branchMeshGeneratorNode.branchMeshGeneratorElement.shapeBottomParam2,
									shaper.minParam2, shaper.maxParam2);
						}
						if (shaper.bottomCapParam2Exposed) {
							branchMeshGeneratorNode.branchMeshGeneratorElement.shapeBottomCapParam2 = 
								EditorGUILayout.Slider (
									shaper.bottomCapParam2Name,
									branchMeshGeneratorNode.branchMeshGeneratorElement.shapeBottomCapParam2,
									shaper.minParam2, shaper.maxParam2);
						}
						if (shaper.topParam2Exposed) {
							branchMeshGeneratorNode.branchMeshGeneratorElement.shapeTopParam2 = 
								EditorGUILayout.Slider (
									shaper.topParam2Name,
									branchMeshGeneratorNode.branchMeshGeneratorElement.shapeTopParam2,
									shaper.minParam2, shaper.maxParam2);
						}
						if (shaper.topCapParam2Exposed) {
							branchMeshGeneratorNode.branchMeshGeneratorElement.shapeTopCapParam2 = 
								EditorGUILayout.Slider (
									shaper.topCapParam2Name,
									branchMeshGeneratorNode.branchMeshGeneratorElement.shapeTopCapParam2,
									shaper.minParam2, shaper.maxParam2);
						}
					}

					/*
					if (GlobalSettings.debugEnabled) {
						EditorGUILayout.Space ();
						EditorGUILayout.Space ();
						EditorGUILayout.Space ();
						EditorGUILayout.PropertyField (propShapeCapPositioning);
						if (branchMeshGeneratorElement.shapeCapPositioning == BranchMeshGeneratorElement.ShapeCapPositioning.LengthRelative) {
							EditorGUILayout.Slider (propShapeBottomCapPos, 0f, 1f);
							EditorGUILayout.Slider (propShapeTopCapPos, 0f, 1f);
						} else {
							EditorGUILayout.Slider (propShapeBottomCapPos, 0f, 2f);
							EditorGUILayout.Slider (propShapeTopCapPos, 0f, 2f);
						}
						EditorGUILayout.IntSlider (propShapeBottomCapFn, 0, 8);
						EditorGUILayout.IntSlider (propShapeTopCapFn, 0, 8);

						//  Bottom Cap.
						EditorGUILayout.Space ();
						EditorGUILayout.Slider (propShapeBottomScale, 0f, 5f);
						EditorGUILayout.Slider (propShapeBottomCapScale, 0f, 5f);
						EditorGUILayout.PropertyField (propShapeBottomParam1);
						EditorGUILayout.PropertyField (propShapeBottomCapParam1);
						EditorGUILayout.PropertyField (propShapeBottomParam2);
						EditorGUILayout.PropertyField (propShapeBottomCapParam2);

						// Top Cap.
						EditorGUILayout.Space ();
						EditorGUILayout.Slider (propShapeTopScale, 0f, 5f);
						EditorGUILayout.Slider (propShapeTopCapScale, 0f, 5f);
						EditorGUILayout.PropertyField (propShapeTopParam1);
						EditorGUILayout.PropertyField (propShapeTopCapParam1);
						EditorGUILayout.PropertyField (propShapeTopParam2);
						EditorGUILayout.PropertyField (propShapeTopCapParam2);
					}
					*/
				}

				if (EditorGUI.EndChangeCheck () && 
					propMinNodes.intValue <= propMaxNodes.intValue && 
					propMinNodeLength.floatValue <= propMaxNodeLength.floatValue)
				{
					/*
					ShapeCatalog.ShapeItem shapeItem = shapeCatalog.GetShapeItem (selectedShapeIndex); // -1 because of the 'default' option
					if (shapeItem == null || propMeshMode.enumValueIndex == (int)BranchMeshGeneratorElement.MeshMode.Default) {
						branchMeshGeneratorNode.branchMeshGeneratorElement.shapeCollection = null;
					} else {
						branchMeshGeneratorNode.branchMeshGeneratorElement.selectedShapeId = shapeItem.id;
						branchMeshGeneratorNode.branchMeshGeneratorElement.shapeCollection = shapeItem.shapeCollection;
					}
					*/
					branchMeshGeneratorNode.branchMeshGeneratorElement.shaperId = BranchShaper.GetShaperId (selectedShapeIndex);
					EditorUtility.SetDirty (branchMeshGeneratorNode);
					ApplySerialized ();
					UpdatePipeline (GlobalSettings.processingDelayHigh);
					NodeEditorFramework.NodeEditor.RepaintClients ();
					branchMeshGeneratorNode.branchMeshGeneratorElement.Validate ();
					SetUndoControlCounter ();
				}
			}
			EditorGUILayout.Space ();

			/*
			if (branchMeshGeneratorNode.branchMeshGeneratorElement.showLODInfoLevel == 1) {
			} else if (branchMeshGeneratorNode.branchMeshGeneratorElement.showLODInfoLevel == 2) {
			} else {
				EditorGUILayout.HelpBox ("LOD0\nVertex Count: " + branchMeshGeneratorNode.branchMeshGeneratorElement.verticesCountSecondPass +
					"\nTriangle Count: " + branchMeshGeneratorNode.branchMeshGeneratorElement.trianglesCountSecondPass + "\nLOD1\nVertex Count: " + branchMeshGeneratorNode.branchMeshGeneratorElement.verticesCountFirstPass +
				"\nTriangle Count: " + branchMeshGeneratorNode.branchMeshGeneratorElement.trianglesCountFirstPass, MessageType.Info);
			}
			EditorGUILayout.Space ();
			*/
			// Seed options.
			DrawSeedOptions ();
			// Field descriptors option.
			DrawFieldHelpOptions ();
		}
		/// <summary>
		/// Called when the ShowFieldHelp flag changed.
		/// </summary>
		protected override void OnShowFieldHelpChanged () {
			lodList.showFieldHelp = showFieldHelp;
		}
		#endregion

		#region LOD List
		/// <summary>
        /// Callback to call when a LOD definition is selected on the list.
        /// </summary>
		void OnSelectLOD (LODDef lod, int index) {}
        /// <summary>
        /// Callback to call before a LOD definition instance gets added to the list.
        /// </summary>
		void OnBeforeAddLOD (LODDef lod) {
			Undo.RecordObject (TreeFactory.GetActiveInstance (), "Adding LOD Definition.");
		}
        /// <summary>
        /// Call back to call after a LOD definition has been added to the list.
        /// </summary>
		void OnAddLOD (LODDef lod, int index) {}
		/// <summary>
        /// Callback to call before a LOD is edited.
        /// </summary>
		void OnBeforeEditLOD (LODDef lod) {
			Undo.RecordObject (TreeFactory.GetActiveInstance (), "Editing LOD Definition.");
		}
		/// <summary>
        /// Callback to call after a LOD is edited.
        /// </summary>
		void OnEditLOD (LODDef lod) {}
        /// <summary>
        /// Callback to call before a LOD definition get deleted from the list.
        /// </summary>
		void OnBeforeRemoveLOD (LODDef lod, int index) {
			Undo.RecordObject (TreeFactory.GetActiveInstance (), "Removing LOD Definition.");
		}
        /// <summary>
        /// Callback to call after a LOD definition has been removed from the list.
        /// </summary>
		void OnRemoveLOD (LODDef lod) {}
		/// <summary>
		/// Callback to call after the list is reordered.
		/// </summary>
		void OnReorderLODs () {}
		/// <summary>
		/// Callback to call when a preview LOD gets assigned.
		/// </summary>
		void OnPreviewLODSet (LODDef lod) {
			TreeFactory.GetActiveInstance ().treeFactoryPreferences.previewLODIndex =
				TreeFactory.GetActiveInstance ().treeFactoryPreferences.lods.IndexOf (lod);
			ApplySerialized ();
			OnRequiresRebuild ();
		}
		/// <summary>
		/// Called when changes on the preview LOD requires the structure to be rebuild.
		/// </summary>
		void OnRequiresRebuild () {
			ApplySerialized ();
			UpdatePipeline (GlobalSettings.processingDelayMedium);
			NodeEditorFramework.NodeEditor.RepaintClients ();
			branchMeshGeneratorNode.branchMeshGeneratorElement.Validate ();
		}
		/// <summary>
		/// Called when the billboard settings change.
		/// </summary>
		/// <param name="hasBillboard"><c>True</c> to include a billboard inthe final Prefab LOD group.</param>
		/// <param name="billboardPercentage">PErcentage in the LOD group.</param>
		void OnEditBillboard (bool hasBillboard, float billboardPercentage) {
			TreeFactory.GetActiveInstance ().treeFactoryPreferences.prefabIncludeBillboard = hasBillboard;
			TreeFactory.GetActiveInstance ().treeFactoryPreferences.prefabBillboardPercentage = billboardPercentage;
			ApplySerialized ();
		}
		#endregion
	}
}