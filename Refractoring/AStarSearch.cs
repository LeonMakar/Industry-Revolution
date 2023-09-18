using System;
using System.Collections.Generic;
using UnityEngine;

public class AStarSearch
{
    private List<Vector3Int> _correctNodes = new List<Vector3Int>();

    private Dictionary<Vector3Int, float> _nodesWithVectorDistance = new Dictionary<Vector3Int, float>();
    private string _idealDirection = "";
    private bool _startDelay = false;
    public List<Vector3Int> GetNodesForPath(Vector3Int startPoint, Vector3Int endPoint)
    {
        _correctNodes.Clear();
        _correctNodes.Add(startPoint);
        Vector3Int currentPoint = startPoint;

        foreach (var item in CalculateClosestNeighbor(startPoint, endPoint))
        {
            currentPoint = item.Key;
            _idealDirection = item.Value;
            _correctNodes.Add(currentPoint);
        }
        for (int i = 0; i < 70; i++)
        {
            if (currentPoint != endPoint)
            {
                if (_idealDirection == Enum.GetName(typeof(Direction), Direction.Up))
                {
                    Vector3Int temporaryPosition = new Vector3Int(currentPoint.x, 0, currentPoint.z + 1);
                    AddNodeWithCorrectAlgaritmDirectionOnZAxes(ref currentPoint, endPoint, temporaryPosition);
                    if (currentPoint == endPoint)
                        break;
                }
                else if (_idealDirection == Enum.GetName(typeof(Direction), Direction.Down))
                {
                    Vector3Int temporaryPosition = new Vector3Int(currentPoint.x, 0, currentPoint.z - 1);
                    AddNodeWithCorrectAlgaritmDirectionOnZAxes(ref currentPoint, endPoint, temporaryPosition);
                    if (currentPoint == endPoint)
                        break;
                }
                else if (_idealDirection == Enum.GetName(typeof(Direction), Direction.Right))
                {
                    Vector3Int temporaryPosition = new Vector3Int(currentPoint.x + 1, 0, currentPoint.z);
                    AddNodeWithCorrectAlgaritmDirectionOnXAxes(ref currentPoint, endPoint, temporaryPosition);
                    if (currentPoint == endPoint)
                        break;
                }
                else if (_idealDirection == Enum.GetName(typeof(Direction), Direction.Left))
                {

                    Vector3Int temporaryPosition = new Vector3Int(currentPoint.x - 1, 0, currentPoint.z);
                    AddNodeWithCorrectAlgaritmDirectionOnXAxes(ref currentPoint, endPoint, temporaryPosition);
                    if (currentPoint == endPoint)
                        break;
                }
            }
            else
                return _correctNodes;
        }
        return _correctNodes;
    }
    private bool CheckThatNodeIsFree(int x, int z)
    {
        if (GridSystem.Instance[x, z].TypeOfNode == NodeType.Empty || GridSystem.Instance[x, z].TypeOfNode == NodeType.Road)
            return true;
        else
            return false;
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
        if (temporaryPosition != null && temporaryPosition.x <= GridSystem.Instance._nodeDatas.GetLength(0) && temporaryPosition.x <= GridSystem.Instance._nodeDatas.GetLength(1))
            if (GridSystem.Instance[temporaryPosition.x, temporaryPosition.z].TypeOfNode == NodeType.Road
                            || GridSystem.Instance[temporaryPosition.x, temporaryPosition.z].TypeOfNode == NodeType.Empty)
            {
                currentPoint = temporaryPosition;
                _correctNodes.Add(currentPoint);
                Vector3Int checkVector;
                Direction temporaryDirection;

                if (currentPoint.x < endPoint.x)
                {
                    checkVector = new Vector3Int(currentPoint.x + 1, 0, currentPoint.z);
                    temporaryDirection = Direction.Right;
                }
                else
                {
                    checkVector = new Vector3Int(currentPoint.x - 1, 0, currentPoint.z);
                    temporaryDirection = Direction.Left;
                }

                if (currentPoint.z == endPoint.z && currentPoint != endPoint && CheckThatNodeIsFree(checkVector.x, checkVector.z))
                {
                    SetNewDirection(ref currentPoint, endPoint); ;
                }
                else if (currentPoint.z == endPoint.z && currentPoint != endPoint && CheckThatNodeIsFree(checkVector.x, checkVector.z) == false && _startDelay == false)
                {
                    DelayTheChangeOfDirection(ref currentPoint, endPoint, _idealDirection);
                    return;
                }
                if (_startDelay)
                {
                    if (CheckThatNodeIsFree(checkVector.x, checkVector.z))
                    {
                        _idealDirection = Enum.GetName(typeof(Direction), temporaryDirection);
                        _startDelay = false;
                    }
                }
            }
            else
                SetNewDirection(ref currentPoint, endPoint);
    }

    //–еализовать отложенную смену направлени€ движени€, если по направлению смены движени€ есть преп€тсвие.
    //ѕри наличии преп€тсви€ продолжить движение по предыдущей траектории.
    private void DelayTheChangeOfDirection(ref Vector3Int currentPoint, Vector3Int endPoint, string idealDirection)
    {
        //if (idealDirection == Enum.GetName(typeof(Direction), Direction.Right))
        //{
        //    if (GridSystem.Instance[currentPoint.x + 1, currentPoint.z].TypeOfNode == NodeType.Road ||
        //        GridSystem.Instance[currentPoint.x + 1, currentPoint.z].TypeOfNode == NodeType.Empty)
        //    {
        //        currentPoint = new Vector3Int(currentPoint.x + 1, 0, currentPoint.z);
        //        _correctNodes.Add(currentPoint);
        //    }
        //}
        _idealDirection = idealDirection;
        _startDelay = true;
    }
    private void AddNodeWithCorrectAlgaritmDirectionOnXAxes(ref Vector3Int currentPoint, Vector3Int endPoint, Vector3Int temporaryPosition)
    {
        if (temporaryPosition != null && temporaryPosition.x <= GridSystem.Instance._nodeDatas.GetLength(0) && temporaryPosition.x <= GridSystem.Instance._nodeDatas.GetLength(1))
            if ((GridSystem.Instance[temporaryPosition.x, temporaryPosition.z].TypeOfNode == NodeType.Road
                                || GridSystem.Instance[temporaryPosition.x, temporaryPosition.z].TypeOfNode == NodeType.Empty))
            {
                currentPoint = temporaryPosition;
                _correctNodes.Add(currentPoint);

                Vector3Int checkVector;
                Direction temporaryDirection;
                if (currentPoint.z < endPoint.z)
                {
                    checkVector = new Vector3Int(currentPoint.x, 0, currentPoint.z + 1);
                    temporaryDirection = Direction.Up;
                }
                else
                {
                    checkVector = new Vector3Int(currentPoint.x, 0, currentPoint.z - 1);
                    temporaryDirection = Direction.Down;
                }

                if (currentPoint.x == endPoint.x && currentPoint != endPoint && CheckThatNodeIsFree(checkVector.x, checkVector.z))
                {
                    SetNewDirection(ref currentPoint, endPoint);
                }
                else if (currentPoint.x == endPoint.x && currentPoint != endPoint && CheckThatNodeIsFree(checkVector.x, checkVector.z) == false)
                    _startDelay = true;
                if (_startDelay)
                {
                    if (CheckThatNodeIsFree(checkVector.x, checkVector.z))
                    {
                        _idealDirection = Enum.GetName(typeof(Direction), temporaryDirection);
                        _startDelay = false;
                    }
                }

                //else if (currentPoint.x == endPoint.x && currentPoint != endPoint && CheckThatNodeIsFree(checkVector.x, checkVector.z) == false && _startDelay == false)
                //{
                //    DelayTheChangeOfDirection(ref currentPoint, endPoint, _idealDirection);
                //    return;
                //}
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
            _idealDirection = item.Value;
            if (!_correctNodes.Contains(currentPoint))
                _correctNodes.Add(currentPoint);
        }
        _startDelay = false;
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




