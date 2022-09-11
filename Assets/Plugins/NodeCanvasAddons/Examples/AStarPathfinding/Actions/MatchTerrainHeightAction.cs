using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvasAddons.AStarPathfinding.Examples
{
    [Category("Movement")]
    [Name("Match Terrain Height")]
    public class MatchTerrainHeightAction : ActionTask<Transform>
    {
        protected override void OnExecute()
        {
            var currentPosition = agent.transform.position;
            var terrainHeight = Terrain.activeTerrain.SampleHeight(currentPosition);
            agent.transform.position = new Vector3(currentPosition.x, terrainHeight, currentPosition.z);
            EndAction(true);
        }
    }
}