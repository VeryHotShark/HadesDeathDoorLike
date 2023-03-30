using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Broccoli.Pipe {
	/// <summary>
	/// Wind effect element.
	/// </summary>
	[System.Serializable]
	public class WindEffectElement : PipelineElement {
		#region Vars
		/// <summary>
		/// Gets the type of the connection.
		/// </summary>
		/// <value>The type of the connection.</value>
		public override ConnectionType connectionType {
			get { return PipelineElement.ConnectionType.Transform; }
		}
		/// <summary>
		/// Gets the type of the element.
		/// </summary>
		/// <value>The type of the element.</value>
		public override ElementType elementType {
			get { return PipelineElement.ElementType.MeshTransform; }
		}
		/// <summary>
		/// Gets the type of the class.
		/// </summary>
		/// <value>The type of the class.</value>
		public override ClassType classType {
			get { return PipelineElement.ClassType.WindEffect; }
		}
		/// <summary>
		/// Value used to position elements in the pipeline. The greater the more towards the end of the pipeline.
		/// </summary>
		/// <value>The position weight.</value>
		public override int positionWeight {
			get { return PipelineElement.effectWeight; }
		}
		/// <summary>
		/// The wind amplitude.
		/// </summary>
		[Range (0f, 3f)]
		public float windAmplitude = 1f;
		/// <summary>
		/// The sprout turbulence.
		/// </summary>
		[Range (0f, 2f)]
		public float sproutTurbulence = 1f;
		/// <summary>
		/// The sprout sway from side to side.
		/// </summary>
		[Range (0f, 2f)]
		public float sproutSway = 1f;
		/// <summary>
		/// The branch sway from side to side.
		/// </summary>
		[Range (0f, 4f)]
		public float branchSway = 1f;
		/// <summary>
		/// Controls the bending of the trunk of the tree.
		/// </summary>
		[Range (0f, 5f)]
		public float trunkBending = 1f;
		public enum WindQuality {
			None,
			Fastest,
			Fast,
			Better,
			Best,
			Palm
		}
		public WindQuality windQuality = WindQuality.Better;
		/// <summary>
		/// For previewing wind on the preview tree all the time (if wind zones are available).
		/// </summary>
		public bool previewWindAlways = false;
		/// <summary>
		/// The animation curve.
		/// </summary>
		public AnimationCurve windFactorCurve = AnimationCurve.Linear (0f, 0f, 1f, 1f);
		/// <summary>
		/// Flag to apply wind mapping to roots.
		/// </summary>
		public bool applyToRoots = false;
		#endregion

		#region Constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="Broccoli.Pipe.WindEffectElement"/> class.
		/// </summary>
		public WindEffectElement () {
			this.elementName = "Wind Effect";
			this.elementHelpURL = "https://docs.google.com/document/d/1Nr6Z808i7X2zMFq8PELezPuSJNP5IvRx9C5lJxZ_Z-A/edit#heading=h.4xje33y63dok";
			this.elementDescription = "This node contains all the parameters related to the wind effect on the tree/plant." +
				" These values are applied to the mesh based on the SpeedTree shaders wind parameters and are managed" + 
				" per tree instance on the BroccoTreeController script or on terrains by the BroccoTerrainController script.";
		}
		#endregion

		#region Cloning
		/// <summary>
		/// Clone this instance.
		/// </summary>
		override public PipelineElement Clone() {
			WindEffectElement clone = ScriptableObject.CreateInstance<WindEffectElement> ();
			SetCloneProperties (clone);
			clone.windAmplitude = windAmplitude;
			clone.sproutTurbulence = sproutTurbulence;
			clone.sproutSway = sproutSway;
			clone.branchSway = branchSway;
			clone.trunkBending = trunkBending;
			clone.previewWindAlways = previewWindAlways;
			clone.windFactorCurve = new AnimationCurve (windFactorCurve.keys);
			clone.windQuality = windQuality;
			clone.applyToRoots = applyToRoots;
			return clone;
		}
		#endregion
	}
}