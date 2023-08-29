using System.Collections.Generic;
using UnityEngine;

public class MouseIsHoldSignal
{
    private List<Vector3Int> _nodesPositions = new List<Vector3Int>();

    public List<Vector3Int> NodePositions => _nodesPositions;

    public MouseIsHoldSignal(List<Vector3Int> nodesPosition)
    {
        _nodesPositions = nodesPosition;
    }

}



