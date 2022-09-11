//#define IS_ASTAR_FREE


#if IS_ASTAR_FREE
namespace Pathfinding
{
    public class AdvancedSmooth : MonoModifier
    {
        public class MaxTurn {}
        public class ConstantTurn {}
        
        public override int Order { get; }
        public override void Apply(Path path)
        {
            throw new System.NotImplementedException();
        }
        
        public MaxTurn turnConstruct1 { get; set; }
        public ConstantTurn turnConstruct2 { get; set; }
        public float turningRadius { get; set; }
    }
}
#endif