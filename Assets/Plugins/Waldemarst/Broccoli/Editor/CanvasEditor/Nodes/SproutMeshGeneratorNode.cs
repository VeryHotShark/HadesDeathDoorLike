using UnityEngine;
using UnityEditor;

using Broccoli.NodeEditorFramework;
using Broccoli.NodeEditorFramework.Utilities;
using Broccoli.Pipe;

namespace Broccoli.TreeNodeEditor
{
	/// <summary>
	/// Sprout mesh generator node.
	/// </summary>
	[Node (false, "Mesh Generator/Sprouts Mesh Generator", 110)]
	public class SproutMeshGeneratorNode : BaseNode
	{
		#region Vars
		/// <summary>
		/// Get the Id of the Node.
		/// </summary>
		/// <value>Id of the node.</value>
		public override string GetID { 
			get { return typeof (SproutMeshGeneratorNode).ToString(); } 
		}
		public override Category category { get { return Category.MeshGenerator; } }
		/// <summary>
		/// The girth transform element.
		/// </summary>
		public SproutMeshGeneratorElement sproutMeshGeneratorElement;
		/// <summary>
		/// Saves the selected option on the node editor.
		/// </summary>
		public int selectedToolbarOption = 0;
		public bool showSectionSize = true;
		public bool showSectionScale = true;
		public bool showSectionHorizontalAlign = true;
		public bool showSectionResolution = true;
		public bool showSectionGravityBending = true;
		public bool showSectionMesh = true;
		#endregion

		#region Constructor
		/// <summary>
		/// Node constructor.
		/// </summary>
		public SproutMeshGeneratorNode () {
			this.nodeName = "Sprout Mesh Generator";
			this.nodeHelpURL = "https://docs.google.com/document/d/1Nr6Z808i7X2zMFq8PELezPuSJNP5IvRx9C5lJxZ_Z-A/edit#heading=h.bs0cjoc69bxn";
			this.nodeDescription = "This node contains the parameters used to build the sprouts meshes on the tree." +
				" Each sprout is meshes based on Sprout Group's parameters.";
		}
		#endregion

		#region Base Node
		/// <summary>
		/// Called when creating the node.
		/// </summary>
		/// <returns>The created node.</returns>
		protected override BaseNode CreateExplicit () {
			SproutMeshGeneratorNode node = CreateInstance<SproutMeshGeneratorNode> ();
			node.rectSize = new Vector2 (160, 72);
			node.name = "Sprout Mesh Generator";
			return node;
		}
		/// <summary>
		/// Sets the pipeline element of this node.
		/// </summary>
		/// <param name="pipelineElement">Pipeline element.</param>
		public override void SetPipelineElement (PipelineElement pipelineElement = null) {
			if (pipelineElement == null) {
				sproutMeshGeneratorElement = ScriptableObject.CreateInstance<SproutMeshGeneratorElement> ();
			} else {
				sproutMeshGeneratorElement = (SproutMeshGeneratorElement)pipelineElement;
			}
			this.pipelineElement = sproutMeshGeneratorElement;
		}
		/// <summary>
		/// Explicit drawing method for this node.
		/// </summary>
		protected override void NodeGUIExplicit () {
			if (sproutMeshGeneratorElement != null) {
				int j = 0;
				Rect sproutGroupsRect = new Rect (7, 3, 8, 8);
				for (int i = 0; i < sproutMeshGeneratorElement.sproutMeshes.Count; i++) {
					EditorGUI.DrawRect (sproutGroupsRect, 
						sproutMeshGeneratorElement.pipeline.sproutGroups.GetSproutGroupColor (
							sproutMeshGeneratorElement.sproutMeshes [i].groupId));
					j++;
					if (j >= 4) {
						sproutGroupsRect.x += 11;
						sproutGroupsRect.y = 3;
						j = 0;
					} else {
						sproutGroupsRect.y += 11;
					}
				}
				if (sproutMeshGeneratorElement != null) {
					DrawLabel ("\tSprout submesh:\n\t" + sproutMeshGeneratorElement.verticesCount + " verts\n\t" + sproutMeshGeneratorElement.trianglesCount + " tris");
				}
			}
		}
		#endregion
	}
}