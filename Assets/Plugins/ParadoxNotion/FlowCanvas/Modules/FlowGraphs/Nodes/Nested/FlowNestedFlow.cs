using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace FlowCanvas.Nodes
{
    [Name("Sub Flow")]
    [DropReferenceType(typeof(FlowCanvas.FlowScript))]
    [Icon("FS")]
    public class FlowNestedFlow : FlowNestedBase<FlowCanvas.FlowScript>
    {

    }
}