using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Global : IInjectable, IService
{
    private bool _houseIsReadyToBeEndPoint;
    public bool HouseIsReadyToBeEndPoint => _houseIsReadyToBeEndPoint;
    public int Deveation = 0;

    private HouseManipilation _houseManipulation;

    public Dictionary<Type, Type> ServiceAndImplamentation { get; } = new Dictionary<Type, Type>()
    {

    };

    public void Inject(params IService[] services)
    {
    }
    public void Test()
    {
        Debug.Log("Åóûå Üôûûôïó");
    }
    public void SetEditableHouse(HouseManipilation house)
    {
        _houseManipulation = house;
    }
    public void SetNewLastBuilding(Vector3Int housePositionForCar)
    {
        _houseManipulation.LastPositionForAi = housePositionForCar;
    }
    public void SetHousesAsReadyToBeEndPoint() => _houseIsReadyToBeEndPoint = true;
    public void SetHousesAsUnreadyToBeEndPoint() => _houseIsReadyToBeEndPoint = false;

}

