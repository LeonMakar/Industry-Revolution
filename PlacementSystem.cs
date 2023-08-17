using System;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private int _wight, _height;

    private Griddd _placementGrid;
    RoadFactory _roadFactory;

    private Dictionary<Vector3Int, StructureModel> temporaryRoadObjects = new Dictionary<Vector3Int, StructureModel>();


    private void Start()
    {
        _placementGrid = new Griddd(_wight, _height);
        _roadFactory = new RoadFactory();
    }
    internal bool CheckIfPositionInBound(Vector3Int position)
    {
        if (position.x >= 0 && position.x < _wight && position.z >= 0 && position.z < _height)
            return true;
        else
            return false;
    }

    internal bool CheckIfPositionIsFree(Vector3Int position)
    {
        return CheckIfPositionIsOfType(position, CellType.Empty);

    }

    private bool CheckIfPositionIsOfType(Vector3Int posititon, CellType type)
    {
        return _placementGrid[posititon.x, posititon.z] == type;
    }

    internal void PlaceTemporaryStructure(Vector3Int posititon, CellType type, StructureType road, GameObject strucutrePrefab)
    {
        _placementGrid[posititon.x, posititon.z] = type;
        //_roadFactory.CreatRoad(road, posititon);
        StructureModel structure = CreateANewStructureModel(posititon, strucutrePrefab, type);
        temporaryRoadObjects.Add(posititon, structure);
    }

    private StructureModel CreateANewStructureModel(Vector3Int posititon, GameObject strucutrePrefab, CellType type)
    {
        GameObject structure = new GameObject(type.ToString());
        structure.transform.SetParent(transform);
        structure.transform.localPosition = posititon;
        var structureModel = structure.AddComponent<StructureModel>();
        structureModel.CreatModel(strucutrePrefab);
        return structureModel;
    }

    public void ModifyStructureModel(Vector3Int position, GameObject newModel, Quaternion rotation)
    {
        if (temporaryRoadObjects.ContainsKey(position))
            temporaryRoadObjects[position].SwapModel(newModel, rotation);
    }

    internal CellType[] GetNeighbourTypesFor(Vector3Int temporaryPosition)
    {
        return _placementGrid.GetAllAdjacentCellTypes(temporaryPosition.x, temporaryPosition.z);

    }
}
