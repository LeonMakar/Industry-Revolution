using System;
using UnityEngine;

public class GameGrid : MonoBehaviour
{
    public static GameGrid Instance { get; private set; }
    public Vector2Int GridSize { get; private set; } = new Vector2Int(10, 10);

    private BildingFoundation[,] _bildingFoundationsGrid;
    private DistrictFoundation[,] _districtFoundationsGrid;
    private CitiesGrid[,] _cityPositionOnGrid;

    private Camera _mainCamera;
    private FlyingStructure _flyingStructure;


    private void Awake()
    {
        //Init//
        _bildingFoundationsGrid = new BildingFoundation[GridSize.x, GridSize.x];
        _districtFoundationsGrid = new DistrictFoundation[GridSize.x, GridSize.y];
        _cityPositionOnGrid = new CitiesGrid[GridSize.x, GridSize.y];
        _mainCamera = Camera.main;
    }
    public void StartPlacing(FlyingStructure prefab)
    {
        if (_flyingStructure != null)
            Destroy(_flyingStructure.gameObject);
        _flyingStructure = prefab;
        switch (_flyingStructure.GetType().ToString())
        {
            case "BildingFoundation":
                BildingCreator BCreator = new BildingFoundationFactory();
                _flyingStructure = BCreator.CreateBilding(prefab.gameObject);
                break;
            case "DistrictFoundation":
                DistrictCreator DCreator = new DistrictFoundationFactory();
                _flyingStructure = DCreator.CreateDistrict(prefab.gameObject);
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

                if (available && IsPlaceTakenForBildings(x, y) && _flyingStructure.GetType() == typeof(BildingFoundation))
                    available = false;
                else if (available && IsPlaceTakenForDistricts(x, y) && _flyingStructure.GetType() == typeof(DistrictFoundation))
                    available = false;


                _flyingStructure.SetError(available);
                _flyingStructure.transform.position = new Vector3(x + 0.5f, 0, y + 0.5f);

                //Rotate
                if (Input.GetKeyDown(KeyCode.R))
                {
                    Debug.Log(_flyingStructure.GetType());
                    _flyingStructure.Rotate(90);
                }
                //Place bilding

                if (available && Input.GetMouseButtonDown(0))
                {
                    switch ((_flyingStructure.GetType()).ToString())
                    {
                        case "BildingFoundation":
                            PlaceFlyingBilding(x, y);
                            break;
                        case "DistrictFoundation":
                            PlaceflyingDistrict(x, y);
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
    private void PlaceFlyingBilding(int placeX, int placeY)
    {
        for (int x = 0; x < _flyingStructure.Size.x; x++)
        {
            for (int y = 0; y < _flyingStructure.Size.y; y++)
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

    public class CitiesGrid
    {
        public void SetCitiesOnGrid(int placeX, int placeY, int citySize)
        {
            for (int i = 0; i < citySize; i++)
            {
                for (int j = 0; j < citySize; j++)
                {
                    //_citiesGrid[placeX + i, placeY + j] = new CitiesGrid();
                }
            }
        }
    }

}
