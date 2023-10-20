using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadInfo4Way : MonoBehaviour, IRoad
{
    [SerializeField] private List<Mark> _roadPointsFirst;
    [SerializeField] private List<Mark> _roadPointsSecond;
    [SerializeField] private List<Mark> _roadPointsTherd;
    [SerializeField] private List<Mark> _roadPointsFour;
    [SerializeField] private List<Mark> _roadPointsFifth;
    [SerializeField] private List<Mark> _roadPointsSixth;
    [SerializeField] private List<Mark> _roadPointsSeventh;
    [SerializeField] private List<Mark> _roadPointsEighth;
    [SerializeField] private List<Mark> _roadPointsNinth;
    [SerializeField] private List<Mark> _roadPointsTenth;
    [SerializeField] private List<Mark> _roadPointsEleventh;
    [SerializeField] private List<Mark> _roadPointsTwelfth;



    [SerializeField] private Mark _lastMarkFirstSide;
    [SerializeField] private Mark _lastMarkSecondSide;
    [SerializeField] private Mark _lastMarkTherdSide;
    [SerializeField] private Mark _lastMarkFourSide;



    public Vector3Int pointOfFirstSide;
    public Vector3Int pointOfSecondSide;
    public Vector3Int pointOfTherdSide;
    public Vector3Int pointOfFourSide;


    public void SetDirectionToDiffrentPath()
    {
        SetDirection(ref pointOfFirstSide, _lastMarkFirstSide);
        SetDirection(ref pointOfSecondSide, _lastMarkSecondSide);
        SetDirection(ref pointOfTherdSide, _lastMarkTherdSide);
        SetDirection(ref pointOfFourSide, _lastMarkFourSide);
    }

    private void SetDirection(ref Vector3Int positionToSet, Mark lastMarkOfPath)
    {
        positionToSet = new Vector3Int(Mathf.FloorToInt(lastMarkOfPath.transform.position.x), 0, Mathf.FloorToInt(lastMarkOfPath.transform.position.z));
    }
    public List<Mark> Getpath(Vector3Int from, Vector3Int to)
    {

        if (from == pointOfFourSide)
        {
            if (to == pointOfFirstSide)
                return _roadPointsFirst;
            else if (to == pointOfSecondSide)
                return _roadPointsSecond;
            else if (to == pointOfTherdSide)
                return _roadPointsTherd;
        }
        else if(from == pointOfFirstSide)
        {
            if (to == pointOfSecondSide)
                return _roadPointsFour;
            else if(to == pointOfTherdSide)
                return _roadPointsFifth;
            else if (to == pointOfFourSide)
                return _roadPointsSixth;
        }
        else if( from == pointOfSecondSide)
        {
            if (to == pointOfTherdSide)
                return _roadPointsSeventh;
            else if (to ==pointOfFourSide)
                return _roadPointsEighth;
            else if(to == pointOfFirstSide)
                return _roadPointsNinth;
        }
        else if(from == pointOfTherdSide)
        {
            if (to == pointOfFourSide)
                return _roadPointsTenth;
            if(to == pointOfFirstSide)
                return _roadPointsEleventh;
            if(to == pointOfSecondSide)
                return _roadPointsTwelfth;
        }

        throw new System.Exception($"Vector {from} and {to} dosnt compare if some path(4Way)");
    }

    public void SetConnectionToRoadInfo(RoadInfo roadInfo)
    {
        roadInfo.SetConnection(this);
    }
}
