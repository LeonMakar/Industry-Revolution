﻿using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Camera gameCamera;
    [SerializeField] private float cameraMovementSpeed = 5;

    private void Start()
    {
        gameCamera = GetComponent<Camera>();
    }
    public void MoveCamera(Vector3 inputVector)
    {
        var movementVector = Quaternion.Euler(0, 0, 0) * inputVector;
        gameCamera.transform.position += movementVector * Time.deltaTime * cameraMovementSpeed;
    }

}