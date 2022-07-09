namespace FlowCanvas.Nodes
{
    ///<summary>Implement in a UnityObject to make it possible to add as a node in a flowScript</summary>
    public interface IExternalImplementedNode
    {
        void RegisterPorts(FlowNode parent);
    }
}