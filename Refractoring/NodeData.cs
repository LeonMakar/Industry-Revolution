using TMPro;
using UnityEngine;

public class NodeData
{
    private NodeType _nodeType = new NodeType();
    private Vector3Int _position;


    public Megapolis City;

    public NodeType TypeOfNode => _nodeType;
    public Vector3Int GetNodePosition => _position;

    public void SetCityForThisNode(Megapolis city)
    {
        City = city;
    }

    public void MakeNodeSetup(NodeType type, Vector3Int position)
    {
        SetNodeType(type);
        SetNodePosition(position);
    }

    private void SetNodeType(NodeType nodeType)
    {
        _nodeType = nodeType;
    }
    private void SetNodePosition(Vector3Int position)
    {
        _position = position;
    }
}


public enum NodeType
{
    Empty = 0,
    Road = 1,
    Bilding = 2,
}