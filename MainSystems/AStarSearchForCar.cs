using PolyPerfect.City;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class AStarSearchForCar : IService
{
    public class PathNode
    {
        // Координаты точки на карте.
        public Vector3Int Position { get; set; }
        // Длина пути от старта (G).
        public int PathLengthFromStart { get; set; }
        // Точка, из которой пришли в эту точку.
        public PathNode CameFrom { get; set; }
        // Примерное расстояние до цели (H).
        public int HeuristicEstimatePathLength { get; set; }
        // Ожидаемое полное расстояние до цели (F).
        public int EstimateFullPathLength
        {
            get
            {
                return this.PathLengthFromStart + this.HeuristicEstimatePathLength;
            }
        }
    }
    public List<Vector3Int> FindPath(Vector3Int startPoint, Vector3Int endPoint)
    {
        // Шаг 1.
        var closedSet = new Collection<PathNode>();
        var openSet = new Collection<PathNode>();
        // Шаг 2.
        PathNode startNode = new PathNode()
        {
            Position = startPoint,
            CameFrom = null,
            PathLengthFromStart = 0,
            HeuristicEstimatePathLength = GetHeuristicPathLength(startPoint, endPoint)
        };
        openSet.Add(startNode);
        while (openSet.Count > 0)
        {
            // Шаг 3.
            var currentNode = openSet.OrderBy(node =>
              node.EstimateFullPathLength).First();
            // Шаг 4.
            if (currentNode.Position == endPoint)
                return GetPathForNode(currentNode);
            // Шаг 5.
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);
            // Шаг 6.
            foreach (var neighbourNode in CalculateNeighborsRoad(currentNode, endPoint))
            {
                // Шаг 7.
                if (closedSet.Count(node => node.Position == neighbourNode.Position) > 0)
                    continue;
                var openNode = openSet.FirstOrDefault(node =>
                  node.Position == neighbourNode.Position);
                // Шаг 8.
                if (openNode == null)
                    openSet.Add(neighbourNode);
                else
                  if (openNode.PathLengthFromStart > neighbourNode.PathLengthFromStart)
                {
                    // Шаг 9.
                    openNode.CameFrom = currentNode;
                    openNode.PathLengthFromStart = neighbourNode.PathLengthFromStart;
                }
            }
        }
        // Шаг 10.
        return null;
    }
    private List<Vector3Int> GetPathForNode(PathNode pathNode)
    {
        var result = new List<Vector3Int>();
        var currentNode = pathNode;
        while (currentNode != null)
        {
            result.Add(currentNode.Position);
            currentNode = currentNode.CameFrom;
        }
        result.Reverse();
        return result;
    }

    private int GetHeuristicPathLength(Vector3Int from, Vector3Int to)
    {
        return Math.Abs(from.x - to.x) + Math.Abs(from.z - to.z);
    }

    private List<PathNode> CalculateNeighborsRoad(PathNode currentNode, Vector3Int finish)
    {
        List<PathNode> intersectionPoints = new ();
        Dictionary<string, NodeData> positionsWithRoadSelector = GridSystem.Instance.GetAllNeighborsNearThePointOffSpecificType(currentNode.Position.x, currentNode.Position.z, NodeType.Road);
        foreach (var item in positionsWithRoadSelector)
        {
            var neighbor = new PathNode()
            {
                Position = item.Value.GetNodePosition,
                CameFrom = currentNode,
                PathLengthFromStart = currentNode.PathLengthFromStart + 1,
                HeuristicEstimatePathLength = GetHeuristicPathLength(item.Value.GetNodePosition, finish)
            };
            intersectionPoints.Add(neighbor);
        }
        return intersectionPoints;
    }
}
