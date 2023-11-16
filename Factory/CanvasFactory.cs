using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasFactory : Factory, IService
{
    public string PathForCanvasPrefab { get; set; }

    public override GameObject Bild(BildingType bildingType)
    {
        switch (bildingType)
        {
            case BildingType.CanvasElement:
                var canvasElement = Resources.Load<GameObject>(PathForCanvasPrefab);
                var element = GameObject.Instantiate(canvasElement);
                return element;
            default:
                throw new InvalidOperationException("Cannot find the path");
        }
    }
}
