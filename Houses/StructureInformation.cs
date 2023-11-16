using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StructureInformation : MonoBehaviour, IInjectable
{
    private HouseDisplay _display;
    private Global _global;
    private EventBus _eventBus;

    [SerializeField] private GameObject _housePositionForCar;

    public Vector3Int CurrentPosition;

    private Dictionary<int, List<Mark>> _paths;
    private bool _isPlaced = false;


    public Dictionary<Type, Type> ServiceAndImplamentation { get; } = new Dictionary<Type, Type>
    {
        [typeof(Global)] = typeof(Global),
        [typeof(EventBus)] = typeof(EventBus),

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
                case nameof(EventBus):
                    _eventBus = (EventBus)service;
                    break;
            }
        }
    }

    public void SetHouseOnGround()
    {
        CurrentPosition = new Vector3Int(Mathf.FloorToInt(_housePositionForCar.transform.position.x), 0, Mathf.FloorToInt(_housePositionForCar.transform.position.z));
        _housePositionForCar.SetActive(false);
        _isPlaced = true;
        IInjectable init = this;
        init.Injecting();
    }


    // Ui Displaing
    private void OnMouseDown()
    {
        if (_isPlaced && _global.CreateRootPointIsActive && Cursor.CursorIsEmpty)
        {
            _eventBus.Invoke(new StructureAddedForRootSignal(this));
        }
        if (_isPlaced && _global.CreateRootPointIsActive)
        {
        }
    }

}
