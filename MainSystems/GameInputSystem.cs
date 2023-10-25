using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// Need to Inject => EventBus, AStarSearch
/// </summary>
public class GameInputSystem : MonoBehaviour, IInjectable
{
    [SerializeField] private Camera _cameraMain;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private GameObject _cursor;
    [SerializeField] private CameraMovement _cameraMovement;


    private GameObject _objectUnderCursor;
    private EventBus _eventBus;

    private AStarSearch _aStarSearch;
    private Vector3Int? _lastPosition = new Vector3Int(0, 1000, 0);

    private Vector3Int _startPointForRoad = new Vector3Int();

    private Vector2 _cameraMovementVector;
    public Vector2 CameraMovementVector => _cameraMovementVector;

    public Dictionary<Type, Type> ServiceAndImplamentation { get; } = new Dictionary<Type, Type>()
    {
        [typeof(EventBus)] = typeof(EventBus),
        [typeof(AStarSearch)] = typeof(AStarSearch),

    };

    public void Inject(params IService[] services)
    {
        foreach (var service in services)
        {
            switch (service.GetType().Name)
            {
                case nameof(EventBus):
                    _eventBus = (EventBus)service;
                    break;
                case nameof(AStarSearch):
                    _aStarSearch = (AStarSearch)service;
                    break;
            }
        }
        _eventBus.Subscrube<MousePositionSignal>(CursorPosition);
    }

    private void OnDisable()
    {
        _eventBus.Unsubscribe<MousePositionSignal>(CursorPosition);
    }
    private void Update()
    {
        CheckMouseIsClicked();
        CheckMousePositionOnGround();
        CheckMouseIsHold();
        CheckMouseButtonIsUp();
        ResetObjectUnderCursor();
        CheckArrowInput();
        RotateBilding();
        _cameraMovement.MoveCamera(new Vector3(_cameraMovementVector.x, 0, _cameraMovementVector.y));
    }
    private void RotateBilding()
    {
        if (_objectUnderCursor != null && Input.GetKeyDown(KeyCode.R))
        {
            _objectUnderCursor.gameObject.transform.Rotate(0, 90, 0);
        }
    }
    private void CheckArrowInput()
    {
        _cameraMovementVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
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
        _objectUnderCursor.TryGetComponent<ObjectDataForBilding>(out ObjectDataForBilding selectedObject);
        _cursor.SetActive(false);
        _eventBus.Invoke<SelectedObjectSignal>(new SelectedObjectSignal(selectedObject));
    }

    public void ResetObjectUnderCursor()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_objectUnderCursor != null)
            {
                Destroy(_objectUnderCursor.gameObject);
                _objectUnderCursor = null;
            }
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
                _eventBus.Invoke<MouseIsClickedSignal>(new MouseIsClickedSignal(position.Value));
                _startPointForRoad = position.Value;
            }
        }
    }
    private void CheckMouseIsHold()
    {
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject() && _objectUnderCursor != null && _objectUnderCursor.GetComponent<ObjectDataForBilding>().SelectedObjectStructureType == StructureType.Road)
        {
            var position = RaycastGround();
            if (position != null && _lastPosition != position)
            {
                if (position.Value != _startPointForRoad && (GridSystem.Instance[position.Value.x, position.Value.z].TypeOfNode == NodeType.Road
                    || GridSystem.Instance[position.Value.x, position.Value.z].TypeOfNode == NodeType.Empty))
                {
                    try
                    {
                        _eventBus.Invoke<MouseIsHoldSignal>(new MouseIsHoldSignal
                            (_aStarSearch.GetNodesForPath(_startPointForRoad, position.Value)));
                    }
                    catch (System.Exception)
                    {
                        Debug.Log("The road path too dificult for calculating");
                    }
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
                _eventBus.Invoke<MousePositionSignal>(new MousePositionSignal(position));
        }
    }

    private void CheckMouseButtonIsUp()
    {
        if (Input.GetMouseButtonUp(0) && EventSystem.current.IsPointerOverGameObject() == false)
        {
            _eventBus.Invoke<MouseIsUpSignal>(new MouseIsUpSignal());
        }
    }


}

