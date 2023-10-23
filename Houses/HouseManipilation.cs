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


    private bool _isPlaced = false;


    public Dictionary<Type, Type> ServiceAndImplamentation { get; } = new Dictionary<Type, Type>
    {
        [typeof(Global)] = typeof(Global),
        [typeof(HouseDisplay)] = typeof(HouseDisplay),

    };

    public void Inject(params IService[] services)
    {
        IEnumerable<IService> findService;
        foreach (var service in services)
        {
            switch (service.GetType().Name)
            {
                case nameof(Global):
                    findService = services.Where(t => t.GetType() == typeof(Global));
                    _global = (Global)findService.First();
                    break;
                case nameof(HouseDisplay):
                    findService = services.Where(t => t.GetType() == typeof(HouseDisplay));
                    _display = (HouseDisplay)findService.First();
                    break;
            }
        }
    }
    private void Start()
    {
        Injector.Instance.Init(this);
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
