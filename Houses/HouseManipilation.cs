using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HouseManipilation : MonoBehaviour, IInjectable
{
    private HouseDisplay _display;
    private Global _global;
    private Factory _canvasBuilder;

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
        [typeof(Factory)] = typeof(CanvasFactory),

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
                case nameof(CanvasFactory):
                    _canvasBuilder = (CanvasFactory)service;
                    break;
            }
        }
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
        if (_isPlaced && !_global.HouseIsReadyToBeEndPoint && Cursor.CursorIsEmpty)
        {
            _display = _canvasBuilder.Bild(BildingType.CanvasHouse).GetComponentInChildren<HouseDisplay>();
            if (_display != null)
            {
                _display.transform.position =
                    new Vector3(_display.transform.position.x + _global.Deveation, _display.transform.position.y, _display.transform.position.z);
                _display.Injecting();
                _display.RefreshAllInformation(CurrentPosition, this);
                _global.Deveation += 100;
            }
            else
                Debug.Log("Нет дисплея ");
        }
        if (_isPlaced && _global.HouseIsReadyToBeEndPoint)
        {
            _global.SetNewLastBuilding(CurrentPosition);
            _global.SetHousesAsUnreadyToBeEndPoint();
        }
    }

}
