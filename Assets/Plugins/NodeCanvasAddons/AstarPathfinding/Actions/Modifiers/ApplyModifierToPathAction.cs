using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Modifiers")]
    [Name("Apply Modifier To Path")]
    [Description("Applies the given modifier to the given path")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class ApplyModifierToPathAction : ActionTask
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<MonoModifier> ModifierToApply;

        [RequiredField] 
        [BlackboardOnly] 
        public BBParameter<Path> Path;

        protected override string info
        {
            get { return string.Format("Applying {0} To {1}", ModifierToApply, Path); }
        }

        protected override void OnExecute()
        {
            ModifierToApply.value.Apply(Path.value);
            EndAction(true);
        }
    }
}
