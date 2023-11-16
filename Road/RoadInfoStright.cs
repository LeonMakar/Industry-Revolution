using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadInfoStright : RoadInfo
{
    [SerializeField] private List<Mark> _roadPointsUp;
    [SerializeField] private List<Mark> _roadPointsDown;

    [SerializeField] private Transform _transform;

    public Vector3Int pathToUp;
    public Vector3Int pathToDown;


    public override void SetLastMarksPosition()
    {
        pathToUp = new Vector3Int(Mathf.FloorToInt(_transform.position.x), 0, Mathf.FloorToInt(_transform.position.z) + 1);
        pathToDown = new Vector3Int(Mathf.FloorToInt(_transform.position.x), 0, Mathf.FloorToInt(_transform.position.z) - 1);
    }

    public override List<Mark> Getpath(Vector3Int from, Vector3Int to)
    {
        if (to == pathToUp)
            return _roadPointsUp;
        else if (to == pathToDown)
            return _roadPointsDown;

        throw new System.Exception($"Vector {from} and {to} dosnt compare if some path(Stright)");

    }
    //public void SetConnectionToRoadInfo()
    //{
    //    SetConnection(this);
    //}
}
