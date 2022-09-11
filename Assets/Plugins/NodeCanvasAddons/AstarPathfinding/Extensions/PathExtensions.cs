using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

namespace NodeCanvasAddons.AStarPathfinding
{
    public static class PathExtensions
    {
        public static Path DuplicatePath(this Path pathToClone)
        {
            return new ABPath
            {
                heuristicScale = pathToClone.heuristicScale,
                enabledTags = pathToClone.enabledTags,
                nnConstraint = pathToClone.nnConstraint,
                path = pathToClone.path,
                vectorPath = pathToClone.vectorPath,
                tagPenalties = pathToClone.tagPenalties
            };
        }

        public static Path ReversePath(this Path pathToReverse)
        {
            var duplicatePath = pathToReverse.DuplicatePath();
            duplicatePath.vectorPath.Reverse();
            duplicatePath.path.Reverse();
            return duplicatePath;
        }

        public static IList<Path> SplitPath(this Path pathToSplit, int chunkSizes)
        {
            var splitVectorPath = pathToSplit.vectorPath.Split(chunkSizes);
            var splitNodePath = pathToSplit.path.Split(chunkSizes);
            var chunkedPaths = new List<Path>();
            for (var i = 0; i < splitVectorPath.Count; i++)
            {
                var chunkedPath = pathToSplit.DuplicatePath();
                chunkedPath.vectorPath = splitVectorPath[0];
                chunkedPath.path = splitNodePath[0];
                chunkedPaths.Add(chunkedPath);
            }
            return chunkedPaths;
        }

        public static int FindClosestIndexTo(this Path path, Vector3 position)
        {
            var closestDistance = float.MaxValue;
            var closestIndex = 0;
            for (var i = 0; i < path.vectorPath.Count; i++)
            {
                var distance = Vector3.Distance(position, path.vectorPath[i]);
                if (distance < closestDistance)
                {
                    closestIndex = i;
                    closestDistance = distance;
                }
            }
            return closestIndex;
        }

        public static GraphNode FindClosestNodeTo(this Path path, Vector3 position)
        {
            var closestIndex = FindClosestIndexTo(path, position);
            return path.path[closestIndex];
        }

        public static Vector3 FindClosestPositionTo(this Path path, Vector3 position)
        {
            var closestIndex = FindClosestIndexTo(path, position);
            return path.vectorPath[closestIndex];
        }
    }
}