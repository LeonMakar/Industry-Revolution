using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Global : IInjectable, IService
{
    private bool _houseIsReadyToBeEndPoint;
    public bool CreateRootPointIsActive => _houseIsReadyToBeEndPoint;
    public int Deveation = 0;

    private Structure _houseManipulation;

    public Dictionary<Type, Type> ServiceAndImplamentation { get; } = new Dictionary<Type, Type>()
    {

    };

    public void Inject(params IService[] services)
    {
    }

    public void RootCreatedIsActive() => _houseIsReadyToBeEndPoint = true;
    public void RootCreatedIsUnActive() => _houseIsReadyToBeEndPoint = false;



}

