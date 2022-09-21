using NodeCanvas.Framework;
using ParadoxNotion.Design;
using ParadoxNotion;
using UnityEngine;


namespace VHS{

	[Category("AI")]
	[Name("Distance To Target")]
	[Description("Check Distance To Target")]
	public class DistanceToTarget : ConditionTask<Npc>{
		public CompareMethod checkType = CompareMethod.LessThan;
		public BBParameter<float> distance = 10;
		
		[SliderField(0, 0.1f)]
		public float floatingPoint = 0.05f;

		protected override bool OnCheck() {
			if (agent.Target == null)
				return false;
			
			return OperationTools.Compare(Vector3.Distance(agent.FeetPosition, agent.Target.FeetPosition), distance.value, checkType, floatingPoint);
		}
		
		public override void OnDrawGizmosSelected() {
			if ( agent != null ) {
				Gizmos.DrawWireSphere(agent.FeetPosition, distance.value);
			}
		}
	}
}