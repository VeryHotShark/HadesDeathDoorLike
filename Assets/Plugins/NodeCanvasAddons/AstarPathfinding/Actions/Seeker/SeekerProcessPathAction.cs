using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Paths")]
    [Name("Seeker Process Path")]
    [Description("Tells the seeker to process the given path making use of modifiers attached")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class SeekerProcessPathAction : ActionTask<Seeker>
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<Path> Path;

        [GetFromAgent]
        private Seeker _seeker = default;
        
        protected override string info
        {
            get
            {
                return string.Format("Seeker processing \npath {0}", Path);
            }
        }

        protected override void OnExecute()
        {
            if(!Path.isNull && !Path.isNone)
            { _seeker.PostProcess(Path.value); }
            EndAction(true);
        }
    }
}