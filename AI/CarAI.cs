using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CarAI : MonoBehaviour, IInjectable
{
    private List<Vector3Int> _pathGridPoints = new();
    private List<Mark> _pathNavigationPoints = new();

    private Vector3Int _curentGridPoint = new Vector3Int();

    private AStarSearchForCar _aStar;
    private BilderSystem _bilderSystem;

    public Vector3Int From;
    public Vector3Int To;
    public LineRenderer LineRenderer;
    public Dictionary<Type, Type> ServiceAndImplamentation { get; } = new Dictionary<Type, Type>()
    {
        [typeof(AStarSearchForCar)] = typeof(AStarSearchForCar),
        [typeof(BilderSystem)] = typeof(BilderSystem),
    };
    public void Inject(params IService[] services)
    {
        IEnumerable<IService> findService;
        foreach (var service in services)
        {
            switch (service.GetType().Name)
            {
                case nameof(AStarSearchForCar):
                    findService = services.Where(t => t.GetType() == typeof(AStarSearchForCar));
                    _aStar = (AStarSearchForCar)findService.First();
                    break;
                case nameof(BilderSystem):
                    findService = services.Where(t => t.GetType() == typeof(BilderSystem));
                    _bilderSystem = (BilderSystem)findService.First();
                    Debug.Log(_bilderSystem.GetType().Name);
                    break;
            }
        }
    }
    private void Start()
    {
        Injector.Instance.Init(this);
    }

    [ContextMenu("Debug")]
    public void CreatPathGridPoints()
    {
        _pathGridPoints.Clear();
        _pathNavigationPoints.Clear();
        _pathGridPoints = _aStar.FindPath(From, To);
        Debug.Log("Точки");
        foreach (var item in _pathGridPoints)
        {
            Debug.Log(item);
        }
        CreatNavigationPoints();
    }

    public void CreatNavigationPoints()
    {
        _curentGridPoint = _pathGridPoints.First();
        for (int i = 1; i < _pathGridPoints.Count; i++)
        {
            _bilderSystem.AllRoads[_curentGridPoint].TryGetComponent<RoadInfo>(out RoadInfo roadInfo);
            AddNavigationPoints(roadInfo.Road.Getpath(_curentGridPoint, _pathGridPoints[i]));
            _curentGridPoint = _pathGridPoints[i];
            if (_curentGridPoint == _pathGridPoints.Last())
                break;
        }
        Debug.Log("Навигационные точки" + _pathNavigationPoints.Count);
        foreach (var item in _pathNavigationPoints)
        {
            Debug.Log(item.transform.position);
        }
        LineRenderer.positionCount = _pathNavigationPoints.Count;
        for (int i = 0; i < LineRenderer.positionCount; i++)
        {
            LineRenderer.SetPosition(i, _pathNavigationPoints[i].transform.position);
        }
    }
    public void AddNavigationPoints(List<Mark> marks)
    {
        foreach (var mark in marks)
        {
            _pathNavigationPoints.Add(mark);
        }
    }
}
