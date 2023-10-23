using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CarAI : MonoBehaviour, IInjectable, IService
{
    private List<Vector3Int> _pathGridPoints = new();
    private List<Mark> _pathNavigationPoints = new();

    private Vector3Int _curentGridPoint = new Vector3Int();
    private Vector3Int _lastGridPoint = new Vector3Int();

    private AStarSearchForCar _aStar;
    private BilderSystem _bilderSystem;


    private int _lastMarkIndex;
    private Mark _markToMoove;

    public LineRenderer LineRenderer;
    public Dictionary<Type, Type> ServiceAndImplamentation { get; } = new Dictionary<Type, Type>()
    {
        [typeof(AStarSearchForCar)] = typeof(AStarSearchForCar),
        [typeof(BilderSystem)] = typeof(BilderSystem),
    };
    public void Inject(params IService[] services)
    {
        foreach (var service in services)
        {
            switch (service.GetType().Name)
            {
                case nameof(AStarSearchForCar):
                    _aStar = (AStarSearchForCar)service;
                    break;
                case nameof(BilderSystem):
                    _bilderSystem = (BilderSystem)service;
                    break;
            }
        }
    }
    private void Start()
    {
        //this.Injecting();
    }

    [ContextMenu("Debug")]
    public List<Mark> CreatPathGridPoints(Vector3Int from, Vector3Int to)
    {
        _pathGridPoints.Clear();
        _pathNavigationPoints.Clear();
        _pathGridPoints = _aStar.FindPath(from, to);
        CreatNavigationPoints();
        gameObject.transform.position = new Vector3(_pathNavigationPoints[0].transform.position.x, 0.15f, _pathNavigationPoints[0].transform.position.x);
        _lastMarkIndex = 1;
        _markToMoove = _pathNavigationPoints[_lastMarkIndex];
        return _pathNavigationPoints;
    }

    public void CreatNavigationPoints()
    {
        _curentGridPoint = _pathGridPoints.First();
        _lastGridPoint = _curentGridPoint;

        for (int i = 1; i < _pathGridPoints.Count; i++)
        {
            _bilderSystem.AllRoads[_curentGridPoint].TryGetComponent<RoadInfo>(out RoadInfo roadInfo);
            AddNavigationPoints(roadInfo.Road.Getpath(_lastGridPoint, _pathGridPoints[i]));
            _curentGridPoint = _pathGridPoints[i];
            _lastGridPoint = _pathGridPoints[i - 1];
            if (_curentGridPoint == _pathGridPoints.Last())
                break;
        }
        LineRenderer.positionCount = _pathNavigationPoints.Count;
        for (int i = 0; i < LineRenderer.positionCount; i++)
        {
            LineRenderer.SetPosition(i, new Vector3(_pathNavigationPoints[i].transform.position.x, 0.1f, _pathNavigationPoints[i].transform.position.z));
        }
    }
    public void DeleteLine()
    {
        LineRenderer.positionCount = 0;
    }
    public void AddNavigationPoints(List<Mark> marks)
    {
        foreach (var mark in marks)
        {
            _pathNavigationPoints.Add(mark);
        }
    }
    //public void StartMooving()
    //{
    //    Debug.Log("Скорость= " + _rigidbody.velocity.magnitude);
    //    if (_markToMoove != null)
    //    {
    //        if (_lastMarkIndex != _pathNavigationPoints.Count)
    //        {
    //            Vector3 direction = _markToMoove.transform.position - gameObject.transform.position;
    //            //Debug.Log(direction.magnitude + " Расстояние до точки");

    //            if (direction.magnitude < 0.35f)
    //            {
    //                if (_pathNavigationPoints.Count - 1 > _lastMarkIndex)
    //                {
    //                    _lastMarkIndex++;
    //                    _markToMoove = _pathNavigationPoints[_lastMarkIndex];
    //                    direction = _markToMoove.transform.position - gameObject.transform.position;
    //                }
    //                else
    //                {
    //                    Debug.Log("Доехал");
    //                    _markToMoove = null;
    //                }
    //            }
    //            direction.Normalize();
    //            _rigidbody.velocity = direction * 2;

    //        }

    //    }
    //}

}
