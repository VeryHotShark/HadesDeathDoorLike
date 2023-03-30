using System;
using System.Collections.Generic;

using UnityEngine;

using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

namespace Broccoli.TreeNodeEditor
{
    public class PipelineNodePort : Port
    {
        protected PipelineNodePort(Orientation portOrientation, Direction portDirection, Capacity portCapacity, Type type) : base(portOrientation, portDirection, portCapacity, type)
        {
     
        }

        /*
        public override void OnStartEdgeDragging()
        {
            base.OnStartEdgeDragging();
            PipelineNode pN = this.node as PipelineNode;
            Debug.Log("Start Dragging node " + pN.pipelineElement.id);
        }
 
        public override void OnStopEdgeDragging()
        {
            base.OnStopEdgeDragging();
            Debug.Log("Stopped Dragging");
        }
        */
 
        public new static Port Create<TEdge>(
            Orientation orientation,
            Direction direction,
            Capacity capacity,
            Type type)
            where TEdge : Edge, new()
        {
            DefaultEdgeConnectorListener listener = new DefaultEdgeConnectorListener();
            PipelineNodePort ele = new PipelineNodePort(orientation, direction, capacity, type)
            {
                m_EdgeConnector = new EdgeConnector<TEdge>(listener)
            };
            ele.AddManipulator(ele.m_EdgeConnector);
            return ele;
        }
 
        private class DefaultEdgeConnectorListener : IEdgeConnectorListener
        {
            private GraphViewChange _graphViewChange;
            private List<Edge> _edgesToCreate;
            private List<GraphElement> _edgesToDelete;
 
            public DefaultEdgeConnectorListener() {
                _edgesToCreate = new List<Edge>();
                _edgesToDelete = new List<GraphElement>();
                _graphViewChange.edgesToCreate = _edgesToCreate;
            }
 
            public void OnDropOutsidePort(Edge edge, Vector2 position) {
                if (edge.output != null && edge.output.node != null) {
                    (edge.output.node as PipelineNode).RiseEdgeDragEnds (false, false, edge);
                } else if (edge.input != null && edge.input.node != null) {
                    (edge.input.node as PipelineNode).RiseEdgeDragEnds (true, false, edge);
                }
            }
 
            public void OnDrop(GraphView graphView, Edge edge) {
                _edgesToCreate.Clear();
                _edgesToCreate.Add(edge);
                _edgesToDelete.Clear();
                if (edge.input.capacity == Capacity.Single)
                {
                    foreach (Edge connection in edge.input.connections)
                    {
                        if (connection != edge)
                            _edgesToDelete.Add(connection);
                    }
                }
 
                if (edge.output.capacity == Capacity.Single)
                {
                    foreach (Edge connection in edge.output.connections)
                    {
                        if (connection != edge)
                            _edgesToDelete.Add(connection);
                    }
                }
 
                if (_edgesToDelete.Count > 0)
                    graphView.DeleteElements(_edgesToDelete);
                List<Edge> edgesToCreate = _edgesToCreate;
                if (graphView.graphViewChanged != null)
                    edgesToCreate = graphView.graphViewChanged(_graphViewChange).edgesToCreate;
                foreach (Edge edge1 in edgesToCreate)
                {
                    graphView.AddElement(edge1);
                    edge.input.Connect(edge1);
                    edge.output.Connect(edge1);
                }
                if (edge.output != null && edge.output.edgeConnector.edgeDragHelper.draggedPort != null) {
                    PipelineNode pN = edge.output.edgeConnector.edgeDragHelper.draggedPort.node as PipelineNode;
                    pN.RiseEdgeDragEnds (false, true, edge);
                } else if (edge.output != null && edge.input.edgeConnector.edgeDragHelper.draggedPort != null) {
                    PipelineNode pN = edge.input.edgeConnector.edgeDragHelper.draggedPort.node as PipelineNode;
                    pN.RiseEdgeDragEnds (true, true, edge);
                }
            }
        }
    }
}