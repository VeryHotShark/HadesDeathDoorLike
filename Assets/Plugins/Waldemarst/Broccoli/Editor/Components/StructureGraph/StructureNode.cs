using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

using Broccoli.Pipe;
using Broccoli.Utils;

namespace Broccoli.TreeNodeEditor
{
    public class StructureNode : Node
    {
        #region Vars
        public int id = -1;
        public bool isEnable = true;
        public int tag = 0;
        public Color mark = Color.clear;
        public Port upstreamChildrenPort = null;
        public Port upstreamParentPort = null;
        public Port downstreamChildrenPort = null;
        public Port downstreamParentPort = null;
        public bool tagColorEnabled = true;
        public new class UxmlFactory : UxmlFactory<StructureNode, VisualElement.UxmlTraits> {}
        public enum NodeType {
            Trunk = 0,
            Branch = 1,
            Sprout = 2,
            Root = 3
        }
        public NodeType nodeType = NodeType.Trunk;
        public enum NodeOrientation {
            Vertical = 0,
            Horizontal = 1,
        }
        public NodeOrientation nodeOrientation = NodeOrientation.Vertical;
        private static string verticalNodeClassName = "node-vertical";
        private static string horizontalNodeClassName = "node-horizontal";
        private static string enabledClassName = "node-enabled";
        private static string disabledClassName = "node-disabled";
        public static string upChildrenPortName = "up-children-port";
        public static string upParentPortName = "up-parent-port";
        public static string downChildrenPortName = "down-children-port";
        public static string downParentPortName = "down-parent-port";
        #endregion

        #region Tag Colors
        /* https://paletton.com/#uid=75o1o0klZeHeGndhjhBq9axvd6k
        #RED TONES
        Light	141, 68, 77
        Mid	    114, 40, 49
        Dark	84, 19, 27

        #OLIVE TONES
        Light	160, 146, 70
        Mid	    129, 115, 41
        Dark	94, 82, 18

        #BLUE TONES
        Light	55, 80, 104
        Mid	35, 59, 83
        Dark	20, 42, 64

        #GREEN TONES
        Light	90, 125, 58
        Mid	    67, 105, 33
        Dark	43, 75, 14
        */
        public static int totalTags = 12;
        private static Color tagColorDefault = Color.clear;
        private static Color[] tagColors = new Color[] {
            new Color (141f/255f, 68f/255f, 77f/255f),
            new Color (114f/255f, 40f/255f, 49f/255f),
            new Color (84f/255f, 19f/255f, 27f/255f),
            new Color (160f/255f, 146f/255f, 70f/255f),
            new Color (129f/255f, 115f/255f, 41f/255f),
            new Color (94f/255f, 82f/255f, 18f/255f),
            new Color (55f/255f, 80f/255f, 104f/255f),
            new Color (35f/255f, 59f/255f, 83f/255f),
            new Color (20f/255f, 42f/255f, 64f/255f),
            new Color (90f/255f, 125f/255f, 58f/255f),
            new Color (67f/255f, 105f/255f, 33f/255f),
            new Color (43f/255f, 75f/255f, 14f/255f)
        };
        #endregion

        #region Delegates
        public delegate void OnSelectionDelegate (StructureNode pipelineNode);
        public delegate void OnEnableDelegate (StructureNode pipelineNode, bool enable);
        public OnSelectionDelegate onSelected;
        public OnSelectionDelegate onUnselected;
        public OnEnableDelegate onEnable;
        #endregion

        #region Constructors
        public StructureNode () {}
        public StructureNode (StructureNode.NodeType nodeType) {
            StructureNodeConstructor (nodeType);
        }
        public StructureNode (StructureNode.NodeType nodeType, VisualTreeAsset nodeXml) : base (AssetDatabase.GetAssetPath(nodeXml)) {
            StructureNodeConstructor (nodeType);
        }
        private void StructureNodeConstructor (StructureNode.NodeType nodeType) {
            this.nodeType = nodeType;
            Orientation portOrientation;
            if (nodeOrientation == NodeOrientation.Vertical) {
                portOrientation = Orientation.Vertical;
            } else {
                portOrientation = Orientation.Horizontal;
            }
            bool addUpstreamChildrenPort = false;
            bool addUpstreamParentPort = false;
            bool addDownstreamChildrenPort = false;
            bool addDownstreamParentPort = false;
            switch (nodeType) {
                case NodeType.Trunk:
                    addUpstreamChildrenPort = true;
                    addDownstreamChildrenPort = true;
                    break;
                case NodeType.Branch:
                    addUpstreamParentPort = true;
                    addUpstreamChildrenPort = true;
                    break;
                case NodeType.Root:
                    addDownstreamChildrenPort = true;
                    addDownstreamParentPort = true;
                    break;
                case NodeType.Sprout:
                    addUpstreamParentPort = true;
                    break;
            }
            if (addUpstreamChildrenPort) {
                upstreamChildrenPort = InstantiatePort (portOrientation, Direction.Input, Port.Capacity.Multi, typeof(StructureNode));
                upstreamChildrenPort.name = upChildrenPortName;
                inputContainer.Add (upstreamChildrenPort);
            }
            if (addDownstreamChildrenPort) {
                downstreamChildrenPort = InstantiatePort (portOrientation, Direction.Output, Port.Capacity.Multi, typeof(StructureNode));
                downstreamChildrenPort.name = downChildrenPortName;
                outputContainer.Add (downstreamChildrenPort);
            }
            if (addUpstreamParentPort) {
                upstreamParentPort = InstantiatePort (portOrientation, Direction.Output, Port.Capacity.Single, typeof(StructureNode));
                upstreamParentPort.name = upParentPortName;
                outputContainer.Add (upstreamParentPort);
            }
            if (addDownstreamParentPort) {
                downstreamParentPort = InstantiatePort (portOrientation, Direction.Input, Port.Capacity.Single, typeof(StructureNode));
                downstreamParentPort.name = downParentPortName;
                inputContainer.Add (downstreamParentPort);
            }
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
            VisualElement icon = this.Q<VisualElement>("icon");

            // Set Icon.
            if (icon != null) {
                switch (nodeType) {
                    case StructureNode.NodeType.Trunk:
                        icon.style.backgroundImage = new StyleBackground (GUITextureManager.GetNodeBgTrunk ());
                        break;
                    case StructureNode.NodeType.Branch:
                        icon.style.backgroundImage = new StyleBackground (GUITextureManager.GetNodeBgBranch ());
                        break;
                    case StructureNode.NodeType.Sprout:
                        icon.style.backgroundImage = new StyleBackground (GUITextureManager.GetNodeBgSprout ());
                        break;
                    case StructureNode.NodeType.Root:
                        icon.style.backgroundImage = new StyleBackground (GUITextureManager.GetNodeBgRoot ());
                        break;
                }
            }
            this.tooltip = nodeType.ToString ();
            
            // Enable/Disable
            if (nodeToggle != null) {
                nodeToggle.value = isEnable;
                if (isEnable) {
                    this.AddToClassList (enabledClassName);
                } else {
                    this.AddToClassList (disabledClassName);
                }
                nodeToggle.RegisterCallback<ChangeEvent<bool>>(x => SetEnableNode (x.newValue));
            }

            // Direction.
            if (nodeOrientation == NodeOrientation.Vertical) {
                this.AddToClassList (verticalNodeClassName);
            } else {
                this.AddToClassList (horizontalNodeClassName);
            }

            if (tagColorEnabled)
                SetTag (tag);
        
            MarkDirtyRepaint();
        }

        public override void OnSelected () {
            base.OnSelected ();
            onSelected?.Invoke (this);
        }

        public override void OnUnselected() {
            base.OnUnselected ();
            onUnselected?.Invoke (this);
        }

        public bool SetEnableNode (bool enable) {
            if (isEnable != enable) {
                isEnable = enable;
                if (enable) {
                    this.AddToClassList (enabledClassName);
                    this.RemoveFromClassList (disabledClassName);
                } else {
                    this.AddToClassList (disabledClassName);
                    this.RemoveFromClassList (enabledClassName);
                }
                Toggle nodeToggle = this.Q<Toggle>("node-toggle");
                if (nodeToggle != null) {
                    nodeToggle.value = enable;
                }
                onEnable?.Invoke (this, enable);
                return true;
            }
            return false;
        }
        public void SetMark (Color markColor) {
            this.mark = markColor;
            VisualElement markElem = this.Q<VisualElement>("mark");
            if (markElem != null) {
                markElem.style.backgroundColor = markColor;
            }
        }
        
        public void SetTag (int tag) {
            this.tag = tag;
            Color tagColor = GetTagColor (tag);
            VisualElement input = this.Q<VisualElement>("input");
            VisualElement output = this.Q<VisualElement>("output");
            if (input != null && output != null) {
                input.style.backgroundColor = tagColor;
                output.style.backgroundColor = tagColor;
            }
        }
        #endregion

        #region Util
        private static Color GetTagColor (int tag) {
            Color tagColor = tagColorDefault;
            if (tag > 0 && tag <= tagColors.Length) {
                tagColor = tagColors [tag -1];
            }
            return tagColor;
        }
        #endregion
    }
}
