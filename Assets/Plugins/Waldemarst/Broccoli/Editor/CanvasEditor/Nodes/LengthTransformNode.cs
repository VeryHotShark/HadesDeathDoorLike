using System.Collections;

using UnityEditor;
using UnityEngine;

using Broccoli.NodeEditorFramework;
using Broccoli.NodeEditorFramework.Utilities;
using Broccoli.Pipe;

namespace Broccoli.TreeNodeEditor
{
	/// <summary>
	/// Length transform node.
	/// </summary>
	[Node (false, "Structure Transformer/Length Transform", 20)]
	public class LengthTransformNode : BaseNode
	{
		#region Vars
		/// <summary>
		/// Gets the get Id of the node.
		/// </summary>
		/// <value>Id of the node.</value>
		public override string GetID { 
			get { return typeof (LengthTransformNode).ToString(); } 
		}
		/// <summary>
		/// Gets the category of the node.
		/// </summary>
		/// <value>Category of the node.</value>
		public override Category category { get { return Category.StructureTransformer; } }
		/// <summary>
		/// The length transform element.
		/// </summary>
		public LengthTransformElement lengthTransformElement;
		#endregion

		#region Constructor
		/// <summary>
		/// Node constructor.
		/// </summary>
		public LengthTransformNode () {
			this.nodeName = "Length Transform";
			this.nodeHelpURL = "https://docs.google.com/document/d/1Nr6Z808i7X2zMFq8PELezPuSJNP5IvRx9C5lJxZ_Z-A/edit#heading=h.wpq06kezyqw";
			this.nodeDescription = "This node display option to modify the length of branches present on the tree structure.";
		}
		#endregion

		#region Base Node
		/// <summary>
		/// Called when creating the node.
		/// </summary>
		/// <returns>The created node.</returns>
		protected override BaseNode CreateExplicit () {
			LengthTransformNode node = CreateInstance<LengthTransformNode> ();
			node.rectSize.x = 150;
			return node;
		}
		/// <summary>
		/// Sets the pipeline element of this node.
		/// </summary>
		/// <param name="pipelineElement">Pipeline element.</param>
		public override void SetPipelineElement (PipelineElement pipelineElement = null) {
			if (pipelineElement == null) {
				lengthTransformElement = ScriptableObject.CreateInstance<LengthTransformElement> ();
			} else {
				lengthTransformElement = (LengthTransformElement)pipelineElement;
			}
			this.pipelineElement = lengthTransformElement;
		}
		#endregion
	}
}