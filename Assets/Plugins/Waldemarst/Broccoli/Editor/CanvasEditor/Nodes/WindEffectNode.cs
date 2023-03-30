using UnityEngine;
using UnityEditor;

using Broccoli.NodeEditorFramework;
using Broccoli.NodeEditorFramework.Utilities;
using Broccoli.Pipe;

namespace Broccoli.TreeNodeEditor
{
	/// <summary>
	/// Wind effect node.
	/// </summary>
	[Node (false, "Function/Wind Effect Node", 300)]
	public class WindEffectNode : BaseNode
	{
		#region Vars
		/// <summary>
		/// Get the Id of the Node.
		/// </summary>
		/// <value>Id of the node.</value>
		public override string GetID { 
			get { return typeof (WindEffectNode).ToString(); } 
		}
		public override Category category { get { return Category.Function; } }
		/// <summary>
		/// The wind effect element.
		/// </summary>
		public WindEffectElement windEffectElement;
		#endregion

		#region Constructor
		/// <summary>
		/// Node constructor.
		/// </summary>
		public WindEffectNode () {
			this.nodeName = "Wind Effect";
			this.nodeHelpURL = "https://docs.google.com/document/d/1Nr6Z808i7X2zMFq8PELezPuSJNP5IvRx9C5lJxZ_Z-A/edit#heading=h.4xje33y63dok";
			this.nodeDescription = "This node contains all the parameters related to the wind effect on the tree/plant." +
				" These values are applied to the mesh based on the SpeedTree shaders wind parameters and are managed" + 
				" per tree instance on the BroccoTreeController script or on terrains by the BroccoTerrainController script.";
		}
		#endregion

		#region Base Node
		/// <summary>
		/// Called when creating the node.
		/// </summary>
		/// <returns>The created node.</returns>
		protected override BaseNode CreateExplicit () {
			WindEffectNode node = CreateInstance<WindEffectNode> ();
			node.rectSize.x = 100;
			return node;
		}
		/// <summary>
		/// Sets the pipeline element of this node.
		/// </summary>
		/// <param name="pipelineElement">Pipeline element.</param>
		public override void SetPipelineElement (PipelineElement pipelineElement = null) {
			if (pipelineElement == null) {
				windEffectElement = ScriptableObject.CreateInstance<WindEffectElement> ();
			} else {
				windEffectElement = (WindEffectElement)pipelineElement;
			}
			this.pipelineElement = windEffectElement;
		}
		#endregion
	}
}