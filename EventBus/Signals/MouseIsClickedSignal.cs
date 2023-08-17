using System;
using UnityEngine;

public class MouseIsClickedSignal
{
    public readonly Vector3Int position;
    public MouseIsClickedSignal(Vector3Int? vector3Int)
    {
        try
        {
            position = vector3Int.Value;
        }
        catch (System.Exception)
        {
            Debug.Log("Sended signal is null");
            throw new NullReferenceException();
        }
    }
}
