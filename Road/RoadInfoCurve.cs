using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoadInfoCurve : MonoBehaviour
{
    [SerializeField] private List<Mark> _roadPointsFirst;
    [SerializeField] private List<Mark> _roadPointsSecond;
    [SerializeField] private Mark _lastMarkFirstSide;
    [SerializeField] private Mark _lastMarkSecondSide;


    public Vector3Int pathToFirstSide;
    public Vector3Int pathToSecondSide;


    public void SetDirectionToDiffrentPath()
    {
        pathToFirstSide = new Vector3Int(Mathf.FloorToInt(_lastMarkFirstSide.transform.position.x), 0, Mathf.FloorToInt(_lastMarkFirstSide.transform.position.z));
        pathToSecondSide = new Vector3Int(Mathf.FloorToInt(_lastMarkSecondSide.transform.position.x), 0, Mathf.FloorToInt(_lastMarkSecondSide.transform.position.z));
    }
}
