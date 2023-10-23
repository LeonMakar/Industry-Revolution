using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HouseManipilation : MonoBehaviour, IInjectable
{
    private HouseDisplay _display;
    private Global _global;

    [SerializeField] private GameObject _housePositionForCar;

    public Vector3Int CurrentPosition;
    public Vector3Int LastPositionForAi
    {
        set => _display.SetOnDisplayEndPoint(value);
    }

    private Dictionary<int, List<Mark>> _paths;
    private bool _isPlaced = false;


    public Dictionary<Type, Type> ServiceAndImplamentation { get; } = new Dictionary<Type, Type>
    {
        [typeof(Global)] = typeof(Global),
        [typeof(HouseDisplay)] = typeof(HouseDisplay),

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
                case nameof(HouseDisplay):
                    _display = (HouseDisplay)service;
                    break;
            }
        }
    }
    private void Start()
    {
        this.Injecting();
    }

    public void SetHouseOnGround()
    {
        CurrentPosition = new Vector3Int(Mathf.FloorToInt(_housePositionForCar.transform.position.x), 0, Mathf.FloorToInt(_housePositionForCar.transform.position.z));
        _housePositionForCar.SetActive(false);
        _isPlaced = true;
    }


    // Ui Displaing
    private void OnMouseDown()
    {
        if (_isPlaced && !_global.HouseIsReadyToBeEndPoint)
        {
            _display.gameObject.SetActive(true);
            _display.RefreshAllInformation(CurrentPosition, this);
        }
        if (_global.HouseIsReadyToBeEndPoint)
        {
            _global.SetNewLastBuilding(CurrentPosition);
            _global.SetHousesAsUnreadyToBeEndPoint();
        }
    }

}
