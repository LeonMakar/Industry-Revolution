using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class AStarSearch
{
    private List<Vector3Int> _correctNodes = new List<Vector3Int>();

    private Dictionary<Vector3Int, float> _nodesWithVectorDistance = new Dictionary<Vector3Int, float>();
    string idealDirection = "";
    public List<Vector3Int> GetNodesForPath(Vector3Int startPoint, Vector3Int endPoint)
    {
        _correctNodes.Clear();
        _correctNodes.Add(startPoint);
        Vector3Int currentPoint = startPoint;

        foreach (var item in CalculateClosestNeighbor(startPoint, endPoint))
        {
            currentPoint = item.Key;
            idealDirection = item.Value;
            _correctNodes.Add(currentPoint);
        }
        for (int i = 0; i < 15; i++)
        {
            if (currentPoint != endPoint)
            {

                if (idealDirection == Enum.GetName(typeof(Direction), Direction.Up))
                {
                    if (currentPoint.z != endPoint.z)
                    {
                        Vector3Int temporaryPosition = new Vector3Int(currentPoint.x, 0, currentPoint.z + 1);
                        AddNodeWithCorrectAlgaritmDirectionOnZAxes(ref currentPoint, endPoint, temporaryPosition);
                        if (currentPoint == endPoint)
                            break;
                    }
                    else
                        SetNewDirection(ref currentPoint, endPoint);
                }
                else if (idealDirection == Enum.GetName(typeof(Direction), Direction.Down))
                {
                    if (currentPoint.z != endPoint.z)
                    {
                        Vector3Int temporaryPosition = new Vector3Int(currentPoint.x, 0, currentPoint.z - 1);
                        AddNodeWithCorrectAlgaritmDirectionOnZAxes(ref currentPoint, endPoint, temporaryPosition);
                        if (currentPoint == endPoint)
                            break;
                    }
                    else
                        SetNewDirection(ref currentPoint, endPoint);
                }
                else if (idealDirection == Enum.GetName(typeof(Direction), Direction.Right))
                {
                    if (currentPoint.x != endPoint.x)
                    {
                        Vector3Int temporaryPosition = new Vector3Int(currentPoint.x + 1, 0, currentPoint.z);
                        AddNodeWithCorrectAlgaritmDirectionOnXAxes(ref currentPoint, endPoint, temporaryPosition);
                        if (currentPoint == endPoint)
                            break;
                    }
                    else
                        SetNewDirection(ref currentPoint, endPoint);
                }
                else if (idealDirection == Enum.GetName(typeof(Direction), Direction.Left))
                {
                    if (currentPoint.x != endPoint.x)
                    {
                        Vector3Int temporaryPosition = new Vector3Int(currentPoint.x - 1, 0, currentPoint.z);
                        AddNodeWithCorrectAlgaritmDirectionOnXAxes(ref currentPoint, endPoint, temporaryPosition);
                        if (currentPoint == endPoint)
                            break;
                    }
                    else
                        SetNewDirection(ref currentPoint, endPoint);
                }
            }
            else
                return _correctNodes;
        }
        return _correctNodes;
    }
    public enum Direction
    {
        Up = 0,
        Down = 1,
        Left = 2,
        Right = 3,
    }
    private void AddNodeWithCorrectAlgaritmDirectionOnZAxes(ref Vector3Int currentPoint, Vector3Int endPoint, Vector3Int temporaryPosition)
    {
        if (currentPoint == endPoint)
            return;
        if (GridSystem.Instance[temporaryPosition.x, temporaryPosition.z].TypeOfNode == NodeType.Road
                            || GridSystem.Instance[temporaryPosition.x, temporaryPosition.z].TypeOfNode == NodeType.Empty)
        {
            currentPoint = temporaryPosition;
            _correctNodes.Add(currentPoint);

            if (currentPoint.z == endPoint.z && currentPoint != endPoint)
                SetNewDirection(ref currentPoint, endPoint);
        }
        else
            SetNewDirection(ref currentPoint, endPoint);
    }
    private void AddNodeWithCorrectAlgaritmDirectionOnXAxes(ref Vector3Int currentPoint, Vector3Int endPoint, Vector3Int temporaryPosition)
    {
        if ((GridSystem.Instance[temporaryPosition.x, temporaryPosition.z].TypeOfNode == NodeType.Road
                            || GridSystem.Instance[temporaryPosition.x, temporaryPosition.z].TypeOfNode == NodeType.Empty))
        {
            currentPoint = temporaryPosition;
            _correctNodes.Add(currentPoint);
            if (currentPoint.x == endPoint.x && currentPoint != endPoint)
                SetNewDirection(ref currentPoint, endPoint);
        }
        else
            SetNewDirection(ref currentPoint, endPoint);
    }
    private void SetNewDirection(ref Vector3Int currentPoint, Vector3Int endPoint)
    {
        Dictionary<Vector3Int, string> closestPoint = CalculateClosestNeighbor(currentPoint, endPoint);
        foreach (var item in closestPoint)
        {
            currentPoint = item.Key;
            idealDirection = item.Value;
            _correctNodes.Add(currentPoint);
        }
    }
    private Dictionary<Vector3Int, string> CalculateClosestNeighbor(Vector3Int currentPosition, Vector3Int endPosition)
    {
        Dictionary<string, NodeData> positionsWithRoadSelector = GridSystem.Instance.GetAllNeighborsNearThePointOffSpecificType(currentPosition.x, currentPosition.z, NodeType.Road);
        Dictionary<string, NodeData> positionsWithEmptySelector = GridSystem.Instance.GetAllNeighborsNearThePointOffSpecificType(currentPosition.x, currentPosition.z, NodeType.Empty);
        Dictionary<Vector3Int, string> position = new Dictionary<Vector3Int, string>();

        foreach (var item in positionsWithEmptySelector)
        {
            position.Add(item.Value.GetNodePosition, item.Key);
        }
        foreach (var item in positionsWithRoadSelector)
        {
            position.Add(item.Value.GetNodePosition, item.Key);
        }

        double smolestVector = Double.MaxValue;
        double vectorCalculation;
        Dictionary<Vector3Int, string> closestNode = new Dictionary<Vector3Int, string>();

        foreach (var item in position)
        {
            vectorCalculation = Math.Sqrt(Math.Pow(Math.Abs(endPosition.x - item.Key.x), 2) + Math.Pow(Math.Abs(endPosition.z - item.Key.z), 2));
            if (vectorCalculation <= smolestVector)
            {
                smolestVector = vectorCalculation;
                closestNode.Clear();
                closestNode.Add(item.Key, item.Value);
            }
        }
        return closestNode;
    }

    private int GetManhattanDistance(Vector3Int startPoint, Vector3Int endPoint)
    {
        int manhattanDistance = Mathf.Abs(startPoint.x - endPoint.x) + Mathf.Abs(startPoint.z - endPoint.z);
        return manhattanDistance;
    }
}




