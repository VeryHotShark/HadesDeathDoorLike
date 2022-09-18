using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding{
	[Category("A* Pathfinding/Rich AI")]
	[Name("Clear Path")]
	[Description("Clears underlying Rich AI agent path")]
	[ParadoxNotion.Design.Icon("PathfindingPath")]
	public class ClearPath : ActionTask<RichAI>{

		[GetFromAgent]
		private RichAI _richAI = default;
		
		protected override void OnExecute(){
			_richAI.SetPath(null);
			EndAction(true);
		}
	}
}