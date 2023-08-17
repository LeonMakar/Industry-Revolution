using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    public Action<Vector3Int> OnMouseClick, OnMouseHold;
    public Action OnMouseUp;



    [SerializeField] Camera mainCamera;

    public LayerMask groundMask;

    private Vector2 _cameraMovementVector;
    public Vector2 CameraMovementVector => _cameraMovementVector;

    private void Update()
    {
        CheckClickDownEvent();
        CheckClickUpEvent();
        CheckClickHoldEvent();
        CheckArrowInput();
    }
    public Vector2Int? CursorPosiiton()
    {
        Vector2Int postion2Int;
        if (RaycastGround() != null)
        {
            postion2Int = new Vector2Int(RaycastGround().Value.x, RaycastGround().Value.z);
            return postion2Int;
        }
        else
            return null;

    }
    private Vector3Int? RaycastGround()
    {
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundMask))
        {
            Vector3Int positionInt = Vector3Int.FloorToInt(hit.point);
            Vector3Int position = new Vector3Int(positionInt.x, 0, positionInt.z);
            return position;
        }
        return null;
    }
    private void CheckArrowInput()
    {
        _cameraMovementVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    private void CheckClickHoldEvent()
    {
        if (Input.GetMouseButton(0) && EventSystem.current.IsPointerOverGameObject() == false)
        {
            var position = RaycastGround();
            if (position != null)
            {
                OnMouseHold?.Invoke(position.Value);
            }
        }
    }

    private void CheckClickUpEvent()
    {
        if (Input.GetMouseButtonUp(0) && EventSystem.current.IsPointerOverGameObject() == false)
        {
            OnMouseUp?.Invoke();
        }
    }

    private void CheckClickDownEvent()
    {
        if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject() == false)
        {
            var position = RaycastGround();
            if (position != null)
            {
                EventBus.Instance.Invoke<MouseIsClickedSignal>(new MouseIsClickedSignal(position.Value));
            }
        }
    }
}
