﻿using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public partial class BilderSystem : MonoBehaviour
{
    public static BilderSystem Instance { get; private set; }
    public FirstCity _firstCity = new FirstCity(5);
    public SecondCity SecondCity = new SecondCity(6);
    public GridSystem _grid;
    public Dictionary<Vector3Int, GameObject> TemporaryRoads = new Dictionary<Vector3Int, GameObject>();
    public Dictionary<Vector3Int, GameObject> AllRoads = new Dictionary<Vector3Int, GameObject>();

    private ObjectDataForBilding _selectedObjectData;

    private Bilder _bilder;
    private RoadFixer _roadFixer;

    private List<Vector3Int> _roadsToRecheck = new List<Vector3Int>();
    private Dictionary<Vector3Int, GameObject> _firstRoad = new Dictionary<Vector3Int, GameObject>();

    public List<Vector3Int> RoadsToRecheck => _roadsToRecheck;



    private void Start()
    {
        Instance = this;
        EventBus.Instance.Subscrube<MouseIsClickedSignal>(BildRoadWhenClick);
        EventBus.Instance.Subscrube<MouseIsClickedSignal>(BildBildingWhenClick);
        EventBus.Instance.Subscrube<MouseIsHoldSignal>(BildRoadWhenHold);
        EventBus.Instance.Subscrube<MouseIsUpSignal>(SetAllTemporaryRoads);
        EventBus.Instance.Subscrube<SelectedObjectSignal>(SetSelectedObject);

        _grid = new GridSystem(10, 10);
        _roadFixer = new RoadFixer();
    }

    private void OnDisable()
    {
        EventBus.Instance.Unsubscribe<MouseIsClickedSignal>(BildRoadWhenClick);
        EventBus.Instance.Subscrube<MouseIsClickedSignal>(BildBildingWhenClick);
        EventBus.Instance.Unsubscribe<MouseIsHoldSignal>(BildRoadWhenHold);
        EventBus.Instance.Unsubscribe<MouseIsUpSignal>(SetAllTemporaryRoads);
        EventBus.Instance.Unsubscribe<SelectedObjectSignal>(SetSelectedObject);
    }

    private bool CheckThatIsNodeFree(int positionX, int positionY)
    {
        if (_grid[positionX, positionY].TypeOfNode == NodeType.Empty)
            return true;
        else
            return false;
    }


    private void SetSelectedObject(SelectedObjectSignal signal)
        => _selectedObjectData = signal.ObjectData;

    public void DestroyRoadFromTemporaryRoads(int positionX, int positionZ)
        => Destroy(TemporaryRoads[(new Vector3Int(positionX, 0, positionZ))].gameObject);

    public void DestroyRoadFromAllRoads(int positionX, int positionZ)
        => Destroy(AllRoads[(new Vector3Int(positionX, 0, positionZ))].gameObject);

    private void BildBildingWhenClick(MouseIsClickedSignal signal)
    {
        bool canBild = false;
        if (_selectedObjectData.SelectedObjectStructureType == StructureType.Bilding && _selectedObjectData != null)
        {
            for (int i = 0; i < _selectedObjectData.BildingSize.x; i++)
            {
                for (int j = 0; j < _selectedObjectData.BildingSize.y; j++)
                {
                    if (signal.position != null)
                    {
                        if (CheckThatIsNodeFree(signal.position.x + i, signal.position.z + j))
                            canBild = true;
                        else
                        {
                            canBild = false;
                            Debug.Log("Node is not Empty");
                            return;
                        }
                    }
                    else
                    {
                        Debug.Log("signal is null");
                        return;
                    }
                }
            }

            if (canBild)
            {
                _bilder = new BildingBilder();
                if (_selectedObjectData.PathToPrefab != null)
                {
                    BildingBilder bilder = _bilder as BildingBilder;
                    bilder.PathForBildingPrefab = _selectedObjectData.PathToPrefab;
                    GameObject bilding = bilder.Bild(BildingType.Bilding);
                    bilding.transform.position = signal.position;


                    for (int i = 0; i < _selectedObjectData.BildingSize.x; i++)
                    {
                        for (int j = 0; j < _selectedObjectData.BildingSize.y; j++)
                        {
                            _grid[signal.position.x + i, signal.position.z + j].MakeNodeSetup(NodeType.Bilding);
                        }
                    }
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
        if (_selectedObjectData.SelectedObjectStructureType == StructureType.Road && _selectedObjectData != null)
        {
            if (signal.position != null)
            {
                if (CheckThatIsNodeFree(signal.position.x, signal.position.z))
                {
                    _bilder = new RoadBilder();
                    GameObject road = _bilder.Bild(BildingType.StrightRoad);
                    road.transform.position = signal.position;
                    _grid[signal.position.x, signal.position.z].MakeNodeSetup(NodeType.Road);

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
        if (_selectedObjectData.SelectedObjectStructureType == StructureType.Road)
        {
            DestroyTemporaryRoads();
            _roadsToRecheck.Clear();
            if (signal.NodePositions != null)
            {
                foreach (var nodePosition in signal.NodePositions)
                {
                    if (!AllRoads.ContainsKey(nodePosition))
                    {
                        _bilder = new RoadBilder();
                        GameObject road = _bilder.Bild(BildingType.StrightRoad);
                        road.transform.position = nodePosition;
                        _grid[nodePosition.x, nodePosition.z].MakeNodeSetup(NodeType.Road);

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
            _grid[road.Key.x, road.Key.z].MakeNodeSetup(NodeType.Empty);
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
                _grid[road.Key.x, road.Key.z].MakeNodeSetup(NodeType.Road);
                _roadFixer.FixRoad(road.Key.x, road.Key.z);

                foreach (var item in _roadFixer.RoadNeighbors)
                    _roadsToRecheck.Add(item.Value.GetNodePosition);

                _roadFixer.FixNeighborsRoad();
            }
            foreach (var road in TemporaryRoads)
            {
                _grid[road.Key.x, road.Key.z].MakeNodeSetup(NodeType.Road);
                if (AllRoads.ContainsKey(road.Key) == false)
                {
                    AllRoads.Add(road.Key, road.Value);
                }
            }
            TemporaryRoads.Clear();
        }
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



