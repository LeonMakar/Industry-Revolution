using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Structure : MonoBehaviour, IInjectable
{
    [SerializeField] private GameObject _housePositionForCar;

    public Vector3Int CurrentPosition;

    protected bool _isPlaced = false;


    protected Global _global;
    protected EventBus _eventBus;
    protected GridSystem _gridSystem;


    public Dictionary<Type, Type> ServiceAndImplamentation { get; } = new Dictionary<Type, Type>
    {
        [typeof(Global)] = typeof(Global),
        [typeof(EventBus)] = typeof(EventBus),
        [typeof(GridSystem)] = typeof(GridSystem),
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
                case nameof(GridSystem):
                    _gridSystem = (GridSystem)service;
                    break;
            }
        }
    }
    public virtual bool CheckThatNodeIsFree(int positionX, int positionY)
    {
        if (_gridSystem[positionX, positionY].TypeOfNode == NodeType.Empty)
            return true;
        else
            return false;
    }
    public bool CheckPositionConformity(ObjectDataForBilding cursorObjectData, MouseIsClickedSignal signal)
    {
        bool canBuild = false;
        if (cursorObjectData.IsNotSemmetric)
        {
            foreach (var cells in cursorObjectData.CellsPosition)
            {
                if (CheckThatNodeIsFree(Mathf.FloorToInt(cells.transform.position.x), Mathf.FloorToInt(cells.transform.position.z)))
                    canBuild = true;
                else
                {
                    Debug.Log("Node is't free ");
                    canBuild = false;
                }
            }
            return canBuild;
        }
        else
        {
            for (int i = 0; i < cursorObjectData.BildingSize.x; i++)
            {
                for (int j = 0; j < cursorObjectData.BildingSize.y; j++)
                {
                    if (signal.position != null)
                    {
                        if (CheckThatNodeIsFree(signal.position.x + i, signal.position.z + j))
                            canBuild = true;
                        else
                        {
                            Debug.Log("Node is't free");
                            canBuild =  false;
                        }
                    }
                    else
                    {
                        Debug.Log("signal is null");
                        canBuild =  false;
                    }
                }
            }
            return canBuild;
        }
        throw new NotImplementedException();
    }
    public virtual void SetStructureOnGround(ObjectDataForBilding ObjectToBildData, ObjectDataForBilding cursorObjectData, MouseIsClickedSignal signal)
    {
        CurrentPosition = new Vector3Int(Mathf.FloorToInt(_housePositionForCar.transform.position.x), 0, Mathf.FloorToInt(_housePositionForCar.transform.position.z));
        _housePositionForCar.SetActive(false);
        _isPlaced = true;
        IInjectable init = this;
        init.Injecting();
    }


    // Ui Displaing
    public abstract void OnMouseDown();

}
