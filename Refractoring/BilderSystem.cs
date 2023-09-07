using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public partial class BilderSystem : MonoBehaviour
{
    public static BilderSystem Instance { get; private set; }

    [SerializeField] private GameObject _cursor;

    public FirstCity _firstCity = new FirstCity(5);
    public SecondCity SecondCity = new SecondCity(6);
    public GridSystem _grid;
    public Dictionary<Vector3Int, GameObject> TemporaryRoads = new Dictionary<Vector3Int, GameObject>();
    public Dictionary<Vector3Int, GameObject> AllRoads = new Dictionary<Vector3Int, GameObject>();

    private Bilder _bilder;
    private RoadFixer _roadFixer;

    private List<Vector3Int> _roadsToRecheck = new List<Vector3Int>();

    private Dictionary<Vector3Int, GameObject> _firstRoad = new Dictionary<Vector3Int, GameObject>();

    public List<Vector3Int> RoadsToRecheck => _roadsToRecheck;


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

    public void DestroyRoadFromTemporaryRoads(int positionX, int positionZ)
    {
        Destroy(TemporaryRoads[(new Vector3Int(positionX, 0, positionZ))].gameObject);

    }
    public void DestroyRoadFromAllRoads(int positionX, int positionZ)
    {
        Destroy(AllRoads[(new Vector3Int(positionX, 0, positionZ))].gameObject);
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
    private void BildRoadWhenHold(MouseIsHoldSignal signal)
    {
        DestroyTemporaryRoads();
        foreach (var item in _firstRoad)
        {
            if (!TemporaryRoads.ContainsKey(item.Key))
                TemporaryRoads.Add(item.Key, item.Value);
        }

        _roadsToRecheck.Clear();
        if (signal.NodePositions != null)
        {
            foreach (var nodePosition in signal.NodePositions)
            {
                if (CheckThatIsNodeFree(nodePosition.x, nodePosition.y))
                {
                    if (!AllRoads.ContainsKey(nodePosition))
                    {
                        _bilder = new RoadBilder();
                        GameObject road = _bilder.Bild(RoadType.Stright);
                        road.transform.position = nodePosition;
                        Debug.Log(nodePosition);
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

    public void DestroyTemporaryRoads()
    {
        foreach (var road in TemporaryRoads)
        {
            Destroy(road.Value.gameObject);
            _grid[road.Key.x, road.Key.z].MakeNodeSetup(NodeType.Empty);
        }
        TemporaryRoads.Clear();
    }

    public void SetAllTemporaryRoads(MouseIsUpSignal signal)
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



