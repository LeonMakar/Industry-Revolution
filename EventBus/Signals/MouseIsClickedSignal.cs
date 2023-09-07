using System;
using UnityEngine;

public class MouseIsClickedSignal
{
    public readonly Vector3Int position;
    public MouseIsClickedSignal(Vector3Int? vector3Int)
    {
        try
        {
            position = new Vector3Int(vector3Int.Value.x,0,vector3Int.Value.z);
        }
        catch (System.Exception)
        {
            Debug.Log("Sended signal is null");
            throw new NullReferenceException();
        }
    }
}
