using System;
using System.Collections.Generic;
using UnityEngine;

public class AStarSearchForCar : IService
{
    private List<Vector3Int> _correctPoints = new();

    public List<Vector3Int> FindPath(Vector3Int startPoint, Vector3Int endPoint)
    {
        Vector3Int currentPoint = startPoint;
        _correctPoints.Add(currentPoint);
        try
        {
            while (currentPoint != endPoint)
            {
                currentPoint = CalculateClosestNeighbor(currentPoint, endPoint);
                _correctPoints.Add(currentPoint);
            }

        }
        catch (Exception)
        {

            Debug.Log("Возникла ошибка при создания маршрута машины");
        }
        return _correctPoints;
    }

    private Vector3Int CalculateClosestNeighbor(Vector3Int currentPosition, Vector3Int endPosition)
    {
        Dictionary<string, NodeData> positionsWithRoadSelector = GridSystem.Instance.GetAllNeighborsNearThePointOffSpecificType(currentPosition.x, currentPosition.z, NodeType.Road);
        Dictionary<Vector3Int, string> position = new Dictionary<Vector3Int, string>();

        foreach (var item in positionsWithRoadSelector)
        {
            position.Add(item.Value.GetNodePosition, item.Key);
        }

        double smolestVector = Double.MaxValue;
        double vectorCalculation;
        Vector3Int closestNode = new();

        foreach (var item in position)
        {
            vectorCalculation = Math.Sqrt(Math.Pow(Math.Abs(endPosition.x - item.Key.x), 2) + Math.Pow(Math.Abs(endPosition.z - item.Key.z), 2));
            if (vectorCalculation <= smolestVector)
            {
                smolestVector = vectorCalculation;
                closestNode = item.Key;
            }
        }
        return closestNode;
    }
}
