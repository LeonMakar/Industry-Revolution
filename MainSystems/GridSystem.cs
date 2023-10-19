using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : IMainService
{
    public static GridSystem Instance { get; private set; }

    private int _hight;
    private int _width;
    public int Width => _width;
    public int Hight => _hight;

    public NodeData[,] _nodeDatas;

    public GridSystem(int width, int hight)
    {
        Instance = this;

        _hight = hight;
        _width = width;

        _nodeDatas = new NodeData[_width, _hight];

        for (int i = 0; i < _nodeDatas.GetLength(0); i++)
        {
            for (int j = 0; j < _nodeDatas.GetLength(1); j++)
            {
                _nodeDatas[i, j] = new NodeData(i, j);
            }
        }
    }
    ///<summary>
    /// Returns Left,Right,Up,Down node
    ///</summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public List<NodeData> GetAllNeighborsNearThePoint(int positionX, int positionY)
    {
        List<NodeData> neighbors = new List<NodeData>();
        if (positionX > 0)
            neighbors.Add(_nodeDatas[positionX - 1, positionY]);
        if (positionX + 1 < _width)
            neighbors.Add(_nodeDatas[positionX + 1, positionY]);
        if (positionY + 1 < _hight)
            neighbors.Add(_nodeDatas[positionX, positionY + 1]);
        if (positionY > 0)
            neighbors.Add(_nodeDatas[positionX, positionY - 1]);
        return neighbors;
    }


    ///<summary>
    ///Returns Left,Right,Up and Down nodes wich is prefer type
    ///</summary>
    /// <param name="x">position x</param>
    /// <param name="y">position z</param>
    /// <returns></returns>
    public Dictionary<string, NodeData> GetAllNeighborsNearThePointOffSpecificType(int positionX, int positionY, NodeType type)
    {
        Dictionary<string, NodeData> neighbors = new Dictionary<string, NodeData>();

        if (positionX > 0)
            if (_nodeDatas[positionX - 1, positionY].TypeOfNode == type)
                neighbors.Add("Left", _nodeDatas[positionX - 1, positionY]);
        if (positionX + 1 < _width)
            if (_nodeDatas[positionX + 1, positionY].TypeOfNode == type)
                neighbors.Add("Right", _nodeDatas[positionX + 1, positionY]);
        if (positionY + 1 < _hight)
            if (_nodeDatas[positionX, positionY + 1].TypeOfNode == type)
                neighbors.Add("Up", _nodeDatas[positionX, positionY + 1]);
        if (positionY > 0)
            if (_nodeDatas[positionX, positionY - 1].TypeOfNode == type)
                neighbors.Add("Down", _nodeDatas[positionX, positionY - 1]);
        return neighbors;
    }


    public NodeData this[int i, int j]
    {
        get
        {
            return _nodeDatas[i, j];
        }
        set
        {
            _nodeDatas[i, j] = value;
        }
    }

}
