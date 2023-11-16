using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoadInfoCurve : RoadInfo
{
    [SerializeField] private List<Mark> _roadPointsFirst;
    [SerializeField] private List<Mark> _roadPointsSecond;
    [SerializeField] private Mark _lastMarkFirstSide;
    [SerializeField] private Mark _lastMarkSecondSide;


    public Vector3Int pathToFirstSide;
    public Vector3Int pathToSecondSide;


    public override void SetLastMarksPosition()
    {
        pathToFirstSide = new Vector3Int(Mathf.FloorToInt(_lastMarkFirstSide.transform.position.x), 0, Mathf.FloorToInt(_lastMarkFirstSide.transform.position.z));
        pathToSecondSide = new Vector3Int(Mathf.FloorToInt(_lastMarkSecondSide.transform.position.x), 0, Mathf.FloorToInt(_lastMarkSecondSide.transform.position.z));
    }

    public override List<Mark> Getpath(Vector3Int from, Vector3Int to)
    {
        if (to == pathToFirstSide)
            return _roadPointsFirst;
        else if (to == pathToSecondSide)
            return _roadPointsSecond;

        throw new System.Exception($"Vector {from} and {to} dosnt compare if some path(Curve)");

    }

    //public void SetConnectionToRoadInfo()
    //{
    //    SetConnection(this);
    //}
}
