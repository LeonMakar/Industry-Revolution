﻿using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Need To Inject => GridSystem,RoadFixer,EventBus
/// </summary>
public class BilderSystem : MonoBehaviour, IInjectable, IService
{
    private ObjectDataForBilding _objectUnderCursorData;

    public FirstCity _firstCity = new FirstCity(5);
    public SecondCity SecondCity = new SecondCity(6);

    public Dictionary<Vector3Int, GameObject> TemporaryRoads = new Dictionary<Vector3Int, GameObject>();
    public Dictionary<Vector3Int, GameObject> AllRoads = new Dictionary<Vector3Int, GameObject>();
    private Dictionary<Vector3Int, GameObject> _firstRoad = new Dictionary<Vector3Int, GameObject>();
    private List<Vector3Int> _roadsToRecheck = new List<Vector3Int>();


    public GridSystem Grid;
    private Factory _bilder;
    private RoadFixer _roadFixer;
    private EventBus _eventBus;
    private Cursor _cursor;


    public List<Vector3Int> RoadsToRecheck => _roadsToRecheck;

    public Dictionary<Type, Type> ServiceAndImplamentation { get; } = new Dictionary<Type, Type>()
    {
        [typeof(GridSystem)] = typeof(GridSystem),
        [typeof(RoadFixer)] = typeof(RoadFixer),
        [typeof(EventBus)] = typeof(EventBus),
        [typeof(Cursor)] = typeof(Cursor)
    };


    public void Inject(params IService[] services)
    {
        foreach (var service in services)
        {
            switch (service.GetType().Name)
            {
                case nameof(GridSystem):
                    Grid = (GridSystem)service;
                    break;
                case nameof(RoadFixer):
                    _roadFixer = (RoadFixer)service;
                    break;
                case nameof(EventBus):
                    _eventBus = (EventBus)service;
                    break;
                case nameof(Cursor):
                    _cursor = (Cursor)service;
                    break;

            }
        }
        _eventBus.Subscrube<MouseIsClickedSignal>(BildRoadWhenClick);
        _eventBus.Subscrube<MouseIsClickedSignal>(BildBildingWhenClick);
        _eventBus.Subscrube<MouseIsHoldSignal>(BildRoadWhenHold);
        _eventBus.Subscrube<MouseIsUpSignal>(SetAllTemporaryRoads);
        _eventBus.Subscrube<SelectedObjectSignal>(SetSelectedObject);
    }

    private void OnDisable()
    {
        _eventBus.Unsubscribe<MouseIsClickedSignal>(BildRoadWhenClick);
        _eventBus.Unsubscribe<MouseIsClickedSignal>(BildBildingWhenClick);
        _eventBus.Unsubscribe<MouseIsHoldSignal>(BildRoadWhenHold);
        _eventBus.Unsubscribe<MouseIsUpSignal>(SetAllTemporaryRoads);
        _eventBus.Unsubscribe<SelectedObjectSignal>(SetSelectedObject);
    }

    private bool CheckThatIsNodeFree(int positionX, int positionY)
    {
        if (Grid[positionX, positionY].TypeOfNode == NodeType.Empty)
            return true;
        else
            return false;
    }


    private void SetSelectedObject(SelectedObjectSignal signal)
        => _objectUnderCursorData = signal.ObjectData;

    public void DestroyRoadFromTemporaryRoads(int positionX, int positionZ)
        => Destroy(TemporaryRoads[(new Vector3Int(positionX, 0, positionZ))].gameObject);

    public void DestroyRoadFromAllRoads(int positionX, int positionZ)
        => Destroy(AllRoads[(new Vector3Int(positionX, 0, positionZ))].gameObject);

    private void BildBildingWhenClick(MouseIsClickedSignal signal)
    {
        if (_objectUnderCursorData != null)
            if (_objectUnderCursorData.SelectedObjectStructureType == StructureType.Bilding)
            {              
                if (_objectUnderCursorData.Structure.CheckPositionConformity(_objectUnderCursorData,signal))
                {
                    _bilder = new BildingFactory();
                    if (_objectUnderCursorData.PathToPrefab != null)
                    {
                        BildingFactory bilder = _bilder as BildingFactory;
                        bilder.PathForBildingPrefab = _objectUnderCursorData.PathToPrefab;
                        GameObject bilding = bilder.Bild(BildingType.Bilding);
                        bilding.transform.position = signal.position;
                        bilding.TryGetComponent(out ObjectDataForBilding bildingData);
                        bildingData.BildingPrefabForRotate.transform.Rotate(Vector3.up, _objectUnderCursorData.RotationAngle);

                        if (!Input.GetKey(KeyCode.LeftShift))
                            _cursor.ResetObjectUnderCursor();

                        bilding.TryGetComponent(out Structure someBilding);
                        someBilding?.Injecting();
                        someBilding?.SetStructureOnGround(bildingData, _objectUnderCursorData, signal);
                    }
                    else
                        Debug.Log("Path to Prefab is null");
                }
                else
                {
                    Debug.Log("Construction is impossible");
                }
            }
    }
    private void BildRoadWhenClick(MouseIsClickedSignal signal)
    {
        if (_objectUnderCursorData != null)
            if (_objectUnderCursorData.SelectedObjectStructureType == StructureType.Road)
            {
                if (signal.position != null)
                {
                    if (CheckThatIsNodeFree(signal.position.x, signal.position.z))
                    {
                        _bilder = new RoadFactory();
                        GameObject road = _bilder.Bild(BildingType.StrightRoad);
                        road.transform.position = signal.position;
                        Grid[signal.position.x, signal.position.z].MakeNodeSetup(NodeType.Road);

                        if (TemporaryRoads.ContainsKey(new Vector3Int(signal.position.x, 0, signal.position.z)) == false)
                        {
                            TemporaryRoads.Add(signal.position, road);
                        }

                        _roadsToRecheck.Add(signal.position);
                        _firstRoad.Add(signal.position, road);
                    }
                }
                else
                    Debug.Log("Vector3Int is null");
            }
    }
    private void BildRoadWhenHold(MouseIsHoldSignal signal)
    {
        if (_objectUnderCursorData != null)
            if (_objectUnderCursorData.SelectedObjectStructureType == StructureType.Road)
            {
                DestroyTemporaryRoads();
                _roadsToRecheck.Clear();
                if (signal.NodePositions != null)
                {
                    foreach (var nodePosition in signal.NodePositions)
                    {
                        if (!AllRoads.ContainsKey(nodePosition))
                        {
                            _bilder = new RoadFactory();
                            GameObject road = _bilder.Bild(BildingType.StrightRoad);
                            road.transform.position = nodePosition;
                            Grid[nodePosition.x, nodePosition.z].MakeNodeSetup(NodeType.Road);

                            if (!TemporaryRoads.ContainsKey(nodePosition))
                            {
                                TemporaryRoads.Add(nodePosition, road);
                            }
                            _roadFixer.FixRoad(nodePosition.x, nodePosition.z);
                            _roadsToRecheck.Add(nodePosition);
                        }
                    }
                }
            }
    }

    private void DestroyTemporaryRoads()
    {
        foreach (var road in TemporaryRoads)
        {
            Destroy(road.Value.gameObject);
            Grid[road.Key.x, road.Key.z].MakeNodeSetup(NodeType.Empty);
        }
        TemporaryRoads.Clear();
    }

    private void SetAllTemporaryRoads(MouseIsUpSignal signal)
    {
        if (TemporaryRoads.Count > 0)
        {
            Dictionary<Vector3Int, GameObject> TemporaryRoadsSnapShot = new Dictionary<Vector3Int, GameObject>(TemporaryRoads);
            foreach (var road in TemporaryRoadsSnapShot)
            {
                Grid[road.Key.x, road.Key.z].MakeNodeSetup(NodeType.Road);
                _roadFixer.FixRoad(road.Key.x, road.Key.z);

                foreach (var item in _roadFixer.RoadNeighbors)
                    _roadsToRecheck.Add(item.Value.GetNodePosition);

                _roadFixer.FixNeighborsRoad();
            }
            foreach (var road in TemporaryRoads)
            {
                Grid[road.Key.x, road.Key.z].MakeNodeSetup(NodeType.Road);
                if (AllRoads.ContainsKey(road.Key) == false)
                {
                    AllRoads.Add(road.Key, road.Value);
                    SetRoadMarksPositions(road.Value);
                }
            }
            TemporaryRoads.Clear();
        }
    }

    public void SetRoadMarksPositions(GameObject road)
    {
        road.TryGetComponent(out RoadInfo roadInfo);
        //roadInfo.SetConnectionToRoadInfo();
        roadInfo.SetLastMarksPosition();
    }
    public List<Vector3Int> CheckNeighbors(Vector3Int position)
    {
        List<Vector3Int> neighbors = new List<Vector3Int>();
        if (AllRoads.ContainsKey(new Vector3Int(position.x - 1, 0, position.z))) //Left
            neighbors.Add(new Vector3Int(position.x - 1, 0, position.z));
        else if (AllRoads.ContainsKey(new Vector3Int(position.x + 1, 0, position.z))) //Right
            neighbors.Add(new Vector3Int(position.x + 1, 0, position.z));
        else if (AllRoads.ContainsKey(new Vector3Int(position.x, 0, position.z + 1))) //Up
            neighbors.Add(new Vector3Int(position.x, 0, position.z + 1));
        else if (AllRoads.ContainsKey(new Vector3Int(position.x, 0, position.z - 1))) //Down
            neighbors.Add(new Vector3Int(position.x, 0, position.z - 1));
        return neighbors;
    }
}
public enum StructureType
{
    None = 0,
    Road = 1,
    Bilding = 2,
    District = 3,
    City = 4,
}



