using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadInfo3Way : MonoBehaviour
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
    [SerializeField] private Mark _lastMarkFourSide;
    [SerializeField] private Mark _lastMarkFifthSide;
    [SerializeField] private Mark _lastMarkSixthSide;



    public Vector3Int pathToFirstSide;
    public Vector3Int pathToSecondSide;
    public Vector3Int pathToTherdSide;
    public Vector3Int pathToFourSide;
    public Vector3Int pathToFifthSide;
    public Vector3Int pathToSixthSide;



    public void SetDirectionToDiffrentPath()
    {
        pathToFirstSide = new Vector3Int(Mathf.FloorToInt(_lastMarkFirstSide.transform.position.x), 0, Mathf.FloorToInt(_lastMarkFirstSide.transform.position.z));
        pathToSecondSide = new Vector3Int(Mathf.FloorToInt(_lastMarkSecondSide.transform.position.x), 0, Mathf.FloorToInt(_lastMarkSecondSide.transform.position.z));
        pathToTherdSide = new Vector3Int(Mathf.FloorToInt(_lastMarkTherdSide.transform.position.x), 0, Mathf.FloorToInt(_lastMarkTherdSide.transform.position.z));
        pathToFourSide = new Vector3Int(Mathf.FloorToInt(_lastMarkFourSide.transform.position.x), 0, Mathf.FloorToInt(_lastMarkFourSide.transform.position.z));
        pathToFifthSide = new Vector3Int(Mathf.FloorToInt(_lastMarkFifthSide.transform.position.x), 0, Mathf.FloorToInt(_lastMarkFifthSide.transform.position.z));
        pathToSixthSide = new Vector3Int(Mathf.FloorToInt(_lastMarkSixthSide.transform.position.x), 0, Mathf.FloorToInt(_lastMarkSixthSide.transform.position.z));
    }
}
