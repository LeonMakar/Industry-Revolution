using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using UnityEngine.WSA;

public class GameInputSystem : MonoBehaviour
{
    [SerializeField] private Camera _cameraMain;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private GameObject _cursor;

    private GameObject _objectUnderCursor;

    private AStarSearch _aStarSearch;
    private Vector3Int? _lastPosition = new Vector3Int(0, 1000, 0);

    private Vector3Int _startPointForRoad = new Vector3Int();

    private Vector2 _cameraMovementVector;
    public Vector2 CameraMovementVector => _cameraMovementVector;

    private void Start()
    {
        _aStarSearch = new AStarSearch();
        EventBus.Instance.Subscrube<MousePositionSignal>(CursorPosition);
    }
    private void OnDisable()
    {
        EventBus.Instance.Unsubscribe<MousePositionSignal>(CursorPosition);
    }
    private void Update()
    {
        CheckMouseIsClicked();
        CheckMousePositionOnGround();
        CheckMouseIsHold();
        CheckMouseButtonIsUp();
        ResetObjectUnderCursor();
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
    private void CursorPosition(MousePositionSignal signal)
    {
        _cursor.transform.position = signal.positionVector3int;
        if (_objectUnderCursor != null)
            _objectUnderCursor.transform.position = signal.positionVector3int;
    }
    public void SetObjectUnderCursor(GameObject gameObject)
    {
        if (_objectUnderCursor != null)
            Destroy(_objectUnderCursor.gameObject);
        _objectUnderCursor = Instantiate(gameObject, new Vector3(_cursor.transform.position.x, 0.02f, _cursor.transform.position.z), Quaternion.identity);
        ObjectDataForBilding selectedObject;
        _objectUnderCursor.TryGetComponent<ObjectDataForBilding>(out selectedObject);
        _cursor.SetActive(false);
        EventBus.Instance.Invoke<SelectedObjectSignal>(new SelectedObjectSignal(selectedObject));
    }

    public void ResetObjectUnderCursor()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Destroy(_objectUnderCursor.gameObject);
            _objectUnderCursor = null;
            _cursor.SetActive(true);
        }
    }
    private void CheckMouseIsClicked()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            var position = RaycastGround();
            if (position != null)
            {
                EventBus.Instance.Invoke<MouseIsClickedSignal>(new MouseIsClickedSignal(position.Value));
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

