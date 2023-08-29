using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AStarSearch
{
    private List<Vector3Int> _correctNodes = new List<Vector3Int>();

    private Dictionary<Vector3Int, float> _nodesWithVectorDistance = new Dictionary<Vector3Int, float>();
    //private int _perfectDistance;

    public List<Vector3Int> GetNodesForPath(Vector3Int startPoint, Vector3Int endPoint)
    {
        _correctNodes.Clear();
        _correctNodes.Add(startPoint);
        int distance = GetManhattanDistance(startPoint, endPoint);
        //_perfectDistance = distance;
        Vector3Int currentPoint = startPoint;
        _nodesWithVectorDistance.Clear();
        BilderSystem.Instance.DestroyTemporaryRoads();
        while (currentPoint != endPoint)
        {
            Debug.Log("Цикл начат");
            if (currentPoint != endPoint)
            {
                foreach (var node in GetNodeNeighborsOfType(currentPoint))
                {
                    if (!_nodesWithVectorDistance.ContainsKey(node.GetNodePosition))
                        _nodesWithVectorDistance.Add(node.GetNodePosition, GetVectorDistance(node.GetNodePosition, endPoint));                  
                }

                currentPoint = FindClosestNode(_nodesWithVectorDistance);
            }
            _correctNodes.Add(currentPoint);
            if (currentPoint == endPoint)
                break;
        }
        return _correctNodes;
    }

    private Vector3Int FindClosestNode(Dictionary<Vector3Int, float> nodesWithVector)
    {
        Dictionary<Vector3Int, float> vectorValues = new Dictionary<Vector3Int, float>();
        float dictionaryValue = new float();
        Vector3Int dictionaryKey = new Vector3Int();
        foreach (var node in nodesWithVector)
        {
            if (vectorValues.Count == 0)
            {
                vectorValues.Add(node.Key, node.Value);
            }
            else if (vectorValues.Count != 0)
            {
                if (node.Value < dictionaryValue)
                {
                    vectorValues.Remove(dictionaryKey);
                    vectorValues.Add(node.Key, node.Value);
                }
            }
            dictionaryKey = node.Key;
            dictionaryValue = node.Value;
        }
        foreach (var item in vectorValues)
        {
            dictionaryKey = item.Key;
        }
        return dictionaryKey;
    }

    private int GetManhattanDistance(Vector3Int startPoint, Vector3Int endPoint)
    {
        int manhattanDistance = Mathf.Abs(startPoint.x - endPoint.x) + Mathf.Abs(startPoint.z - endPoint.z);
        return manhattanDistance;
    }

    private float GetVectorDistance(Vector3Int startPoint, Vector3Int endPoint)
    {
        float vectorDistance = Mathf.Sqrt(Mathf.Pow(endPoint.x - startPoint.x, 2) + Mathf.Pow(endPoint.z - startPoint.z, 2));
        return vectorDistance;
    }


    private List<NodeData> GetNodeNeighborsOfType(Vector3Int position)
    {
        List<NodeData> nodes = new List<NodeData>();
        foreach (var item in GridSystem.Instance.GetAllNeighborsNearThePointOffSpecificType
            (position.x, position.z, NodeType.Empty))
        {
            nodes.Add(item.Value);
        }
        foreach (var item in GridSystem.Instance.GetAllNeighborsNearThePointOffSpecificType
           (position.x, position.z, NodeType.Road))
        {
            nodes.Add(item.Value);
        }
        return nodes;
    }
}




