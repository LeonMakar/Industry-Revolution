using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Field : MonoBehaviour, IInjectable
{
    [SerializeField] private Size _size;

    private GridSystem _grid;

    public abstract NodeType NodeType { get; }


    public Dictionary<Type, Type> ServiceAndImplamentation { get; } = new Dictionary<Type, Type>()
    {
        [typeof(GridSystem)] = typeof(GridSystem),
    };

    public void Inject(params IService[] services)
    {
        foreach (var service in services)
        {
            switch (service.GetType().Name)
            {
                case nameof(GridSystem):
                    _grid = (GridSystem)service;
                    break;
            }
        }
    }

    private void Start()
    {
        this.Injecting();

        SetNodeType();
    }

    public void SetNodeType()
    {
        for (int i = 0; i < _size.x; i++)
        {
            for (int y = 0; y < _size.y; y++)
            {
                _grid[Mathf.FloorToInt(transform.position.x + i), Mathf.FloorToInt(transform.position.z + y)].MakeNodeSetup(NodeType);
            }
        }
    }


}

