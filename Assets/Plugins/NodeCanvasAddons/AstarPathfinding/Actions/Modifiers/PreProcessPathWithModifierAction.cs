using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    [Category("A* Pathfinding/Modifiers")]
    [Name("Preprocess Path With Modifier")]
    [Description("Preprocesses the given path with the modifier provided")]
    [ParadoxNotion.Design.Icon("PathfindingPath")]
    public class PreProcessPathWithModifierAction : ActionTask
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<MonoModifier> ModifierToApply;

        [RequiredField] 
        [BlackboardOnly] 
        public BBParameter<Path> Path;

        protected override string info
        {
            get { return string.Format("Preprocessing {0} With {1}", Path, ModifierToApply); }
        }

        protected override void OnExecute()
        {
            ModifierToApply.value.PreProcess(Path.value);
            EndAction(true);
        }
    }
}