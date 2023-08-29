using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public partial class BilderSystem : MonoBehaviour
{
    public static BilderSystem Instance { get; private set; }

    [SerializeField] private GameObject _cursor;

    public FirstCity _firstCity = new FirstCity(5);
    public SecondCity SecondCity = new SecondCity(6);
    public GridSystem _grid;

    private Bilder _bilder;
    private RoadFixer _roadFixer;

    private Dictionary<Vector3Int, GameObject> _allRoads = new Dictionary<Vector3Int, GameObject>();
    private Dictionary<Vector3Int, GameObject> _temporaryRoads = new Dictionary<Vector3Int, GameObject>();
    private List<Vector3Int> _roadsToRecheck = new List<Vector3Int>();

    private Vector3Int _firstRoad;

    public Dictionary<Vector3Int, GameObject> AllRoads => _allRoads;
    public List<Vector3Int> RoadsToRecheck => _roadsToRecheck;
    public Dictionary<Vector3Int, GameObject> TemporaryRoads => _temporaryRoads;


    private void Start()
    {
        Instance = this;
        EventBus.Instance.Subscrube<MouseIsClickedSignal>(BildRoadWhenClick);
        EventBus.Instance.Subscrube<MousePositionSignal>(CursorPosition);
        EventBus.Instance.Subscrube<MouseIsHoldSignal>(BildRoadWhenHold);
        EventBus.Instance.Subscrube<MouseIsUpSignal>(SetAllTemporaryRoads);

        _grid = new GridSystem(10, 10);
        _roadFixer = new RoadFixer();
    }

    private void OnDisable()
    {
        EventBus.Instance.Unsubscribe<MouseIsClickedSignal>(BildRoadWhenClick);
        EventBus.Instance.Unsubscribe<MousePositionSignal>(CursorPosition);
        EventBus.Instance.Unsubscribe<MouseIsHoldSignal>(BildRoadWhenHold);
        EventBus.Instance.Unsubscribe<MouseIsUpSignal>(SetAllTemporaryRoads);


    }

    private bool CheckThatIsNodeFree(int positionX, int positionY)
    {
        if (_grid[positionX, positionY].TypeOfNode == NodeType.Empty)
            return true;
        else
            return false;
    }

    private void CursorPosition(MousePositionSignal signal)
    {
        _cursor.transform.position = signal.positionVector3int;
    }

    public void DestroyRoad(int positionX, int positionZ)
    {
        Destroy(_allRoads[(new Vector3Int(positionX, 0, positionZ))].gameObject);
    }

    private void BildRoadWhenClick(MouseIsClickedSignal signal)
    {
        if (signal.position != null)
        {
            if (CheckThatIsNodeFree(signal.position.x, signal.position.z))
            {
                _bilder = new RoadBilder();
                GameObject road = _bilder.Bild(RoadType.Stright);
                road.transform.position = signal.position;
                _grid[signal.position.x, signal.position.z].MakeNodeSetup(NodeType.Road);

                _allRoads.Add(signal.position, road);
                _roadFixer.FixRoad(signal.position.x, signal.position.z);

                foreach (var item in _roadFixer.RoadNeighbors)
                    _roadsToRecheck.Add(item.Value.GetNodePosition);

                _roadFixer.FixNeighborsRoad();
                _firstRoad = signal.position;
            }
        }
        else
            Debug.Log("Vector3Int is null");
    }

    private void DestroyFirstRoad(Vector3Int position)
    {
        if (_allRoads.ContainsKey(position))
        {

            _grid[position.x, position.z].MakeNodeSetup(NodeType.Empty);
            Destroy(_allRoads[position].gameObject);
            _allRoads.Remove(position);
        }

    }
    private void BildRoadWhenHold(MouseIsHoldSignal signal)
    {
        _roadsToRecheck.Clear();
        DestroyFirstRoad(_firstRoad);
        if (signal.NodePositions != null)
        {
            List<Vector3Int> roadPositions = new List<Vector3Int>();
            roadPositions.Add(_firstRoad);
            foreach (var position in signal.NodePositions)
                roadPositions.Add(position);


            foreach (var nodePosition in roadPositions)
            {
                if (CheckThatIsNodeFree(nodePosition.x, nodePosition.y))
                {
                    if (!_allRoads.ContainsKey(nodePosition))
                    {
                        _bilder = new RoadBilder();
                        GameObject road = _bilder.Bild(RoadType.Stright);
                        road.transform.position = nodePosition;
                        Debug.Log(nodePosition);
                        _grid[nodePosition.x, nodePosition.z].MakeNodeSetup(NodeType.Road);

                        if (!_temporaryRoads.ContainsKey(nodePosition))
                        {
                            _temporaryRoads.Add(nodePosition, road);
                        }
                        _roadFixer.FixRoadTemporary(nodePosition.x, nodePosition.z);
                        _roadsToRecheck.Add(nodePosition);
                    }
                }
            }
            _roadFixer.FixNeighborsRoadForTemporary();
        }
        else
            Debug.Log("Vector3Int is null");
    }

    public void DestroyTemporaryRoads()
    {
        foreach (var road in _temporaryRoads)
        {
            Destroy(road.Value.gameObject);
            _grid[road.Key.x, road.Key.z].MakeNodeSetup(NodeType.Empty);
        }
        _temporaryRoads.Clear();
    }

    public void SetAllTemporaryRoads(MouseIsUpSignal signal)
    {

        _roadFixer.FixNeighborsRoad();
        foreach (var road in _temporaryRoads)
        {
            _grid[road.Key.x, road.Key.z].MakeNodeSetup(NodeType.Road);
            _allRoads.Add(road.Key, road.Value);
        }
        _temporaryRoads.Clear();
    }


}



