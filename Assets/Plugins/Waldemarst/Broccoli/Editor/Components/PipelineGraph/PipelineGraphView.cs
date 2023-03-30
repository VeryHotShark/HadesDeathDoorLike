using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

using Broccoli.Base;
using Broccoli.Pipe;

namespace Broccoli.TreeNodeEditor
{
    public class PipelineGraphView : GraphView {
		#region Pipeline Edge Class
		public class PipelineEdge {
			public PipelineNode srcNode = null;
			public PipelineNode sinkNode = null;
			public int upWeight = 0;
			public int downWeight = 0;
		}
		#endregion

        #region Vars
        private Pipeline pipeline;
		private float currentZoom = 1f;
		private Vector3 currentOffset = Vector3.zero;
		/// <summary>
		/// Last value of undo count registered from a processed pipeline.
		/// </summary>
		public int lastUndoProcessed = 0;
		/// <summary>
		/// The current undo group.
		/// </summary>
		int currentUndoGroup = -1;
		/// <summary>
		/// Marks the graph as dirty.
		/// </summary>
		public bool isDirty = false;
		private bool isPortDragging = false;
		private static string connectorCandidateClassName = "connector-candidate";
		private static string titleElementClassName = "graph-title";
        #endregion

		#region Delegates
		public delegate void OnZoomDelegate (float currentZoom, float previousZoom);
		public delegate void OnOffsetDelegate (Vector2 currentOffset, Vector2 previousOffset);
		public delegate void OnPipelineElementDelegate (PipelineElement pipelineElement);
		public delegate void OnNodeDelegate (PipelineNode node);
		public delegate void OnMoveNodesDelegate (List<PipelineNode> nodes, Vector2 delta);
		public delegate void OnRemoveNodesDelegate (List<PipelineNode> nodes);
		public delegate void OnEdgeDelegate (PipelineNode srcNode, PipelineNode sinkNode);
		public delegate void OnEdgesDelegate (List<Edge> edges);
		public delegate void OnEnableNodeDelegate (PipelineNode node, bool enable);
		public delegate void OnDelegate ();
		public OnZoomDelegate onZoomDone;
		public OnOffsetDelegate onPanDone;
		public OnNodeDelegate onSelectNode;
		public OnNodeDelegate onDeselectNode;
		public OnMoveNodesDelegate onBeforeMoveNodes;
		public OnMoveNodesDelegate onMoveNodes;
		public OnPipelineElementDelegate onBeforeAddNode;
		public OnNodeDelegate onAddNode;
		public OnRemoveNodesDelegate onBeforeRemoveNodes;
		public OnRemoveNodesDelegate onRemoveNodes;
		public OnEdgeDelegate onBeforeAddConnection;
		public OnEdgeDelegate onAddConnection;
		public OnEdgesDelegate onBeforeRemoveConnections;
		public OnEdgesDelegate onRemoveConnections;
		public OnEnableNodeDelegate onBeforeEnableNode;
		public OnEnableNodeDelegate onEnableNode;
		public OnDelegate onRequestUpdatePipeline;
		#endregion

		#region Messages
		private static string MSG_REMOVE_CONNECTIONS_TITLE = "Remove Connections";
		private static string MSG_REMOVE_CONNECTIONS_MESSAGE = "Are you sure you want to remove the selected connections?";
		private static string MSG_REMOVE_CONNECTIONS_OK = "Yes, Remove them";
		private static string MSG_REMOVE_CONNECTIONS_CANCEL = "Cancel";
		private static string MSG_REMOVE_NODES_TITLE = "Remove Elements";
		private static string MSG_REMOVE_NODES_MESSAGE = "Are you sure you want to remove the selected elements?";
		private static string MSG_REMOVE_NODES_OK = "Yes, Remove them";
		private static string MSG_REMOVE_NODES_CANCEL = "Cancel";
		#endregion

        #region GUI Vars
        /// <summary>
		/// Relationship between pipeline element id and canvas nodes
		/// </summary>
		Dictionary<int, PipelineNode> idToNode = new Dictionary<int, PipelineNode> ();
		public VisualTreeAsset nodeXml;
		public Label titleElement;
        public StyleSheet nodeStyle;
		public StyleSheet graphViewStyle;
        #endregion

        #region Events
        #endregion

        #region Init/Destroy
        public void Init (Vector2 offset, float zoom) {
			// Zoom
            this.SetupZoom (0.05f, ContentZoomer.DefaultMaxScale, 0.05f, zoom);
			currentZoom = zoom;
			this.viewTransform.scale = new Vector3 (zoom, zoom, zoom);

			// Offset
			currentOffset = offset;
			viewTransform.position = offset;

			//this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
			this.AddManipulator(new SelectionDragger());
			this.AddManipulator(new ClickSelector());
            this.RegisterCallback<KeyDownEvent>(KeyDown);
	
			this.graphViewChanged = _GraphViewChanged;

            this.viewTransformChanged = _ViewTransformChanged;
            GridBackground gridBackground = new GridBackground() { name = "Grid" };
			this.Add(gridBackground);
			gridBackground.SendToBack();

			bool undoRedoPerformedExists = false;
			System.Delegate[] invocations = Undo.undoRedoPerformed.GetInvocationList ();
			for (int i = 0; i < invocations.Length; i++) {
				if (invocations[i].Method.Name == "OnBroccoliUndoRedoPerformed") {
					undoRedoPerformedExists = true;
				}
			}
			if (!undoRedoPerformedExists) {
				Undo.undoRedoPerformed -= OnBroccoliUndoRedoPerformed;
				Undo.undoRedoPerformed += OnBroccoliUndoRedoPerformed;
			}
			//onRepaint?.Invoke ();

			nodeXml = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(ExtensionManager.extensionPath + "Editor/Resources/GUI/PipelineNodeView.uxml");
			nodeStyle = AssetDatabase.LoadAssetAtPath<StyleSheet>(ExtensionManager.extensionPath + "Editor/Resources/GUI/PipelineNodeViewStyle.uss");
			graphViewStyle = AssetDatabase.LoadAssetAtPath<StyleSheet>(ExtensionManager.extensionPath + "Editor/Resources/GUI/PipelineGraphViewStyle.uss");

			if (graphViewStyle != null) {
				this.styleSheets.Add (graphViewStyle);
			}

			// Add title element.
			titleElement = new Label ();
			titleElement.AddToClassList (titleElementClassName);
			this.Add (titleElement);
        }
		public void SetTitle (string graphTitle) {
			if (titleElement != null) {
				titleElement.text = graphTitle;
			}
		}
		private void OnDestroy () {
			Undo.undoRedoPerformed -= OnBroccoliUndoRedoPerformed;
		}
		private void OnDisable () {
			idToNode.Clear ();
		}
        public bool LoadPipeline (Pipeline pipelineToLoad, Vector2 offset, float zoom = 1f) {
            if (pipelineToLoad != null) {
				// Zoom.
				this.SetupZoom (0.05f, ContentZoomer.DefaultMaxScale, 0.05f, zoom);
				currentZoom = zoom;
				this.viewTransform.scale = new Vector3 (zoom, zoom, zoom);
				// Offset.
				currentOffset = offset;
				viewTransform.position = offset;

                this.pipeline = pipelineToLoad;
                CreatePipelineNodes ();

				lastUndoProcessed = pipeline.undoControl.undoCount;

				//onRepaint?.Invoke ();

                return true;
            }
            return false;
        }
        public void ClearElements () {
            //base.Clear ();
            ClearNodes ();
			ClearEdges ();;
            pipeline = null;
        }
		public override void BuildContextualMenu (ContextualMenuPopulateEvent evt) {
            //base.BuildContextualMenu (evt);
			var position = viewTransform.matrix.inverse.MultiplyPoint(evt.localMousePosition);
			if (evt.target is GraphView) {
				evt.menu.AppendAction("Add Generator/Structure Generator", 
					(e) => { AddNode (PipelineElement.ClassType.StructureGenerator, position); });
				evt.menu.AppendAction("Add Generator/Sprout Generator", 
					(e) => { AddNode (PipelineElement.ClassType.SproutGenerator, position); });
				evt.menu.AppendAction("Add Transformer/Length Transformer", 
					(e) => { AddNode (PipelineElement.ClassType.LengthTransform, position); });
				evt.menu.AppendAction("Add Transformer/Girth Transformer", 
					(e) => { AddNode (PipelineElement.ClassType.GirthTransform, position); });
				evt.menu.AppendAction("Add Transformer/Sparsing Transformer", 
					(e) => { AddNode (PipelineElement.ClassType.SparsingTransform, position); });
				evt.menu.AppendAction("Add Transformer/Branch Bender", 
					(e) => { AddNode (PipelineElement.ClassType.BranchBender, position); });
				evt.menu.AppendAction("Add Mesh Generator/Branch Mesh Generator", 
					(e) => { AddNode (PipelineElement.ClassType.BranchMeshGenerator, position); });
				evt.menu.AppendAction("Add Mesh Generator/Sprout Mesh Generator", 
					(e) => { AddNode (PipelineElement.ClassType.SproutMeshGenerator, position); });
				evt.menu.AppendAction("Add Mesh Generator/Trunk Mesh Generator", 
					(e) => { AddNode (PipelineElement.ClassType.TrunkMeshGenerator, position); });
				evt.menu.AppendAction("Mapper/Branch Mapper", 
					(e) => { AddNode (PipelineElement.ClassType.BranchMapper, position); });
				evt.menu.AppendAction("Mapper/Sprout Mapper", 
					(e) => { AddNode (PipelineElement.ClassType.SproutMapper, position); });
				evt.menu.AppendAction("Function/Wind Effect", 
					(e) => { AddNode (PipelineElement.ClassType.WindEffect, position); });
				evt.menu.AppendAction("Function/Positioner", 
					(e) => { AddNode (PipelineElement.ClassType.Positioner, position); });
				evt.menu.AppendAction("Function/Baker", 
					(e) => { AddNode (PipelineElement.ClassType.Baker, position); });
			} else if (evt.target is Node) {
				PipelineNode targetNode = evt.target as PipelineNode;
				evt.menu.AppendAction("Remove Node", (e) => { RemoveNodes (new List<PipelineNode> () {targetNode}); });
			}
			/*
            if (evt.target is GraphView || evt.target is Node) {
                evt.menu.AppendAction("Convert To Sub-graph", ConvertToSubgraph, ConvertToSubgraphStatus);
                evt.menu.AppendAction("Convert To Inline Node", ConvertToInlineNode, ConvertToInlineNodeStatus);
                evt.menu.AppendAction("Convert To Property", ConvertToProperty, ConvertToPropertyStatus);
                if (selection.OfType<MaterialNodeView>().Count() == 1)
                {
                    evt.menu.AppendSeparator();
                    evt.menu.AppendAction("Open Documentation", SeeDocumentation, SeeDocumentationStatus);
                }
                if (selection.OfType<MaterialNodeView>().Count() == 1 && selection.OfType<MaterialNodeView>().First().node is SubGraphNode)
                {
                    evt.menu.AppendSeparator();
                    evt.menu.AppendAction("Open Sub Graph", OpenSubGraph, ContextualMenu.MenuAction.StatusFlags.Normal);
                }
            } else if (evt.target is BlackboardField) {
                evt.menu.AppendAction("Delete", (e) => DeleteSelectionImplementation("Delete", AskUser.DontAskUser), (e) => canDeleteSelection ? ContextualMenu.MenuAction.StatusFlags.Normal : ContextualMenu.MenuAction.StatusFlags.Disabled);
            } if (evt.target is MaterialGraphView) {
                evt.menu.AppendAction("Collapse Previews", CollapsePreviews, ContextualMenu.MenuAction.StatusFlags.Normal);
                evt.menu.AppendAction("Expand Previews", ExpandPreviews, ContextualMenu.MenuAction.StatusFlags.Normal);
                evt.menu.AppendSeparator();
            }
			*/
        }
		public override List<Port> GetCompatiblePorts (Port startPort, NodeAdapter nodeAdapter) {
			List<Port> _ports = ports.ToList ();
			_ports = ports.ToList().Where(endPort =>
				endPort.direction != startPort.direction &&
				endPort.node != startPort.node &&
				!endPort.connected &&
				endPort.portType == startPort.portType)
				.ToList();
			(startPort.node as PipelineNode).RiseEdgeDragBegin (startPort, _ports);
			return _ports;
		}
        #endregion

        #region Node Ops
        public void ClearNodes () {
            var nodesEnumerator = idToNode.GetEnumerator ();
            while (nodesEnumerator.MoveNext ()) {
                this.RemoveElement (nodesEnumerator.Current.Value);
            }
            idToNode.Clear ();
        }
        private void CreatePipelineNodes () {
            // Remove existing nodes.
            ClearNodes ();
			ClearEdges ();

            // Add nodes.
            if (pipeline != null) {
                List<PipelineElement> pipelineElements = pipeline.GetElements ();
				for (int i = 0; i < pipelineElements.Count; i++) {
                    CreateNode (pipelineElements [i], true);
				}
				// Connect nodes.
				for (int i = 0; i < pipelineElements.Count; i++) {
					if (pipelineElements [i].sinkElementId != -1 &&
						idToNode.ContainsKey (pipelineElements [i].sinkElementId))
					{
						PipelineNode srcNode = idToNode [pipelineElements [i].id];
						PipelineNode sinkNode = idToNode [pipelineElements [i].sinkElementId];
						Edge edge = srcNode.srcPort.ConnectTo (sinkNode.sinkPort);
						SetEdgeUserData (edge, srcNode, sinkNode);
						AddElement (edge);
					}
				}
            }
        }
        public PipelineNode CreateNode (PipelineElement pipelineElement, bool addAfterCreation = false) {
            if (!idToNode.ContainsKey (pipelineElement.id)) {
				PipelineNode pipeNode;
				if (nodeXml == null) {
					pipeNode = new PipelineNode (pipelineElement) { name = pipelineElement.name };
				} else {
					pipeNode = new PipelineNode (pipelineElement, nodeXml) { name = pipelineElement.name };
				}
                idToNode.Add (pipelineElement.id, pipeNode);
				pipeNode.onSelected += OnSelectNodeInternal;
				pipeNode.onUnselected += OnDeselectNodeInternal;
				pipeNode.onEnable += OnEnableNodeInternal;
				pipeNode.onBeginDragPort += OnBeginDragNodePortInternal;
				pipeNode.onEndDragPort += OnEndDragNodePortInternal;
				if (addAfterCreation) {
                	this.AddElement (pipeNode);
				}
                pipeNode.InitializeNode (nodeStyle);
                return pipeNode;
            }
            return null;
        }
		public void AddNode (PipelineElement.ClassType classType, Vector2 nodePosition) {
			PipelineElement pipelineElement = GetPipelineElement (classType);
			if (pipelineElement != null) {
				onBeforeAddNode?.Invoke (pipelineElement);
				pipeline.AddElement (pipelineElement);
				PipelineNode pipelineNode = CreateNode (pipelineElement);
				if (pipelineNode != null) {
					pipelineElement.nodePosition = nodePosition;
					this.AddElement (pipelineNode);
					pipelineNode.SetPosition (new Rect(
						pipelineElement.nodePosition.x, 
						pipelineElement.nodePosition.y, 
						0, 0));
					onAddNode?.Invoke (pipelineNode);
					isDirty = true;
				}
			}
		}
		public bool RemoveNodes (List<PipelineNode> pipelineNodesToRemove, bool overrideConfirm = false) {
			if (pipelineNodesToRemove != null && pipelineNodesToRemove.Count > 0) {
				if (overrideConfirm ||
					EditorUtility.DisplayDialog (MSG_REMOVE_NODES_TITLE, 
					MSG_REMOVE_NODES_MESSAGE, 
					MSG_REMOVE_NODES_OK, 
					MSG_REMOVE_NODES_CANCEL)) 
				{
					onBeforeRemoveNodes?.Invoke (pipelineNodesToRemove);
					for (int i = 0; i < pipelineNodesToRemove.Count; i++) {
						if (idToNode.ContainsKey (pipelineNodesToRemove [i].pipelineElement.id)) {
							idToNode.Remove (pipelineNodesToRemove [i].pipelineElement.id);
						}
						if (pipelineNodesToRemove [i].pipelineElement != null) {
							pipeline.RemoveElement (pipelineNodesToRemove [i].pipelineElement);
						}
						RemoveElement (pipelineNodesToRemove [i]);
					}
					onRemoveNodes?.Invoke (pipelineNodesToRemove);
					isDirty = true;
					return true;
				}
			}
			return false;
		}
		public void ClearEdges () {
			List<Edge> _edges = edges.ToList ();
			for (int i = 0; i < _edges.Count; i++) {
				RemoveElement (_edges [i]);
			}
			_edges.Clear ();
		}
		private PipelineElement GetPipelineElement (PipelineElement.ClassType classType) {
			PipelineElement pipelineElement = null;
			switch (classType) {
				case PipelineElement.ClassType.StructureGenerator: return new StructureGeneratorElement ();
				case PipelineElement.ClassType.SproutGenerator: return new SproutGeneratorElement ();
				case PipelineElement.ClassType.LengthTransform: return new LengthTransformElement ();
				case PipelineElement.ClassType.GirthTransform: return new GirthTransformElement ();
				case PipelineElement.ClassType.SparsingTransform: return new SparsingTransformElement ();
				case PipelineElement.ClassType.BranchBender: return new BranchBenderElement ();
				case PipelineElement.ClassType.BranchMeshGenerator: return new BranchMeshGeneratorElement ();
				case PipelineElement.ClassType.SproutMeshGenerator: return new BranchMeshGeneratorElement ();
				case PipelineElement.ClassType.TrunkMeshGenerator: return new TrunkMeshGeneratorElement ();
				case PipelineElement.ClassType.BranchMapper: return new BranchMapperElement ();
				case PipelineElement.ClassType.SproutMapper: return new SproutMapperElement ();
				case PipelineElement.ClassType.WindEffect: return new WindEffectElement ();
				case PipelineElement.ClassType.Positioner: return new PositionerElement ();
				case PipelineElement.ClassType.Baker: return new BakerElement ();
			}
			return pipelineElement;
		}
		private bool AddConnectionInternal (PipelineNode srcNode, PipelineNode sinkNode) {
			if (srcNode != null && sinkNode != null &&
				srcNode.pipelineElement != null && sinkNode.pipelineElement != null) 
			{
				if (srcNode.pipelineElement.positionWeight < sinkNode.pipelineElement.positionWeight || 
					(!srcNode.pipelineElement.uniqueOnPipeline && 
						srcNode.pipelineElement.positionWeight == sinkNode.pipelineElement.positionWeight)) 
				{
					onBeforeAddConnection?.Invoke (srcNode, sinkNode);

					srcNode.pipelineElement.sinkElementId = sinkNode.pipelineElement.id;
					sinkNode.pipelineElement.srcElementId = srcNode.pipelineElement.id;
					srcNode.pipelineElement.sinkElement = sinkNode.pipelineElement;
					sinkNode.pipelineElement.srcElement = srcNode.pipelineElement;

					pipeline.Validate ();

					onAddConnection?.Invoke (srcNode, sinkNode);

					isDirty = true;
					return true;
				} else {
					//treeFactory.AddLogWarn ("Invalid node connection.");
				}
			}
			return false;
		}
		private void ShowConnectorCandidates (List<Port> candidates) {
			for (int i = 0; i < candidates.Count; i++) {
				if (candidates [i].parent != null) {
					VisualElement connectorStatus = candidates [i].parent.Q<VisualElement>("connector-status");
					if (connectorStatus != null) {
						connectorStatus.AddToClassList (connectorCandidateClassName);
					}
				}
			}
		}
		private void HideConnectorCandidates () {
			List<Port> _ports = ports.ToList ();
			for (int i = 0; i < _ports.Count; i++) {
				VisualElement connectorStatus = _ports [i].parent.Q<VisualElement>("connector-status");
				if (connectorStatus != null) {
					connectorStatus.RemoveFromClassList (connectorCandidateClassName);
				}
			}
		}
		public bool RemoveConnections (List<Edge> edgesToRemove, bool overrideConfirm = false) {
			if (edgesToRemove.Count > 0) {
				if (overrideConfirm ||
					EditorUtility.DisplayDialog (MSG_REMOVE_CONNECTIONS_TITLE, 
					MSG_REMOVE_CONNECTIONS_MESSAGE, 
					MSG_REMOVE_CONNECTIONS_OK, 
					MSG_REMOVE_CONNECTIONS_CANCEL))
				{
					onBeforeRemoveConnections?.Invoke (edgesToRemove);
					// Remove connections.
					for (int i = 0; i < edgesToRemove.Count; i++) {
						PipelineNode sinkNode = edgesToRemove [i].input.node as PipelineNode;
						PipelineNode srcNode = edgesToRemove [i].output.node as PipelineNode;

						srcNode.pipelineElement.sinkElement = null;
						//srcNode.pipelineElement.sinkElementId = -1;
						sinkNode.pipelineElement.srcElement = null;
						//sinkNode.pipelineElement.srcElementId = -1;
					}
					pipeline.Validate ();
					onRemoveConnections?.Invoke (edgesToRemove);
					isDirty = true;
					return true;
				}
			}
			return false;
		}
		public void RefreshNode (int nodeId) {
			if (idToNode.ContainsKey (nodeId)) {
				idToNode [nodeId].RefreshNodeStatus ();
			}
		}
        #endregion

        #region Graph Events
        private GraphViewChange _GraphViewChanged(GraphViewChange graphViewChange) {
			// Elements MOVED.
			if (graphViewChange.movedElements != null) {
				List<PipelineNode> movedNodes = new List<PipelineNode> ();
				for (int i = 0; i < graphViewChange.movedElements.Count; i++) {
					movedNodes.Add (graphViewChange.movedElements [i] as PipelineNode);
				}
				if (movedNodes.Count > 0) {
					onBeforeMoveNodes?.Invoke (movedNodes, graphViewChange.moveDelta);
					// Reflect new node position on pipeline elements.
					for (int i = 0; i < movedNodes.Count; i++) {
						movedNodes [i].pipelineElement.nodePosition = movedNodes [i].GetPosition ().position;
					}
					onMoveNodes?.Invoke (movedNodes, graphViewChange.moveDelta);
				}
			}

			// Elements REMOVED (Nodes or edges).
			if (graphViewChange.elementsToRemove != null && graphViewChange.elementsToRemove.Count > 0) {
				List<PipelineNode> nodesToRemove = new List<PipelineNode> ();
				List<Edge> edgesToRemove = new List<Edge> ();
				for (int i = 0; i < graphViewChange.elementsToRemove.Count; i++) {
					PipelineNode pipelineNodeToRemove = graphViewChange.elementsToRemove [i] as PipelineNode;
					if (pipelineNodeToRemove != null) {
						nodesToRemove.Add (pipelineNodeToRemove);
					}
					Edge edgeToRemove = graphViewChange.elementsToRemove [i] as Edge;
					if (edgeToRemove != null) {
						edgesToRemove.Add (edgeToRemove);
					}
				}
				if (nodesToRemove.Count > 0) {
					bool hasRemoved = RemoveNodes (nodesToRemove);
					if (!hasRemoved) {
						graphViewChange.elementsToRemove.Clear ();
					}
				} else if (edgesToRemove.Count > 0) {
					bool hasRemoved = RemoveConnections (edgesToRemove);
					if (!hasRemoved) {
						graphViewChange.elementsToRemove.Clear ();
					}
				}
			}

			// Elements CONNECTED.
			if (graphViewChange.edgesToCreate != null) {
				graphViewChange.edgesToCreate.ForEach (edge => {
                    PipelineNode srcNode = edge.output.node as PipelineNode;
                    PipelineNode sinkNode = edge.input.node as PipelineNode;
					bool connectionDone = AddConnectionInternal (srcNode, sinkNode);
					if (connectionDone) {
						SetEdgeUserData (edge, srcNode, sinkNode);
					}
                });
			}
			return graphViewChange;
		}
		private void SetEdgeUserData (Edge edge, PipelineNode srcNode, PipelineNode sinkNode) {
			PipelineEdge pipelineEdge = new PipelineEdge () {
				srcNode = srcNode,
				sinkNode = sinkNode,
				upWeight = srcNode.pipelineElement.positionWeight,
				downWeight = sinkNode.pipelineElement.positionWeight
			};
			edge.userData = pipelineEdge;
		}
        private void _ViewTransformChanged (GraphView graphView) {
			// If zoom done.
			if (this.scale != currentZoom) {
				onZoomDone?.Invoke (this.scale, currentZoom);
				currentZoom = this.scale;
			}
			// If pan done.
			if (this.viewTransform.position != currentOffset) {
				onPanDone?.Invoke (this.viewTransform.position, currentOffset);
				currentOffset = this.viewTransform.position;
			}
        }
		private void OnSelectNodeInternal (PipelineNode pipelineNode) {
			onSelectNode?.Invoke (pipelineNode);
		}
		private void OnDeselectNodeInternal (PipelineNode pipelineNode) {
			onDeselectNode?.Invoke (pipelineNode);
		}
		private void OnEnableNodeInternal (PipelineNode pipelineNode, bool enable) {
			onBeforeEnableNode?.Invoke (pipelineNode, enable);
			pipelineNode.pipelineElement.isActive = enable;
			onEnableNode?.Invoke (pipelineNode, enable);
		}
		private void OnBeginDragNodePortInternal (Port draggingPort, List<Port> candidatePorts) {
			ShowConnectorCandidates (candidatePorts);
			isPortDragging = true;
		}
		private void OnEndDragNodePortInternal (bool isUpstream, bool connected, Edge edge) {
			HideConnectorCandidates ();
			isPortDragging = false;
		}
		private void OnCancelDragNodePortInternal () {
			HideConnectorCandidates ();
			isPortDragging = false;
		}
		/// <summary>
		/// Updates the pipeline nodes on the canvas.
		/// </summary>
		public void UpdatePipeline () {
			// If nodes have been added or deleted then load the pipeline again.
			/*
			bool reloadPipeline = false;
			if (idToNode.Count != pipeline.GetElementsCount ()) {
				reloadPipeline = true;
			} else {
				List<PipelineElement> pipelineElements = pipeline.GetElements ();
				for (int i = 0; i < pipelineElements.Count; i++) {
					if (!idToNode.ContainsKey (pipelineElements[i].id)) {
						reloadPipeline = true;
					}
				}
			}
			if (reloadPipeline) {
				*/
				CreatePipelineNodes ();
			//}

			lastUndoProcessed = pipeline.undoControl.undoCount;

			pipeline.Validate ();
		}
		/// <summary>
		/// Raises the undo redo performed event.
		/// </summary>
		void OnBroccoliUndoRedoPerformed () {
			if (pipeline != null && pipeline.undoControl.undoCount != lastUndoProcessed) {
				pipeline.OnAfterDeserialize ();
				UpdatePipeline ();
				onRequestUpdatePipeline?.Invoke ();
			}
		}
		private void KeyDown (KeyDownEvent evt) {
			if (evt.keyCode == KeyCode.Escape) {
				if (isPortDragging) OnCancelDragNodePortInternal ();
			}
		}
        #endregion
    }
}
