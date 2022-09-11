using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvasAddons.AStarPathfinding.Examples
{
    [Category("Materials")]
    [Name("Change Material Colour")]
    public class SetMaterialColorAction : ActionTask<Renderer>
    {
        [RequiredField]
        public BBParameter<Color> Color;

        protected override string info
        {
            get { return string.Format("Changing Color \nOf {0} \nTo <b>{1}</b>", agent, Color.value); }
        }

        protected override void OnExecute()
        {
            agent.GetComponent<Renderer>().material.color = Color.value;
            EndAction(true);
        }
    }
}