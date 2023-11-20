using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class HouseDisplay : MonoBehaviour, IInjectable, IService
{
    [SerializeField] private TextMeshProUGUI _textStart;
    [SerializeField] private TextMeshProUGUI _textEndPoint;

    private Vector3Int _startPosition;
    private Vector3Int _endPosition;


    private Global _global;
    private Structure _currentHouse;
    private CarAI _carAI;



    public Dictionary<Type, Type> ServiceAndImplamentation { get; } = new Dictionary<Type, Type>()
    {
        [typeof(Global)] = typeof(Global),
        [typeof(CarAI)] = typeof(CarAI),
    };


    public void Inject(params IService[] services)
    {
        foreach (var service in services)
        {
            switch (service.GetType().Name)
            {
                case nameof(Global):
                    _global = (Global)service;
                    break;
                case nameof(CarAI):
                    _carAI = (CarAI)service;
                    break;
            }
        }
    }

}
