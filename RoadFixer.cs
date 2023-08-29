using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class RoadFixerOLd : MonoBehaviour
{
    public GameObject deadEnd, roadStright, corner, threeWay, fourWay;

    public void FixRoadAtPosition(PlacementSystem placementSystem, Vector3Int temporaryPosition)
    {
        //[right,up,left,down]
        var result = placementSystem.GetNeighbourTypesFor(temporaryPosition);
        int roadCount = 0;
        roadCount = result.Where(x => x == CellType.Road).Count();
        if (roadCount == 0 || roadCount == 1)
        {
            CreateDeadEnd(placementSystem, result, temporaryPosition);
        }
        else if (roadCount == 2)
        {
            if (CreateStraightRoad(placementSystem, result, temporaryPosition))
                return;
            CreateCorner(placementSystem, result, temporaryPosition);
        }
        else if (roadCount == 3)
        {
            Create3Way(placementSystem, result, temporaryPosition);
        }
        else
        {
            Create4Way(placementSystem, result, temporaryPosition);
        }

    }

    private void Create4Way(PlacementSystem placementSystem, CellType[] result, Vector3Int temporaryPosition)
    {
        placementSystem.ModifyStructureModel(temporaryPosition, fourWay, Quaternion.identity);
    }

    //[left,up,right, down]
    private void Create3Way(PlacementSystem placementSystem, CellType[] result, Vector3Int temporaryPosition)
    {
        if (result[1] == CellType.Road && result[2] == CellType.Road && result[3] == CellType.Road)
        {
            placementSystem.ModifyStructureModel(temporaryPosition, threeWay, Quaternion.identity);
        }
        else if (result[2] == CellType.Road && result[3] == CellType.Road && result[0] == CellType.Road)
        {
            placementSystem.ModifyStructureModel(temporaryPosition, threeWay, Quaternion.Euler(0, 180, 0));
        }
        else if (result[3] == CellType.Road && result[0] == CellType.Road && result[1] == CellType.Road)
        {
            placementSystem.ModifyStructureModel(temporaryPosition, threeWay, Quaternion.Euler(0, 180, 0));
        }
        else if (result[0] == CellType.Road && result[1] == CellType.Road && result[2] == CellType.Road)
        {
            placementSystem.ModifyStructureModel(temporaryPosition, threeWay, Quaternion.Euler(0, 180, 0));
        }
    }
    //[left,up,right, down]
    private void CreateCorner(PlacementSystem placementSystem, CellType[] result, Vector3Int temporaryPosition)
    {
        if (result[1] == CellType.Road && result[2] == CellType.Road)
        {
            placementSystem.ModifyStructureModel(temporaryPosition, corner, Quaternion.Euler(0, 90, 0));
        }
        else if (result[2] == CellType.Road && result[3] == CellType.Road)
        {
            placementSystem.ModifyStructureModel(temporaryPosition, corner, Quaternion.Euler(0, 180, 0));
        }
        else if (result[3] == CellType.Road && result[0] == CellType.Road)
        {
            placementSystem.ModifyStructureModel(temporaryPosition, corner, Quaternion.Euler(0, 270, 0));
        }
        else if (result[0] == CellType.Road && result[1] == CellType.Road)
        {
            placementSystem.ModifyStructureModel(temporaryPosition, corner, Quaternion.Euler(0, 0, 0));
        }
    }

    //[left,up,right, down]
    private bool CreateStraightRoad(PlacementSystem placementSystem, CellType[] result, Vector3Int temporaryPosition)
    {
        if (result[0] == CellType.Road && result[2] == CellType.Road)
        {
            placementSystem.ModifyStructureModel(temporaryPosition, roadStright, Quaternion.identity);
            return true;
        }
        else if (result[1] == CellType.Road && result[3] == CellType.Road)
        {
            placementSystem.ModifyStructureModel(temporaryPosition, roadStright, Quaternion.Euler(0, 90, 0));
            return true;
        }
        return false;
    }
    //[left,up,right, down]
    private void CreateDeadEnd(PlacementSystem placementSystem, CellType[] result, Vector3Int temporaryPosition)
    {
        if (result[1] == CellType.Road)
        {
            placementSystem.ModifyStructureModel(temporaryPosition, deadEnd, Quaternion.Euler(0, 90, 0));
        }
        else if (result[2] == CellType.Road)
        {
            placementSystem.ModifyStructureModel(temporaryPosition, deadEnd, Quaternion.Euler(0, 0, 0));
        }
        else if (result[3] == CellType.Road)
        {
            placementSystem.ModifyStructureModel(temporaryPosition, deadEnd, Quaternion.Euler(0, 90, 0));
        }
        else if (result[0] == CellType.Road)
        {
            placementSystem.ModifyStructureModel(temporaryPosition, deadEnd, Quaternion.Euler(0, 180, 0));
        }
    }
}
