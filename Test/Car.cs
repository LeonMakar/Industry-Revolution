using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : Vehicle
{
    public override void GetName()
    {
        Debug.Log("����� ������");
    }

    public override void OnMouseDown()
    {
        Debug.Log("��� ������");
    } 

}
