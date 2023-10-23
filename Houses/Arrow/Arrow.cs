using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Vector3 StartPosition;
    public Vector3 EndPosition;
    private bool _arrowIsActive = false;
    [SerializeField] LineRenderer _lineRenderer;

    public void EnableArrow()
    {
        _arrowIsActive = true;
        _lineRenderer.positionCount = 2;
        _lineRenderer.SetPosition(0, StartPosition);
    }

    private void FixedUpdate()
    {
        if (_arrowIsActive)
        {
          
            _lineRenderer.SetPosition(1, EndPosition);
        }
    }
}
