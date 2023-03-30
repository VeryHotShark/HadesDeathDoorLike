using System.Collections;
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
    public class StructureGraphView : GraphView {
		#region StructureEdge Class
		public class StructureEdge {
			public StructureNode parentNode = null;
			public StructureNode childNode = null;
		}
		#endregion

        #region Vars
        public StructureLevelNode rootNode;
		private float currentZoom = 1f;
		private Vector3 currentOffset = Vector3.zero;
        public StructureNode.NodeOrientation nodeOrientation = StructureNode.NodeOrientation.Vertical;
        bool isDirty = true;
        private bool removingEdgesFromRemoveNode = false;
		private Vector2 contentOffset = Vector2.zero;
        #endregion

        #region Config Vars
        public bool isUniqueTrunk = true;
		public bool addTrunkEnabled = false;
		public bool removeTrunkEnabled = false;
		public bool toggleTrunkEnabled = false;
		public bool taggingEnabled = false;
        #endregion

		#region Delegates
		public delegate void OnZoomDelegate (float currentZoom, float previousZoom);
		public delegate void OnOffsetDelegate (Vector2 currentOffset, Vector2 previousOffset);
		public delegate void OnNodeDelegate (StructureNode node);
		public delegate void OnNodePosDelegate (StructureNode node, Vector2 nodePosition);
        public delegate void OnEnableNodeDelegate (StructureNode node, bool enable);
		public delegate void OnMoveNodesDelegate (List<StructureNode> nodes, Vector2 delta);

		public delegate void OnRemoveNodesDelegate (List<StructureNode> nodes);
		public delegate void OnEdgeDelegate (StructureNode parentNode, StructureNode childNode);
		public delegate void OnEdgesDelegate (List<Edge> edges);
		public delegate void OnDelegate ();
		public OnZoomDelegate onZoomDone;
		public OnOffsetDelegate onPanDone;
		public OnNodeDelegate onSelectNode;
		public OnNodeDelegate onDeselectNode;
		public OnMoveNodesDelegate onMoveNodes;
		public OnNodePosDelegate onBeforeAddNode;
		public OnNodePosDelegate onAddNode;
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
		/// Id to nodes dictionary.
		/// </summary>
		Dictionary<int, StructureNode> idToNode = new Dictionary<int, StructureNode> ();
        Dictionary<int, StructureNode> idToTrunkNode = new Dictionary<int, StructureNode> ();
		public VisualTreeAsset nodeXml;
        public StyleSheet nodeStyle;
		public StyleSheet graphViewStyle;
		/// <summary>
		/// GUI container.
		/// </summary>
		public VisualElement guiContainer;
		/// <summary>
		/// Name for the GUI container.
		/// </summary>
		private static string guiContainerName = "gui-container";
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

            this.AddManipulator(new ContentDragger());
			this.AddManipulator(new SelectionDragger());
			this.AddManipulator(new ClickSelector());
            this.RegisterCallback<KeyDownEvent>(KeyDown);
	
			this.graphViewChanged = _GraphViewChanged;

            this.viewTransformChanged = _ViewTransformChanged;
            GridBackground gridBackground = new GridBackground() { name = "Grid" };
			this.Add(gridBackground);
			gridBackground.SendToBack();

			nodeXml = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(ExtensionManager.extensionPath + "Editor/Resources/GUI/StructureNodeView.uxml");
			nodeStyle = AssetDatabase.LoadAssetAtPath<StyleSheet>(ExtensionManager.extensionPath + "Editor/Resources/GUI/StructureNodeViewStyle.uss");
			graphViewStyle = AssetDatabase.LoadAssetAtPath<StyleSheet>(ExtensionManager.extensionPath + "Editor/Resources/GUI/StructureGraphViewStyle.uss");

			if (graphViewStyle != null) {
				this.styleSheets.Add (graphViewStyle);
			}

			guiContainer = new VisualElement ();
			guiContainer.name = guiContainerName;
			this.Add (guiContainer);
        }
		/// <summary>
		/// Sets the content view of the canvas to center selection on the graph without affecting te pan value.
		/// </summary>
		/// <param name="offset">Content view offset.</param>
		public void SetContentViewOffset (Vector2 offset) {
			this.contentViewContainer.style.left = offset.x;
			this.contentViewContainer.style.top = offset.y;
			contentOffset.x = offset.x;
			contentOffset.y = offset.y;
		}
        public void ClearElements () {
            ClearEdges ();
            ClearNodes ();
        }
		public override void BuildContextualMenu (ContextualMenuPopulateEvent evt) {
			var position = viewTransform.matrix.inverse.MultiplyPoint(evt.localMousePosition - contentOffset);

            // On GraphView area.
			if (evt.target is GraphView) {
				if (addTrunkEnabled) {
					evt.menu.AppendAction("Add Trunk Node", 
						(e) => { AddNode (StructureNode.NodeType.Trunk, position); }, (e) => { return OptionStatusAddTrunkNode (); });
				}
                evt.menu.AppendAction("Add Branch Node", 
					(e) => { AddNode (StructureNode.NodeType.Branch, position); }, DropdownMenuAction.AlwaysEnabled);
                evt.menu.AppendAction("Add Root Node", 
					(e) => { AddNode (StructureNode.NodeType.Root, position); }, DropdownMenuAction.AlwaysEnabled);
                evt.menu.AppendSeparator();
                evt.menu.AppendAction("Add Sprout Node", 
					(e) => { AddNode (StructureNode.NodeType.Sprout, position); }, DropdownMenuAction.AlwaysEnabled);
			} else if (evt.target is Node) {
				StructureNode targetNode = evt.target as StructureNode;
				if (!removeTrunkEnabled && !(targetNode.nodeType == StructureNode.NodeType.Trunk)) {
					evt.menu.AppendAction("Remove Node", (e) => { RemoveNodes (new List<StructureNode> () {targetNode}); });
				}
				if (taggingEnabled) {
					evt.menu.AppendSeparator ();
					if (targetNode.tag > 0) {
						evt.menu.AppendAction("Remove Tag", (e) => { SetNodeTag (0, targetNode); });
					}
					for (int i = 1; i <= StructureNode.totalTags; i++) {
						var tagId = i;
						evt.menu.AppendAction("Set Tag/Tag " + i, (e) => { SetNodeTag (tagId, targetNode); });
					}
				}
			}
        }
		void DropDownPrepareForDisplay (EventBase e) {

		}
        DropdownMenuAction.Status OptionStatusAddTrunkNode () {
            if (isUniqueTrunk && idToTrunkNode.Count > 0) {
                return DropdownMenuAction.Status.Disabled;
            }
            return DropdownMenuAction.Status.Normal;
        }
		public override List<Port> GetCompatiblePorts (Port startPort, NodeAdapter nodeAdapter) {
			List<Port> _ports = ports.ToList ();
			return ports.ToList().Where(endPort =>
						endPort.direction != startPort.direction &&
						endPort.node != startPort.node &&
						endPort.portType == startPort.portType)
			.ToList();
		}
        #endregion

        #region Node Ops
        /// <summary>
        /// Clear all the nodes on this graph.
        /// </summary>
        public void ClearNodes () {
            var nodesEnumerator = idToNode.GetEnumerator ();
            while (nodesEnumerator.MoveNext ()) {
                this.RemoveElement (nodesEnumerator.Current.Value);
            }
            idToNode.Clear ();
            idToTrunkNode.Clear ();
        }
        /// <summary>
        /// Clear all the edges on this graph.
        /// </summary>
        public void ClearEdges () {
			List<Edge> _edges = edges.ToList ();
			for (int i = 0; i < _edges.Count; i++) {
				RemoveElement (_edges [i]);
			}
			_edges.Clear ();
		}
        /// <summary>
        /// Creates a structure node.
        /// </summary>
        /// <returns></returns>
        public StructureNode CreateNode (int nodeId, StructureNode.NodeType nodeType, bool isEnable) {
            StructureNode structureNode;
            if (nodeXml == null) {
                structureNode = new StructureNode (nodeType) { id = nodeId, name = "structure-node", 
                    isEnable = isEnable, nodeOrientation = nodeOrientation };
            } else {
                structureNode = new StructureNode (nodeType, nodeXml) { id = nodeId, name = "structure-node", 
                    isEnable = isEnable, nodeOrientation = nodeOrientation };
            }
            structureNode.nodeType = nodeType;
			if (!toggleTrunkEnabled && structureNode.nodeType == StructureNode.NodeType.Trunk) {
				Toggle nodeToggle = structureNode.Q<Toggle>("node-toggle");
				if (nodeToggle != null) {
					nodeToggle.style.display = DisplayStyle.None;
				}
			}
            structureNode.InitializeNode (nodeStyle);
            return structureNode;
        }
        private bool CanAddNode (StructureNode.NodeType nodeType) {
            if (nodeType == StructureNode.NodeType.Trunk && isUniqueTrunk && idToTrunkNode.Count > 0) {
                return false;
            }
            return true;
        }
		private bool CanRemoveNodes (List<StructureNode> nodesToRemove) {
			bool canRemove = false;
			if (nodesToRemove != null && nodesToRemove.Count > 0) {
				canRemove = true;
				if (!removeTrunkEnabled) {
					for (int i = 0; i < nodesToRemove.Count; i++) {
						if (nodesToRemove [i].nodeType == StructureNode.NodeType.Trunk) {
							canRemove = false;
							break;
						}
					}
				}
			}
			return canRemove;
		}
        private int GetNodeId (StructureNode.NodeType nodeType) {
            int nodeId = 0;
            while (idToNode.ContainsKey (nodeId)) {
                nodeId += 1;
            }
            return nodeId;
        }
        private bool RegisterNode (StructureNode node) {
            if (!idToNode.ContainsKey (node.id)) {
                idToNode.Add (node.id, node);
                if (node.nodeType == StructureNode.NodeType.Trunk) {
                    idToTrunkNode.Add (node.id, node);
                }
                return true;
            }
            return false;
        }
        private bool DeregisterNode (StructureNode node) {
            if (idToNode.ContainsKey (node.id)) {
                idToNode.Remove (node.id);
                if (node.nodeType == StructureNode.NodeType.Trunk && idToTrunkNode.ContainsKey (node.id)) {
                    idToTrunkNode.Remove (node.id);
                }
            }
            return false;
        }
        private void RemoveNodeEdges (StructureNode node) {
            if (node != null) {
                if (node.upstreamChildrenPort != null) {
                    DeleteElements (node.upstreamChildrenPort.connections);
                    node.upstreamChildrenPort?.DisconnectAll ();
                }
                if (node.upstreamParentPort != null) {
                    DeleteElements (node.upstreamParentPort.connections);
                    node.upstreamParentPort?.DisconnectAll ();
                }
                if (node.downstreamChildrenPort != null) {
                    DeleteElements (node.downstreamChildrenPort.connections);
                    node.downstreamChildrenPort?.DisconnectAll ();
                }
                if (node.downstreamParentPort != null) {
                    DeleteElements (node.downstreamParentPort.connections);
                    node.downstreamParentPort?.DisconnectAll ();
                }
                MarkDirtyRepaint ();
            }
        }
		private bool CanConnect (StructureNode.NodeType parentNodeType, StructureNode.NodeType childNodeType) {
			bool canConnect = false;
			switch (parentNodeType) {
				case StructureNode.NodeType.Trunk:
					if (childNodeType != StructureNode.NodeType.Trunk) canConnect = true;
					break;
				case StructureNode.NodeType.Branch:
					if (childNodeType == StructureNode.NodeType.Branch || 
						childNodeType == StructureNode.NodeType.Sprout) canConnect = true;
					break;
				case StructureNode.NodeType.Root:
					if (childNodeType == StructureNode.NodeType.Root) canConnect = true;
					break;
				case StructureNode.NodeType.Sprout:
					break;
			}
			return canConnect;
		}
		private Edge CreateEdge (StructureNode parentNode, StructureNode childNode) {
			Edge edge = null;
			// TRUNK.
			if (parentNode.nodeType == StructureNode.NodeType.Trunk) {
				if (childNode.nodeType == StructureNode.NodeType.Branch ||
					childNode.nodeType == StructureNode.NodeType.Sprout)
				{
					edge = parentNode.upstreamChildrenPort.ConnectTo (childNode.upstreamParentPort);
				} else if (childNode.nodeType == StructureNode.NodeType.Root) {
					edge = parentNode.downstreamChildrenPort.ConnectTo (childNode.downstreamParentPort);
				}
			}
			// UPSTREAM.
			else if (parentNode.nodeType == StructureNode.NodeType.Branch ||
				parentNode.nodeType == StructureNode.NodeType.Sprout)
			{
				if (childNode.nodeType == StructureNode.NodeType.Branch ||
					childNode.nodeType == StructureNode.NodeType.Sprout)
				{
					edge = parentNode.upstreamChildrenPort.ConnectTo (childNode.upstreamParentPort);
				}
			}
			// DOWNSTREAM.
			else if (parentNode.nodeType == StructureNode.NodeType.Root) {
				if (childNode.nodeType == StructureNode.NodeType.Root) {
					edge = parentNode.downstreamChildrenPort.ConnectTo (childNode.downstreamParentPort);
				}
			}
			if (edge != null) {
				AddElement (edge);
			}
			return edge;
		}
        public bool AddNode (StructureNode.NodeType nodeType, Vector2 nodePosition, bool isEnable = true) {
            int nodeId = GetNodeId (nodeType);
            return AddNode (nodeType, nodeId, nodePosition, isEnable);
        }
        public bool AddNode (StructureNode.NodeType nodeType, int nodeId, Vector2 nodePosition, bool isEnable = true) {
            if (CanAddNode (nodeType)) {
                if (nodeId >= 0) {
                    StructureNode structureNode = CreateNode (nodeId, nodeType, isEnable);
                    if (structureNode != null && !idToNode.ContainsKey (nodeId)) {
                        onBeforeAddNode?.Invoke (structureNode, nodePosition);
                        this.AddElement (structureNode);
                        structureNode.onSelected -= OnSelectNodeInternal;
                        structureNode.onSelected += OnSelectNodeInternal;
                        structureNode.onUnselected -= OnDeselectNodeInternal;
                        structureNode.onUnselected += OnDeselectNodeInternal;
                        structureNode.onEnable -= OnEnableNodeInternal;
                        structureNode.onEnable += OnEnableNodeInternal;
                        structureNode.SetPosition (new Rect(
                            nodePosition.x, 
                            nodePosition.y, 
                            0, 0));
                        RegisterNode (structureNode);
                        onAddNode?.Invoke (structureNode, nodePosition);
                        isDirty = true;
                    }
                }
            }
            return false;
		}
        public bool RemoveNodes (List<StructureNode> nodesToRemove, bool overrideConfirm = false) {
			if (CanRemoveNodes (nodesToRemove)) {
				if (overrideConfirm ||
					EditorUtility.DisplayDialog (MSG_REMOVE_NODES_TITLE, 
					MSG_REMOVE_NODES_MESSAGE, 
					MSG_REMOVE_NODES_OK, 
					MSG_REMOVE_NODES_CANCEL)) 
				{
					onBeforeRemoveNodes?.Invoke (nodesToRemove);
                    removingEdgesFromRemoveNode = true;
					for (int i = 0; i < nodesToRemove.Count; i++) {
                        DeregisterNode (nodesToRemove [i]);
                        RemoveNodeEdges (nodesToRemove [i]);
						RemoveElement (nodesToRemove [i]);
					}
                    removingEdgesFromRemoveNode = false;
					onRemoveNodes?.Invoke (nodesToRemove);
					isDirty = true;
					return true;
				}
			}
			return false;
		}
		public bool AddConnection (int parentNodeId, int childNodeId) {
			StructureNode parentNode = null;
			StructureNode childNode = null;
			idToNode.TryGetValue (parentNodeId, out parentNode);
			idToNode.TryGetValue (childNodeId, out childNode);
			if (parentNode != null && childNode != null) {
				bool connectionAdded = AddConnectionInternal (parentNode, childNode);
				if (connectionAdded) {
					Edge edge = CreateEdge (parentNode, childNode);
					SetEdgeUserData (edge, parentNode, childNode);
				}
				return connectionAdded;
			}
			return false;
		}
		private bool AddConnectionInternal (StructureNode parentNode, StructureNode childNode) {
			if (parentNode != null && childNode != null) {
                bool canConnect = CanConnect (parentNode.nodeType, childNode.nodeType);
                if (canConnect) {
					onBeforeAddConnection?.Invoke (parentNode, childNode);
					onAddConnection?.Invoke (parentNode, childNode);
					isDirty = true;
                    return true;
				}
			}
            return false;
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
					onRemoveConnections?.Invoke (edgesToRemove);
					isDirty = true;
					return true;
				}
			}
			return false;
		}
		public bool SetNodeEnabled (int nodeId, bool enabled) {
			if (idToNode.ContainsKey (nodeId)) {
				return idToNode [nodeId].SetEnableNode (enabled);
			}
			return false;
		}
		public void SetNodeMark (int nodeId, Color markColor) {
			if (idToNode.ContainsKey (nodeId)) {
				idToNode [nodeId].SetMark (markColor);
			}
		}
		public void SetNodeTag (int nodeTag, StructureNode node) {
			node.SetTag (nodeTag);
		}
        #endregion

        #region Graph Events
        private GraphViewChange _GraphViewChanged(GraphViewChange graphViewChange) {
			// Elements MOVED.
			if (graphViewChange.movedElements != null) {
				List<StructureNode> movedNodes = new List<StructureNode> ();
				for (int i = 0; i < graphViewChange.movedElements.Count; i++) {
					movedNodes.Add (graphViewChange.movedElements [i] as StructureNode);
				}
				if (movedNodes.Count > 0) {
					onMoveNodes?.Invoke (movedNodes, graphViewChange.moveDelta);
				}
			}

			if (graphViewChange.elementsToRemove != null && graphViewChange.elementsToRemove.Count > 0) {
			// Elements REMOVED (Nodes or edges).
				List<StructureNode> nodesToRemove = new List<StructureNode> ();
				List<Edge> edgesToRemove = new List<Edge> ();
				for (int i = 0; i < graphViewChange.elementsToRemove.Count; i++) {
					StructureNode pipelineNodeToRemove = graphViewChange.elementsToRemove [i] as StructureNode;
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
				} else if (edgesToRemove.Count > 0 && !removingEdgesFromRemoveNode) {
					bool hasRemoved = RemoveConnections (edgesToRemove);
					if (!hasRemoved) {
						graphViewChange.elementsToRemove.Clear ();
					}
				}
			}

			// Elements CONNECTED.
			if (graphViewChange.edgesToCreate != null && graphViewChange.edgesToCreate.Count > 0) {
				StructureNode parentNode;
				StructureNode childNode;
				bool isUpstream = false;
				if (graphViewChange.edgesToCreate [0].input.name.Equals (StructureNode.upChildrenPortName)) {
					isUpstream = true;
				}
				if (isUpstream) {
					parentNode = graphViewChange.edgesToCreate [0].input.node as StructureNode;
					childNode = graphViewChange.edgesToCreate [0].output.node as StructureNode;
				} else {
					parentNode = graphViewChange.edgesToCreate [0].output.node as StructureNode;
					childNode = graphViewChange.edgesToCreate [0].input.node as StructureNode;
				}
                bool connectionAdded = AddConnectionInternal (parentNode, childNode);
                if (connectionAdded) {
                    SetEdgeUserData (graphViewChange.edgesToCreate [0], parentNode, childNode);
                } else {
                    graphViewChange.edgesToCreate.Clear ();
                }
			}
            
			return graphViewChange;
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
		private void OnSelectNodeInternal (StructureNode pipelineNode) {
			onSelectNode?.Invoke (pipelineNode);
		}
		private void OnDeselectNodeInternal (StructureNode pipelineNode) {
			onDeselectNode?.Invoke (pipelineNode);
		}
		private void OnEnableNodeInternal (StructureNode node, bool enable) {
			onBeforeEnableNode?.Invoke (node, enable);
			node.isEnable = enable;
			onEnableNode?.Invoke (node, enable);
		}
		private void SetEdgeUserData (Edge edge, StructureNode parentNode, StructureNode childNode) {
			StructureEdge structureEdge = new StructureEdge () { parentNode = parentNode, childNode = childNode};
			edge.userData = structureEdge;
		}
		private void KeyDown(KeyDownEvent evt)
		{
		
		}
        #endregion
    }
}
