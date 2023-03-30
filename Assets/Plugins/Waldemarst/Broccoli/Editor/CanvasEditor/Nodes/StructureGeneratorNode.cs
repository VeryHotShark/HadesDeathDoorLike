using UnityEngine;
using UnityEditor;

using Broccoli.NodeEditorFramework;
using Broccoli.NodeEditorFramework.Utilities;
using Broccoli.Pipe;

namespace Broccoli.TreeNodeEditor
{
	/// <summary>
	/// Structure generator node.
	/// </summary>
	[Node (false, "Structure Generator/Structure Generator", 0)]
	public class StructureGeneratorNode : BaseNode 
	{
		#region Vars
		/// <summary>
		/// Gets the get Id of the node.
		/// </summary>
		/// <value>Id of the node.</value>
		public override string GetID { 
			get { return typeof (StructureGeneratorNode).ToString(); } 
		}
		/// <summary>
		/// Gets the category.
		/// </summary>
		/// <value>The category.</value>
		public override Category category { get { return Category.StructureGenerator; } }
		/// <summary>
		/// The structure generator element.
		/// </summary>
		public StructureGeneratorElement structureGeneratorElement = null;
		/// <summary>
		/// Flag to control displaying the tree structure.
		/// </summary>
		public bool inspectStructureEnabled = true;
		#endregion

		#region Constructor
		/// <summary>
		/// Node constructor.
		/// </summary>
		public StructureGeneratorNode () {
			this.nodeName = "Structure Generator";
			this.nodeHelpURL = "https://docs.google.com/document/d/1Nr6Z808i7X2zMFq8PELezPuSJNP5IvRx9C5lJxZ_Z-A/edit#heading=h.nupd9ljjpzl";
			this.nodeDescription = "This node give you access to the structure levels used to build the tree." +
				" It displays a canvas to select and edit trunk, branches, roots and sprout level parameters.";
		}
		#endregion

		#region Base Node
		/// <summary>
		/// Called when creating the node.
		/// </summary>
		/// <returns>The created node.</returns>
		protected override BaseNode CreateExplicit () {
			StructureGeneratorNode node = CreateInstance<StructureGeneratorNode> ();
			node.rectSize.x = 144;
			return node;
		}
		/// <summary>
		/// Sets the pipeline element of this node.
		/// </summary>
		/// <param name="pipelineElement">Pipeline element.</param>
		public override void SetPipelineElement (PipelineElement pipelineElement = null) {
			if (pipelineElement == null) {
				structureGeneratorElement = ScriptableObject.CreateInstance<StructureGeneratorElement> ();
			} else {
				structureGeneratorElement = (StructureGeneratorElement)pipelineElement;
			}
			this.pipelineElement = structureGeneratorElement;
		}
		#endregion
	}
}