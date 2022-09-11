using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;

namespace NodeCanvasAddons.AStarPathfinding
{
    public abstract class GetValueFromModifierAction<T, V> : ActionTask<T>
        where T : MonoModifier
    {
        [GetFromAgent]
        public BBParameter<T> Modifier;

        [BlackboardOnly] 
        public BBParameter<V> Value;

        protected override string info
        {
            get { return string.Format("Getting {0} From {1}", Value, Modifier); }
        }

        protected abstract V GetValueFrom(T modifier);
        
        protected override void OnExecute()
        {
            Value.value = GetValueFrom(Modifier.value);
            EndAction(true);
        }
    }
}