using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CarAI : IInjectable
{
    private List<Vector3Int> _pathGridPoints = new();
    private List<Mark> _pathNavigationPoints = new();

    private Vector3Int _cuurentGridPoint = new Vector3Int();

    private AStarSearchForCar _aStar;

    public Dictionary<Type, Type> ServiceAndImplamentation { get; } = new Dictionary<Type, Type>()
    {
        [typeof(AStarSearchForCar)] = typeof(AStarSearchForCar),
    };
    public void Inject(params IService[] services)
    {
        foreach (var service in services)
        {
            Debug.Log(service.GetType().Name);
            switch (service.GetType().Name)
            {
                case nameof(AStarSearchForCar):
                   var findService = services.Where(t=>t.GetType()==typeof(AStarSearchForCar));
                    _aStar = (AStarSearchForCar)findService.First();
                    break;
            }
        }
    }

    public void CreatPathGridPoints(Vector3Int startPoint, Vector3Int endPoint)
    {

    }

}
