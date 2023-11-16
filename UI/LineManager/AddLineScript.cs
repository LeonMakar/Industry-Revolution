using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;

public class AddLineScript : MonoBehaviour
{
    [SerializeField] private string _pathToLinePrefab;
    [SerializeField] private Transform _parentTransform;
    [SerializeField] private LineManager _lineManager;

    private CanvasFactory _factory = new CanvasFactory();

    public void AddLine()
    {
        _factory.PathForCanvasPrefab = _pathToLinePrefab;
        GameObject line = _factory.Bild(BildingType.CanvasElement);
        line.transform.SetParent(_parentTransform);
        line.TryGetComponent(out Line lineScript);
        if (lineScript != null)
        {
            if (lineScript is IInjectable init)
                init.Injecting();
            _lineManager.AddLineToManager(lineScript);
        }
    }
}
