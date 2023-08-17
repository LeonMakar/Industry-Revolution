using System;
using UnityEngine;

public class MousePositionSignal
{
     public readonly Vector3Int positionVector3int;

    public MousePositionSignal(Vector3Int? position)
    {
        if (positionVector3int != null)
            positionVector3int = position.Value;
        else
        {
            Debug.Log("position is null");
            throw new NullReferenceException();
        }
    }
}


