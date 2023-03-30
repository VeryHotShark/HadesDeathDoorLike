using System.Collections.Generic;

using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

using Broccoli.Base;
using Broccoli.Utils;
using Broccoli.Pipe;
using Broccoli.Model;
using Broccoli.NodeEditorFramework;
using Broccoli.Generator;
using Broccoli.Component;
using Broccoli.Factory;

namespace Broccoli.TreeNodeEditor
{
	/// <summary>
	/// Structure generator node editor.
	/// </summary>
	[CustomEditor(typeof(StructureGeneratorElement))]
	public class StructureGeneratorElementEditor : PipelineElementEditor {
		bool useGraphView = GlobalSettings.experimentalGraphViewCanvas;

		#region Vars
		/// <summary>
		/// The structure generator node.
		/// </summary>
		public StructureGeneratorElement structureGeneratorElement;
		/// <summary>
		/// Component used to commit structure changes.
		/// </summary>
		public StructureGeneratorComponent structureGeneratorComponent;
		/// <summary>
		/// Bezier curve editor to edit branch curves.
		/// </summary>
		/// <returns>Bezier curve editor.</returns>
		public BezierCurveEditor curveEditor;
		/// <summary>
		/// Id of a single selected curve.
		/// </summary>
		private System.Guid _singleSelectedCurveId = System.Guid.Empty;
		/// <summary>
		/// The selected curve required processing.
		/// </summary>
		private bool _singleSelectedCurveProcessRequired = false;
		/// <summary>
		/// The canvas used for the level nodes.
		/// </summary>
		StructureCanvas structureCanvas;
		StructureGraphView structureGraph;
		/// <summary>
		/// The structure canvas cache.
		/// </summary>
		NodeEditorUserCache structureCanvasCache;
		SerializedProperty propRootStructureLevel;
		/// <summary>
		/// The property minimum frequency.
		/// </summary>
		SerializedProperty propMinFrequency;
		/// <summary>
		/// The property max frequency.
		/// </summary>
		SerializedProperty propMaxFrequency;
		/// <summary>
		/// The length of the property minimum.
		/// </summary>
		SerializedProperty propMinLength;
		/// <summary>
		/// The length of the property max.
		/// </summary>
		SerializedProperty propMaxLength;
		/// <summary>
		/// The property radius.
		/// </summary>
		SerializedProperty propRadius;
		/// <summary>
		/// The property to override noise.
		/// </summary>
		SerializedProperty propOverrideNoise;
		/// <summary>
		/// The property noise.
		/// </summary>
		SerializedProperty propNoise;
		/// <summary>
		/// The property noise scale.
		/// </summary>
		SerializedProperty propNoiseScale;
		/// <summary>
		/// The property structure levels.
		/// </summary>
		SerializedProperty propStructureLevels;
		/// <summary>
		/// True when the canvas needs reinitialization.
		/// </summary>
		bool reinitCanvas = false;
		/// <summary>
		/// Saves the id of the structure currently being drawn.
		/// </summary>
		private System.Guid _editStructureId = System.Guid.Empty;
		/// <summary>
		/// Structure instance being drawn.
		/// </summary>
		private StructureGenerator.Structure _editStructure = null;
		/// <summary>
		/// Saves the id of the structure level selected.
		/// </summary>
		private int _editStructureLevelId = -1;
		public int selectedStructureLevelId {
			get { return _editStructureLevelId; }
		}
		/// <summary>
		/// Dictionary to save the id of a node and the curve (branch) it belongs to.
		/// </summary>
		/// <typeparam name="System.Guid">Curve unique id.</typeparam>
		/// <typeparam name="int">Id of the curve.</typeparam>
		/// <returns></returns>
		private Dictionary<System.Guid, System.Guid> _nodeToCurve = new Dictionary<System.Guid, System.Guid> ();
		/// <summary>
		/// List of selected nodes.
		/// </summary>
		/// <typeparam name="BezierNode">Node of bezier curves.</typeparam>
		/// <returns></returns>
		private List<BezierNode> _selectedNodes = new List<BezierNode> ();
		private List<int> _selectedIndexes = new List<int> ();
		/// <summary>
		/// Id of selected curves.
		/// </summary>
		/// <typeparam name="System.Guid">Id of curve.</typeparam>
		/// <returns></returns>
		private List<System.Guid> _selectedCurveIds = new List<System.Guid> ();
		private List<int> _tunedBranchIds = new List<int> ();
		/// <summary>
		/// Color of bezier curves.
		/// </summary>
		/// <returns>Color.</returns>
		Color curveColor = new Color (1, 0.372f, 0.058f);
		/// <summary>
		/// Color of selected bezier curves.
		/// </summary>
		/// <returns>Color.</returns>
		Color selectedCurveColor = new Color (1, 0.239f, 0.058f);
		/// <summary>
		/// Selected toolbar to edit structure level properties.
		/// </summary>
		int selectedPanel = 0;
		/// <summary>
		/// Range mask to apply curves from 0,0 to 1,1 values.
		/// </summary>
		private static Rect scaleCurveRange = new Rect (0f, 0f, 1f, 1f);
		Button contentsButton;
		private static float minRootTreeLengthValue = 0.1f;
		private static float maxRootTreeLengthValue = 8f;
		private static float minRootShrubLengthValue = 0.1f;
		private static float maxRootShrubLengthValue = 1.2f;
		private static float minTreeLengthValue = 0.1f;
		private static float maxTreeLengthValue = 6f;
		private static float minShrubLengthValue = 0.1f;
		private static float maxShrubLengthValue = 1.2f;
		#endregion

		#region GUI Contents and Labels
		private static GUIContent rootFrequencyLabel = new GUIContent ("Frequency", "Number of steam branches (trunks) to generate.");
		private static GUIContent rootLengthLabel = new GUIContent ("Length", "Length range for the steam branches (trunks).");
		private static GUIContent rootRadiusLabel = new GUIContent ("Radius", "Radius area to randomly place the steam branches (trunks).");
		private static GUIContent rootNoiseLabel = new GUIContent ("Noise", "Noise weight to apply to steam branches (trunks).");
		private static GUIContent rootNoiseScaleLabel = new GUIContent ("Noise Scale", "Noise scale (multiplier) to apply to steam branches (trunks).");
		private static GUIContent sproutGroupsLabel = new GUIContent ("Sprout Group", "Selects the sprout group applied to the generated sprouts. A sprout group is required in order for the sprouts to be meshed.");
		private static GUIContent frequencyLabel = new GUIContent ("Frequency", "Number of structures to generate.");
		private static GUIContent probabilityLabel = new GUIContent ("Probability", "The probability of this level to produce structures.");
		private static GUIContent sharedProbabilityLabel = new GUIContent ("Shared Probability", "When the level is part of a shared group this value is the probability within this group to be selected to generate its structures.");
		private static GUIContent distributionLabel = new GUIContent ("Distribution", "How the structures will be generated along their parent branch.");
		private static GUIContent whorledStepLabel = new GUIContent ("Whorled Step", "For whorled distribution, how many structures each whorled step would contain.");
		private static GUIContent distributionSpacingVarianceLabel = new GUIContent ("Spacing Variance", "Adds length variance between the structures.");
		private static GUIContent distributionAngleVarianceLabel = new GUIContent ("Angle Variance", "Adds angle variance to the structures.");
		private static GUIContent distributionCurveLabel = new GUIContent ("Curve", "Distribution curve to place the structures along the parent structure.");
		private static GUIContent twirlLabel = new GUIContent ("Twirl", "Rotation angle on the spawned elements taking the parent branch direction as axis.");
		private static GUIContent twirlOffsetLabel = new GUIContent ("Offset", "Add an angle offset to each twirl step.");
		private static GUIContent randomTwirlOffsetLabel = new GUIContent ("Random Offset", "If enabled each twirl steps has a random angle offset.");
		private static GUIContent lengthAtTopLabel = new GUIContent ("Length at Top", "Length value for spawned structures at the top end of the parent branch.");
		private static GUIContent lengthAtBaseLabel = new GUIContent ("Length at Base", "Length value for spawned structures at the base end of the parent branch.");
		private static GUIContent lengthCurveLabel = new GUIContent ("Length Curve", "Length distribution curve, from base to the top of the parent branch.");
		private static GUIContent girthScaleLabel = new GUIContent ("Girth Scale", "Scale to apply to the girth of the generated branches.");
		private static GUIContent fromBranchCenterLabel = new GUIContent ("From Branch Center", "If set sprouts origin is at the center of the branch and not its surface.");
		private static GUIContent enableStructureLabel = new GUIContent ("Enabled", "Flag to enable/disable this level to generate structures on the working tree. " +
				"Note: disabling a parent structure stops all of its descendants levels from generation structures as well.");
		private static GUIContent[] aspectsTrunkPanelOptions = new GUIContent[] {
			new GUIContent ("Structure", "Options to control the frequency, position, twirl, length and girth of structures."), 
			new GUIContent ("Advanced", "Other options to control per structure generator, like overriding noise.")};
		private static GUIContent[] aspectsSproutPanelOptions = new GUIContent[] {
			new GUIContent ("Structure", "Options to control the frequency, position, twirl, length and girth of structures."), 
			new GUIContent ("Alignment", "Options to control the direction and orientation of structures."), 
			new GUIContent ("Range", "Options to control the spawning range of the structures.")};
		private static GUIContent[] aspectsFullPanelOptions = new GUIContent[] {
			new GUIContent ("Structure", "Options to control the frequency, position, twirl, length and girth of structures."), 
			new GUIContent ("Alignment", "Options to control the direction and orientation of structures."), 
			new GUIContent ("Range", "Options to control the spawning range of the structures."),
			new GUIContent ("Advanced", "Other options to control per structure generator, like overriding noise.")};
		#endregion

		#region Constants
		public const int PANEL_STRUCTURE = 0;
		public const int PANEL_ALIGNMENT = 1;
		public const int PANEL_RANGE = 2;
		public const int PANEL_ADVANCED = 3;
		#endregion

		#region Messages
		private static string MSG_ENABLED = "Enables/disabled this structure level element generation. " +
			"If disabled then all the downstream levels are disabled as well.";
		private static string MSG_MAIN_MIN_MAX_FREQUENCY = "Number of possible branches to produce.";
		private static string MSG_MAIN_MIN_MAX_LENGTH = "Length range for each produced branch.";
		private static string MSG_MAIN_RADIUS = "Radius for the circular area where the branches will spawn.";
		private static string MSG_OVERRIDE_NOISE = "Overrides global noise parameters for branch structures generated by this node.";
		private static string MSG_NOISE = "Override noise value for this structure generator.";
		private static string MSG_NOISE_SCALE = "Override noise scale value for this structure generator.";
		private static string MSG_SPROUT_GROUP = "Selects the sprout group applied to the generated sprouts. " +
			"Sprouts must belong to a sprout group in order to be meshed.";
		private static string MSG_MIN_MAX_FREQUENCY = "Number of possible structures to produce.";
		private static string MSG_MIN_FREQUENCY = "Minimum number of possible elements to produce.";
		private static string MSG_MAX_FREQUENCY = "Maximum number of possible elements to produce.";
		private static string MSG_PROBABILITY = "Probability of occurrence for this level.";
		private static string MSG_SHARED_PROBABILITY = "Probability to be chosen from a group of shared levels.";
		private static string MSG_DISTRIBUTION_MODE = "Distribution mode to place te elements along the parent branch.";
		private static string MSG_DISTRIBUTION_WHORLED = "Number of elements per node on the parent branch.";
		private static string MSG_DISTRIBUTION_SPACING_VARIANCE = "Adds spacing variance between branches along the parent branch.";
		private static string MSG_DISTRIBUTION_ANGLE_VARIANCE = "Add angle variance between branches along the parent branch.";
		private static string MSG_DISTRIBUTION_CURVE = "Curve of distribution for the nodes of elements along the parent branch. " +
			"From the base of the branch to the tip.";
		private static string MSG_RANDOM_TWIRL_OFFSET_ENABLED = "If enabled each twirl steps has a random angle offset.";
		private static string MSG_TWIRL_OFFSET = "Add an angle offset to each twirl step.";
		private static string MSG_TWIRL = "Rotation angle on the spawned elements taking the parent branch direction as axis.";
		private static string MSG_PARALLEL_ALIGN_AT_TOP = "Value of direction alignment for spawned element following " +
			"their parent branch direction at the top end of it.";
		private static string MSG_PARALLEL_ALIGN_AT_BASE = "Value of direction alignment for spawned element following " +
			"their parent branch direction at the base end of it.";
		private static string MSG_PARALLEL_ALIGN_CURVE = "Parallel alignment distribution curve from base to top of the parent branch.";
		private static string MSG_GRAVITY_ALIGN_AT_TOP = "Value of direction alignment for spawned element against " +
			"gravity at the top end of the parent branch.";
		private static string MSG_GRAVITY_ALIGN_AT_BASE = "Value of direction alignment for spawned element against " +
			"gravity at the base end of the parent branch.";
		private static string MSG_GRAVITY_ALIGN_CURVE = "Gravity alignment distribution curve from base to top of the parent branch.";
		private static string MSG_HORIZONTAL_ALIGN_AT_TOP = "Value of direction alignment for spawned elements " + 
			"to the horizontal plane at the top end of the parent branch.";
		private static string MSG_HORIZONTAL_ALIGN_AT_BASE = "Value of direction alignment for spawned elements " + 
			"to the horizontal plane at the base end of the parent branch.";
		private static string MSG_HORIZONTAL_ALIGN_CURVE = "Horizontal alignment distribution curve from base to top of the parent branch.";
		private static string MSG_LENGTH_AT_TOP = "Length value for spawned structures at the top end of the parent branch.";
		private static string MSG_LENGTH_AT_BASE = "Length value for spawned structures at the base end of the parent branch.";
		private static string MSG_LENGTH_CURVE = "Length distribution curve, from base to the top of the parent branch.";
		private static string MSG_GIRTH_SCALE = "Girth scale to apply to generated branches.";
		private static string MSG_RANGE_ENABLED = "If enabled spawned elements will only appear along the specified " +
			"range of their parent branch length.";
		private static string MSG_RANGE = "The total number of structures to generate by this level is distributed along this range (0 = base of the structure, 1 = top of the structure).";
		private static string MSG_MASK_RANGE = "From the total number of structures generated, only those falling within this range are added to their parent structure (0 = base of the structure, 1 = top of the structure).";
		private static string MSG_FROM_BRANCH_CENTER = "If set sprouts origin is at the center of the branch and not its surface.";
		private static string MSG_APPLY_BREAK = "Branches generated have a chance to break at some point, thus not getting meshed after the break point.";
		private static string MSG_BREAK_PROBABILITY = "Probability for branches generated by this structure to break. " +
			"The x axis is the position of the branch at its parent branch (0 at base, 1 at top.). The y axis is the probability to break (0 to 1).";
		private static string MSG_BREAK_RANGE = "Length range for the break point to appear.";
		#endregion

		#region Events
		/// <summary>
		/// Creates the UI Elements to be displayed in this inspector.
		/// </summary>
		/// <returns>UI elements to be displayed.</returns>
		public override VisualElement CreateInspectorGUI () {
			var container = new VisualElement();
 
			container.Add(new IMGUIContainer(OnInspectorGUI));
			if (useGraphView) {
				if (structureGraph == null) {
					structureGraph = new StructureGraphView ();
					structureGraph.Init (structureGeneratorElement.canvasOffset, 1f);
					SetStructureGraphGUI (structureGraph.guiContainer);
					container.Add (structureGraph);
					structureGraph.style.position = UnityEngine.UIElements.Position.Absolute;
					structureGraph.StretchToParentSize ();
					BindStructureGraphEvents ();
					reinitCanvas = true;
				}
			}
		
			return container;
		}
		/// <summary>
		/// Raises the destroy event.
		/// </summary>
		void OnDestroy () {
			if (useGraphView) {
				structureGraph.parent.Remove (structureGraph);
				structureGraph = null;
			} else {
				DestroyImmediate (structureCanvas);
			}
		}
		/// <summary>
		/// Actions to perform on the concrete class when the enable event is raised.
		/// </summary>
		protected override void OnEnableSpecific () {
			structureGeneratorElement = target as StructureGeneratorElement;

			structureGeneratorElement.BuildStructureLevelTree ();

			// SETUP CANVAS
			if (useGraphView) {
				ReinitGraph ();
			} else {
				ReinitCanvas ();
				structureCanvas.onSelectNode -= OnSelectStructureLevel;
				structureCanvas.onSelectNode += OnSelectStructureLevel;
				structureCanvas.onDeselectNode -= OnDeselectStructureLevel;
				structureCanvas.onDeselectNode += OnDeselectStructureLevel;
			}

			// SETUP BEZIER CURVE EDITOR
			curveEditor = new BezierCurveEditor ();
			curveEditor.OnEnable ();
			curveEditor.showTools = true;
			/*
			curveEditor.debugEnabled = true;
			curveEditor.debugShowCurvePoints = true;
			curveEditor.debugShowPointForward = true;
			curveEditor.debugShowPointNormal = true;
			curveEditor.debugShowPointUp = true;
			curveEditor.debugShowPointTangent = true;
			*/
			curveEditor.onEditModeChanged += OnEditModeChanged;
			curveEditor.onSelectionChanged += OnNodeSelectionChanged;
			curveEditor.onCheckMoveNodes += OnCheckMoveNodes;

			// Move nodes.
			curveEditor.onBeginMoveNodes += OnBeginMoveNodes;
			curveEditor.onMoveNodes += OnMoveNodes;
			curveEditor.onEndMoveNodes += OnEndMoveNodes;
			curveEditor.onBeforeEditNode += OnBeforeEditNode;
			curveEditor.onEditNode += OnEditNode;

			// Add nodes.
			curveEditor.onBeforeAddNode += OnBeforeAddNode;
			curveEditor.onAddNode += OnAddNode;

			// Remove nodes.
			curveEditor.onBeforeRemoveNodes += OnBeforeRemoveNodes;
			curveEditor.onRemoveNodes += OnRemoveNodes;

			// Move handles.
			curveEditor.onBeginMoveHandle += OnBeginMoveHandle;
			curveEditor.onMoveHandle += OnMoveHandle;
			curveEditor.onEndMoveHandle += OnEndMoveHandle;

			curveEditor.onCheckNodeControls += OnCheckNodeMoveControls;
			curveEditor.nodeSize = 0.065f;
			curveEditor.curveWidth = 3f;
			curveEditor.selectedCurveWidth = 4f;
			curveEditor.curveColor = curveColor;
			curveEditor.selectedCurveColor = selectedCurveColor;
			curveEditor.nodeColor = curveColor;
			curveEditor.selectedNodeColor = selectedCurveColor;
			curveEditor.nodeHandleColor = curveColor;
			curveEditor.selectedNodeHandleColor = selectedCurveColor;
			curveEditor.preselectedNodeColor = Color.red;

			propRootStructureLevel = GetSerializedProperty ("rootStructureLevel");
			propMinFrequency = propRootStructureLevel.FindPropertyRelative ("minFrequency");
			propMaxFrequency = propRootStructureLevel.FindPropertyRelative ("maxFrequency");
			propMinLength = propRootStructureLevel.FindPropertyRelative ("minLengthAtBase");
			propMaxLength = propRootStructureLevel.FindPropertyRelative ("maxLengthAtBase");
			propRadius = propRootStructureLevel.FindPropertyRelative ("radius");
			propOverrideNoise = propRootStructureLevel.FindPropertyRelative ("overrideNoise");
			propNoise = propRootStructureLevel.FindPropertyRelative ("noise");
			propNoiseScale = propRootStructureLevel.FindPropertyRelative ("noiseScale");
			propStructureLevels = GetSerializedProperty ("flatStructureLevels");

			structureGeneratorComponent = (StructureGeneratorComponent)TreeFactory.GetActiveInstance ().componentManager.GetFactoryComponent (structureGeneratorElement);

			TreeFactory.GetActiveInstance ().onBeforeProcessPipelinePreview += onBeforeProcessPipelinePreview;
			TreeFactory.GetActiveInstance ().onProcessPipeline += onProcessPipeline;

			SetStructureInspectorEnabled (structureGeneratorElement.inspectStructureEnabled);

			if (structureGeneratorElement.selectedLevel != null) {
				SetSelectedStructureLevel (structureGeneratorElement.selectedLevel.id);
			} else {
				SetSelectedStructureLevel (structureGeneratorElement.rootStructureLevel.id);
			}
		}
		private void SetStructureInspectorEnabled (bool enabled) {
			TreeFactory.GetActiveInstance().forcePreviewModeColored = enabled;
			TreeFactory.GetActiveInstance ().ProcessMaterials (TreeFactory.GetActiveInstance ().previewTree);
		}
		/// <summary>
		/// Event called after this editor lose focus.
		/// </summary>
		override protected void OnDisableSpecific () {
			if (curveEditor != null)
				curveEditor.ClearSelection ();
			else
				return;
			curveEditor.OnDisable ();
			_selectedCurveIds.Clear ();
			_selectedIndexes.Clear ();
			_selectedNodes.Clear ();
			_tunedBranchIds.Clear ();
			if (TreeFactory.GetActiveInstance() != null) {
				TreeFactory.GetActiveInstance().forcePreviewModeColored = false;
				TreeFactory.GetActiveInstance ().ProcessMaterials (TreeFactory.GetActiveInstance ().previewTree);

				TreeFactory.GetActiveInstance ().onBeforeProcessPipelinePreview -= onBeforeProcessPipelinePreview;
				TreeFactory.GetActiveInstance ().onProcessPipeline -= onProcessPipeline;

				// Clear structure level branches
				SetSelectedStructureLevel (-1);
			}
		}
		void GetTunedBranches () {
			_tunedBranchIds.Clear ();
			GetTunedBranchesRecursive (TreeFactory.GetActiveInstance ().previewTree.branches);
		}
		void GetTunedBranchesRecursive (List<BroccoTree.Branch> branches) {
			for (int i = 0; i < branches.Count; i++) {
				if (branches[i].isTuned) _tunedBranchIds.Add (branches[i].id);
				GetTunedBranchesRecursive (branches[i].branches);
			}
		}
		bool onBeforeProcessPipelinePreview (Broccoli.Pipe.Pipeline pipeline, 
			BroccoTree tree, 
			int lodIndex,
			PipelineElement referenceElement = null, 
			bool useCache = false, 
			bool forceNewTree = false)
		{
			if (useCache) { // Save the selected nodes and their curves.
				_nodeToCurve.Clear ();
				foreach (System.Guid guid in curveEditor.nodeToCurve.Keys) {
					_nodeToCurve.Add (guid, curveEditor.nodeToCurve[guid]);	
				}
			} else {
				_nodeToCurve.Clear ();
			}
			return true;
		}
		
		bool onProcessPipeline (Broccoli.Pipe.Pipeline pipeline, 
			BroccoTree tree, 
			int lodIndex,
			PipelineElement referenceElement = null, 
			bool useCache = false, 
			bool forceNewTree = false)
		{
			if (useCache) {
				_selectedNodes.Clear ();
				_selectedIndexes.Clear ();
				_selectedCurveIds.Clear ();
				//MatchSelectedBranches (tree.branches);
				curveEditor.AddNodesToSelection (_selectedNodes, _selectedIndexes, _selectedCurveIds);
				//OnNodeSelectionChanged (_selectedNodes, _selectedIndexes, _selectedCurveIds);
			}
			GetTunedBranches ();
			return true;
		}
		/// <summary>
		/// Persists selection of nodes and curves while processing the pipeline.
		/// </summary>
		/// <param name="branches">Branches to inspect for selected nodes and curves.</param>
		void MatchSelectedBranches (List<BroccoTree.Branch> branches) {
			for (int i = 0; i < branches.Count; i++) {
				for (int j = 0; j < branches[i].curve.nodes.Count; j++) {
					if (_nodeToCurve.ContainsKey(branches[i].curve.nodes[j].guid)) {
						_selectedNodes.Add (branches[i].curve.nodes[j]);
						_selectedIndexes.Add (j);
						_selectedCurveIds.Add (branches[i].guid);
					}
				}
				MatchSelectedBranches (branches[i].branches);
			}
		}
		/// <summary>
		/// Reinits the canvas.
		/// </summary>
		private void ReinitCanvas () {
			structureCanvas = StructureCanvas.GetInstance ();
			structureCanvas.Clear ();
			structureCanvasCache = new NodeEditorUserCache (structureCanvas);
			structureCanvasCache.SetupCacheEvents();
            NodeEditor.BeginEditingCanvas (structureCanvas);
			structureCanvas.LoadStructureGenerator (structureGeneratorElement);
            NodeEditor.EndEditingCanvas ();
			reinitCanvas = false;
		}
		private void ReinitGraph () {
			if (structureGraph == null) return;
			LoadStructureGraph ();
			reinitCanvas = false;
		}
		/// <summary>
		/// Raises the scene GUI event.
		/// </summary>
		/// <param name="sceneView">Scene view.</param>
		protected override void OnSceneGUI (SceneView sceneView) {
			Handles.color = Color.yellow;
			if (structureGeneratorElement != null && 
				structureGeneratorElement.selectedLevel == null && 
				structureGeneratorElement.rootStructureLevel.radius > 0) {
				Handles.DrawWireArc (structureGeneratorElement.pipeline.origin,
					GlobalSettings.againstGravityDirection,
					Vector3.right,
					360,
					structureGeneratorElement.rootStructureLevel.radius);
			} if (structureGeneratorElement.selectedLevel == null) {
				DrawStructures (structureGeneratorElement.flatStructures, 0,
					TreeFactoryEditorWindow.editorWindow.treeFactory.GetPreviewTreeWorldOffset (),
					TreeFactoryEditorWindow.editorWindow.treeFactory.treeFactoryPreferences.factoryScale);
			} else {
				if (structureGeneratorElement.selectedLevel.isSprout) {
					TreeEditorUtils.DrawTreeSproutsForStructureLevel (
						structureGeneratorElement.selectedLevel.id, 
						TreeFactoryEditorWindow.editorWindow.treeFactory.previewTree,
						TreeFactoryEditorWindow.editorWindow.treeFactory.GetPreviewTreeWorldOffset (),
						TreeFactoryEditorWindow.editorWindow.treeFactory.treeFactoryPreferences.factoryScale);
				} else {
					DrawStructures (structureGeneratorElement.flatStructures,
						structureGeneratorElement.selectedLevel.id,
						TreeFactoryEditorWindow.editorWindow.treeFactory.GetPreviewTreeWorldOffset (),
						TreeFactoryEditorWindow.editorWindow.treeFactory.treeFactoryPreferences.factoryScale);
				}
			}
		}
		/// <summary>
		/// Raises the inspector GUI event.
		/// </summary>
		protected override void OnInspectorGUISpecific () {
			CheckUndoRequest ();

			UpdateSerialized ();

			if (!useGraphView) {
				NodeEditorGUI.BeginUsingDefaultSkin ();
			}
	
			// Log box.
			DrawLogBox ();

			//NodeEditorGUI.BeginUsingDefaultSkin ();
			if (reinitCanvas) {
				if (useGraphView) {
					if (Event.current.type == EventType.Repaint)
						ReinitGraph ();
				} else {
					ReinitCanvas ();
				}
			}

			if (!useGraphView) {
				NodeEditorGUI.EndUsingSkin ();
			}

			if (curveEditor == null) return;

			bool selectedStructureChanged = false;
			System.Guid selectedStructureId = curveEditor.selectedCurveId;

			StructureGenerator.StructureLevel selectedLevel = 
				structureGeneratorElement.selectedLevel;

			// Node canvas.
			DrawNodeCanvas ();
			selectedStructureChanged = DrawNodeCanvasControls ();
			EditorGUILayout.Space ();

			// When a single branch is selected show its information and options to tune it.
			if (selectedStructureId != System.Guid.Empty && curveEditor.hasSingleSelection && 
				structureGeneratorElement.guidToStructure.ContainsKey (selectedStructureId)) {
				// ROOT LEVEL OPTIONS.
				EditorGUILayout.LabelField ("Selected Branch", EditorStyles.boldLabel);

				// Get the selected structure.
				StructureGenerator.Structure selectedStructure = structureGeneratorElement.guidToStructure[selectedStructureId];

				// Display selected structure information.
				EditorGUILayout.HelpBox ("Branch Id: " + selectedStructure.id + (selectedStructure.isTuned?" (Tuned)":"") +
					"\nLength: " + selectedStructure.branch.length, MessageType.None);

				// Tune branch girth scale.
				float girthScale = selectedStructure.branch.girthScale;
				girthScale = EditorGUILayout.Slider ("Girth Scale", girthScale, 0f, 1f);
				if (girthScale != selectedStructure.branch.girthScale) {
					Undo.RecordObject (structureGeneratorElement, "Girth Scale");
					selectedStructure.branch.girthScale = girthScale;
					structureGeneratorComponent.CommitStructure (selectedStructure);
					selectedStructureChanged = true;
					ApplySerialized ();
				}

				// Branch is tuned, show option to unlock it.
				if (selectedStructure.isTuned) {
					EditorGUILayout.LabelField ("You have selected a tuned branch. Unlock it to remove changes.");
					if (GUILayout.Button ("Unlock Branch")) {
						if (EditorUtility.DisplayDialog ("Unlock branch",
						"Unlocking this branch will unlock all of its ascending branches in the hierarchy. The next time the pipeline updates it will lose the changes made to this branch too. Do you want to continue?", "Yes", "No")) {
							Undo.RecordObject (structureGeneratorElement, "Move Nodes");
							structureGeneratorComponent.UnlockStructure (selectedStructureId);
							ApplySerialized ();
							curveEditor.ClearSelection ();
							GetTunedBranches ();
							UpdateVertexToSelection (curveEditor.selectedCurveIds);
							SceneView.RepaintAll ();
						}
						GUIUtility.ExitGUI();
					}
					EditorGUILayout.Space ();
				}
			}

			// Structure level edition.
			bool rootElementChanged = false;
			bool selectedElementChanged = false;


			if (selectedLevel == null) {
				// MAIN LEVEL OPTIONS.
				EditorGUILayout.LabelField ("Main Level Node", EditorStyles.boldLabel);
				EditorGUILayout.Space ();

				bool rootChanged = false;

				if (selectedPanel > 1) selectedPanel = PANEL_STRUCTURE;
				selectedPanel = GUILayout.Toolbar (selectedPanel, aspectsTrunkPanelOptions);

				// STRUCTURE PANEL.
				if (selectedPanel == PANEL_STRUCTURE) {
					// FREQUENCY
					EditorGUI.BeginChangeCheck ();
					IntRangePropertyField (propMinFrequency, propMaxFrequency, 0, 30, rootFrequencyLabel);
					ShowHelpBox (MSG_MAIN_MIN_MAX_FREQUENCY);

					// LENGTH
					if (pipelineElement.preset == PipelineElement.Preset.Tree) {
						FloatRangePropertyField (propMinLength, propMaxLength, minRootTreeLengthValue, maxRootTreeLengthValue, rootLengthLabel);
					} else {
						FloatRangePropertyField (propMinLength, propMaxLength, minRootShrubLengthValue, maxRootShrubLengthValue, rootLengthLabel);
					}
					ShowHelpBox (MSG_MAIN_MIN_MAX_LENGTH);
					if (EditorGUI.EndChangeCheck ()) {
						rootChanged = true;
					}
						
					// RADIUS
					float radius = propRadius.floatValue;
					EditorGUILayout.Slider (propRadius, 0f, 20f, rootRadiusLabel);
					ShowHelpBox (MSG_MAIN_RADIUS);
					if (radius != propRadius.floatValue) {
						rootChanged = true;
					}
				}
				// ADVANCED PANEL.
				else {
					// OVERRIDE NOISE.
					bool overrideNoise = propOverrideNoise.boolValue;
					EditorGUILayout.PropertyField (propOverrideNoise);
					ShowHelpBox (MSG_OVERRIDE_NOISE);
					if (overrideNoise != propOverrideNoise.boolValue) {
						rootChanged = true;
					}
					if (overrideNoise) {
						// NOISE.
						EditorGUI.BeginChangeCheck ();
						EditorGUILayout.Slider (propNoise, 0f, 1f, rootNoiseLabel);
						ShowHelpBox (MSG_NOISE);

						// NOISE SCALE.
						EditorGUILayout.Slider (propNoiseScale, 0f, 1f, rootNoiseScaleLabel);
						ShowHelpBox (MSG_NOISE_SCALE);
						if (EditorGUI.EndChangeCheck ()) {
							rootChanged = true;
						}
					}
				}

				if (rootChanged &&
					propMinFrequency.intValue <= propMaxFrequency.intValue &&
					propMinLength.floatValue <= propMaxLength.floatValue) {
					rootElementChanged = true;
				} else if (propMinLength.floatValue > propMaxLength.floatValue) { // FIX
					propMinLength.floatValue = propMaxLength.floatValue;
					rootElementChanged = true;
				}
			} else {
				// NON ROOT LEVEL OPTIONS.
				int index = structureGeneratorElement.GetStructureLevelIndex (selectedLevel);
				if (index >= 0) {
					SerializedProperty propStructureLevel = propStructureLevels.GetArrayElementAtIndex (index);

					bool levelChanged = false;

					if (selectedLevel.isSprout) {
						EditorGUILayout.LabelField ("Sprout Level " + selectedLevel.level + " Node", EditorStyles.boldLabel);
					} else if (selectedLevel.isRoot) {
						EditorGUILayout.LabelField ("Root Level " + selectedLevel.level + " Node", EditorStyles.boldLabel);
					} else {
						EditorGUILayout.LabelField ("Branch Level " + selectedLevel.level + " Node", EditorStyles.boldLabel);
					}

					// ENABLED
					EditorGUI.BeginChangeCheck ();
					// Enabled.
					SerializedProperty propIsEnabled = propStructureLevel.FindPropertyRelative ("enabled");
					bool isEnabled = propIsEnabled.boolValue;
					EditorGUILayout.PropertyField (propIsEnabled, enableStructureLabel);
					ShowHelpBox (MSG_ENABLED);
					if (isEnabled != propIsEnabled.boolValue) {
						bool isDrawVisible = propIsEnabled.boolValue;
						selectedLevel.enabled = propIsEnabled.boolValue;
						structureGeneratorElement.UpdateDrawVisible();
						if (useGraphView) {
							SetGraphNodeEnable (selectedLevel.id, selectedLevel.enabled);
						}
					}
					if (EditorGUI.EndChangeCheck ()) {
						levelChanged = true;
					}
					EditorGUILayout.Space ();

					if (selectedLevel.isSprout) {
						if (selectedPanel >= PANEL_ADVANCED) selectedPanel = PANEL_STRUCTURE;
						selectedPanel = GUILayout.Toolbar (selectedPanel, aspectsSproutPanelOptions);
					} else {
						selectedPanel = GUILayout.Toolbar (selectedPanel, aspectsFullPanelOptions);
					}
					
					EditorGUILayout.Space ();
					EditorGUI.BeginDisabledGroup (!isEnabled);
					switch (selectedPanel) {
						case PANEL_STRUCTURE: // Structure
							levelChanged = DrawStructurePanel (selectedLevel, propStructureLevel, levelChanged);
							break;
						case PANEL_ALIGNMENT: // Alignment
							levelChanged = DrawAlignmentPanel (selectedLevel, propStructureLevel, levelChanged);
							break;
						case PANEL_RANGE: // Range
							levelChanged = DrawRangePanel (selectedLevel, propStructureLevel, levelChanged);
							break;
						case PANEL_ADVANCED: // Advanced
							levelChanged = DrawAdvancedPanel (selectedLevel, propStructureLevel, levelChanged);
							break;
					}
					EditorGUI.EndDisabledGroup ();

					if (levelChanged) {
						selectedElementChanged = true;
					}
				}
			}
			DrawSeparator ();

			// Seed options.
			DrawSeedOptions ();

			if (rootElementChanged || selectedElementChanged || selectedStructureChanged) {
				ApplySerialized ();
				UpdatePipeline (GlobalSettings.processingDelayMedium, true);
				SetUndoControlCounter ();
			}

			//NodeEditorGUI.EndUsingSkin ();

			// Field descriptors option.
			DrawFieldHelpOptions ();
			// Preset options.
			DrawPresetOptions ();
		}
		bool DrawStructurePanel (StructureGenerator.StructureLevel selectedLevel, SerializedProperty propStructureLevel, bool levelChanged) {
			// SPROUT GROUP
			if (selectedLevel.isSprout) {
				EditorGUI.BeginChangeCheck ();
				if (structureGeneratorElement.pipeline.sproutGroups.Count () > 0) {
					int sproutGroupIndex = EditorGUILayout.Popup (sproutGroupsLabel,
												structureGeneratorElement.pipeline.sproutGroups.GetSproutGroupIndex (selectedLevel.sproutGroupId, true),
												structureGeneratorElement.pipeline.sproutGroups.GetPopupOptions (true));
					ShowHelpBox (MSG_SPROUT_GROUP);
					int selectedSproutGroupId = structureGeneratorElement.pipeline.sproutGroups.GetSproutGroupId (sproutGroupIndex);
					if (selectedLevel.sproutGroupId != selectedSproutGroupId) {
						SproutGroups.SproutGroup sproutGroup = 
							structureGeneratorElement.pipeline.sproutGroups.GetSproutGroup (selectedSproutGroupId);
						if (sproutGroup != null) {
							selectedLevel.sproutGroupId = sproutGroup.id;
							selectedLevel.sproutGroupColor = sproutGroup.GetColor ();
						} else {
							selectedLevel.sproutGroupId = -1;
							selectedLevel.sproutGroupColor = Color.clear;
						}
						if (useGraphView) {
							structureGraph.SetNodeMark (selectedLevel.id, selectedLevel.sproutGroupColor);
						}
					}
				} else {
					EditorGUILayout.HelpBox ("Add at least one Sprout Group to the pipeline to assign it to this sprout node.", MessageType.Warning);
				}
				if (EditorGUI.EndChangeCheck ()) {
					levelChanged = true;
				}
				EditorGUILayout.Space ();
			}

			// FREQUENCY & PROBABILITY
			EditorGUI.BeginChangeCheck ();
			/*
			// Max frequency.
			EditorGUILayout.IntSlider (propStructureLevel.FindPropertyRelative ("maxFrequency"), 0, 30, maxFrequencyLabel);
			int maxFrequency = propStructureLevel.FindPropertyRelative ("maxFrequency").intValue;
			ShowHelpBox (MSG_MAX_FREQUENCY);
			// Min frequency.
			EditorGUILayout.IntSlider (propStructureLevel.FindPropertyRelative ("minFrequency"), 0, 30, minFrequencyLabel);
			int minFrequency = propStructureLevel.FindPropertyRelative ("minFrequency").intValue;
			ShowHelpBox (MSG_MIN_FREQUENCY);
			*/
			// Frequency.
			IntRangePropertyField (propStructureLevel.FindPropertyRelative ("minFrequency"), propStructureLevel.FindPropertyRelative ("maxFrequency"), 0, 30, frequencyLabel);
			ShowHelpBox (MSG_MIN_MAX_FREQUENCY);
			// Probability.
			EditorGUILayout.Slider (propStructureLevel.FindPropertyRelative ("probability"), 0f, 1f, probabilityLabel);
			ShowHelpBox (MSG_PROBABILITY);
			// Shared Probability.
			if (selectedLevel.IsShared ()) {
				// Shared probability.
				EditorGUILayout.Slider (propStructureLevel.FindPropertyRelative ("sharedProbability"), 0f, 1f, sharedProbabilityLabel);
				ShowHelpBox (MSG_SHARED_PROBABILITY);
			}
			EditorGUILayout.Space ();
			if (EditorGUI.EndChangeCheck ()) {
				levelChanged = true;
			}

			// DISTRIBUTION
			EditorGUI.BeginChangeCheck ();
			EditorGUILayout.PropertyField (propStructureLevel.FindPropertyRelative ("distribution"), distributionLabel);
			ShowHelpBox (MSG_DISTRIBUTION_MODE);
			if (selectedLevel.distribution == StructureGenerator.StructureLevel.Distribution.Whorled) {
				EditorGUILayout.IntSlider (propStructureLevel.FindPropertyRelative ("childrenPerNode"), 1, 10, whorledStepLabel);
				ShowHelpBox (MSG_DISTRIBUTION_WHORLED);
			}
			EditorGUI.indentLevel++;
			EditorGUILayout.Slider (propStructureLevel.FindPropertyRelative ("distributionSpacingVariance"), 0f, 1f, distributionSpacingVarianceLabel);
			ShowHelpBox (MSG_DISTRIBUTION_SPACING_VARIANCE);
			EditorGUILayout.Slider (propStructureLevel.FindPropertyRelative ("distributionAngleVariance"), 0f, 1f, distributionAngleVarianceLabel);
			ShowHelpBox (MSG_DISTRIBUTION_ANGLE_VARIANCE);
			EditorGUILayout.PropertyField (propStructureLevel.FindPropertyRelative ("distributionCurve"), distributionCurveLabel);
			ShowHelpBox (MSG_DISTRIBUTION_CURVE);
			EditorGUI.indentLevel--;
			EditorGUILayout.Space ();
			if (EditorGUI.EndChangeCheck ()) {
				levelChanged = true;
			}

			// TWIRL
			EditorGUI.BeginChangeCheck ();
			FloatRangePropertyField (propStructureLevel.FindPropertyRelative ("minTwirl"), 
				propStructureLevel.FindPropertyRelative ("maxTwirl"), -1f, 1f, twirlLabel);
			ShowHelpBox (MSG_TWIRL);
			if (EditorGUI.EndChangeCheck ()) {
				levelChanged = true;
			}
			// TWIRL OFFSET
			EditorGUI.indentLevel++;
			EditorGUI.BeginChangeCheck ();
			EditorGUILayout.PropertyField (propStructureLevel.FindPropertyRelative ("randomTwirlOffsetEnabled"), randomTwirlOffsetLabel);
			ShowHelpBox (MSG_RANDOM_TWIRL_OFFSET_ENABLED);
			if (!propStructureLevel.FindPropertyRelative ("randomTwirlOffsetEnabled").boolValue) {
				EditorGUILayout.Slider (propStructureLevel.FindPropertyRelative ("twirlOffset"), -1f, 1f, twirlOffsetLabel);
				ShowHelpBox (MSG_TWIRL_OFFSET);
			}
			EditorGUI.indentLevel--;
			if (EditorGUI.EndChangeCheck ()) {
				levelChanged = true;
			}

			if (!selectedLevel.isSprout) {
				EditorGUILayout.Space ();
				// LENGTH
				EditorGUI.BeginChangeCheck ();
				if (pipelineElement.preset == PipelineElement.Preset.Tree) {
					FloatRangePropertyField (propStructureLevel.FindPropertyRelative ("minLengthAtTop"), 
						propStructureLevel.FindPropertyRelative ("maxLengthAtTop"), minTreeLengthValue, maxTreeLengthValue, lengthAtTopLabel);
					ShowHelpBox (MSG_LENGTH_AT_TOP);
					FloatRangePropertyField (propStructureLevel.FindPropertyRelative ("minLengthAtBase"), 
						propStructureLevel.FindPropertyRelative ("maxLengthAtBase"), minTreeLengthValue, maxTreeLengthValue, lengthAtBaseLabel);
					ShowHelpBox (MSG_LENGTH_AT_BASE);
				} else {
					FloatRangePropertyField (propStructureLevel.FindPropertyRelative ("minLengthAtTop"), 
						propStructureLevel.FindPropertyRelative ("maxLengthAtTop"), minShrubLengthValue, maxShrubLengthValue, lengthAtTopLabel);
					ShowHelpBox (MSG_LENGTH_AT_TOP);
					FloatRangePropertyField (propStructureLevel.FindPropertyRelative ("minLengthAtBase"), 
						propStructureLevel.FindPropertyRelative ("maxLengthAtBase"), minShrubLengthValue, maxShrubLengthValue, lengthAtBaseLabel);
					ShowHelpBox (MSG_LENGTH_AT_BASE);
				}

				EditorGUILayout.PropertyField (propStructureLevel.FindPropertyRelative ("lengthCurve"), lengthCurveLabel);
				ShowHelpBox (MSG_LENGTH_CURVE);
				EditorGUILayout.Space ();

				// GIRTH SCALE
				SerializedProperty propMinGirthScale = propStructureLevel.FindPropertyRelative ("minGirthScale");
				SerializedProperty propMaxGirthScale = propStructureLevel.FindPropertyRelative ("maxGirthScale");
				float minGirthScale = propMinGirthScale.floatValue;
				float maxGirthScale = propMaxGirthScale.floatValue;
				EditorGUILayout.BeginHorizontal ();
				EditorGUILayout.MinMaxSlider (girthScaleLabel, ref minGirthScale, ref maxGirthScale, 0.01f, 1f);
				EditorGUILayout.LabelField (minGirthScale.ToString("F2") + "-" + maxGirthScale.ToString("F2"), GUILayout.Width (60));
				EditorGUILayout.EndHorizontal ();
				if (minGirthScale != propMinGirthScale.floatValue || maxGirthScale != propMaxGirthScale.floatValue) {
					propMinGirthScale.floatValue = minGirthScale;
					propMaxGirthScale.floatValue = maxGirthScale;
				}
				ShowHelpBox (MSG_GIRTH_SCALE);

				if (EditorGUI.EndChangeCheck ()) {
					levelChanged = true;
				}
			}

			// FROM BRANCH CENTER
			if (selectedLevel.isSprout) {
				EditorGUILayout.Space ();
				EditorGUI.BeginChangeCheck ();
				EditorGUILayout.PropertyField (propStructureLevel.FindPropertyRelative ("fromBranchCenter"), fromBranchCenterLabel);
				ShowHelpBox (MSG_FROM_BRANCH_CENTER);
				if (EditorGUI.EndChangeCheck ()) {
					levelChanged = true;
				}
			}

			return levelChanged;
		}
		bool DrawAlignmentPanel (StructureGenerator.StructureLevel selectedLevel, SerializedProperty propStructureLevel, bool levelChanged) {
			EditorGUI.BeginChangeCheck ();
			// Parallel align at top and at base.
			//EditorGUILayout.Slider (propStructureLevel.FindPropertyRelative ("parallelAlignAtTop"), -1f, 1f);
			FloatRangePropertyField (propStructureLevel.FindPropertyRelative ("minParallelAlignAtTop"), 
				propStructureLevel.FindPropertyRelative ("maxParallelAlignAtTop"), -1f, 1f, "Parallel Align at Top");
			ShowHelpBox (MSG_PARALLEL_ALIGN_AT_TOP);
			//EditorGUILayout.Slider (propStructureLevel.FindPropertyRelative ("parallelAlignAtBase"), -1f, 1f);
			FloatRangePropertyField (propStructureLevel.FindPropertyRelative ("minParallelAlignAtBase"), 
				propStructureLevel.FindPropertyRelative ("maxParallelAlignAtBase"), -1f, 1f, "Parallel Align at Base");
			ShowHelpBox (MSG_PARALLEL_ALIGN_AT_BASE);
			EditorGUILayout.PropertyField (propStructureLevel.FindPropertyRelative ("parallelAlignCurve"));
			ShowHelpBox (MSG_PARALLEL_ALIGN_CURVE);
			EditorGUILayout.Space ();
			// Gravity align at top and at base.
			//EditorGUILayout.Slider (propStructureLevel.FindPropertyRelative ("gravityAlignAtTop"), -1f, 1f);
			FloatRangePropertyField (propStructureLevel.FindPropertyRelative ("minGravityAlignAtTop"), 
				propStructureLevel.FindPropertyRelative ("maxGravityAlignAtTop"), -1f, 1f, "Gravity Align at Top");
			ShowHelpBox (MSG_GRAVITY_ALIGN_AT_TOP);
			//EditorGUILayout.Slider (propStructureLevel.FindPropertyRelative ("gravityAlignAtBase"), -1f, 1f);
			FloatRangePropertyField (propStructureLevel.FindPropertyRelative ("minGravityAlignAtBase"), 
				propStructureLevel.FindPropertyRelative ("maxGravityAlignAtBase"), -1f, 1f, "Gravity Align at Base");
			ShowHelpBox (MSG_GRAVITY_ALIGN_AT_BASE);
			EditorGUILayout.PropertyField (propStructureLevel.FindPropertyRelative ("gravityAlignCurve"));
			ShowHelpBox (MSG_GRAVITY_ALIGN_CURVE);
			EditorGUILayout.Space ();
			if (!selectedLevel.isSprout) {
				// Horizontal align at top and at base.
				//EditorGUILayout.Slider (propStructureLevel.FindPropertyRelative ("horizontalAlignAtTop"), -1f, 1f);
				FloatRangePropertyField (propStructureLevel.FindPropertyRelative ("minHorizontalAlignAtTop"), 
					propStructureLevel.FindPropertyRelative ("maxHorizontalAlignAtTop"), -1f, 1f, "Horizontal Align at Top");
				ShowHelpBox (MSG_HORIZONTAL_ALIGN_AT_TOP);
				//EditorGUILayout.Slider (propStructureLevel.FindPropertyRelative ("horizontalAlignAtBase"), -1f, 1f);
				FloatRangePropertyField (propStructureLevel.FindPropertyRelative ("minHorizontalAlignAtBase"), 
					propStructureLevel.FindPropertyRelative ("maxHorizontalAlignAtBase"), -1f, 1f, "Horizontal Align at Base");
				ShowHelpBox (MSG_HORIZONTAL_ALIGN_AT_BASE);
				EditorGUILayout.PropertyField (propStructureLevel.FindPropertyRelative ("horizontalAlignCurve"));
				ShowHelpBox (MSG_HORIZONTAL_ALIGN_CURVE);
			} else {
				SerializedProperty propFlipAlign = propStructureLevel.FindPropertyRelative ("flipSproutAlign");
				float flipAlign = propFlipAlign.floatValue;
				flipAlign = EditorGUILayout.Slider ("Flip Align", flipAlign, 0f, 1f);
				if (flipAlign != propFlipAlign.floatValue) {
					propFlipAlign.floatValue = flipAlign;
				}
				SerializedProperty propFlipDirection = propStructureLevel.FindPropertyRelative ("flipSproutDirection");
				EditorGUILayout.PropertyField (propFlipDirection);
			}
			if (EditorGUI.EndChangeCheck ()) {
				levelChanged = true;
			}
			return levelChanged;
		}
		bool DrawRangePanel (StructureGenerator.StructureLevel selectedLevel, SerializedProperty propStructureLevel, bool levelChanged) {
			EditorGUI.BeginChangeCheck ();
			EditorGUILayout.PropertyField (propStructureLevel.FindPropertyRelative ("actionRangeEnabled"));
			ShowHelpBox (MSG_RANGE_ENABLED);
			if (selectedLevel.actionRangeEnabled) {
				FloatRangePropertyField (propStructureLevel.FindPropertyRelative ("minRange"), 
					propStructureLevel.FindPropertyRelative ("maxRange"), 0f, 1f, "Range");
				ShowHelpBox (MSG_RANGE);
				FloatRangePropertyField (propStructureLevel.FindPropertyRelative ("minMaskRange"), 
					propStructureLevel.FindPropertyRelative ("maxMaskRange"), 0f, 1f, "Mask");
				ShowHelpBox (MSG_MASK_RANGE);
			}
			
			if (!selectedLevel.isSprout && GlobalSettings.experimentalBranchBreak) {
				EditorGUILayout.Space ();
				EditorGUILayout.PropertyField (propStructureLevel.FindPropertyRelative ("applyBranchBreak"));
				ShowHelpBox (MSG_APPLY_BREAK);
				if (selectedLevel.applyBranchBreak) {
					EditorGUILayout.CurveField (propStructureLevel.FindPropertyRelative ("breakBranchProbability"), Color.green, scaleCurveRange);
					ShowHelpBox (MSG_BREAK_PROBABILITY);
					FloatRangePropertyField (propStructureLevel.FindPropertyRelative ("minBreakRange"), 
						propStructureLevel.FindPropertyRelative ("maxBreakRange"), 0f, 1f, "BreakRange");
					ShowHelpBox (MSG_BREAK_RANGE);
				}
			}
			if (EditorGUI.EndChangeCheck ()) {
				levelChanged = true;
			}
			return levelChanged;
		}
		bool DrawAdvancedPanel (StructureGenerator.StructureLevel selectedLevel, SerializedProperty propStructureLevel, bool levelChanged) {
			EditorGUI.BeginChangeCheck ();
			// OVERRIDE NOISE.
			SerializedProperty _propOverrideNoise = propStructureLevel.FindPropertyRelative ("overrideNoise");
			bool overrideNoise = EditorGUILayout.Toggle ("Override Noise", _propOverrideNoise.boolValue);
			ShowHelpBox (MSG_OVERRIDE_NOISE);
			if (overrideNoise != _propOverrideNoise.boolValue) {
				_propOverrideNoise.boolValue = overrideNoise;
				levelChanged = true;
			}
			if (overrideNoise) {
				// NOISE.
				EditorGUI.BeginChangeCheck ();
				EditorGUILayout.Slider (propStructureLevel.FindPropertyRelative ("noise"), 0f, 1f, "Noise");
				ShowHelpBox (MSG_NOISE);

				// NOISE SCALE.
				EditorGUILayout.Slider (propStructureLevel.FindPropertyRelative ("noiseScale"), 0f, 1f, "Noise Scale");
				ShowHelpBox (MSG_NOISE_SCALE);
				if (EditorGUI.EndChangeCheck ()) {
					levelChanged = true;
				}
			}
			return levelChanged;
		}
		protected override void OnUndo() {
			structureGeneratorElement.DeserializeStructures ();
			structureGeneratorElement.BuildStructureLevelTree ();
			structureGeneratorElement.RaiseChangeEvent ();
			reinitCanvas = true;
			UpdatePipeline (GlobalSettings.processingDelayMedium, true);
		}
		#endregion

		#region Structure Graph Events
		private bool listenGraphEvents = true;
		private void LoadStructureGraph () {
			listenGraphEvents = false;
			// Clear existing elements.
			structureGraph.ClearElements ();

			// Create trunk node.
			Vector2 rootNodePosition = structureGeneratorElement.rootStructureLevel.nodePosition;
			structureGraph.AddNode (StructureNode.NodeType.Trunk,
				structureGeneratorElement.rootStructureLevel.id, rootNodePosition, true);

			// Traverse structures and create nodes.
			StructureNode.NodeType nodeType;
			StructureGenerator.StructureLevel structureLevel;
			for (int i = 0; i < structureGeneratorElement.flatStructureLevels.Count; i++) {
				structureLevel = structureGeneratorElement.flatStructureLevels [i];
				nodeType = GetStructureLevelNodeType (structureLevel);
				structureGraph.AddNode (nodeType, structureLevel.id, 
					structureLevel.nodePosition, structureLevel.enabled);
				if (nodeType == StructureNode.NodeType.Sprout) {
					structureGraph.SetNodeMark (structureLevel.id, structureLevel.sproutGroupColor);
				}
			}

			// Create connections.
			structureLevel = structureGeneratorElement.rootStructureLevel;
			for (int j = 0; j < structureLevel.structureLevels.Count; j++) {
				structureGraph.AddConnection (structureLevel.id, structureLevel.structureLevels [j].id);
			}
			for (int i = 0; i < structureGeneratorElement.flatStructureLevels.Count; i++) {
				structureLevel = structureGeneratorElement.flatStructureLevels [i];
				for (int j = 0; j < structureLevel.structureLevels.Count; j++) {
					structureGraph.AddConnection (structureLevel.id, structureLevel.structureLevels [j].id);
				}
			}

			listenGraphEvents = true;
		}
		private StructureNode.NodeType GetStructureLevelNodeType (StructureGenerator.StructureLevel structureLevel) {
			if (structureLevel.isSprout) {
				return StructureNode.NodeType.Sprout;
			} else if (structureLevel.isRoot) {
				return StructureNode.NodeType.Root;
			}
			return StructureNode.NodeType.Branch;
		}
		private void SetGraphNodeEnable (int nodeId, bool enabled) {
			listenGraphEvents = false;
			structureGraph.SetNodeEnabled (nodeId, enabled);
			listenGraphEvents = true;
		}
		private void BindStructureGraphEvents () {
			structureGraph.onZoomDone -= OnGraphZoomDone;
			structureGraph.onZoomDone += OnGraphZoomDone;
			structureGraph.onPanDone -= OnGraphPanDone;
			structureGraph.onPanDone += OnGraphPanDone;
			structureGraph.onSelectNode -= OnGraphSelectNode;
			structureGraph.onSelectNode += OnGraphSelectNode;
			structureGraph.onDeselectNode -= OnGraphDeselectNode;
			structureGraph.onDeselectNode += OnGraphDeselectNode;
			structureGraph.onBeforeEnableNode -= OnGraphBeforeEnableNode;
			structureGraph.onBeforeEnableNode += OnGraphBeforeEnableNode;
			structureGraph.onEnableNode -= OnGraphEnableNode;
			structureGraph.onEnableNode += OnGraphEnableNode;
			structureGraph.onMoveNodes -= OnGraphMoveNodes;
			structureGraph.onMoveNodes += OnGraphMoveNodes;
			structureGraph.onBeforeAddNode -= OnGraphBeforeAddNode;
			structureGraph.onBeforeAddNode += OnGraphBeforeAddNode;
			structureGraph.onAddNode -= OnGraphAddNode;
			structureGraph.onAddNode += OnGraphAddNode;
			structureGraph.onBeforeRemoveNodes -= OnGraphBeforeRemoveNodes;
			structureGraph.onBeforeRemoveNodes += OnGraphBeforeRemoveNodes;
			structureGraph.onRemoveNodes -= OnGraphRemoveNodes;
			structureGraph.onRemoveNodes += OnGraphRemoveNodes;
			structureGraph.onBeforeAddConnection -= OnGraphBeforeAddConnection;
			structureGraph.onBeforeAddConnection += OnGraphBeforeAddConnection;
			structureGraph.onAddConnection -= OnGraphAddConnection;
			structureGraph.onAddConnection += OnGraphAddConnection;
			structureGraph.onBeforeRemoveConnections -= OnGraphBeforeRemoveConnections;
			structureGraph.onBeforeRemoveConnections += OnGraphBeforeRemoveConnections;
			structureGraph.onRemoveConnections -= OnGraphRemoveConnections;
			structureGraph.onRemoveConnections += OnGraphRemoveConnections;
		}
		void OnGraphZoomDone (float currentZoom, float previousZoom) {}
		void OnGraphPanDone (Vector2 currentOffset, Vector2 previousOffset) {
			if (listenGraphEvents) {
				structureGeneratorElement.canvasOffset = currentOffset;
				ApplySerialized ();
			}
		}
		void OnGraphBeforeEnableNode (StructureNode node, bool enable) {
			if (listenGraphEvents) {
				Undo.RecordObject (structureGeneratorElement, "Enable/Disable structure.");
			}
		}
		void OnGraphEnableNode (StructureNode node, bool enable) {
			if (listenGraphEvents) {
				if (structureGeneratorElement.idToStructureLevels.ContainsKey (node.id)) {
					structureGeneratorElement.idToStructureLevels [node.id].enabled = enable;
				}
				ApplySerialized ();
				UpdatePipeline (GlobalSettings.processingDelayMedium, true);
				SetUndoControlCounter ();
			}
		}
		void OnGraphSelectNode (StructureNode node) {
			if (listenGraphEvents) {
				curveEditor.ClearSelection ();
				if (structureGeneratorElement.idToStructureLevels.ContainsKey (node.id)) {
					structureGeneratorElement.selectedLevel = structureGeneratorElement.idToStructureLevels [node.id];
					if (node.id == structureGeneratorElement.rootStructureLevel.id) {
						structureGeneratorElement.selectedLevel = null;
						SetSelectedStructureLevel (structureGeneratorElement.rootStructureLevel.id);
					} else {
						structureGeneratorElement.selectedLevel = structureGeneratorElement.idToStructureLevels [node.id];
						SetSelectedStructureLevel (node.id);
					}
				} else {
					structureGeneratorElement.selectedLevel = null;
					SetSelectedStructureLevel (structureGeneratorElement.rootStructureLevel.id);
				}
			}
		}
		void OnGraphDeselectNode (StructureNode node) {
			if (listenGraphEvents) {
				curveEditor.ClearSelection ();
			}
		}
		void OnGraphMoveNodes (List<StructureNode> nodes, Vector2 delta) {
			if (listenGraphEvents) {
				for (int i = 0; i < nodes.Count; i++) {
					if (structureGeneratorElement.idToStructureLevels.ContainsKey (nodes [i].id)) {
						structureGeneratorElement.idToStructureLevels [nodes [i].id].nodePosition = nodes [i].GetPosition ().position;
					}
				}
				ApplySerialized ();
			}
		}
		void OnGraphBeforeAddNode (StructureNode node, Vector2 nodePosition) {
			if (listenGraphEvents) {
				Undo.RecordObject (structureGeneratorElement, "Adding structure.");
			}
			
		}
		void OnGraphAddNode (StructureNode node, Vector2 nodePosition) {
			if (listenGraphEvents) {
				bool isSprout = false;
				bool isRoot = false;
				if (node.nodeType == StructureNode.NodeType.Sprout) {
					isSprout = true;
				} else if (node.nodeType == StructureNode.NodeType.Root) {
					isRoot = true;
				}
				StructureGenerator.StructureLevel newLevel = 
					structureGeneratorElement.AddStructureLevel (node.id, isSprout, isRoot);
				newLevel.nodePosition = nodePosition;
				structureGeneratorElement.selectedLevel = newLevel;
				curveEditor.ClearSelection ();
				ApplySerialized ();
				SetUndoControlCounter ();
			}
		}
		void OnGraphBeforeRemoveNodes (List<StructureNode> nodesToRemove) {
			if (listenGraphEvents) {
				Undo.RecordObject (structureGeneratorElement, "Adding structure.");
			}
		}
		void OnGraphRemoveNodes (List<StructureNode> nodesRemoved) {
			if (listenGraphEvents) {
				List<int> ids = new List<int> ();
				for (int i = 0; i < nodesRemoved.Count; i++) {
					ids.Add (nodesRemoved [i].id);
				}
				structureGeneratorElement.RemoveStructureLevels (ids);
				structureGeneratorElement.selectedLevel = null;
				curveEditor.ClearSelection ();
				ApplySerialized ();
				UpdatePipeline (GlobalSettings.processingDelayMedium, true);
				SetUndoControlCounter ();
			}
		}
		void OnGraphBeforeAddConnection (StructureNode parentNode, StructureNode childNode) {
			if (listenGraphEvents) {
				Undo.RecordObject (structureGeneratorElement, "Adding structure connection.");
			}
		}
		void OnGraphAddConnection (StructureNode parentNode, StructureNode childNode) {
			if (listenGraphEvents) {
				structureGeneratorElement.AddConnection (parentNode.id, childNode.id);
				ApplySerialized ();
				UpdatePipeline (GlobalSettings.processingDelayMedium, true);
				SetUndoControlCounter ();
			}
		}
		void OnGraphBeforeRemoveConnections (List<UnityEditor.Experimental.GraphView.Edge> edges) {
			if (listenGraphEvents) {
				Undo.RecordObject (structureGeneratorElement, "Removing structure connections.");
			}
		}
		void OnGraphRemoveConnections (List<UnityEditor.Experimental.GraphView.Edge> edges) {
			if (listenGraphEvents) {
				List<int> childIds = new List<int> ();
				for (int i = 0; i < edges.Count; i++) {
					StructureGraphView.StructureEdge edgeData = edges [i].userData as StructureGraphView.StructureEdge;
					if (edgeData != null && edgeData.childNode != null) {
						childIds.Add (edgeData.childNode.id);
					}
				}
				structureGeneratorElement.RemoveConnections (childIds);
				ApplySerialized ();
				UpdatePipeline (GlobalSettings.processingDelayMedium, true);
				SetUndoControlCounter ();
			}
		}
		void SetStructureGraphGUI (VisualElement guiContainer) {
			guiContainer.Clear ();
			contentsButton = new Button(() => {
				structureGeneratorElement.inspectStructureEnabled = !structureGeneratorElement.inspectStructureEnabled;
				SetStructureInspectorEnabled (structureGeneratorElement.inspectStructureEnabled);
				SetStructureInspectionButtonStyle (contentsButton);
            });
			SetStructureInspectionButtonStyle (contentsButton);
			contentsButton.AddToClassList ("gui-button");
            guiContainer.Add(contentsButton);

		}
		void SetStructureInspectionButtonStyle (Button button) {
			if (structureGeneratorElement.inspectStructureEnabled) {
				button.style.backgroundImage = Background.FromTexture2D (GUITextureManager.inspectMeshOnTexture);
				button.tooltip = "Structure Inspection View is On. Click to turn it off.";
			} else {
				button.style.backgroundImage = Background.FromTexture2D (GUITextureManager.inspectMeshOffTexture);
				button.tooltip = "Structure Inspection View is Off. Click to turn it on.";
			}
		}
		#endregion

		#region Draw Functions
		/// <summary>
		/// Draws the node canvas.
		/// </summary>
		private void DrawNodeCanvas () {
			GUILayout.Box ("", GUIStyle.none, 
				GUILayout.Width (Screen.width * 0.9f), 
				GUILayout.Height (Screen.width * 0.9f));
			Rect canvasRect = GUILayoutUtility.GetLastRect ();

			if (useGraphView) {
				structureGraph.style.marginTop = canvasRect.y;
				structureGraph.style.height = canvasRect.height;
				structureGraph.SetContentViewOffset (new Vector2(canvasRect.width / 2f, canvasRect.height / 2f));
			} else {
				if (structureCanvasCache.editorState != null) {
					structureCanvasCache.editorState.canvasRect = canvasRect;
				}

				NodeEditor.checkInit (true);
				NodeEditorGUI.StartNodeGUI ("StructureGenerator", false);
				NodeEditor.DrawCanvas (structureCanvasCache.nodeCanvas, structureCanvasCache.editorState);
			}

			if (structureGeneratorElement.inspectStructureEnabled) {
				if (GUI.Button (new Rect (canvasRect.x, canvasRect.y, 32, 32),
					new GUIContent("", GUITextureManager.inspectMeshOnTexture,
						"Structure Inspection View is On. Click to turn it off."))) {
							structureGeneratorElement.inspectStructureEnabled = false;
							SetStructureInspectorEnabled (false);
						}
			} else {
				if (GUI.Button (new Rect (canvasRect.x, canvasRect.y, 32, 32),
					new GUIContent("", GUITextureManager.inspectMeshOffTexture,
						"Structure Inspection View is Off. Click to turn it on."))) {
							structureGeneratorElement.inspectStructureEnabled = true;
							SetStructureInspectorEnabled (true);
						}
			}

			if (!useGraphView) {
				NodeEditorGUI.EndNodeGUI();
				if (structureCanvas.isDirty) {
					structureCanvas.isDirty = false;
					EditorUtility.SetDirty (structureGeneratorElement);
				}
			}
		}
		/// <summary>
		/// Draws the node canvas controls.
		/// </summary>
		/// <returns>True if a change has been made to the structure.</returns>
		private bool DrawNodeCanvasControls () {
			bool changed = false;

			// ADD STRUCTURE GENERATORS.
			// Structure canvas edit options.
			if (!useGraphView) {
				GUILayout.BeginHorizontal ();
				bool mainStructureSelected = structureGeneratorElement.selectedLevel == null;
				bool sproutStructureSelected = structureGeneratorElement.selectedLevel != null && 
					structureGeneratorElement.selectedLevel.isSprout == true;
				bool rootStructureSelected = structureGeneratorElement.selectedLevel != null && 
					structureGeneratorElement.selectedLevel.isRoot == true;

				// Branch Add Button.
				EditorGUI.BeginDisabledGroup (sproutStructureSelected || rootStructureSelected);
				if (GUILayout.Button (new GUIContent ("+ Branch Level", "Adds a child branch level to the selected structure level."))) {
					StructureGenerator.StructureLevel newLevel = 
						structureGeneratorElement.AddStructureLevel (
							structureGeneratorElement.selectedLevel);
					structureGeneratorElement.selectedLevel = newLevel;
					curveEditor.ClearSelection ();
					reinitCanvas = true;
					changed = true;
				}
				EditorGUI.EndDisabledGroup ();

				// Sprout Add Button
				EditorGUI.BeginDisabledGroup (sproutStructureSelected || rootStructureSelected);
				if (GUILayout.Button (new GUIContent ("+ Sprout Level", "Adds a child sprout level to the selected structure level."))) {
					StructureGenerator.StructureLevel newLevel = 
						structureGeneratorElement.AddStructureLevel (
							structureGeneratorElement.selectedLevel, true);
					structureGeneratorElement.selectedLevel = newLevel;
					curveEditor.ClearSelection ();
					reinitCanvas = true;
					changed = true;
				}
				EditorGUI.EndDisabledGroup ();

				// Root Add Button
				EditorGUI.BeginDisabledGroup (!rootStructureSelected && !mainStructureSelected);
				if (GUILayout.Button (new GUIContent ("+ Root Level", "Add a child root level to the selected structure level."))) {
					StructureGenerator.StructureLevel newLevel = 
						structureGeneratorElement.AddStructureLevel (
							structureGeneratorElement.selectedLevel, false, true);
					structureGeneratorElement.selectedLevel = newLevel;
					curveEditor.ClearSelection ();
					reinitCanvas = true;
					changed = true;
				}
				EditorGUI.EndDisabledGroup ();
				GUILayout.EndHorizontal ();

				// ADD STRUCTURE GENERATORS.
				GUILayout.BeginHorizontal ();
				// Delete level.
				EditorGUI.BeginDisabledGroup (mainStructureSelected);
				if (GUILayout.Button (new GUIContent ("- Remove Level", "Removes the selected structure level."))) {
					StructureGenerator.StructureLevel selectedLevel =
						structureGeneratorElement.selectedLevel;
					curveEditor.ClearSelection ();
					if (selectedLevel != null) {
						if (EditorUtility.DisplayDialog ("Delete Structure Level",
							"Delete this level and its children?", "Yes", "No")) {
							structureGeneratorElement.RemoveStructureLevel (selectedLevel);
							structureGeneratorElement.selectedLevel = null;
							curveEditor.ClearSelection ();
							reinitCanvas = true;
							changed = true;
						}
						GUIUtility.ExitGUI();
					}
				}
				EditorGUI.EndDisabledGroup ();
				GUILayout.EndHorizontal ();
			}

			// SHARED STRUCTURES OPTIONS.
			// Structure canvas shared nodes options.
			/*
			GUILayout.BeginHorizontal ();
			// Add new shared branch level.
			bool addSharedBranchLevelDisabled = 
				structureGeneratorElement.selectedLevel != null &&
				structureGeneratorElement.selectedLevel.isSprout == true;
			EditorGUI.BeginDisabledGroup (addSharedBranchLevelDisabled || rootStructureSelected);
			if (GUILayout.Button (new GUIContent ("+ Shared Branch Level", "Add shared branch level."))) {
				StructureGenerator.StructureLevel newLevel = 
					structureGeneratorElement.AddSharedStructureLevel (
						structureGeneratorElement.selectedLevel);
				structureGeneratorElement.selectedLevel = newLevel;
				curveEditor.ClearSelection ();
				reinitCanvas = true;
				changed = true;
			}
			EditorGUI.EndDisabledGroup ();

			// Add new shared sprout level.
			bool addSharedSproutLevelDisabled = false;
			EditorGUI.BeginDisabledGroup (addSharedSproutLevelDisabled || rootStructureSelected);
			if (GUILayout.Button (new GUIContent ("+ Shared Sprout Level", "Add shared sprout level."))) {
				StructureGenerator.StructureLevel newLevel = 
					structureGeneratorElement.AddSharedStructureLevel (
						structureGeneratorElement.selectedLevel, true);
				structureGeneratorElement.selectedLevel = newLevel;
				curveEditor.ClearSelection ();
				reinitCanvas = true;
				changed = true;
			}
			EditorGUI.EndDisabledGroup ();
			GUILayout.EndHorizontal ();
			*/

			return changed;
		}
		#endregion
		
		#region Drawing
		void DrawStructures (List<StructureGenerator.Structure> structures, int structureLevelId, Vector3 offset, float scale = 1) {
			curveEditor.scale = scale;
			for (int i = 0; i < structures.Count; i++) {
				if (structures[i].generatorId == structureLevelId) {
					_editStructure = structures[i];
					_editStructureId = _editStructure.branch.guid;
					curveEditor.curveId = _editStructureId;
					_editStructureLevelId = structureLevelId;
					curveEditor.scale = scale;
					bool isSelected = _selectedCurveIds.Contains (_editStructureId);
					curveEditor.showFirstHandleAlways = false;
					curveEditor.showSecondHandleAlways = false;
					if (structures[i].branch.isFollowUp) {
						curveEditor.showFirstHandleAlways = true;
					}
					if (structures[i].branch.followUp != null) {
						curveEditor.showSecondHandleAlways = true;
					}
					// Draw a unique curve if edit mode is add.
					if (curveEditor.editMode == BezierCurveEditor.EditMode.Add) {
						if (_singleSelectedCurveId == curveEditor.curveId) {
							curveEditor.OnSceneGUI (structures[i].branch.curve, offset + (structures[i].branch.originOffset * scale), isSelected);
							if (_singleSelectedCurveProcessRequired) {
								//curveEditor.SetAddNodeCandidates ();
								_singleSelectedCurveProcessRequired = false;
							}
						}
					} else {
						curveEditor.OnSceneGUI (structures[i].branch.curve, offset + (structures[i].branch.originOffset * scale), isSelected);
					}
					if (structures[i].branch.isFollowUp) {
						_editStructure = structures[i].parentStructure;
						_editStructureId = _editStructure.branch.guid;
						curveEditor.curveId = _editStructureId;
						curveEditor.OnSceneGUIDrawSingleNode (
							structures[i].branch.parent.curve,
							structures[i].branch.parent.curve.nodes.Count - 1, 
							offset + (structures[i].branch.parent.origin * scale),
							isSelected);
					}
				}
			}
			_editStructureId = System.Guid.Empty;
			_editStructure = null;
		}
		void DrawStructureLevel (StructureGenerator.StructureLevel level, Vector3 offset, float scale = 1) {
			curveEditor.scale = scale;
			for (int i = 0; i < level.generatedBranches.Count; i++) {
				curveEditor.OnSceneGUI (level.generatedBranches[i].curve, offset + (level.generatedBranches[i].origin * scale));
			}
		}
		void UpdateVertexToSelection (List<System.Guid> selectedBranchIds) {
			/// UV5 information of the mesh.
			/// x: id of the branch.
			/// y: if of the branch skin.
			/// z: id of the struct.
			/// w: tuned.
			List<Vector4> uv5s = new List<Vector4> ();
			MeshFilter meshFilter = TreeFactory.GetActiveInstance().previewTree.obj.GetComponent<MeshFilter>();
			if (meshFilter != null) {
				meshFilter.sharedMesh.GetUVs (4, uv5s);
				for (int i = 0; i < uv5s.Count; i++) {
					int isTuned = 0;
					if (_tunedBranchIds.Contains ((int)uv5s[i].x)) {
						isTuned = 1;
					}
					uv5s[i] = new Vector4 (uv5s[i].x, uv5s[i].y, uv5s[i].z, isTuned);
				}
				meshFilter.sharedMesh.SetUVs (4, uv5s);
			}
			Repaint ();
		}
		#endregion

		#region Canvas Editor
		void OnSelectStructureLevel (Broccoli.NodeEditorFramework.Node node) {
			bool shouldExitGUI = curveEditor.hasSingleSelection && curveEditor.showTools;
			curveEditor.ClearSelection ();
			if (shouldExitGUI) GUIUtility.ExitGUI ();
			if (structureGeneratorElement.selectedLevel == null) {
				SetSelectedStructureLevel (structureGeneratorElement.rootStructureLevel.id);
			} else {
				SetSelectedStructureLevel (structureGeneratorElement.selectedLevel.id);
			}
		}
		void OnDeselectStructureLevel () {
			curveEditor.ClearSelection ();
		}
		void SetSelectedStructureLevel (int selectedLevelId) {
			MeshRenderer meshRenderer = TreeFactory.GetActiveInstance ().previewTree.obj.GetComponent<MeshRenderer> ();
			MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock ();
			if (meshRenderer != null) {
				meshRenderer.GetPropertyBlock (propertyBlock);
				propertyBlock.SetFloat ("_SelectedLevel", selectedLevelId);
				meshRenderer.SetPropertyBlock (propertyBlock);
			}
		}
		#endregion

		#region Curve Editor
		void DrawBranchForStructureLevel (int structureLevelId, BroccoTree.Branch branch, 
			Vector3 origin, Vector3 offset, float scale = 1f) 
		{
			#if UNITY_EDITOR
			if (branch.helperStructureLevelId == structureLevelId) {
				_editStructureId = branch.guid;
				_editStructureLevelId = structureLevelId;
				curveEditor.scale = scale;
				curveEditor.OnSceneGUI (branch.curve, offset + (branch.origin * scale));
			}
			for (int i = 0; i < branch.branches.Count; i++) {
				Vector3 childBranchOrigin = branch.branches[i].origin;
				DrawBranchForStructureLevel (structureLevelId, branch.branches[i], childBranchOrigin, offset, scale);
			}
			#endif
		}
		/// <summary>
		/// Called when the editor changed mode.
		/// </summary>
		/// <param name="editMode">New edit mode.</param>
		void OnEditModeChanged (BezierCurveEditor.EditMode editMode) {
			if (editMode == BezierCurveEditor.EditMode.Add) {
				_singleSelectedCurveId = curveEditor.selectedCurveId;
				_singleSelectedCurveProcessRequired = true;
			} else {
				_singleSelectedCurveId = System.Guid.Empty;
			}
		}
		/// <summary>
		/// Called when the node selection changes.
		/// </summary>
		/// <param name="nodes">Nodes in the selection.</param>
		/// <param name="indexes">Indexes of the nodes in the selection.</param>
		void OnNodeSelectionChanged (List<BezierNode> nodes, List<int> indexes, List<System.Guid> curveIds) {
			// If more than one node is selected disable bezier tools.
			if (nodes.Count == 1) {
				if (curveIds.Count > 0) curveEditor.focusedCurveId = curveIds [0];
				curveEditor.showTools = true;
			} else {
				curveEditor.focusedCurveId = System.Guid.Empty;
				curveEditor.showTools = false;
				curveEditor.editMode = BezierCurveEditor.EditMode.Selection;
			}
		}
		/// <summary>
		/// Checks the offset used to move the selected nodes.
		/// </summary>
		/// <param name="offset">Offset value.</param>
		/// <returns>The offset to use to move the selected nodes.</returns>
		Vector3 OnCheckMoveNodes (Vector3 offset) {
			return offset;
		}
		/// <summary>
		/// Called right before a list of nodes get moved.
		/// </summary>
		/// <param name="nodes">Nodes to be moved.</param>
		/// <param name="indexes">Index of nodes to be moved.</param>
		void OnBeginMoveNodes (List<BezierNode> nodes, List<int> indexes, List<System.Guid> curveIds) {
			Undo.RecordObject (structureGeneratorElement, "Move Nodes");
		}
		/// <summary>
		/// Called after a list of nodes have been moved.
		/// </summary>
		/// <param name="nodes">Nodes moved.</param>
		/// <param name="indexes">Index of nodes moved.</param>
		void OnMoveNodes (List<BezierNode> nodes, List<int> indexes, List<System.Guid> curveIds) {
			for (int i = 0; i < curveIds.Count; i++) {
				structureGeneratorComponent.CommitBranchCurve (curveIds[i], indexes[i], curveEditor.offsetStep);
			}
			GetTunedBranches ();
			ApplySerialized ();
			UpdatePipeline (GlobalSettings.processingDelayMedium);
		}
		/// <summary>
		/// Called at the end of moving nodes.
		/// </summary>
		/// <param name="nodes">Nodes to be moved.</param>
		/// <param name="indexes">Index of nodes to be moved.</param>
		void OnEndMoveNodes (List<BezierNode> nodes, List<int> indexes, List<System.Guid> curveIds) {
			SetUndoControlCounter (false);
		}
		/// <summary>
		/// Called right before a node is edited (ex. handle style changed).
		/// </summary>
		/// <param name="node">Node to edit.</param>
		/// <param name="index">Index of edited node.</param>
		void OnBeforeEditNode (BezierNode node, int index) {
			Undo.RecordObject (structureGeneratorElement, "Change Node Mode");
		}
		/// <summary>
		/// Called after a node is edited (ex. handle style changed).
		/// </summary>
		/// <param name="node">Node to edit.</param>
		/// <param name="index">Index of edited node.</param>
		void OnEditNode (BezierNode node, int index) {
			structureGeneratorComponent.CommitBranchCurve (curveEditor.selectedCurveId, index, curveEditor.offsetStep, true);
			ApplySerialized ();
			UpdatePipeline (GlobalSettings.processingDelayMedium);
		}
		/// <summary>
		/// Called before a new node gets added.
		/// </summary>
		/// <param name="node">Node to add.</param>
		void OnBeforeAddNode (BezierNode node) {
			Undo.RecordObject (structureGeneratorElement, "Add Node");
		}
		/// <summary>
		/// Called after a new node gets added.
		/// </summary>
		/// <param name="node">Node to add.</param>
		/// <param name="index">Index of the node added.</param>
		/// <param name="relativePosition">Relative position of the new node.</param>
		void OnAddNode (BezierNode node, int index, float relativePosition) {
			node.handleStyle = BezierNode.HandleStyle.Auto;
			SetUndoControlCounter (false);
			GetTunedBranches ();
			ApplySerialized ();
			UpdatePipeline (GlobalSettings.processingDelayMedium);
			curveEditor.editMode = BezierCurveEditor.EditMode.Selection;
			_singleSelectedCurveId = node.curve.guid;
			curveEditor.ClearSelection (true, false);
			curveEditor.AddNodeToSelection (node, index, node.curve.guid);
			GUIUtility.hotControl = 0;
		}
		/// <summary>
		/// Called before removing nodes fron the curve.
		/// </summary>
		/// <param name="nodes">Nodes to remove.</param>
		/// <param name="index">Index of the nodes in the curve.</param>
		/// <param name="curveIds">Ids of the curves to remove nodes from.</param>
		void OnBeforeRemoveNodes (List<BezierNode> nodes, List<int> index, List<System.Guid> curveIds) {
			Undo.RecordObject (structureGeneratorElement, "Remove Nodes");
		}
		/// <summary>
		/// Called before removing nodes fron the curve.
		/// </summary>
		/// <param name="nodes">Nodes to remove.</param>
		/// <param name="index">Index of the nodes in the curve.</param>
		/// <param name="curveIds">Ids of the curves to remove nodes from.</param>
		void OnRemoveNodes (List<BezierNode> nodes, List<int> index, List<System.Guid> curveIds) {
			SetUndoControlCounter (false);
			GetTunedBranches ();
			ApplySerialized ();
			UpdatePipeline (GlobalSettings.processingDelayMedium);
		}
		/// <summary>
		/// Called when a handle begins to move.
		/// </summary>
		/// <param name="node">Node owner of the handle.</param>
		/// <param name="index">Index of the node.</param>
		/// <param name="curveId">Id of the curve the node.</param>
		/// <param name="handle">Number of the handle.</param>
		bool OnBeginMoveHandle (BezierNode node, int index, System.Guid curveId, int handle) {
			Undo.RecordObject (structureGeneratorElement, "Move Handle");
			return true;
		}
		/// <summary>
		/// Called when a handle of a node is moved.
		/// </summary>
		/// <param name="node">Node owner of the handle.</param>
		/// <param name="index">Index of the node.</param>
		/// <param name="curveId">Id of the curve the node.</param>
		/// <param name="handle">Number of the handle.</param>
		bool OnMoveHandle (BezierNode node, int index, System.Guid curveId, int handle) {
			structureGeneratorComponent.CommitBranchCurve (_editStructureId, index, Vector3.zero, true);
			GetTunedBranches ();
			ApplySerialized ();
			UpdatePipeline (GlobalSettings.processingDelayLow);
			return true;
		}
		/// <summary>
		/// Called when a handle of a node is moved.
		/// </summary>
		/// <param name="node">Node owner of the handle.</param>
		/// <param name="index">Index of the node.</param>
		/// <param name="curveId">Id of the curve the node.</param>
		/// <param name="handle">Number of the handle.</param>
		bool OnEndMoveHandle (BezierNode node, int index, System.Guid curveId, int handle) {
			SetUndoControlCounter (false);
			return true;
		}
		/// <summary>
		/// Called to check if a node should draw move controls.
		/// </summary>
		/// <param name="node">Node to check.</param>
		/// <param name="index">Index of the node.</param>
		/// <param name="curveId">Id of the curve the node belongs to.</param>
		/// <returns>True if the node should be drawn.</returns>
		BezierCurveEditor.ControlType OnCheckNodeMoveControls (BezierNode node, int index, System.Guid curveId) {
			if (index == 0 && _editStructure != null) {
				if (!_editStructure.branch.isFollowUp || _editStructure.branch.parent == null || curveEditor.hasMultipleSelection) {
					return BezierCurveEditor.ControlType.DrawOnly;
				} else if (_editStructure.branch.isFollowUp) {
					return BezierCurveEditor.ControlType.None;
				}
			}
			return BezierCurveEditor.ControlType.FreeMove;
		}
		#endregion
	}
}
