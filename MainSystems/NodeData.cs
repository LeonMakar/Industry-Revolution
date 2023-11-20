using TMPro;
using UnityEngine;

public class NodeData
{
    private NodeType _nodeType = new NodeType();
    private Vector3Int _position;


    public NodeData(int x, int z)
    {
        _position = new Vector3Int(x, 0, z);
    }

    public Megapolis City;

    public NodeType TypeOfNode => _nodeType;
    public Vector3Int GetNodePosition => _position;

    public void SetCityForThisNode(Megapolis city)
    {
        City = city;
    }

    public void MakeNodeSetup(NodeType type)
    {
        SetNodeType(type);
    }

    private void SetNodeType(NodeType nodeType)
    {
        _nodeType = nodeType;
    }
}


public enum NodeType
{
    Empty = 0,
    Road = 1,
    Bilding = 2,
    Obstacle = 3,
    CoalMine = 4,
}