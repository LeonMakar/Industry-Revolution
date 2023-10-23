using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Need To Inject => GridSystem,RoadFixer,EventBus
/// </summary>
public class BilderSystem : MonoBehaviour, IInjectable, IService
{
    private ObjectDataForBilding _selectedObjectData;

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


    public List<Vector3Int> RoadsToRecheck => _roadsToRecheck;

    public Dictionary<Type, Type> ServiceAndImplamentation { get; } = new Dictionary<Type, Type>()
    {
        [typeof(GridSystem)] = typeof(GridSystem),
        [typeof(RoadFixer)] = typeof(RoadFixer),
        [typeof(EventBus)] = typeof(EventBus)
    };


    public void Inject(params IService[] services)
    {
        //Grid = gridSystem;
        //_roadFixer = roadFixer;
        //_eventBus = eventBus;
    }
    public void InjectSingletone(GridSystem gridSystem, RoadFixer roadFixer, EventBus eventBus)
    {
        Grid = gridSystem;
        _roadFixer = roadFixer;
        _eventBus = eventBus;

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
        => _selectedObjectData = signal.ObjectData;

    public void DestroyRoadFromTemporaryRoads(int positionX, int positionZ)
        => Destroy(TemporaryRoads[(new Vector3Int(positionX, 0, positionZ))].gameObject);

    public void DestroyRoadFromAllRoads(int positionX, int positionZ)
        => Destroy(AllRoads[(new Vector3Int(positionX, 0, positionZ))].gameObject);

    private void BildBildingWhenClick(MouseIsClickedSignal signal)
    {
        bool canBild = false;
        if (_selectedObjectData != null)
            if (_selectedObjectData.SelectedObjectStructureType == StructureType.Bilding)
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
                    _bilder = new BildingFactory();
                    if (_selectedObjectData.PathToPrefab != null)
                    {
                        BildingFactory bilder = _bilder as BildingFactory;
                        bilder.PathForBildingPrefab = _selectedObjectData.PathToPrefab;
                        GameObject bilding = bilder.Bild(BildingType.Bilding);
                        bilding.transform.position = signal.position;


                        for (int i = 0; i < _selectedObjectData.BildingSize.x; i++)
                        {
                            for (int j = 0; j < _selectedObjectData.BildingSize.y; j++)
                            {
                                Grid[signal.position.x + i, signal.position.z + j].MakeNodeSetup(NodeType.Bilding);
                            }
                        }
                        bilding.TryGetComponent(out HouseManipilation house);
                        if (house != null)
                        {
                            house.SetHouseOnGround();
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
        if (_selectedObjectData != null)
            if (_selectedObjectData.SelectedObjectStructureType == StructureType.Road)
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
        if (_selectedObjectData != null)
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
        roadInfo.RoadSetDirectionEvent.Invoke();
        roadInfo.GetIRoadEvent.Invoke(roadInfo);
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



