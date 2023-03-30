using UnityEngine;
using UnityEditor;

using Broccoli.NodeEditorFramework;
using Broccoli.NodeEditorFramework.Utilities;
using Broccoli.Pipe;

namespace Broccoli.TreeNodeEditor
{
	/// <summary>
	/// Branch mesh generator node.
	/// </summary>
	[Node (false, "Mesh Generator/Branches Mesh Generator", 100)]
	public class BranchMeshGeneratorNode : BaseNode 
	{
		#region Vars
		/// <summary>
		/// Gets the get Id of the node.
		/// </summary>
		/// <value>Id of the node.</value>
		public override string GetID { 
			get { return typeof (BranchMeshGeneratorNode).ToString(); } 
		}
		/// <summary>
		/// Gets the category of the node.
		/// </summary>
		/// <value>Category of the node.</value>
		public override Category category { get { return Category.MeshGenerator; } }
		/// <summary>
		/// The branch mesh generator element.
		/// </summary>
		public BranchMeshGeneratorElement branchMeshGeneratorElement;
		/// <summary>
		/// Selected toolbar option for the node.
		/// </summary>
		[System.NonSerialized]
		public int selectedToolbarOption = 0;
		#endregion

		#region Constructor
		/// <summary>
		/// Node constructor.
		/// </summary>
		public BranchMeshGeneratorNode () {
			this.nodeName = "Branch Mesh Generator";
			this.nodeHelpURL = "https://docs.google.com/document/d/1Nr6Z808i7X2zMFq8PELezPuSJNP5IvRx9C5lJxZ_Z-A/edit#heading=h.y4f3f0sgdmn";
			this.nodeDescription = "This node contains the parameters used to create the trunk, branches and roots mesh." + 
				" It also contains the Level of Detail (LOD) configuration to be used when creating a Prefab.";
		}
		#endregion

		#region Base Node
		/// <summary>
		/// Called when creating the node.
		/// </summary>
		/// <returns>The created node.</returns>
		protected override BaseNode CreateExplicit () {
			BranchMeshGeneratorNode node = CreateInstance<BranchMeshGeneratorNode> ();
			node.rectSize = new Vector2 (162, 72);
			return node;
		}
		/// <summary>
		/// Sets the pipeline element of this node.
		/// </summary>
		/// <param name="pipelineElement">Pipeline element.</param>
		public override void SetPipelineElement (PipelineElement pipelineElement = null) {
			if (pipelineElement == null) {
				branchMeshGeneratorElement = ScriptableObject.CreateInstance<BranchMeshGeneratorElement> ();
			} else {
				branchMeshGeneratorElement = (BranchMeshGeneratorElement)pipelineElement;
			}
			this.pipelineElement = branchMeshGeneratorElement;
		}
		/// <summary>
		/// Explicit drawing method for this node.
		/// </summary>
		protected override void NodeGUIExplicit () {
			if (branchMeshGeneratorElement != null) {
				DrawLabel ("Branch submesh:\n " + branchMeshGeneratorElement.verticesCount + " verts\n " + branchMeshGeneratorElement.trianglesCount + " tris");
			}
		}
		#endregion
	}
}