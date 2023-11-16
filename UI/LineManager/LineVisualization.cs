using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineVisualization : MonoBehaviour
{
    [SerializeField] LineRenderer _lineRenderer;
    

    public void DrowLine(int numberOfPoints, List<Mark> pathNavigationPoints)
    {
        _lineRenderer.positionCount = numberOfPoints;
        for (int i = 0; i < _lineRenderer.positionCount; i++)
        {
            _lineRenderer.SetPosition(i, 
                new Vector3(pathNavigationPoints[i].transform.position.x, 0.1f, pathNavigationPoints[i].transform.position.z));
        }
    }

    public void DeleteLine()
    {
        _lineRenderer.positionCount = 0;
    }
}
