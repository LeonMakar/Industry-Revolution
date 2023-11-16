using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class Vehicle : MonoBehaviour
{
    protected float speed;

    [SerializeField] private MonoBehaviour _implamentation;
    public abstract void GetName();
    public abstract void OnMouseDown();

    public void FixedUpdate()
    { }


}
