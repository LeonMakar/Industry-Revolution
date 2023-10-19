using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadInfoHorizontal : MonoBehaviour
{
    [SerializeField] private List<Mark> _roadPointsLeft;
    [SerializeField] private List<Mark> _roadPointsRight;

    [SerializeField] private Transform _transform;

    public Vector3Int pathToLeft;
    public Vector3Int pathToRight;


    public void SetDirectionToDiffrentPath()
    {
        pathToLeft = new Vector3Int(Mathf.RoundToInt(_transform.position.x)-1, 0, Mathf.RoundToInt(_transform.position.z));
        pathToRight = new Vector3Int(Mathf.RoundToInt(_transform.position.x)+1, 0, Mathf.RoundToInt(_transform.position.z));
    }
}
