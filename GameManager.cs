using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CameraMovement CameraMovement;
    public InputManager InputManager;
    public RoadSystem RoadSystem;

    [SerializeField] private Transform _cursorTransform;

    private void Start()
    {
        InputManager.OnMouseClick += HandleMouseClick;
    }

    private void HandleMouseClick(Vector3Int posiiton)
    {
        Debug.Log(posiiton);
        RoadSystem.PlaceRoad(posiiton);
    }

    private void Update()
    {
        CameraMovement.MoveCamera(new Vector3(InputManager.CameraMovementVector.x, 0, InputManager.CameraMovementVector.y));
        if (InputManager.CursorPosiiton() != null)
            _cursorTransform.position = new Vector3(InputManager.CursorPosiiton().Value.x, 0.01f, InputManager.CursorPosiiton().Value.y);
    }

}
