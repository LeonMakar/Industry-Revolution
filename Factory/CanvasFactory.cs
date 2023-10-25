using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasFactory : Factory, IService
{
    public override GameObject Bild(BildingType bildingType)
    {
        switch (bildingType)
        {
            case BildingType.CanvasHouse:
                var canvasHouse = Resources.Load<GameObject>("Ui/Canvas");
                var house = GameObject.Instantiate(canvasHouse);
                return house;
            default:
                throw new InvalidOperationException("Cannot find the path");
        }
    }
}
