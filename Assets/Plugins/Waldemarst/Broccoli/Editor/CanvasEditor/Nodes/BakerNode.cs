using UnityEngine;
using UnityEditor;

using Broccoli.NodeEditorFramework;
using Broccoli.NodeEditorFramework.Utilities;
using Broccoli.Pipe;

namespace Broccoli.TreeNodeEditor
{
	/// <summary>
	/// Baker node.
	/// </summary>
	[Node (false, "Function/Baker", 320)]
	public class BakerNode : BaseNode 
	{
		#region Vars
		/// <summary>
		/// Gets the get Id of the node.
		/// </summary>
		/// <value>Id of the node.</value>
		public override string GetID { 
			get { return typeof (BakerNode).ToString(); } 
		}
		/// <summary>
		/// Gets the category.
		/// </summary>
		/// <value>The category.</value>
		public override Category category { get { return Category.Function; } }
		/// <summary>
		/// The positioner element.
		/// </summary>
		public BakerElement bakerElement;
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
		public BakerNode () {
			this.nodeName = "Baker";
			this.nodeHelpURL = "https://docs.google.com/document/d/1Nr6Z808i7X2zMFq8PELezPuSJNP5IvRx9C5lJxZ_Z-A/edit#heading=h.r0h3ix5ubi8g";
			this.nodeDescription = "This node display options to apply to the final tree GameObject, including LOD, Collider and Ambien Oclussion parameters.";
		}
		#endregion

		#region Base Node
		/// <summary>
		/// Called when creating the node.
		/// </summary>
		/// <returns>The created node.</returns>
		protected override BaseNode CreateExplicit () {
			BakerNode node = CreateInstance<BakerNode> ();
			node.rectSize.x = 100;
			return node;
		}
		/// <summary>
		/// Sets the pipeline element of this node.
		/// </summary>
		/// <param name="pipelineElement">Pipeline element.</param>
		public override void SetPipelineElement (PipelineElement pipelineElement = null) {
			if (pipelineElement == null) {
				bakerElement = ScriptableObject.CreateInstance<BakerElement> ();
			} else {
				bakerElement = (BakerElement)pipelineElement;
			}
			this.pipelineElement = bakerElement;
		}
		#endregion
	}
}