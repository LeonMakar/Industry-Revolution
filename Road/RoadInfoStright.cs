using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadInfoStright : MonoBehaviour, IRoad
{
    [SerializeField] private List<Mark> _roadPointsUp;
    [SerializeField] private List<Mark> _roadPointsDown;

    [SerializeField] private Transform _transform;

    public Vector3Int pathToUp;
    public Vector3Int pathToDown;


    public void SetDirectionToDiffrentPath()
    {
        pathToUp = new Vector3Int(Mathf.RoundToInt(_transform.position.x), 0, Mathf.RoundToInt(_transform.position.z) + 1);
        pathToDown = new Vector3Int(Mathf.RoundToInt(_transform.position.x), 0, Mathf.RoundToInt(_transform.position.z) - 1);
    }

    public List<Mark> Getpath(Vector3Int from, Vector3Int to)
    {
        if (to == pathToUp)
            return _roadPointsUp;
        else if (to == pathToDown)
            return _roadPointsDown;

        throw new System.Exception($"Vector {from} and {to} dosnt compare if some path(Stright)");

    }
    public void SetConnectionToRoadInfo(RoadInfo roadInfo)
    {
        roadInfo.SetConnection(this);
    }
}
