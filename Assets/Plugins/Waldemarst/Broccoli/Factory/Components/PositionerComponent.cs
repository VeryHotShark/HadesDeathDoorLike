using System.Collections.Generic;

using UnityEngine;

using Broccoli.Model;
using Broccoli.Pipe;
using Broccoli.Factory;

namespace Broccoli.Component
{
	/// <summary>
	/// Positioner component.
	/// Does nothing, knows nothing... just like Jon.
	/// </summary>
	public class PositionerComponent : TreeFactoryComponent {
		#region Vars
		/// <summary>
		/// The positioner element.
		/// </summary>
		PositionerElement positionerElement = null;
		#endregion

		#region Configuration
		/// <summary>
		/// Gets the changed aspects on the tree for this component.
		/// </summary>
		/// <returns>The changed aspects.</returns>
		public override int GetChangedAspects () {
			return (int)TreeFactoryProcessControl.ChangedAspect.None;
		}
		#endregion

		#region Processing
		/// <summary>
		/// Process the tree according to the pipeline element.
		/// </summary>
		/// <param name="treeFactory">Parent tree factory.</param>
		/// <param name="useCache">If set to <c>true</c> use cache.</param>
		/// <param name="useLocalCache">If set to <c>true</c> use local cache.</param>
		/// <param name="ProcessControl">Process control.</param>
		public override bool Process (TreeFactory treeFactory, 
			bool useCache = false, 
			bool useLocalCache = false, 
			TreeFactoryProcessControl ProcessControl = null) 
		{
			if (pipelineElement != null && tree != null) {
				positionerElement = pipelineElement as PositionerElement;
				return true;
			}
			return false;
		}
		#endregion
	}
}