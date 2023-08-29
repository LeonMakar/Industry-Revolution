using System.Collections.Generic;
using UnityEngine;

public class RoadFixer
{
    private Bilder _bilder = new RoadBilder();
    private Dictionary<string, NodeData> _roadNeighbors = new Dictionary<string, NodeData>();
    public Dictionary<string, NodeData> RoadNeighbors => _roadNeighbors;

    /// <summary>
    /// Get neighbors near by coordinate X,Y then change road prefab and his rotation,depending on neighbors
    /// </summary>
    /// <param name="positionX"></param>
    /// <param name="positionZ"></param>
    public void FixRoad(int positionX, int positionZ)
    {
        _roadNeighbors = BilderSystem.Instance._grid.GetAllNeighborsNearThePointOffSpecificType(positionX, positionZ, NodeType.Road);
        #region CurveRoad

        //Left, Up , Right and Down is Road
        if (_roadNeighbors.ContainsKey("Left") && _roadNeighbors.ContainsKey("Up") && _roadNeighbors.ContainsKey("Right") && _roadNeighbors.ContainsKey("Down"))
        {
            SetRoadRotation(positionX, positionZ, RoadType.FourWay, 0);
        }

        //Down, Left, Up is Road and  Right something else
        else if (_roadNeighbors.ContainsKey("Down") && _roadNeighbors.ContainsKey("Left") && _roadNeighbors.ContainsKey("Up"))
        {
            SetRoadRotation(positionX, positionZ, RoadType.TreeWay, 180);
        }
        //Left, Up and Right is Road and  Down something else
        else if (_roadNeighbors.ContainsKey("Left") && _roadNeighbors.ContainsKey("Up") && _roadNeighbors.ContainsKey("Right"))
        {
            SetRoadRotation(positionX, positionZ, RoadType.TreeWay, 270);
        }
        //Up, Right and Down  is Road and  Left something else
        else if (_roadNeighbors.ContainsKey("Up") && _roadNeighbors.ContainsKey("Right") && _roadNeighbors.ContainsKey("Down"))
        {
            SetRoadRotation(positionX, positionZ, RoadType.TreeWay, 0);
        }
        //Right, Down and Left is Road and Up something else
        else if (_roadNeighbors.ContainsKey("Right") && _roadNeighbors.ContainsKey("Down") && _roadNeighbors.ContainsKey("Left"))
        {
            SetRoadRotation(positionX, positionZ, RoadType.TreeWay, 90);
        }

        //Left and Up is Road, Right and Down something else
        else if (_roadNeighbors.ContainsKey("Left") && _roadNeighbors.ContainsKey("Up"))
        {
            SetRoadRotation(positionX, positionZ, RoadType.Curve, 0);

        }

        //Up and Right is Road, Left and Down something else
        else if (_roadNeighbors.ContainsKey("Up") && _roadNeighbors.ContainsKey("Right"))
        {
            SetRoadRotation(positionX, positionZ, RoadType.Curve, 90);
        }

        //Right and Down is Road, Left and Up something else
        else if (_roadNeighbors.ContainsKey("Right") && _roadNeighbors.ContainsKey("Down"))
        {
            SetRoadRotation(positionX, positionZ, RoadType.Curve, 180);
        }
        //Down and Left is Road, Right and Up something else
        else if (_roadNeighbors.ContainsKey("Down") && _roadNeighbors.ContainsKey("Left"))
        {
            SetRoadRotation(positionX, positionZ, RoadType.Curve, 270);
        }
        #endregion

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
    public void FixRoadTemporary(int positionX, int positionZ)
    {
        _roadNeighbors = BilderSystem.Instance._grid.GetAllNeighborsNearThePointOffSpecificType(positionX, positionZ, NodeType.Road);
        #region CurveRoad

        //Left, Up , Right and Down is Road
        if (_roadNeighbors.ContainsKey("Left") && _roadNeighbors.ContainsKey("Up") && _roadNeighbors.ContainsKey("Right") && _roadNeighbors.ContainsKey("Down"))
        {
            SetRoadRotationForTemporaryRoad(positionX, positionZ, RoadType.FourWay, 0);
        }

        //Down, Left, Up is Road and  Right something else
        else if (_roadNeighbors.ContainsKey("Down") && _roadNeighbors.ContainsKey("Left") && _roadNeighbors.ContainsKey("Up"))
        {
            SetRoadRotationForTemporaryRoad(positionX, positionZ, RoadType.TreeWay, 180);
        }
        //Left, Up and Right is Road and  Down something else
        else if (_roadNeighbors.ContainsKey("Left") && _roadNeighbors.ContainsKey("Up") && _roadNeighbors.ContainsKey("Right"))
        {
            SetRoadRotationForTemporaryRoad(positionX, positionZ, RoadType.TreeWay, 270);
        }
        //Up, Right and Down  is Road and  Left something else
        else if (_roadNeighbors.ContainsKey("Up") && _roadNeighbors.ContainsKey("Right") && _roadNeighbors.ContainsKey("Down"))
        {
            SetRoadRotationForTemporaryRoad(positionX, positionZ, RoadType.TreeWay, 0);
        }
        //Right, Down and Left is Road and Up something else
        else if (_roadNeighbors.ContainsKey("Right") && _roadNeighbors.ContainsKey("Down") && _roadNeighbors.ContainsKey("Left"))
        {
            SetRoadRotationForTemporaryRoad(positionX, positionZ, RoadType.TreeWay, 90);
        }

        //Left and Up is Road, Right and Down something else
        else if (_roadNeighbors.ContainsKey("Left") && _roadNeighbors.ContainsKey("Up"))
        {
            SetRoadRotationForTemporaryRoad(positionX, positionZ, RoadType.Curve, 0);

        }

        //Up and Right is Road, Left and Down something else
        else if (_roadNeighbors.ContainsKey("Up") && _roadNeighbors.ContainsKey("Right"))
        {
            SetRoadRotationForTemporaryRoad(positionX, positionZ, RoadType.Curve, 90);
        }

        //Right and Down is Road, Left and Up something else
        else if (_roadNeighbors.ContainsKey("Right") && _roadNeighbors.ContainsKey("Down"))
        {
            SetRoadRotationForTemporaryRoad(positionX, positionZ, RoadType.Curve, 180);
        }
        //Down and Left is Road, Right and Up something else
        else if (_roadNeighbors.ContainsKey("Down") && _roadNeighbors.ContainsKey("Left"))
        {
            SetRoadRotationForTemporaryRoad(positionX, positionZ, RoadType.Curve, 270);
        }
        #endregion

        //Left and Right is Road, Up and Down something else
        else if (_roadNeighbors.ContainsKey("Left") && _roadNeighbors.ContainsKey("Right"))
        {
            SetRoadRotationForTemporaryRoad(positionX, positionZ, 90);
        }
        //Left is Road, Right, Up and Down something else
        else if (_roadNeighbors.ContainsKey("Left"))
        {
            SetRoadRotationForTemporaryRoad(positionX, positionZ, 90);
        }
        //Right is Road, Left, Up and Down something else
        else if (_roadNeighbors.ContainsKey("Right"))
        {
            SetRoadRotationForTemporaryRoad(positionX, positionZ, 90);
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
    public void FixNeighborsRoadForTemporary()
    {
        foreach (var road in BilderSystem.Instance.RoadsToRecheck)
        {
            FixRoadTemporary(road.x, road.z);
        }
        BilderSystem.Instance.RoadsToRecheck.Clear();
    }

    #region FixMetods
    private void SetRoadRotation(int positionX, int positionZ, RoadType roadType, int nodeRotation)
    {
        if (roadType != RoadType.Nothing)
        {
            BilderSystem.Instance.DestroyRoad(positionX, positionZ);
            BilderSystem.Instance.AllRoads.Remove(new Vector3Int(positionX, 0, positionZ));
            var road = _bilder.Bild(roadType);
            road.transform.position = new Vector3Int(positionX, 0, positionZ);
            BilderSystem.Instance.AllRoads.Add(new Vector3Int(positionX, 0, positionZ), road);
        }
        BilderSystem.Instance.AllRoads[new Vector3Int(positionX, 0, positionZ)].GetComponentInChildren<FixTransform>().FixRotation(nodeRotation);
    }
    private void SetRoadRotationForTemporaryRoad(int positionX, int positionZ, RoadType roadType, int nodeRotation)
    {
        if (roadType != RoadType.Nothing)
        {
            BilderSystem.Instance.DestroyRoad(positionX, positionZ);
            BilderSystem.Instance.TemporaryRoads.Remove(new Vector3Int(positionX, 0, positionZ));
            var road = _bilder.Bild(roadType);
            road.transform.position = new Vector3Int(positionX, 0, positionZ);
            BilderSystem.Instance.TemporaryRoads.Add(new Vector3Int(positionX, 0, positionZ), road);
        }
        BilderSystem.Instance.AllRoads[new Vector3Int(positionX, 0, positionZ)].GetComponentInChildren<FixTransform>().FixRotation(nodeRotation);
    }
    private void SetRoadRotation(int positionX, int positionZ, int nodeRotation)
    {
        BilderSystem.Instance.AllRoads[new Vector3Int(positionX, 0, positionZ)].GetComponentInChildren<FixTransform>().FixRotation(nodeRotation);
    }
    private void SetRoadRotationForTemporaryRoad(int positionX, int positionZ, int nodeRotation)
    {
        BilderSystem.Instance.TemporaryRoads[new Vector3Int(positionX, 0, positionZ)].GetComponentInChildren<FixTransform>().FixRotation(nodeRotation);
    }

    #endregion

}


