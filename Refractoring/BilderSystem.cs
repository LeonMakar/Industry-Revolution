using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UIElements;

public class BilderSystem : MonoBehaviour
{
    private Bilder _bilder;
    public FirstCity _firstCity = new FirstCity(5);
    public SecondCity SecondCity = new SecondCity(6);

    [SerializeField] GameObject _cursor;

    public GridSystem _grid;

    private Dictionary<Vector3Int, GameObject> _allRoads = new Dictionary<Vector3Int, GameObject>();
    private Dictionary<string, NodeData> _roadNeighbors = new Dictionary<string, NodeData>();

    private void Start()
    {
        EventBus.Instance.Subscrube<MouseIsClickedSignal>(BildRoad);
        EventBus.Instance.Subscrube<MousePositionSignal>(CursorPosition);


        _grid = new GridSystem(10, 10);
    }
    private void OnDisable()
    {
        EventBus.Instance.Unsubscribe<MouseIsClickedSignal>(BildRoad);
        EventBus.Instance.Unsubscribe<MousePositionSignal>(CursorPosition);
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

    private void BildRoad(MouseIsClickedSignal signal)
    {
        if (signal.position != null)
        {
            if (CheckThatIsNodeFree(signal.position.x, signal.position.z))
            {
                _bilder = new RoadBilder();
                GameObject road = _bilder.Bild(RoadType.Stright);
                road.transform.position = signal.position;
                _grid[signal.position.x, signal.position.z].MakeNodeSetup(NodeType.Road, signal.position);
                _allRoads.Add(signal.position, road);
                FixRoad(signal.position.x, signal.position.z);
            }
        }
        else
            Debug.Log("Vector3Int is null");
    }

    //Get neighbors in an order Left,Right,Up, Down
    private void FixRoad(int positionX, int positionZ)
    {
        _roadNeighbors = _grid.GetAllNeighborsNearThePointOffSpecificType(positionX, positionZ, NodeType.Road);
        #region CurveRoad

        //Left and Up is Road, Right and Down something else
        if (_roadNeighbors.ContainsKey("Left") && _roadNeighbors.ContainsKey("Up"))
        {
            SetNewRoadChainAndRoadType(positionX, positionZ, RoadType.Curve, "Left", "Up", 0, 90, 0);
        }

        //Up and Right is Road, Left and Down something else
        else if (_roadNeighbors.ContainsKey("Up") && _roadNeighbors.ContainsKey("Right"))
        {
            SetNewRoadChainAndRoadType(positionX, positionZ, RoadType.Curve, "Up", "Right", 90, 0, 90);
        }

        //Right and Down is Road, Left and Up something else
        else if (_roadNeighbors.ContainsKey("Right") && _roadNeighbors.ContainsKey("Down"))
        {
            SetNewRoadChainAndRoadType(positionX, positionZ, RoadType.Curve, "Right", "Down", 180, 90, 0);
        }
        //Down and Left is Road, Right and Up something else
        else if (_roadNeighbors.ContainsKey("Down") && _roadNeighbors.ContainsKey("Left"))
        {
            SetNewRoadChainAndRoadType(positionX, positionZ, RoadType.Curve, "Down", "Left", 270, 0, 90);
        }
        #endregion


        //Left and Right is Road, Up and Down something else
        else if (_roadNeighbors.ContainsKey("Left") && _roadNeighbors.ContainsKey("Right"))
        {
            SetNewRoadChainAndRoadType(positionX, positionZ, "Left", "Right", 90, 90, 90);
        }
        //Left is Road, Right, Up and Down something else
        else if (_roadNeighbors.ContainsKey("Left"))
        {
            SetNewRoadChainAndRoadType(positionX, positionZ, "Left", 90, 90);
        }
        //Right is Road, Left, Up and Down something else
        else if (_roadNeighbors.ContainsKey("Right"))
        {
           SetNewRoadChainAndRoadType(positionX, positionZ, "Left", 90, 90);
        }

    }

    #region FixMetods
    private void SetNewRoadChainAndRoadType(int positionX, int positionZ, RoadType roadType, string firsSide, string secondSide,
        int nodeRotation, int firstSideRotation, int secondSideRotation)
    {
        _roadNeighbors = _grid.GetAllNeighborsNearThePointOffSpecificType(positionX, positionZ, NodeType.Road);
        //CheckFirstSide
        _allRoads[_roadNeighbors[firsSide].GetNodePosition].GetComponentInChildren<FixTransform>().FixRotation(firstSideRotation);
        _allRoads[_roadNeighbors[secondSide].GetNodePosition].GetComponentInChildren<FixTransform>().FixRotation(secondSideRotation);
        if (roadType != RoadType.Nothing)
        {
            Destroy(_allRoads[(new Vector3Int(positionX, 0, positionZ))].gameObject);
            _allRoads.Remove(new Vector3Int(positionX, 0, positionZ));
            var road = _bilder.Bild(roadType);
            road.transform.position = new Vector3Int(positionX, 0, positionZ);
            _allRoads.Add(new Vector3Int(positionX, 0, positionZ), road);
        }
        _allRoads[new Vector3Int(positionX, 0, positionZ)].GetComponentInChildren<FixTransform>().FixRotation(nodeRotation);
    }
    private void SetNewRoadChainAndRoadType(int positionX, int positionZ, string firsSide, string secondSide,
        int nodeRotation, int firstSideRotation, int secondSideRotation)
    {
        _roadNeighbors = _grid.GetAllNeighborsNearThePointOffSpecificType(positionX, positionZ, NodeType.Road);
        _allRoads[_roadNeighbors[firsSide].GetNodePosition].GetComponentInChildren<FixTransform>().FixRotation(firstSideRotation);
        _allRoads[_roadNeighbors[secondSide].GetNodePosition].GetComponentInChildren<FixTransform>().FixRotation(secondSideRotation);
        _allRoads[new Vector3Int(positionX, 0, positionZ)].GetComponentInChildren<FixTransform>().FixRotation(nodeRotation);
    }

    private void SetNewRoadChainAndRoadType(int positionX, int positionZ, string firsSide,int nodeRotation, int firstSideRotation)
    {
        _roadNeighbors = _grid.GetAllNeighborsNearThePointOffSpecificType(positionX, positionZ, NodeType.Road);
        _allRoads[_roadNeighbors[firsSide].GetNodePosition].GetComponentInChildren<FixTransform>().FixRotation(firstSideRotation);
        _allRoads[new Vector3Int(positionX, 0, positionZ)].GetComponentInChildren<FixTransform>().FixRotation(nodeRotation);
    }

    #endregion


}


