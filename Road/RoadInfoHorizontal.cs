using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadInfoHorizontal : RoadInfo
{
    [SerializeField] private List<Mark> _roadPointsLeft;
    [SerializeField] private List<Mark> _roadPointsRight;

    [SerializeField] private Transform _transform;

    public Vector3Int pathToLeft;
    public Vector3Int pathToRight;


    public override void SetLastMarksPosition()
    {
        pathToLeft = new Vector3Int(Mathf.FloorToInt(_transform.position.x) - 1, 0, Mathf.FloorToInt(_transform.position.z) );
        pathToRight = new Vector3Int(Mathf.FloorToInt(_transform.position.x) + 1, 0, Mathf.FloorToInt(_transform.position.z) );
    }

    public  override List<Mark> Getpath(Vector3Int from, Vector3Int to)
    {
        if (to == pathToLeft)
            return _roadPointsLeft;
        else if (to == pathToRight)
            return _roadPointsRight;

        throw new System.Exception($"Vector {from} and {to} dosnt compare if some path (horizontal)");
    }

    //public void SetConnectionToRoadInfo()
    //{
    //   SetConnection(this);
    //}
}
