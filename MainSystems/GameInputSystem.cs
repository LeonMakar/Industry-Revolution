using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// Player inputs controller
/// </summary>
public class GameInputSystem : MonoBehaviour, IInjectable
{
    [SerializeField] private Camera _cameraMain;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private CameraMovement _cameraMovement;

    private EventBus _eventBus;

    private AStarSearch _aStarSearch;
    private Cursor _cursor;
    private Vector3Int? _lastPosition = new Vector3Int(0, 1000, 0);

    private Vector3Int _startPointForRoad = new Vector3Int();

    private Vector2 _cameraMovementVector;
    public Vector2 CameraMovementVector => _cameraMovementVector;

    public Dictionary<Type, Type> ServiceAndImplamentation { get; } = new Dictionary<Type, Type>()
    {
        [typeof(EventBus)] = typeof(EventBus),
        [typeof(AStarSearch)] = typeof(AStarSearch),
        [typeof(Cursor)] = typeof(Cursor),
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
                case nameof(Cursor):
                    _cursor = (Cursor)service;
                    break;
            }
        }
    }

    private void Update()
    {
        CheckMouseIsClicked();
        CheckMousePositionOnGround();
        CheckMouseIsHold();
        CheckMouseButtonIsUp();
        CheckArrowInput();
        _cameraMovement.MoveCamera(new Vector3(_cameraMovementVector.x, 0, _cameraMovementVector.y));
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
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject() && _cursor.ObjectUnderCursor != null && _cursor.ObjectUnderCursor.GetComponent<ObjectDataForBilding>().SelectedObjectStructureType == StructureType.Road)
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

