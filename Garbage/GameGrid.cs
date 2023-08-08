using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameGrid : MonoBehaviour
{
    public static GameGrid Instance { get; private set; }
    public Vector2Int GridSize { get; private set; } = new Vector2Int(10, 10);

    private BildingFoundation[,] _bildingFoundationsGrid;
    private DistrictFoundation[,] _districtFoundationsGrid;
    private CityFoundation[,] _cityPositionOnGrid;

    public bool IsUnderCity = false;
    private Camera _mainCamera;
    private FlyingStructure _flyingStructure;


    private void Awake()
    {
        Instance = this;
        //Init//
        _bildingFoundationsGrid = new BildingFoundation[GridSize.x, GridSize.x];
        _districtFoundationsGrid = new DistrictFoundation[GridSize.x, GridSize.y];
        _cityPositionOnGrid = new CityFoundation[GridSize.x, GridSize.y];
        _mainCamera = Camera.main;
    }
    public void StartPlacing(GameObject prefab)
    {
        if (_flyingStructure != null)
            Destroy(_flyingStructure.gameObject);
        _flyingStructure = prefab.GetComponentInChildren<FlyingStructure>();
        FoundationCreator creator = new FoundationFactory();
        switch (_flyingStructure.GetType().ToString())
        {
            case "BildingFoundation":
                _flyingStructure = creator.CreateBilding("path");
                break;
            case "DistrictFoundation":
                _flyingStructure = creator.CreateDistrict("path");
                break;
            case "CityFoundation":
                _flyingStructure = creator.CreateCity("path");
                break;
        }
    }


    private void Update()
    {
        if (_flyingStructure != null)
        {
            var groundPlane = new Plane(Vector3.up, Vector3.zero);
            var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            if (groundPlane.Raycast(ray, out float position))
            {
                Vector3 worldPosition = ray.GetPoint(position);

                int x = Mathf.RoundToInt(worldPosition.x);
                int y = Mathf.RoundToInt(worldPosition.z);

                bool available = true;

                if (x < 0 || x > GridSize.x - _flyingStructure.Size.x)
                    available = false;
                if (y < 0 || y > GridSize.y - _flyingStructure.Size.y)
                    available = false;

                if (available)
                    switch ((_flyingStructure.GetType()).ToString())
                    {
                        case "BildingFoundation":
                            if (IsPlaceTakenForBildings(x, y) == true || IsUnderCity == false || (_flyingStructure.GetType().ToString() == "BildingFoundation") == false)
                                available = false;
                            break;
                        case "DistrictFoundation":
                            if (IsPlaceTakenForDistricts(x, y) || IsUnderCity == false || (_flyingStructure.GetType().ToString() == "DistrictFoundation") == false)
                                available = false;
                            break;
                        case "CityFoundation":
                            if (IsPlaceTakenForCity(x, y) || (_flyingStructure.GetType().ToString() == "CityFoundation") == false)
                                available = false;
                            break;
                    }

                Debug.Log("IsUnderCity= " + IsUnderCity);
                _flyingStructure.SetError(available);
                _flyingStructure.transform.position = new Vector3(x, 0, y);

                //Rotate
                if (Input.GetKeyDown(KeyCode.R))
                {
                    Debug.Log(_flyingStructure.GetType());
                    _flyingStructure.Rotate(90);
                }
                //Place structure

                if (available && Input.GetMouseButtonDown(0))
                {
                    switch ((_flyingStructure.GetType()).ToString())
                    {
                        case "BildingFoundation":
                            if (IsUnderCity)
                                PlaceFlyingBilding(x, y);
                            break;
                        case "DistrictFoundation":
                            if (IsUnderCity)
                                PlaceflyingDistrict(x, y);
                            break;
                        case "CityFoundation":
                            PlaceflyingCity(x, y);
                            break;
                    }
                }
            }
        }
    }
    private bool IsPlaceTakenForBildings(int placeX, int placeY)
    {
        for (int x = 0; x < _flyingStructure.Size.x; x++)
        {
            for (int y = 0; y < _flyingStructure.Size.y; y++)
            {
                if (_bildingFoundationsGrid[placeX + x, placeY + y] != null)
                    return true;
            }
        }

        return false;
    }
    private bool IsPlaceTakenForDistricts(int placeX, int placeY)
    {
        for (int x = 0; x < _flyingStructure.Size.x; x++)
        {
            for (int y = 0; y < _flyingStructure.Size.y; y++)
            {
                if (_districtFoundationsGrid[placeX + x, placeY + y] != null)
                    return true;
            }
        }

        return false;
    }
    private bool IsPlaceTakenForCity(int placeX, int placeY)
    {
        for (int x = 0; x < _flyingStructure.Size.x; x++)
        {
            for (int y = 0; y < _flyingStructure.Size.y; y++)
            {
                if (_cityPositionOnGrid[placeX + x, placeY + y] != null)
                    return true;
            }
        }
        return false;
    }
    private void PlaceFlyingBilding(int placeX, int placeY)
    {
        for (int x = 1; x < _flyingStructure.Size.x; x++)
        {
            for (int y = 1; y < _flyingStructure.Size.y; y++)
            {
                _bildingFoundationsGrid[placeX + x, placeY + y] = (BildingFoundation)_flyingStructure;
            }
        }
        _flyingStructure = null;
    }

    private void PlaceflyingDistrict(int placeX, int placeY)
    {
        for (int x = 0; x < _flyingStructure.Size.x; x++)
        {
            for (int y = 0; y < _flyingStructure.Size.y; y++)
            {
                _districtFoundationsGrid[placeX + x, placeY + y] = (DistrictFoundation)_flyingStructure;
            }
        }
        _flyingStructure = null;
    }
    private void PlaceflyingCity(int placeX, int placeY)
    {
        for (int x = 0; x < _flyingStructure.Size.x; x++)
        {
            for (int y = 0; y < _flyingStructure.Size.y; y++)
            {
                _cityPositionOnGrid[placeX + x, placeY + y] = (CityFoundation)_flyingStructure;
            }
        }
        _flyingStructure = null;
    }
}
//////////////////////
///



