using System.Collections.Generic;
using NodeCanvas;
using NodeCanvas.BehaviourTrees;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;
using UnityEngine;

namespace NodeCanvasAddons.AStarPathfinding
{
    [AddComponentMenu("")]
    [Name("Iterate Path Nodes")]
    [Category("A* Pathfinding")]
    [Description(
        "Iterate through a paths nodes and execute the node for each object in the list. Keeps iterating until the Termination Condition is met or the whole list is iterated and return the child node status"
        )]
    [ParadoxNotion.Design.Icon("PathfindingPath")]

    public class IteratePathNodes : BTDecorator
    {
        [RequiredField]
        [BlackboardOnly]
        public BBParameter<Path> Path;

        [BlackboardOnly] 
        public BBParameter<GraphNode> Node = new BBParameter<GraphNode>();

        [BlackboardOnly]
        public BBParameter<int> CurrentIndex = new BBParameter<int>();

        public BBParameter<int> MaxIteration;
        public BBParameter<float> MaxDuration;

        public enum TerminationConditions
        {
            FirstSuccess,
            FirstFailure,
            None
        }

        public TerminationConditions TerminationCondition = TerminationConditions.None;
        public bool ListenForPathChanges = true;

        private float _elapsedTime;
        private int _currentIndex;
        private List<GraphNode> _currentNodes;

        protected override Status OnExecute(Component agent, IBlackboard blackboard)
        {
            if (Path.isNone || Path.isNull)
            {
                Debug.LogWarning(string.Format("Path object: {0} is not set", Path));
                return Status.Failure;
            }

            if (decoratedConnection == null)
            { return Status.Resting; }

            CheckForPathReset();
            CheckAndApplyLatestIndex();

            if (_currentNodes.Count <= _currentIndex || _currentNodes.Count == 0)
            { return Status.Failure; }

            _elapsedTime += Time.deltaTime;
            Node.value = _currentNodes[_currentIndex];
            status = decoratedConnection.Execute(agent, blackboard);

            if (status == Status.Success && TerminationCondition == TerminationConditions.FirstSuccess)
            { return Status.Success; }

            if (status == Status.Failure && TerminationCondition == TerminationConditions.FirstFailure)
            { return Status.Failure; }

            if (status != Status.Running)
            {
                if (_currentIndex == _currentNodes.Count - 1 || _currentIndex == MaxIteration.value - 1)
                { return status; }

                if (MaxDuration.value > 0 && _elapsedTime >= MaxDuration.value)
                { return status; }

                decoratedConnection.Reset();

                _currentIndex++;
                return Status.Running;
            }

            return status;
        }

        private void CheckForPathReset()
        {
            if (ListenForPathChanges && _currentNodes != Path.value.path)
            { ResetPath(); }
            else if (_currentNodes == null)
            { ResetPath(); }
        }

        private void CheckAndApplyLatestIndex()
        {
            if (_currentIndex == 0 && _currentNodes.Count > 0)
            { _currentIndex++; }

            CurrentIndex.value = _currentIndex;
        }

        private void StorePathStatus()
        {
            _currentNodes = Path.value.path;
        }

        private void ResetPath()
        {
            _currentIndex = 0;
            _elapsedTime = 0;
            if (!Path.isNone && !Path.isNull)
            { StorePathStatus(); }
        }

        protected override void OnReset()
        {
            ResetPath();
        }

#if UNITY_EDITOR

        protected override void OnNodeGUI()
        {
            GUILayout.Label(string.Format("For Each Node {0}", Node));
            GUILayout.Label(string.Format("In Path {0}", Path));

            if(ListenForPathChanges)
            { GUILayout.Label("And Listen For Path Changes"); }

            if(MaxDuration.value > 0)
            { GUILayout.Label(string.Format("Exit after duration {0}", MaxDuration)); }

            if (TerminationCondition != TerminationConditions.None)
            { GUILayout.Label(string.Format("Exit on {0}", TerminationCondition)); }

            if (Application.isPlaying && _currentNodes != null)
            {
                var waypointCount = _currentNodes != null && _currentNodes.Count != 0
                    ? (_currentNodes.Count - 1).ToString() : "?";
                var message = string.Format("Index: {0} / {1}", _currentIndex, waypointCount);
                GUILayout.Label(message);
            }
        }
#endif
    }
}