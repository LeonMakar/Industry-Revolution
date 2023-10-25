using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class HouseDisplay : MonoBehaviour, IInjectable, IService, IDragHandler
{
    [SerializeField] private TextMeshProUGUI _textStart;
    [SerializeField] private TextMeshProUGUI _textEndPoint;

    private Vector3Int _startPosition;
    private Vector3Int _endPosition;


    private Global _global;
    private HouseManipilation _currentHouse;
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

    public void RefreshAllInformation(Vector3Int startPosition, HouseManipilation currentHouse)
    {
        _startPosition = startPosition;
        _currentHouse = currentHouse;
        _global.SetEditableHouse(currentHouse);
        _textStart.text = $"Start = {_startPosition}";
        _textEndPoint.text = $"EndPoint = --- ";
        _carAI.DeleteLine();
    }
    public void SetOnDisplayEndPoint(Vector3Int endPoint)
    {
        _textEndPoint.text = $"EndPoint = {endPoint}";
        _carAI.CreatPathGridPoints(_startPosition, endPoint);
    }

    public void DestroyGameObject()
    {
        Destroy(transform.parent.gameObject);
    }
    public void StartAddGoalHouse() => _global.SetHousesAsReadyToBeEndPoint();
    public void OnDrag(PointerEventData eventData) => transform.position = eventData.position;

}
