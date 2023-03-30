using UnityEngine;
using UnityEditor;

using Broccoli.NodeEditorFramework;
using Broccoli.NodeEditorFramework.Utilities;
using Broccoli.Pipe;
using Broccoli.Utils;
using Broccoli.Factory;

namespace Broccoli.TreeNodeEditor
{
	/// <summary>
	/// Sprout mapper node.
	/// </summary>
	[Node (false, "Mapper/Sprout Texture Mapper", 210)]
	public class SproutMapperNode : BaseNode
	{
		#region Vars
		/// <summary>
		/// Get the Id of the Node.
		/// </summary>
		/// <value>Id of the node.</value>
		public override string GetID { 
			get { return typeof (SproutMapperNode).ToString(); } 
		}
		public override Category category { get { return Category.Mapper; } }
		/// <summary>
		/// The girth transform element.
		/// </summary>
		public SproutMapperElement sproutMapperElement;
		#endregion

		#region Constructor
		/// <summary>
		/// Node constructor.
		/// </summary>
		public SproutMapperNode () {
			this.nodeName = "Sprout Mapper";
			this.nodeHelpURL = "https://docs.google.com/document/d/1Nr6Z808i7X2zMFq8PELezPuSJNP5IvRx9C5lJxZ_Z-A/edit#heading=h.9muzernokzif";
			this.nodeDescription = "This node contains the configuration to apply the materials and UV mapping to the sprouts." +
				" The mapping on the sprouts is based on Sprout Groups.";
		}
		#endregion

		#region Base Node
		/// <summary>
		/// Called when creating the node.
		/// </summary>
		/// <returns>The created node.</returns>
		protected override BaseNode CreateExplicit () {
			SproutMapperNode node = CreateInstance<SproutMapperNode> ();
			node.name = "Sprout Mapper";
			node.rectSize = new Vector2 (132, 72);
			return node;
		}
		/// <summary>
		/// Sets the pipeline element of this node.
		/// </summary>
		/// <param name="pipelineElement">Pipeline element.</param>
		public override void SetPipelineElement (PipelineElement pipelineElement = null) {
			if (pipelineElement == null) {
				sproutMapperElement = ScriptableObject.CreateInstance<SproutMapperElement> ();
			} else {
				sproutMapperElement = (SproutMapperElement)pipelineElement;
			}
			this.pipelineElement = sproutMapperElement;
		}
		/// <summary>
		/// Explicit drawing method for this node.
		/// </summary>
		protected override void NodeGUIExplicit () {
			if (sproutMapperElement != null) {
				int j = 0;
				Rect sproutGroupsRect = new Rect (7, 3, 8, 8);
				for (int i = 0; i < sproutMapperElement.sproutMaps.Count; i++) {
					EditorGUI.DrawRect (sproutGroupsRect, 
						sproutMapperElement.pipeline.sproutGroups.GetSproutGroupColor (
							sproutMapperElement.sproutMaps [i].groupId));
					j++;
					if (j >= 4) {
						sproutGroupsRect.x += 11;
						sproutGroupsRect.y = 3;
						j = 0;
					} else {
						sproutGroupsRect.y += 11;
					}
				}
			}
		}
		#endregion
	}
}