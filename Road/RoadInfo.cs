using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class RoadInfo : MonoBehaviour
{
    //public IRoad Road;

    //public void SetConnection(IRoad road)
    //{
    //    Road = road;
    //}
    public abstract List<Mark> Getpath(Vector3Int from, Vector3Int to);
    public abstract void SetLastMarksPosition();
    //public abstract void SetConnectionToRoadInfo();
}
