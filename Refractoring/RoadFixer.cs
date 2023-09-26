using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Need to Inject => RoadFactory
/// </summary>
public class RoadFixer: IService
{
    private Factory _bilder;
    private Dictionary<string, NodeData> _roadNeighbors = new Dictionary<string, NodeData>();
    public Dictionary<string, NodeData> RoadNeighbors => _roadNeighbors;


    public void Inject(Factory factory)
    {
        _bilder = factory;
    }

    public void FixRoad(int positionX, int positionZ)
    {
        _roadNeighbors = BilderSystem.Instance.Grid.GetAllNeighborsNearThePointOffSpecificType(positionX, positionZ, NodeType.Road);

        //Left, Up , Right and Down is Road
        if (_roadNeighbors.ContainsKey("Left") && _roadNeighbors.ContainsKey("Up") && _roadNeighbors.ContainsKey("Right") && _roadNeighbors.ContainsKey("Down"))
        {
            SetRoadRotation(positionX, positionZ, BildingType.FourWayRoad, 0);
        }

        //Down, Left, Up is Road and  Right something else
        else if (_roadNeighbors.ContainsKey("Down") && _roadNeighbors.ContainsKey("Left") && _roadNeighbors.ContainsKey("Up"))
        {
            SetRoadRotation(positionX, positionZ, BildingType.TreeWayRoad, 180);
        }
        //Left, Up and Right is Road and  Down something else
        else if (_roadNeighbors.ContainsKey("Left") && _roadNeighbors.ContainsKey("Up") && _roadNeighbors.ContainsKey("Right"))
        {
            SetRoadRotation(positionX, positionZ, BildingType.TreeWayRoad, 270);
        }
        //Up, Right and Down  is Road and  Left something else
        else if (_roadNeighbors.ContainsKey("Up") && _roadNeighbors.ContainsKey("Right") && _roadNeighbors.ContainsKey("Down"))
        {
            SetRoadRotation(positionX, positionZ, BildingType.TreeWayRoad, 0);
        }
        //Right, Down and Left is Road and Up something else
        else if (_roadNeighbors.ContainsKey("Right") && _roadNeighbors.ContainsKey("Down") && _roadNeighbors.ContainsKey("Left"))
        {
            SetRoadRotation(positionX, positionZ, BildingType.TreeWayRoad, 90);
        }

        //Left and Up is Road, Right and Down something else
        else if (_roadNeighbors.ContainsKey("Left") && _roadNeighbors.ContainsKey("Up"))
        {
            SetRoadRotation(positionX, positionZ, BildingType.CurveRoad, 0);

        }

        //Up and Right is Road, Left and Down something else
        else if (_roadNeighbors.ContainsKey("Up") && _roadNeighbors.ContainsKey("Right"))
        {
            SetRoadRotation(positionX, positionZ, BildingType.CurveRoad, 90);
        }

        //Right and Down is Road, Left and Up something else
        else if (_roadNeighbors.ContainsKey("Right") && _roadNeighbors.ContainsKey("Down"))
        {
            SetRoadRotation(positionX, positionZ, BildingType.CurveRoad, 180);
        }

        //Down and Left is Road, Right and Up something else
        else if (_roadNeighbors.ContainsKey("Down") && _roadNeighbors.ContainsKey("Left"))
        {
            SetRoadRotation(positionX, positionZ, BildingType.CurveRoad, 270);
        }

        //Left and Right is Road, Up and Down something else
        else if (_roadNeighbors.ContainsKey("Left") && _roadNeighbors.ContainsKey("Right"))
        {
            SetRoadRotation(positionX, positionZ, 90);
        }
        //Left is Road, Right, Up and Down something else
        else if (_roadNeighbors.ContainsKey("Left"))
        {
            SetRoadRotation(positionX, positionZ, 90);
        }
        //Right is Road, Left, Up and Down something else
        else if (_roadNeighbors.ContainsKey("Right"))
        {
            SetRoadRotation(positionX, positionZ, 90);
        }
    }

    public void FixNeighborsRoad()
    {
        foreach (var road in BilderSystem.Instance.RoadsToRecheck)
        {
            FixRoad(road.x, road.z);
        }
        BilderSystem.Instance.RoadsToRecheck.Clear();
    }
    private void SetRoadRotation(int positionX, int positionZ, BildingType roadType, int nodeRotation)
    {
        if (roadType != BildingType.NothingRoad)
        {
            var road = _bilder.Bild(roadType);
            road.transform.position = new Vector3Int(positionX, 0, positionZ);

            if (BilderSystem.Instance.TemporaryRoads.ContainsKey(new Vector3Int(positionX, 0, positionZ)))
            {
                BilderSystem.Instance.DestroyRoadFromTemporaryRoads(positionX, positionZ);
                BilderSystem.Instance.TemporaryRoads[new Vector3Int(positionX, 0, positionZ)] = road;
            }
            if (BilderSystem.Instance.AllRoads.ContainsKey(new Vector3Int(positionX, 0, positionZ)))
            {
                BilderSystem.Instance.DestroyRoadFromAllRoads(positionX, positionZ);
                BilderSystem.Instance.AllRoads[new Vector3Int(positionX, 0, positionZ)] = road;
            }
        }

        if (BilderSystem.Instance.TemporaryRoads.ContainsKey(new Vector3Int(positionX, 0, positionZ)))
            BilderSystem.Instance.TemporaryRoads[new Vector3Int(positionX, 0, positionZ)].GetComponentInChildren<FixTransform>().FixRotation(nodeRotation);
        else
            BilderSystem.Instance.AllRoads[new Vector3Int(positionX, 0, positionZ)].GetComponentInChildren<FixTransform>().FixRotation(nodeRotation);

    }

    private void SetRoadRotation(int positionX, int positionZ, int nodeRotation)
    {
        if (BilderSystem.Instance.TemporaryRoads.ContainsKey(new Vector3Int(positionX, 0, positionZ)))
            BilderSystem.Instance.TemporaryRoads[new Vector3Int(positionX, 0, positionZ)].GetComponentInChildren<FixTransform>().FixRotation(nodeRotation);
        else
            BilderSystem.Instance.AllRoads[new Vector3Int(positionX, 0, positionZ)].GetComponentInChildren<FixTransform>().FixRotation(nodeRotation);
    }

}


