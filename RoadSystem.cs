using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadSystem : MonoBehaviour
{
    [SerializeField] private PlacementSystem _placementSystem;

    public List<Vector3Int> temporaryPlacementPositions = new List<Vector3Int>();

    public RoadFixer roadFixer;
    public GameObject prefab;

    private void Start()
    {
        roadFixer = GetComponent<RoadFixer>();

    }
    public void PlaceRoad(Vector3Int position)
    {
        if (_placementSystem.CheckIfPositionInBound(position) == false)
            return;
        if (_placementSystem.CheckIfPositionIsFree(position) == false)
            return;
        temporaryPlacementPositions.Clear();
        temporaryPlacementPositions.Add(position);
        _placementSystem.PlaceTemporaryStructure(position, CellType.Road, StructureType.Road, prefab) ;
        FixRoadPrefabs();
    }

    private void FixRoadPrefabs()
    {
        foreach (var temporaryPosition in temporaryPlacementPositions)
        {
            roadFixer.FixRoadAtPosition(_placementSystem, temporaryPosition);
        }
    }
}

