using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameInputSystem : MonoBehaviour
{
    [SerializeField] private Camera _cameraMain;
    [SerializeField] private LayerMask _layerMask;

    private AStarSearch _aStarSearch;
    private Vector3Int? _lastPosition = new Vector3Int(0, 1000, 0);

    private Vector3Int _startPointForRoad = new Vector3Int();

    private Vector2 _cameraMovementVector;
    public Vector2 CameraMovementVector => _cameraMovementVector;

    private void Start()
    {
        _aStarSearch = new AStarSearch();
    }
    private void Update()
    {
        CheckMouseIsClicked();
        CheckMousePositionOnGround();
        CheckMouseIsHold();
        CheckMouseButtonIsUp();
    }
    private Vector3Int? RaycastGround()
    {
        RaycastHit hit;
        Ray ray = _cameraMain.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _layerMask))
        {
            Vector3Int position = Vector3Int.FloorToInt(hit.point);
            Vector3Int positionToReturn = new Vector3Int(position.x, 0, position.z);
            return positionToReturn;
        }   

        return null;
    }


    private void CheckMouseIsClicked()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            var position = RaycastGround();
            if (position != null)
            {
                EventBus.Instance.Invoke<MouseIsClickedSignal>(new MouseIsClickedSignal(position));
                _startPointForRoad = position.Value;
            }
        }
    }
    private void CheckMouseIsHold()
    {
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            var position = RaycastGround();
            if (position != null && _lastPosition != position)
            {
                if (position.Value != _startPointForRoad)
                {
                    EventBus.Instance.Invoke<MouseIsHoldSignal>(new MouseIsHoldSignal
                        (_aStarSearch.GetNodesForPath(_startPointForRoad, position.Value)));
                    _lastPosition = position;
                }
            }
        }
    }

    private void CheckMousePositionOnGround()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            var position = RaycastGround();
            if (position != null)
                EventBus.Instance.Invoke<MousePositionSignal>(new MousePositionSignal(position));
        }
    }

    private void CheckMouseButtonIsUp()
    {
        if (Input.GetMouseButtonUp(0) && EventSystem.current.IsPointerOverGameObject() == false)
        {
            EventBus.Instance.Invoke<MouseIsUpSignal>(new MouseIsUpSignal());
        }
    }
}

