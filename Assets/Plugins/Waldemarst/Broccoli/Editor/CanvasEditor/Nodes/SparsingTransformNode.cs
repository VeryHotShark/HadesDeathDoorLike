using System.Collections;

using UnityEditor;
using UnityEngine;

using Broccoli.NodeEditorFramework;
using Broccoli.NodeEditorFramework.Utilities;
using Broccoli.Pipe;

namespace Broccoli.TreeNodeEditor
{
	/// <summary>
	/// Sparsing transform node.
	/// </summary>
	[Node (false, "Structure Transformer/Sparsing Transform", 40)]
	public class SparsingTransformNode : BaseNode
	{
		#region Vars
		/// <summary>
		/// Gets the get Id of the node.
		/// </summary>
		/// <value>Id of the node.</value>
		public override string GetID { 
			get { return typeof (SparsingTransformNode).ToString(); } 
		}
		/// <summary>
		/// Gets the category of the node.
		/// </summary>
		/// <value>The category.</value>
		public override Category category { get { return Category.StructureTransformer; } }
		/// <summary>
		/// The sparsing transform element.
		/// </summary>
		public SparsingTransformElement sparsingTransformElement;
		#endregion

		#region Constructor
		/// <summary>
		/// Node constructor.
		/// </summary>
		public SparsingTransformNode () {
			this.nodeName = "Sparse Transform";
			this.nodeHelpURL = "https://docs.google.com/document/d/1Nr6Z808i7X2zMFq8PELezPuSJNP5IvRx9C5lJxZ_Z-A/edit#heading=h.qhtlgz8vf6cy";
			this.nodeDescription = "This node display options to modifiy the spacing and distribution of branches on the levels of the tree structure.";
		}
		#endregion

		#region Base Node
		/// <summary>
		/// Called when creating the node.
		/// </summary>
		/// <returns>The created node.</returns>
		protected override BaseNode CreateExplicit () {
			SparsingTransformNode node = CreateInstance<SparsingTransformNode> ();
			node.rectSize.x = 150;
			return node;
		}
		/// <summary>
		/// Sets the pipeline element of this node.
		/// </summary>
		/// <param name="pipelineElement">Pipeline element.</param>
		public override void SetPipelineElement (PipelineElement pipelineElement = null) {
			if (pipelineElement == null) {
				sparsingTransformElement = ScriptableObject.CreateInstance<SparsingTransformElement> ();
			} else {
				sparsingTransformElement = (SparsingTransformElement)pipelineElement;
			}
			this.pipelineElement = sparsingTransformElement;
		}
		#endregion
	}
}