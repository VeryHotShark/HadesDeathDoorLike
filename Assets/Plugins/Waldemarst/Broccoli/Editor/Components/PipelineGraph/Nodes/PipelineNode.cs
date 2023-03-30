using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

using Broccoli.Pipe;
using Broccoli.Utils;

namespace Broccoli.TreeNodeEditor
{
    public class PipelineNode : Node
    {
        #region Vars
        public PipelineElement pipelineElement { get; private set; }
        public Port srcPort = null;
        public Port sinkPort = null;
        public new class UxmlFactory : UxmlFactory<PipelineNode, VisualElement.UxmlTraits> {}
        #endregion

        #region Delegates
        public delegate void OnSelectionDelegate (PipelineNode pipelineNode);
        public delegate void OnEnableDelegate (PipelineNode pipelineNode, bool enable);
        public delegate void OnBeginDragPort (Port startPort, List<Port> candidatePorts);
        public delegate void OnEndDragPort (bool isUpstream, bool connected, Edge edge);
        public OnSelectionDelegate onSelected;
        public OnSelectionDelegate onUnselected;
        public OnEnableDelegate onEnable;
        public OnBeginDragPort onBeginDragPort;
        public OnEndDragPort onEndDragPort;
        private static string sproutGroupClassName = "node-group";
        #endregion

        #region Constructors
        public PipelineNode () {}
        public PipelineNode (PipelineElement pipelineElement) {
            PipelineNodeConstructor (pipelineElement);
        }
        public PipelineNode (PipelineElement pipelineElement, VisualTreeAsset nodeXml) : base (AssetDatabase.GetAssetPath(nodeXml)) {
            PipelineNodeConstructor (pipelineElement);
        }
        private void PipelineNodeConstructor (PipelineElement pipelineElement) {
            this.pipelineElement = pipelineElement;

            // Create ports.
            if (pipelineElement.connectionType == PipelineElement.ConnectionType.Source ||
                pipelineElement.connectionType == PipelineElement.ConnectionType.Transform) {
                srcPort = InstantiatePort (Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(PipelineNode));
                srcPort.name = "source-port";
                outputContainer.Add (srcPort);
            }
            if (pipelineElement.connectionType == PipelineElement.ConnectionType.Sink ||
                pipelineElement.connectionType == PipelineElement.ConnectionType.Transform) {
                sinkPort = InstantiatePort (Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(PipelineNode));
                sinkPort.name = "sink-port";
                inputContainer.Add (sinkPort);
            }

            // Bind element events.
            pipelineElement.onValidate -= RefreshNodeStatus;
            pipelineElement.onValidate += RefreshNodeStatus;

            pipelineElement.onChange -= RefreshNodeGroups;
            pipelineElement.onChange += RefreshNodeGroups;

            this.RegisterCallback<DetachFromPanelEvent>(c => { pipelineElement.onValidate -= RefreshNodeStatus; });
        }
        #endregion

        #region Node Ops
        public void InitializeNode (StyleSheet nodeStyle)
        {
            //This was a big part of the issue, right here. In custom nodes, this doesn't get called automatically.
            //Short of supplying your own stylesheet that covers all the bases, this needs to be explicitly called to give a node visible attributes.
            UseDefaultStyling();
            if (nodeStyle != null) {
                this.styleSheets.Add (nodeStyle);
            }
    
            VisualElement contentsElement = this.Q<VisualElement>("contents");
            VisualElement titleElement = this.Q<VisualElement>("title");
            VisualElement inputHeader = this.Q<VisualElement>("input");
            Toggle nodeToggle = this.Q<Toggle>("node-toggle");
            if (nodeToggle != null) {
                nodeToggle.value = pipelineElement.isActive;
                nodeToggle.RegisterCallback<ChangeEvent<bool>>(x => OnEnableNode (x.newValue));
            }

            // Title
            title = pipelineElement.elementName;
            inputHeader.style.backgroundColor = BroccoEditorGUI.GetElementColor (pipelineElement);

            /*
            Button contentsButton = new Button(() => { 
                Debug.Log("Clicked!");
                UnityEditor.Selection.activeObject = pipelineElement;
            });
            contentsButton.text = contentsButton.name = "Test Button";
            contentsElement.Add(contentsButton);
            */
        
            SetPosition(new Rect(
                pipelineElement.nodePosition.x, 
                pipelineElement.nodePosition.y, 
                0, 0));

            RefreshNodeStatus ();
            RefreshNodeGroups ();
        
            MarkDirtyRepaint();
        }

        public void RefreshNodeStatus () {
            // Node status icon.
            VisualElement nodeStatus = this.Q<VisualElement>("node-status");
            if (nodeStatus != null) {
                if (pipelineElement.log.Count > 0) {
                    LogItem logItem = pipelineElement.log.Peek ();
                    switch (logItem.messageType) {
                        case LogItem.MessageType.Info:
                            nodeStatus.style.backgroundImage = new StyleBackground (GUITextureManager.infoTexture);
                            break;
                        case LogItem.MessageType.Warning:
                            nodeStatus.style.backgroundImage = new StyleBackground (GUITextureManager.warnTexture);
                            break;
                        case LogItem.MessageType.Error:
                            nodeStatus.style.backgroundImage = new StyleBackground (GUITextureManager.errorTexture);
                            break;
                    }
                } else{
                    nodeStatus.style.backgroundImage = null;
                }
            }
        }
        public void RefreshNodeGroups () {
            if (pipelineElement is ISproutGroupConsumer) {
                ISproutGroupConsumer consumerElement = pipelineElement as ISproutGroupConsumer;
                Color[] groupColors = consumerElement.GetGroupColors ();
                VisualElement nodeDescriptor = this.Q<VisualElement>("description");
                if (nodeDescriptor != null) {
                    nodeDescriptor.Clear ();
                    for (int i = groupColors.Length - 1; i >= 0; i--) {
                        VisualElement group = new VisualElement ();
                        group.style.backgroundColor = groupColors [i];
                        group.AddToClassList (sproutGroupClassName);
                        nodeDescriptor.Add (group);
                    }
                }
            }
        }

        public override void OnSelected () {
            base.OnSelected ();
            onSelected?.Invoke (this);
        }

        public override void OnUnselected() {
            base.OnUnselected ();
            onUnselected?.Invoke (this);
        }

        private void OnEnableNode (bool enable) {
            onEnable?.Invoke (this, enable);
        }

        public override Port InstantiatePort(Orientation orientation, Direction direction, Port.Capacity capacity, Type type) {
            return PipelineNodePort.Create<Edge>(orientation, direction, capacity, type);
        }

        public void RiseEdgeDragBegin (Port startPort, List<Port> candidatePorts) {
            onBeginDragPort?.Invoke (startPort, candidatePorts);
        }
        public void RiseEdgeDragEnds (bool isUpstream, bool connected, Edge edge) {
            onEndDragPort?.Invoke (isUpstream, connected, edge);
        }
        #endregion
    }
}