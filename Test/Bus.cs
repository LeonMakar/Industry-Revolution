using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bus : Vehicle
{
    [SerializeField] private Renderer _renderer;
    private Material _newMaterial;
    public override void OnMouseDown()
    {
        if (_renderer.material.color == Color.green)
            Debug.Log("Это автобус");
        foreach (var vehicle in FindObjectsOfType(typeof(Vehicle)))
        {

            var transport = vehicle.GetComponent(typeof(Vehicle));
            transport.gameObject.TryGetComponent(out Vehicle newVehicle);
            newVehicle.GetName();
            Destroy(vehicle.GameObject());
        }
    }

    private void Start()
    {
        _newMaterial = _renderer.material;
        _renderer.material = _newMaterial;
        StartCoroutine(ColorChange());
    }
    public Color RandomColor()
    {
        Color color = new Color();
        int i = UnityEngine.Random.Range(0, 4);
        switch (i)
        {
            case 0:
                color = Color.white;
                break;
            case 1:
                color = Color.red;
                break;
            case 2:
                color = Color.green;
                break;

        }
        return color;
    }
    private IEnumerator ColorChange()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            _renderer.material.color = RandomColor();

        }

    }

    public override void GetName()
    {
        Debug.Log("суперавтобус");
    }
}
