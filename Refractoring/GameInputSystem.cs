using UnityEngine;
using UnityEngine.EventSystems;

public class GameInputSystem : MonoBehaviour
{
    [SerializeField] private Camera _cameraMain;
    [SerializeField] private LayerMask _layerMask;

    private Vector2 _cameraMovementVector;
    public Vector2 CameraMovementVector => _cameraMovementVector;


    private void Update()
    {
        CheckMouseIsClicked();
        CheckMousePositionOnGround();
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
                EventBus.Instance.Invoke<MouseIsClickedSignal>(new MouseIsClickedSignal(position));
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
}

