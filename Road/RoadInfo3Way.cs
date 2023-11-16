using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadInfo3Way : RoadInfo
{
    [SerializeField] private List<Mark> _roadPointsFirst;
    [SerializeField] private List<Mark> _roadPointsSecond;
    [SerializeField] private List<Mark> _roadPointsTherd;
    [SerializeField] private List<Mark> _roadPointsFour;
    [SerializeField] private List<Mark> _roadPointsFifth;
    [SerializeField] private List<Mark> _roadPointsSixth;




    [SerializeField] private Mark _lastMarkFirstSide;
    [SerializeField] private Mark _lastMarkSecondSide;
    [SerializeField] private Mark _lastMarkTherdSide;



    public Vector3Int pointOfFirstSide;
    public Vector3Int pointOfSecondSide;
    public Vector3Int pointOfTherdSide;




    public override void SetLastMarksPosition()
    {
        pointOfFirstSide = new Vector3Int(Mathf.FloorToInt(_lastMarkFirstSide.transform.position.x), 0, Mathf.FloorToInt(_lastMarkFirstSide.transform.position.z));
        pointOfSecondSide = new Vector3Int(Mathf.FloorToInt(_lastMarkSecondSide.transform.position.x), 0, Mathf.FloorToInt(_lastMarkSecondSide.transform.position.z));
        pointOfTherdSide = new Vector3Int(Mathf.FloorToInt(_lastMarkTherdSide.transform.position.x), 0, Mathf.FloorToInt(_lastMarkTherdSide.transform.position.z));
    }

    public override List<Mark> Getpath(Vector3Int from, Vector3Int to)
    {
        if (from == pointOfTherdSide)
        {
            if (to == pointOfFirstSide)
                return _roadPointsFirst;
            else if (to == pointOfSecondSide)
                return _roadPointsSecond;
        }
        else if (from == pointOfFirstSide)
        {
            if (to == pointOfSecondSide)
                return _roadPointsTherd;
            else if (to == pointOfTherdSide)
                return _roadPointsFour;
        }
        else if (from == pointOfSecondSide)
        {
            if (to == pointOfFirstSide)
                return _roadPointsFifth;
            else if (to == pointOfTherdSide)
                return _roadPointsSixth;
        }


        throw new System.Exception($"Vector {from} and {to} dosnt compare if some path(3Way)");
    }

    //public void SetConnectionToRoadInfo()
    //{
    //    SetConnection(this);
    //}
}
