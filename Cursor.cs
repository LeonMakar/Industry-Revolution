using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Cursor : MonoBehaviour, IInjectable, IService
{
    [SerializeField] private GameObject _cursor;

    private GameObject _objectUnderCursor;
    public GameObject ObjectUnderCursor => _objectUnderCursor;

    private ObjectDataForBilding _bildingData;
    public static bool CursorIsEmpty = true;

    private EventBus _eventBus;
    public Dictionary<Type, Type> ServiceAndImplamentation { get; } = new Dictionary<Type, Type>()
    {
        [typeof(EventBus)] = typeof(EventBus),

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
            }
        }
        _eventBus.Subscrube<MousePositionSignal>(CursorPosition);
    }
    private void Update()
    {
        RotateCursorBilding();
        ResetObjectUnderCursorESC();
    }

    private void OnDisable()
    {
        _eventBus.Unsubscribe<MousePositionSignal>(CursorPosition);
    }
    private void RotateCursorBilding()
    {
        if (_objectUnderCursor != null && Input.GetKeyDown(KeyCode.R))
            _bildingData.ChangeBildingRotation();
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
        _objectUnderCursor = Instantiate(gameObject);
        _objectUnderCursor.transform.position = new Vector3(_cursor.transform.position.x, 0.02f, _cursor.transform.position.z);
        _objectUnderCursor.TryGetComponent<ObjectDataForBilding>(out ObjectDataForBilding selectedObject);
        _cursor.SetActive(false);
        _eventBus.Invoke<SelectedObjectSignal>(new SelectedObjectSignal(selectedObject));
        _bildingData = selectedObject;
        CursorIsEmpty = false;
    }

    public void ResetObjectUnderCursorESC()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_objectUnderCursor != null)
            {
                Destroy(_objectUnderCursor.gameObject);
                _objectUnderCursor = null;
            }
            _cursor.SetActive(true);
            CursorIsEmpty = true;
        }
    }
    public void ResetObjectUnderCursor()
    {
        if (_objectUnderCursor != null)
        {
            Destroy(_objectUnderCursor.gameObject);
            _objectUnderCursor = null;
        }
        _cursor.SetActive(true);
        CursorIsEmpty = true;

    }

}
