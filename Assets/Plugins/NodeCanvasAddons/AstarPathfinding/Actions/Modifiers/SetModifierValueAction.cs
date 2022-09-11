using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    public abstract class SetModifierValueAction<T, V> : ActionTask<T>
        where T : MonoModifier
    {
        [GetFromAgent]
        public BBParameter<T> Modifier;

        [RequiredField]
        [BlackboardOnly] 
        public BBParameter<V> Value;

        protected override string info
        {
            get { return string.Format("Setting {0} On {1}", Value, Modifier); }
        }

        protected abstract void SetModifierValue(V value);
        
        protected override void OnExecute()
        {
            SetModifierValue(Value.value);
            EndAction(true);
        }
    }
}