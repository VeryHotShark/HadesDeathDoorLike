//#define IS_ASTAR_FREE

#if IS_ASTAR_FREE
namespace Pathfinding
{
    public class RadiusModifier : MonoModifier
    {
        public override int Order { get; }
        
        public override void Apply(Path path)
        {
            throw new System.NotImplementedException();
        }
        
        public float detail { get; set; }
        public float radius { get; set; }
    }
}
#endif