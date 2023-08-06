using UnityEngine;

public class GameGrid : MonoBehaviour
{
    [SerializeField] private Vector2Int GridSize = new Vector2Int(10, 10);

    private StructureAndHisSettings[,] _structurePositionOnGrid;
    private StructureAndHisSettings _flyingStructure;
    private Camera _mainCamera;


    private void Awake()
    {
        _structurePositionOnGrid = new StructureAndHisSettings[GridSize.x, GridSize.y];
        _mainCamera = Camera.main;
    }

    public void StartPlacingbuiling(StructureAndHisSettings structureprefab)
    {
        if (_flyingStructure != null)
            Destroy(_flyingStructure.gameObject);

        _flyingStructure = Instantiate(structureprefab);
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
                if (available && IsPlaceTaken(x, y))
                    available = false;

                _flyingStructure.SetError(available);
                _flyingStructure.transform.position = new Vector3(x + 0.5f, 0, y + 0.5f);

                //Rotate

                if (Input.GetKeyDown(KeyCode.R))
                {
                    _flyingStructure.RotateBilding(90);
                }

                //Place bilding

                if (available && Input.GetMouseButtonDown(0))
                {
                    PlaceFlyingStructure(x, y);
                }


            }
        }

    }
    private bool IsPlaceTaken(int placeX, int placeY)
    {
        for (int x = 0; x < _flyingStructure.Size.x; x++)
        {
            for (int y = 0; y < _flyingStructure.Size.y; y++)
            {
                if (_structurePositionOnGrid[placeX + x, placeY + y] != null)
                    return true;
            }
        }

        return false;
    }
    private void PlaceFlyingStructure(int placeX, int placeY)
    {
        for (int x = 0; x < _flyingStructure.Size.x; x++)
        {
            for (int y = 0; y < _flyingStructure.Size.y; y++)
            {
                _structurePositionOnGrid[placeX + x, placeY + y] = _flyingStructure;
            }
        }
        _flyingStructure = null;
    }
}
